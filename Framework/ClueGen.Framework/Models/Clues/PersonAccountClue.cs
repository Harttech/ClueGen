using System;

namespace ClueGen.Framework.Models.Clues
{
    public class PersonAccountClue : ClueBase
    {
        public PersonAccountClue(string displayName, Guid personId, Statement statement, params string[] descriptiveElements) : base(displayName, ClueKind.PersonAccount, descriptiveElements)
        {
            PersonId = personId;
            Statement = statement;
        }
        
        public Guid PersonId { get; }
        public Statement Statement { get; }
    }
}
