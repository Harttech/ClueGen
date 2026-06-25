using ClueGen.Framework.Models.Clues;
using ClueGen.Framework.Models.People;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ClueGen.Framework.Models
{
    public class Case : CaseObjectBase
    {
        public Case(string displayName, List<Person> people, Environment.Environment environment, List<ClueBase> clues, params string[] descriptiveElements) : base(displayName, descriptiveElements)
        {
            AllPeople = new ReadOnlyDictionary<Guid, Person>(people.ToDictionary(p => p.Id, p => p));
            Environment = environment;
            Clues = new ReadOnlyDictionary<Guid, ClueBase>(clues.ToDictionary(c => c.Id, c => c));
        }

        public ReadOnlyDictionary<Guid, ClueBase> Clues { get; }
        public List<Guid> DiscoveredClues { get; } = new List<Guid>();
        public ReadOnlyDictionary<Guid, Person> AllPeople { get; }
        public Environment.Environment Environment { get; }

        public Person[] Victims => AllPeople.Values.Where(x => x.Role == Role.Victim).ToArray();
        public Person[] Witnesses => AllPeople.Values.Where(x => x.Role == Role.Witness).ToArray();
        public Person[] Culprits => AllPeople.Values.Where(x => x.Role == Role.Culprit).ToArray();
        public Person Player => AllPeople.Values.First(x => x.Role == Role.Player);
    }
}
