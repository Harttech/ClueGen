using ClueGen.Framework.Models.People;

namespace ClueGen.Framework.Models.Clues
{
    public class Testimony : CaseContainerChildBase<Person>
    {
        public Testimony(string displayName, Statement firstStatement, Person parent, params string[] descriptiveElements) : base(displayName, parent, descriptiveElements)
        {
            FirstStatement = firstStatement;
        }

        public Statement FirstStatement { get; }
    }
}
