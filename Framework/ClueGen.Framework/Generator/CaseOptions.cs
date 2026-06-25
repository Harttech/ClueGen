using System.Collections.Generic;
using ClueGen.Framework.Models.Clues;
using ClueGen.Framework.Models.Environment;
using ClueGen.Framework.Models.People;

namespace ClueGen.Framework.Generator
{
    public class CaseOptions
    {
        public string CaseName { get; set; }
        public int CluesToGenerate { get; set; }
        public int PeopleToGenerate { get; set; }
        public Environment PresetEnvironment { get; set; }
        public List<ClueBase> PresetClues { get; set; }
        public List<Person> PresetPeople { get; set; }
        public INumberGenerator NumberGenerator { get; set; }
    }
}
