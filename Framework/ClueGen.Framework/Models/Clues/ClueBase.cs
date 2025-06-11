namespace ClueGen.Framework.Models.Clues
{
    public abstract class ClueBase : CaseObjectBase
    {
        protected ClueBase(string displayName, ClueKind kind, params string[] descriptiveElements) : base(displayName, descriptiveElements)
        {
            Kind = kind;
        }

        public ClueKind Kind { get; }
    }
}
