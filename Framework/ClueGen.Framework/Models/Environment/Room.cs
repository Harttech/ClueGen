using System;
using ClueGen.Framework.Models.People;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using ClueGen.Framework.Generator;

namespace ClueGen.Framework.Models.Environment
{
    [DebuggerDisplay("{DisplayName} | {Children.Count} POIs, {PeoplePresent.Count} people")]
    public class Room : CaseContainerChildBase<Environment, PointOfInterest>
    {
        private readonly List<Tuple<Person, Vector2Int>> _peoplePresent = new List<Tuple<Person, Vector2Int>>();
        private readonly List<RoomConnection> _connections = new List<RoomConnection>();

        public Room(string displayName, Environment parent, params string[] descriptiveElements) : base(displayName, parent, descriptiveElements)
        {
            PeoplePresent = new ReadOnlyCollection<Tuple<Person, Vector2Int>>(_peoplePresent);
            Connections = new ReadOnlyCollection<RoomConnection>(_connections);
        }

        public ReadOnlyCollection<Tuple<Person, Vector2Int>> PeoplePresent { get; }
        public int Width { get; } = 5;
        public int Height { get; } = 5;

        public ReadOnlyCollection<RoomConnection> Connections { get; }

        public override PointOfInterest CreateChild(string displayName, params string[] descriptiveElements)
        {
            throw new NotSupportedException("Use the other overload because POIs require a position.");
        }
        public PointOfInterest CreateChild(string displayName, int x, int y, params string[] descriptiveElements)
        {
            if (IsLocationOccupied(x, y))
                throw new InvalidOperationException($"Cannot place a POI at location ({x}, {y}) because it is already occupied.");

            var poi = new PointOfInterest(displayName, this, x, y, descriptiveElements);
            _children.Add(poi);
            return poi;
        }

        public PointOfInterest GetPointOfInterestAtLocation(Vector2Int position) => GetPointOfInterestAtLocation((int)position.X, (int)position.Y);
        public PointOfInterest GetPointOfInterestAtLocation(int x, int y)
        {
            foreach (var poi in Children)
            {
                if (poi.X == x && poi.Y == y)
                    return poi;
            }

            return null;
        }

        public Person GetPersonAtLocation(int x, int y)
        {
            foreach (var person in PeoplePresent)
            {
                if (person.Item2.X == x && person.Item2.Y == y)
                    return person.Item1;
            }

            return null;
        }

        /// <summary>
        /// Returns the object at the given location, if any.
        /// </summary>
        /// <param name="x">The x-coordinate of the location.</param>
        /// <param name="y">The y-coordinate of the location.</param>
        /// <param name="person">The person at the location, if any.</param>
        /// <param name="poi">The point of interest at the location, if any.</param>
        /// <returns>The type of object at the location. 0 = None, 1 = Person, 2 = PointOfInterest</returns>
        public int GetObjectAtLocation(int x, int y, out Person person, out PointOfInterest poi)
        {
            person = GetPersonAtLocation(x, y);
            poi = null;

            if (person != null)
                return 1;

            poi = GetPointOfInterestAtLocation(x, y);
            if (poi != null)
                return 2;

            return 0;
        }

        public bool IsLocationOccupied(int x, int y)
        {
            foreach (var person in PeoplePresent)
            {
                if (person.Item2.X == x && person.Item2.Y == y)
                    return true;
            }

            foreach (var poi in Children)
            {
                if (poi.X == x && poi.Y == y)
                    return true;
            }

            return false;
        }

        public Vector2Int? GetRandomUnoccupiedslot(INumberGenerator generator, int requiredFreeAdjacentSlots, out int directionOfSlots)
        {
            directionOfSlots = requiredFreeAdjacentSlots == 0 ? 0 : -1;

            // First gather all unoccupied slots that fit the criteria. Then pick one at random.
            var validSlots = new List<Vector2Int>();
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (IsLocationOccupied(x, y))
                        continue;

                    // Check if the slot has the required number of free adjacent slots in any direction.
                    if (requiredFreeAdjacentSlots == 0 || DoesSpotHaveRequiredAdjacentSlots(requiredFreeAdjacentSlots, x, y, out directionOfSlots))
                        validSlots.Add(new Vector2Int(x, y));
                }
            }

            if (validSlots.Count == 0)
                return null;

            return validSlots[generator.GetInt(0, validSlots.Count - 1)];
        }

        private bool DoesSpotHaveRequiredAdjacentSlots(int requiredFreeAdjacentSlots, int x, int y, out int directionOfSlots)
        {
            directionOfSlots = -1;

            // Check in all four directions for free adjacent slots.
            // X+
            var valid = true;
            for (var i = 1; i < requiredFreeAdjacentSlots; i++)
            {
                if (x + i < Width - 1 && !IsLocationOccupied(x + i, y))
                    continue;

                valid = false;
                break;
            }

            if (valid)
            {
                directionOfSlots = 1;
                return true;
            }

            // X-
            valid = true;
            for (var i = 1; i < requiredFreeAdjacentSlots; i--)
            {
                if (x - i >= 0 && !IsLocationOccupied(x - i, y))
                    continue;

                valid = false;
                break;
            }

            if (valid)
            {
                directionOfSlots = 2;
                return true;
            }

            // Y+
            valid = true;
            for (var i = 1; i < requiredFreeAdjacentSlots; i++)
            {
                if (y + i < Height - 1 && !IsLocationOccupied(x, y + i))
                    continue;

                valid = false;
                break;
            }

            if (valid)
            {
                directionOfSlots = 3;
                return true;
            }

            // Y-
            valid = true;
            for (var i = 1; i < requiredFreeAdjacentSlots; i--)
            {
                if (y - i >= 0 && !IsLocationOccupied(x, y - i))
                    continue;

                valid = false;
                break;
            }

            if (valid)
                directionOfSlots = 4;

            return valid;
        }

        internal void PlacePerson(Person person, Vector2Int position)
        {
            _peoplePresent.Add(Tuple.Create(person, position));
        }

        public void ConnectTo(Room otherRoom, bool discovered)
        {
            var connectionTo = new RoomConnection($"{DisplayName} --> {otherRoom.DisplayName}", otherRoom, discovered);
            var connectionFrom = new RoomConnection($"{otherRoom.DisplayName} --> {DisplayName}", this, discovered);

            _connections.Add(connectionTo);
            otherRoom._connections.Add(connectionFrom);
        }
    }
}
