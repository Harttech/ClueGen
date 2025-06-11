namespace ClueGen.Framework.Models.Environment
{
    public class Environment : CaseContainerBase<Room>
    {
        public Environment(string displayName, params string[] descriptiveElements) : base(displayName, descriptiveElements)
        {
        }
    }
}
