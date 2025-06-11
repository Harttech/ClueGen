namespace ClueGen.Framework.Models.Clues
{
    public class GenericClue : ClueBase
    {
        public GenericClue(string displayName, params string[] descriptiveElements) : base(displayName, ClueKind.Generic, descriptiveElements)
        {
        }
    }
}
