using System;
using ClueGen.Framework.Models.People;

namespace ClueGen.Framework.Models.Clues
{
    public class Statement : CaseContainerChildBase<Testimony>
    {
        public Statement(string displayName, Testimony parent, Mood mood, Statement nextStatement, ClueBase clue, Statement clueStatement, params string[] descriptiveElements) : this(displayName, parent, mood, nextStatement, descriptiveElements)
        {
            ClueId = clue.Id;
            Clue = clue;
            ClueStatement = clueStatement;
        }
        
        public Statement(string displayName, Testimony parent, Mood mood, Statement nextStatement, params string[] descriptiveElements) : base(displayName, parent, descriptiveElements)
        {
            Mood = mood;
            NextStatement = nextStatement;
        }

        public Mood Mood { get; }
        public Statement NextStatement { get; }
        public Guid? ClueId { get; }
        public ClueBase Clue { get; }
        public Statement ClueStatement { get; }
    }
}
