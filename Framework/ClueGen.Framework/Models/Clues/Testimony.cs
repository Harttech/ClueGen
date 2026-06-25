using System;
using ClueGen.Framework.Models.People;

namespace ClueGen.Framework.Models.Clues
{
    public class Testimony : CaseContainerChildBase<Person, Statement>
    {
        public Testimony(string displayName, Statement firstStatement, Person parent, params string[] descriptiveElements) : base(displayName, parent, descriptiveElements)
        {
            FirstStatement = firstStatement;
        }

        public Statement FirstStatement { get; }

        public override Statement CreateChild(string displayName, params string[] descriptiveElements)
        {
            throw new NotSupportedException("Use overload with more parameters.");
        }

        public Statement CreateChild(string displayName, Mood mood, Statement nextStatement, ClueBase clue, Statement clueStatement, params string[] descriptiveElements)
        {
            var statement = new Statement(displayName, this, mood, nextStatement, clue, clueStatement, descriptiveElements);
            _children.Add(statement);
            return statement;
        }

        public Statement CreateChild(string displayName, Mood mood, Statement nextStatement, params string[] descriptiveElements)
        {
            var statement = new Statement(displayName, this, mood, nextStatement, descriptiveElements);
            _children.Add(statement);
            return statement;
        }

        public Statement CreateChild(string displayName, Mood mood, params string[] descriptiveElements)
        {
            var statement = new Statement(displayName, this, mood, descriptiveElements);
            _children.Add(statement);
            return statement;
        }
    }
}
