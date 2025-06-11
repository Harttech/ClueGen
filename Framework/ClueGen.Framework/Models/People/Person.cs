using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ClueGen.Framework.Models.Actions;
using ClueGen.Framework.Models.Clues;

namespace ClueGen.Framework.Models.People
{
    public class Person : CaseContainerBase<Testimony>
    {
        private readonly Dictionary<Guid, PersonRelationship> _relationships = new Dictionary<Guid, PersonRelationship>();
        private readonly Dictionary<Guid, IPersonAction> _actions = new Dictionary<Guid, IPersonAction>();

        public Person(string displayName, Personality personality, Role role, Guid locationId, params string[] descriptiveElements) : base(displayName, descriptiveElements)
        {
            Relationships = new ReadOnlyDictionary<Guid, PersonRelationship>(_relationships);
            Actions = new ReadOnlyDictionary<Guid, IPersonAction>(_actions);
            Personality = personality;
            Role = role;
            LocationId = locationId;
        }

        internal void AddTestimony(Testimony testimony)
        {
            _children.Add(testimony);
        }

        internal void SetRelationship(Guid targetId, PersonRelationship relationship) => _relationships[targetId] = relationship;
        internal void SetAction(Guid target, IPersonAction action) => _actions[target] = action;
        
        public int CurrentTestimony { get; set; }
        public Mood CurrentMood { get; set; }
        public Role Role { get; }
        public Personality Personality { get; }
        public ReadOnlyDictionary<Guid, PersonRelationship> Relationships { get; }
        public ReadOnlyDictionary<Guid, IPersonAction> Actions { get; }
        public Guid LocationId { get; }
    }
}
