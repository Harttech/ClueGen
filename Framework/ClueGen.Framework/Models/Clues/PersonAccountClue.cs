using ClueGen.Framework.Models.People;

namespace ClueGen.Framework.Models.Clues
{
    public class PersonAccountClue : ClueBase<Person>
    {
        public PersonAccountClue(string displayName, Person clueHolder, Statement statement, params string[] descriptiveElements) : base(displayName, ClueKind.PersonAccount, clueHolder, descriptiveElements)
        {
            Statement = statement;
        }

        public Statement Statement { get; }
    }
}
