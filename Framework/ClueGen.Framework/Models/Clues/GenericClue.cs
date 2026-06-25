namespace ClueGen.Framework.Models.Clues
{
    public class GenericClue<T> : ClueBase<T> where T : CaseObjectBase
    {
        public GenericClue(string displayName, T clueHolder, params string[] descriptiveElements) : base(displayName, ClueKind.Generic, clueHolder, descriptiveElements)
        {
        }
    }
}
