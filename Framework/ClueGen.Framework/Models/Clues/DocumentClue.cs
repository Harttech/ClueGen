using ClueGen.Framework.Models.Environment;

namespace ClueGen.Framework.Models.Clues
{
    public class DocumentClue : ClueBase<PointOfInterest>
    {
        public DocumentClue(string displayName, PointOfInterest clueHolder, params string[] descriptiveElements) : base(displayName, ClueKind.Document, clueHolder, descriptiveElements)
        {
        }
    }
}
