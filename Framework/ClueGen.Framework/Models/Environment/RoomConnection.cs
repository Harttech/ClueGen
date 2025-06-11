namespace ClueGen.Framework.Models.Environment
{
    public class RoomConnection : CaseContainerChildBase<Room>
    {
        public RoomConnection(string displayName, Room parent, params string[] descriptiveElements) : base(displayName, parent, descriptiveElements)
        {
        }

        public bool Discovered { get; set; }
    }
}
