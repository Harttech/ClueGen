namespace ClueGen.Framework.Models.Clues
{
    public class WeaponClue : ClueBase
    {
        public WeaponClue(string displayName, WeaponKind kind, params string[] descriptiveElements) : base(displayName, ClueKind.Weapon, descriptiveElements)
        {
            WeaponKind = kind;
        }
        
        public WeaponKind WeaponKind { get; }
    }
}
