namespace ClueGen.Framework.Models.Clues
{
    public class DocumentClue : ClueBase
    {
        public DocumentClue(string displayName, params string[] descriptiveElements) : base(displayName, ClueKind.Document, descriptiveElements)
        {
        }
    }
}
