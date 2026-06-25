namespace ClueGen.Framework.Generator
{
    public class XorShiftGenerator : INumberGenerator
    {
        private uint _state;

        public XorShiftGenerator()
        {
            // Not the best way to generate a seed, but it should be good enough for this purpose.
            InitialSeed = _state = (uint)System.DateTime.Now.Ticks;
        }

        public XorShiftGenerator(uint seed)
        {
            InitialSeed = _state = seed;
        }

        /// <summary>
        /// Seed for the generator. Serves as the current state and will change with each call to GetFloat() or GetInt(). If the same seed is used, the same sequence of numbers will be generated.
        /// </summary>
        public uint Seed => _state;

        /// <summary>
        /// The initial seed used to create the generator. This will not change and can be used to reset the generator to its initial state if needed.
        /// </summary>
        public uint InitialSeed { get; }

        private void PerformShift()
        {
            _state ^= _state << 13;
            _state ^= _state >> 7;
            _state ^= _state << 17;
        }

        public float GetFloat()
        {
            PerformShift();
            return (float)_state / uint.MaxValue;
        }

        public int GetInt(int min, int max)
        {
            PerformShift();
            return (int)(_state % (max - min + 1)) + min;
        }
    }
}
