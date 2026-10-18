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
        private readonly List<MidiOut> outputs = new();
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

        /// <summary>
        ///  Irgendeine Taste/Regler am Gerät wurde betätigt oder losgelassen (auch Note Off, Wert 0
        ///  und während des Anlernens). Wird u.a. genutzt, um LED-Farben neu zu senden.
        /// </summary>
        public event EventHandler? ActivityReceived;

        public string Selection { get; private set; } = "";
        public string Status { get; private set; } = "MIDI: aus";
        public bool IsOpen => inputs.Count > 0;

        /// <summary>
        ///  Zu den geöffneten Eingängen wurde mindestens ein passender MIDI-Ausgang gefunden (für LED-Feedback).
        /// </summary>
        public bool CanSend => outputs.Count > 0;
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

            // Ausgang desselben Geräts für LED-Feedback öffnen. Windows benennt ihn meist gleich wie den
            // Eingang; weitere Ports heißen z.B. "MIDIOUT2 (Gerät)" und werden nur genutzt, wenn es keinen
            // gleichnamigen Ausgang gibt.
            var outputNames = Enumerable.Range(0, MidiOut.NumberOfDevices).Select(i => MidiOut.DeviceInfo(i).ProductName).ToList();
            foreach (string input in opened.Distinct())
            {
                List<int> matches = outputNames.Select((name, index) => (name, index)).Where(o => o.name == input).Select(o => o.index).ToList();
                if (matches.Count == 0)
                {
                    matches = outputNames.Select((name, index) => (name, index))
                        .Where(o => o.name.Contains(input, StringComparison.OrdinalIgnoreCase)).Select(o => o.index).ToList();
                }

                foreach (int index in matches)
                {
                    try
                    {
                        outputs.Add(new MidiOut(index));
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"{outputNames[index]} (Ausgang): {ex.Message}");
                    }
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
        ///  Sendet Note On an alle geöffneten Ausgänge (Kanal 1–16).
        /// </summary>
        public void SendNoteOn(int channel, int note, int velocity)
        {
            int message = MidiMessage.StartNote(note, velocity, channel).RawData;
            foreach (MidiOut output in outputs)
            {
                try
                {
                    output.Send(message);
                }
                catch (Exception)
                {
                    // Gerät evtl. abgezogen; beim nächsten Öffnen wird neu verbunden
                }
            }
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

            if (e.MidiEvent is NoteEvent or ControlChangeEvent or PatchChangeEvent)
            {
                context.Post(_ => ActivityReceived?.Invoke(this, EventArgs.Empty), null);
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

            foreach (MidiOut output in outputs)
            {
                output.Dispose();
            }
            outputs.Clear();
        }

        public void Dispose()
        {
            learnCallback = null;
            TriggerReceived = null;
            StatusChanged = null;
            ActivityReceived = null;
            close();
        }
    }
}
