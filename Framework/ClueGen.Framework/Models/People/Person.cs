using ClueGen.Framework.Models.Actions;
using ClueGen.Framework.Models.Clues;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ClueGen.Framework.Models.People
{
    [DebuggerDisplay("{DisplayName} G:{Gender} R:{Role}")]
    public class Person : CaseContainerBase<Testimony>
    {
        private readonly Dictionary<Guid, PersonRelationship> _relationships = new Dictionary<Guid, PersonRelationship>();
        private readonly Dictionary<Guid, IPersonAction> _actions = new Dictionary<Guid, IPersonAction>();

        public Person(string firstname, string lastname, bool gender, Personality personality, Role role, Guid locationId, params string[] descriptiveElements) : base($"{firstname} {lastname}", descriptiveElements)
        {
            Relationships = new ReadOnlyDictionary<Guid, PersonRelationship>(_relationships);
            Actions = new ReadOnlyDictionary<Guid, IPersonAction>(_actions);
            Personality = personality;
            Role = role;
            Gender = gender;
            LocationId = locationId;
            Firstname = firstname;
            Lastname = lastname;
        }

        internal void SetRelationship(Guid targetId, PersonRelationship relationship) => _relationships[targetId] = relationship;
        internal void SetAction(Guid target, IPersonAction action) => _actions[target] = action;

        public FamilyRelationshipKind? IsRelatedTo(Guid target)
        {
            if (!_relationships.TryGetValue(target, out var relationship))
                return null;

            if (relationship is PersonFamilyRelationship familyRelationship)
                return familyRelationship.FamilyKind;
            return null;
        }

        public string Firstname { get; }
        public string Lastname { get; }
        /// <summary>
        /// Gender of the person. 0 = Male, 1 = Female.
        ///
        /// Gender fluidity is outside the scope of this project. Just consider it a "gender code" rather than actual sex.
        /// </summary>
        public bool Gender { get; }

        public int CurrentTestimony { get; set; }
        public Mood CurrentMood { get; set; }
        public Role Role { get; }
        public Personality Personality { get; }
        public ReadOnlyDictionary<Guid, PersonRelationship> Relationships { get; }
        public ReadOnlyDictionary<Guid, IPersonAction> Actions { get; }
        public Guid LocationId { get; }
    }
}
