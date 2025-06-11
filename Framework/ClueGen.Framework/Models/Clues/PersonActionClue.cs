using System;

namespace ClueGen.Framework.Models.Clues
{
    public class PersonActionClue : ClueBase
    {
        public PersonActionClue(string displayName, Guid personId, Guid actionId, params string[] descriptiveElements) : base(displayName, ClueKind.PersonAction, descriptiveElements)
        {
            PersonId = personId;
            ActionId = actionId;
        }
        
        public Guid PersonId { get; }
        public Guid ActionId { get; }
    }
}
