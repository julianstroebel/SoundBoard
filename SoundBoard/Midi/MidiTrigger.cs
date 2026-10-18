namespace SoundBoard
{
    public enum MidiMessageType
    {
        Note,
        ControlChange,
        ProgramChange
    }

    /// <summary>
    ///  Ein MIDI-Befehl, der eine Aktion auslöst (z.B. Note 36 auf Kanal 10).
    /// </summary>
    public sealed record MidiTrigger(MidiMessageType Type, int Channel, int Number)
    {
        private static readonly string[] noteNames = ["C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "H"];

        public override string ToString()
        {
            return Type switch
            {
                MidiMessageType.Note => $"Note {noteNames[Number % 12]}{Number / 12 - 1} ({Number}) · Kanal {Channel}",
                MidiMessageType.ControlChange => $"Controller {Number} · Kanal {Channel}",
                _ => $"Program Change {Number} · Kanal {Channel}",
            };
        }
    }
}
