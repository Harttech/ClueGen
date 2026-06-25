namespace ClueGen.Framework.Models.Environment
{
    public class RoomConnection : CaseObjectBase
    {
        public RoomConnection(string displayName, Room connectsTo, bool discovered, params string[] descriptiveElements) : base(displayName, descriptiveElements)
        {
            ConnectsTo = connectsTo;
            Discovered = discovered;
        }

        public Room ConnectsTo { get; }
        public bool Discovered { get; }
    }
}
