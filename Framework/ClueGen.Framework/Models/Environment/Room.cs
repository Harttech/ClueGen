using System.Collections.Generic;
using System.Collections.ObjectModel;
using ClueGen.Framework.Models.People;

namespace ClueGen.Framework.Models.Environment
{
    public class Room : CaseContainerChildContainerBase<Environment, RoomConnection>
    {
        private readonly List<Person> _peoplePresent = new List<Person>();

        public Room(string displayName, Environment parent, params string[] descriptiveElements) : base(displayName, parent, descriptiveElements)
        {
            PeoplePresent = new ReadOnlyCollection<Person>(_peoplePresent);
        }

        public ReadOnlyCollection<Person> PeoplePresent { get; }
        public List<PointOfInterest> PointsOfInterest { get; } = new List<PointOfInterest>();
    }
}
