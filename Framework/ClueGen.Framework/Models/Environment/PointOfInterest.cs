using System;

namespace ClueGen.Framework.Models.Environment
{
    public class PointOfInterest : CaseContainerChildBase<Room>
    {
        public PointOfInterest(string displayName, Guid? clueId, Room parent, params string[] descriptiveElements) : base(displayName, parent, descriptiveElements)
        {
        }

        public Guid? ClueId { get; }
        public bool Inspected { get; set; }
    }
}
