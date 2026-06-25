namespace ClueGen.Framework.Generator
{
    public interface INumberGenerator
    {
        public uint Seed { get; }

        public float GetFloat();
        public int GetInt(int min, int max);
    }
}
