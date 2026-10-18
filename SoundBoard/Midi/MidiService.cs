using NAudio.Midi;

namespace SoundBoard
{
    /// <summary>
    ///  Eintrag für die Geräteauswahl.
    /// </summary>
    internal sealed record MidiDeviceItem(string Key, string Name)
    {
        public override string ToString() => Name;
    }

    /// <summary>
    ///  Hört auf die ausgewählten MIDI-Eingänge und meldet Auslöser im UI-Thread.
    ///  Während des Anlernens geht der nächste Auslöser nur an den Anlern-Callback.
    /// </summary>
    internal sealed class MidiService : IDisposable
    {
        /// <summary>
        ///  Auswahlwert für "alle angeschlossenen Geräte".
        /// </summary>
        public const string AllDevices = "*";

        private readonly SynchronizationContext context;
        private readonly List<MidiIn> inputs = new();
        private Action<MidiTrigger>? learnCallback;

        public MidiService()
        {
            context = SynchronizationContext.Current ?? new WindowsFormsSynchronizationContext();
        }

        /// <summary>
        ///  Ein Auslöser kam an (nicht während des Anlernens).
        /// </summary>
        public event EventHandler<MidiTrigger>? TriggerReceived;

        public event EventHandler? StatusChanged;

        public string Selection { get; private set; } = "";
        public string Status { get; private set; } = "MIDI: aus";
        public bool IsOpen => inputs.Count > 0;
        public bool IsLearning => learnCallback != null;

        public static List<MidiDeviceItem> GetDeviceItems()
        {
            var items = new List<MidiDeviceItem>
            {
                new("", "Kein MIDI-Gerät"),
                new(AllDevices, "Alle angeschlossenen Geräte")
            };
            for (int i = 0; i < MidiIn.NumberOfDevices; i++)
            {
                string name = MidiIn.DeviceInfo(i).ProductName;
                if (!items.Any(d => d.Key == name))
                {
                    items.Add(new(name, name));
                }
            }
            return items;
        }

        /// <summary>
        ///  Öffnet das gewählte Gerät ("" = keins, <see cref="AllDevices"/> = alle, sonst Produktname).
        /// </summary>
        public void Open(string selection)
        {
            close();
            Selection = selection;

            if (string.IsNullOrEmpty(selection))
            {
                setStatus("MIDI: aus");
                return;
            }

            var opened = new List<string>();
            var errors = new List<string>();
            for (int i = 0; i < MidiIn.NumberOfDevices; i++)
            {
                string name = MidiIn.DeviceInfo(i).ProductName;
                if (selection != AllDevices && name != selection)
                {
                    continue;
                }

                try
                {
                    var input = new MidiIn(i);
                    input.MessageReceived += input_MessageReceived;
                    input.Start();
                    inputs.Add(input);
                    opened.Add(name);
                }
                catch (Exception ex)
                {
                    errors.Add($"{name}: {ex.Message}");
                }
            }

            string status = opened.Count > 0
                ? "MIDI: " + string.Join(", ", opened)
                : selection == AllDevices ? "MIDI: kein Gerät gefunden" : $"MIDI: „{selection}“ nicht gefunden";
            if (errors.Count > 0)
            {
                status += " – Fehler bei " + string.Join("; ", errors);
            }
            setStatus(status);
        }

        /// <summary>
        ///  Der nächste MIDI-Befehl geht an <paramref name="callback"/> statt eine Aktion auszulösen.
        /// </summary>
        public void BeginLearn(Action<MidiTrigger> callback)
        {
            learnCallback = callback;
        }

        public void CancelLearn()
        {
            learnCallback = null;
        }

        private void input_MessageReceived(object? sender, MidiInMessageEventArgs e)
        {
            // Läuft im Thread des MIDI-Treibers
            MidiTrigger? trigger = e.MidiEvent switch
            {
                NoteEvent note when note.CommandCode == MidiCommandCode.NoteOn && note.Velocity > 0
                    => new MidiTrigger(MidiMessageType.Note, note.Channel, note.NoteNumber),
                ControlChangeEvent cc when cc.ControllerValue > 0
                    => new MidiTrigger(MidiMessageType.ControlChange, cc.Channel, (int)cc.Controller),
                PatchChangeEvent patch
                    => new MidiTrigger(MidiMessageType.ProgramChange, patch.Channel, patch.Patch),
                _ => null
            };

            if (trigger != null)
            {
                context.Post(_ => dispatch(trigger), null);
            }
        }

        private void dispatch(MidiTrigger trigger)
        {
            Action<MidiTrigger>? callback = learnCallback;
            if (callback != null)
            {
                learnCallback = null;
                callback(trigger);
                return;
            }
            TriggerReceived?.Invoke(this, trigger);
        }

        private void setStatus(string status)
        {
            Status = status;
            StatusChanged?.Invoke(this, EventArgs.Empty);
        }

        private void close()
        {
            foreach (MidiIn input in inputs)
            {
                input.MessageReceived -= input_MessageReceived;
                try
                {
                    input.Stop();
                }
                catch (Exception)
                {
                    // Gerät evtl. bereits abgezogen
                }
                input.Dispose();
            }
            inputs.Clear();
        }

        public void Dispose()
        {
            learnCallback = null;
            TriggerReceived = null;
            StatusChanged = null;
            close();
        }
    }
}
