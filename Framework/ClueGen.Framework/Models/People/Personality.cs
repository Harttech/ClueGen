namespace ClueGen.Framework.Models.People
{
    public struct Personality
    {
        public Personality(byte openness, byte consciountiousness, byte extraversion, byte agreeableness, byte neuroticism)
        {
            Openness = openness;
            Consciountiousness = consciountiousness;
            Extraversion = extraversion;
            Agreeableness = agreeableness;
            Neuroticism = neuroticism;
        }

        public byte Openness { get; }
        public byte Consciountiousness { get; }
        public byte Extraversion { get; }
        public byte Agreeableness { get; }
        public byte Neuroticism { get; }

    }
}
