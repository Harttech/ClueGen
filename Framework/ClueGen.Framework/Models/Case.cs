using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ClueGen.Framework.Models.Clues;
using ClueGen.Framework.Models.People;

namespace ClueGen.Framework.Models
{
    public class Case : CaseObjectBase
    {
        public Case(string displayName, params string[] descriptiveElements) : base(displayName, descriptiveElements)
        {
        }

        public ReadOnlyDictionary<Guid, ClueBase> Clues { get; }
        public List<Guid> DiscoveredClues { get; } = new List<Guid>();
        public ReadOnlyDictionary<Guid, Person> AllPeople { get; }

        public Person[] Victims => AllPeople.Values.Where(x => x.Role == Role.Victim).ToArray();
        public Person[] Witnesses => AllPeople.Values.Where(x => x.Role == Role.Witness).ToArray();
        public Person[] Culprits => AllPeople.Values.Where(x => x.Role == Role.Culprit).ToArray();
        public Person Player => AllPeople.TryGetValue(Guid.Empty, out var player) ? player : AllPeople.Values.First(x => x.Role == Role.Player);
    }
}
