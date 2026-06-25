using ClueGen.Framework.Models.Actions;
using ClueGen.Framework.Models.People;

namespace ClueGen.Framework.Models.Clues
{
    public class PersonActionClue : ClueBase<Person>
    {
        public PersonActionClue(string displayName, Person clueHolder, IPersonAction action, params string[] descriptiveElements) : base(displayName, ClueKind.PersonAction, clueHolder, descriptiveElements)
        {
            Action = action;
        }

        public IPersonAction Action { get; }
    }
}
