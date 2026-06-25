using System;
using System.Collections.Generic;
using System.Diagnostics;
using ClueGen.Framework.Models.Clues;

namespace ClueGen.Framework.Models.Environment
{
    [DebuggerDisplay("{DisplayName} (X:{X}, Y:{Y}) | {_clues.Count} Clues")]
    public class PointOfInterest : CaseContainerChildBase<Room>
    {
        private readonly Dictionary<Guid, ClueBase> _clues = new Dictionary<Guid, ClueBase>();
        public PointOfInterest(string displayName, Room parent, int x, int y, params string[] descriptiveElements) : base(displayName, parent, descriptiveElements)
        {
            X = x;
            Y = y;
        }

        public void AddClue(ClueBase clue) => _clues[clue.Id] = clue;
        public void RemoveClue(Guid clueId) => _clues.Remove(clueId);

        public ClueBase this[Guid clueId] => _clues[clueId];

        public bool Inspected { get; set; }
        public int X { get; }
        public int Y { get; }
    }
}
