namespace ClueGen.Framework.Models.Clues
{
    public class WeaponClue<T> : ClueBase<T> where T : CaseObjectBase
    {
        public WeaponClue(string displayName, WeaponKind kind, T clueHolder, params string[] descriptiveElements) : base(displayName, ClueKind.Weapon, clueHolder, descriptiveElements)
        {
            WeaponKind = kind;
        }

        public WeaponKind WeaponKind { get; }
    }
}
