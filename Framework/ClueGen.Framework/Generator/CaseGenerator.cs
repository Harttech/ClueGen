using System;
using System.Collections.Generic;
using System.Linq;
using ClueGen.Framework.Models;
using ClueGen.Framework.Models.Actions;
using ClueGen.Framework.Models.Clues;
using ClueGen.Framework.Models.Environment;
using ClueGen.Framework.Models.People;
using ClueGen.Framework.Properties;

namespace ClueGen.Framework.Generator
{
    public static class CaseGenerator
    {
        internal static string[] MaleFirstNames { get; } = Resources.MaleFirstNames.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Skip(4).ToArray();
        internal static string[] FemaleFirstNames { get; } = Resources.FemaleFirstNames.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Skip(4).ToArray();
        internal static string[] LastNames { get; } = Resources.LastNames.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();

        public static Case GenerateNewCase(CaseOptions options)
        {
            var gen = options.NumberGenerator;
            var personGenerator = new PersonGenerator(options.NumberGenerator);

            var people = new List<Person>();

            var player = personGenerator.GeneratePerson(Role.Player);
            var victim = personGenerator.GeneratePerson(Role.Victim);
            victim.CurrentMood = Mood.Dead;
            var culprit = personGenerator.GeneratePerson(Role.Culprit);

            people.Add(player);
            people.Add(victim);
            people.Add(culprit);

            // ToDo: Generate accomplice

            var peopleToGenerate = options.PeopleToGenerate;

            GeneratePeople(gen, personGenerator, people, peopleToGenerate);
            if (options.PresetPeople != null)
                people.AddRange(options.PresetPeople);

            SetRandomRelationships(people, gen);

            var clues = new List<ClueBase>();

            // Creating an environment procedurally is a bit more complicated, so for now, just use the preset environment.
            // Pick a random room from the preset environment and set it as the crime scene.
            var crimeScene = options.PresetEnvironment.Children[gen.GetInt(0, options.PresetEnvironment.Children.Count - 1)];

            var killAction = new KillAction($"{culprit.DisplayName} {GeneratorStrings.Killed} {victim.DisplayName}", victim, DateTime.Today.AddHours(12), crimeScene.Id, true);
            culprit.SetAction(victim.Id, killAction);

            // Generate random weapon type first as this affects the position of the culprit. (Ex. for poison they don't have to be in the same room, for a gun they can stand a bit further away but for a knife they need to be close up).
            var weaponType = (WeaponKind)gen.GetInt(0, Enum.GetValues(typeof(WeaponKind)).Cast<int>().Max());

            // Place victim and blood evidence based on weapon type
            SetCrimescene(weaponType, crimeScene, gen, victim, clues);

            // Place people in the rest of the rooms
            PlacePeopleInRooms(options, crimeScene, people, gen);

            // Create actions for people in adjacent rooms
            foreach (var crimeSceneConnection in crimeScene.Connections)
            {
                var peopleInRoom = crimeSceneConnection.ConnectsTo.PeoplePresent;
                foreach (var (person, _) in peopleInRoom)
                {
                    var action = new HearingAction($"{GeneratorStrings.Hearing} {victim.DisplayName} {GeneratorStrings.Scream}",
                        killAction, killAction.TimeOfAction, crimeSceneConnection.ConnectsTo.Id, true,
                        GeneratorStrings.Hearing, GeneratorStrings.Scream, victim.DisplayName);

                    person.SetAction(victim.Id, action);
                }
            }

            var newCase = new Case(options.CaseName, people, options.PresetEnvironment, clues);
            return newCase;
        }

        private static void PlacePeopleInRooms(CaseOptions options, Room crimeScene, List<Person> people, INumberGenerator gen)
        {
            var validRooms = new List<Room>();
            validRooms.AddRange(options.PresetEnvironment.Children);
            validRooms.Remove(crimeScene);

            foreach (var person in people)
            {
                if (person.Role != Role.Witness && person.Role != Role.Culprit)
                    continue;

                Room rdmRoom = null;
                Vector2Int? spot = null;

                while (!spot.HasValue) // Potential deadlock if there literally are no spaces available. Unlikely, though. Still, ToDo: find an actual failsafe in the future.
                {
                    rdmRoom = validRooms[gen.GetInt(0, validRooms.Count - 1)];
                    spot = rdmRoom.GetRandomUnoccupiedslot(gen, 1, out _);
                }

                rdmRoom!.PlacePerson(person, spot.Value);
            }
        }

        private static void SetCrimescene(WeaponKind weaponType, Room crimeScene, INumberGenerator gen, Person victim, in List<ClueBase> clues)
        {
            var requiredAdjacentSlots = 0;
            var createBloodClue = false;
            switch (weaponType)
            {
                case WeaponKind.Blunt:
                    requiredAdjacentSlots = 1;
                    createBloodClue = true;
                    break;
                case WeaponKind.Poison:
                    requiredAdjacentSlots = 0;
                    break;
                case WeaponKind.Sharp:
                    requiredAdjacentSlots = 1;
                    createBloodClue = true;
                    break;
                case WeaponKind.Ballistic:
                    requiredAdjacentSlots = 3;
                    createBloodClue = true;
                    break;
                case WeaponKind.Strangulation:
                    requiredAdjacentSlots = 1;
                    break;
                case WeaponKind.Pointy:
                    requiredAdjacentSlots = 1;
                    createBloodClue = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var victimPosition = crimeScene.GetRandomUnoccupiedslot(gen, requiredAdjacentSlots, out var directionOfSlots);

            if (victimPosition == null || directionOfSlots == -1)
                throw new InvalidOperationException("Couldn't find a valid spot for the victim's location."); // ToDo: Maybe make that more sophisticated later with a failsafe that tries to find a new room or something.

            var culpritLocation = directionOfSlots switch
            {
                // Right
                0 => new Vector2Int(victimPosition.Value.X + 1, victimPosition.Value.Y),
                // Left
                1 => new Vector2Int(victimPosition.Value.X - 1, victimPosition.Value.Y),
                // Up
                2 => new Vector2Int(victimPosition.Value.X, victimPosition.Value.Y + 1),
                // Down
                3 => new Vector2Int(victimPosition.Value.X, victimPosition.Value.Y - 1),
                _ => throw new ArgumentOutOfRangeException(),
            };

            if (createBloodClue)
            {

                if (weaponType == WeaponKind.Ballistic)
                {
                    // Blood is instead on the first slot behind the victim to indicate the direction of the shot.
                    // If there is a POI behind the victim, put the blood on that.
                    // If there is a wall behind the victim, move the victim one space away and put a "POI" on that spot that acts as a bullet hole on the wall.
                    var bloodLocation = directionOfSlots switch
                    {
                        // Right
                        0 => new Vector2Int(victimPosition.Value.X - 2, victimPosition.Value.Y),
                        // Left
                        1 => new Vector2Int(victimPosition.Value.X + 2, victimPosition.Value.Y),
                        // Up
                        2 => new Vector2Int(victimPosition.Value.X, victimPosition.Value.Y - 2),
                        // Down
                        3 => new Vector2Int(victimPosition.Value.X, victimPosition.Value.Y + 2),
                        _ => throw new ArgumentOutOfRangeException(),
                    };

                    PointOfInterest poi;
                    if (bloodLocation.X < 0 || bloodLocation.Y < 0)
                    {
                        bloodLocation = victimPosition.Value;
                        // Shift culprit one slot back
                        culpritLocation = directionOfSlots switch
                        {
                            // Right
                            0 => new Vector2Int(culpritLocation.X + 1, culpritLocation.Y),
                            // Left
                            1 => new Vector2Int(culpritLocation.X - 1, culpritLocation.Y),
                            // Up
                            2 => new Vector2Int(culpritLocation.X, culpritLocation.Y + 1),
                            // Down
                            3 => new Vector2Int(culpritLocation.X, culpritLocation.Y - 1),
                            _ => throw new ArgumentOutOfRangeException(),
                        };

                        // Create bullet hole in wall
                        poi = crimeScene.GetPointOfInterestAtLocation(bloodLocation)
                              ?? crimeScene.CreateChild(GeneratorStrings.Bullethole, victimPosition.Value.X, victimPosition.Value.Y, GeneratorStrings.Bullethole, GeneratorStrings.InTheWall);

                        // Shift victim one slot back (because a slot can't be both occupied by a person and a POI)
                        victimPosition = directionOfSlots switch
                        {
                            // Right
                            0 => new Vector2Int(victimPosition.Value.X + 1, victimPosition.Value.Y),
                            // Left
                            1 => new Vector2Int(victimPosition.Value.X - 1, victimPosition.Value.Y),
                            // Up
                            2 => new Vector2Int(victimPosition.Value.X, victimPosition.Value.Y + 1),
                            // Down
                            3 => new Vector2Int(victimPosition.Value.X, victimPosition.Value.Y - 1),
                            _ => throw new ArgumentOutOfRangeException(),
                        };
                    }
                    else
                    {
                        poi = crimeScene.GetPointOfInterestAtLocation(bloodLocation)
                              ?? crimeScene.CreateChild(GeneratorStrings.PoolOfBlood, culpritLocation.X, culpritLocation.Y, GeneratorStrings.Blood, GeneratorStrings.OnTheGround);

                    }

                    var bloodClue = new GenericClue<PointOfInterest>(GeneratorStrings.Blood, poi, GeneratorStrings.VictimsBlood);
                    var bulletholeClue = new GenericClue<PointOfInterest>(GeneratorStrings.Bullethole, poi, GeneratorStrings.Bullethole);

                    poi.AddClue(bloodClue);
                    poi.AddClue(bulletholeClue);
                    
                    clues.Add(bloodClue);
                    clues.Add(bulletholeClue);
                }
                else
                {
                    // Create pool of blood POI to hold the blood clue where the culprit was standing.
                    var poi = crimeScene.CreateChild(GeneratorStrings.PoolOfBlood, culpritLocation.X, culpritLocation.Y, GeneratorStrings.Blood, GeneratorStrings.OnTheGround);
                    var bloodClue = new GenericClue<PointOfInterest>(GeneratorStrings.Blood, poi, GeneratorStrings.VictimsBlood);

                    poi.AddClue(bloodClue);
                    clues.Add(bloodClue);
                }

                crimeScene.PlacePerson(victim, victimPosition.Value);
            }
        }

        private static void GeneratePeople(INumberGenerator gen, PersonGenerator personGenerator, List<Person> people, int peopleToGenerate)
        {
            // For now, only allow two families at max to not overcomplicate things.
            // Potentially make these values configurable later.
            const int maxFamilies = 2;
            const int familySize = 3;
            const int chanceForFamily = 10;

            // Begin by generating families.
            for (var i = 0; i < maxFamilies; i++)
            {
                if (gen.GetInt(0, 100) > chanceForFamily)
                    continue;

                var actualFamilySize = gen.GetInt(1, familySize);
                var family = personGenerator.GenerateFamily(5);
                people.AddRange(family);
                peopleToGenerate -= actualFamilySize;
            }

            // If there's still space, generate random witnesses until we hit the cap.
            for (var i = 0; i < peopleToGenerate; i++)
            {
                var p = personGenerator.GeneratePerson(Role.Witness);
                people.Add(p);
            }
        }

        private static void SetRandomRelationships(List<Person> people, INumberGenerator gen)
        {
            // Set random relationships for the witnesses. They don't matter too much, as they're just flavor.
            // To avoid multiple relationships, loop through each person but only set relationships with people that come after them in the list.

            for (var i = 1; i < people.Count; i++) // Start at 1. The player doesn't have any relationships.
            {
                var person = people[i];

                for (var i2 = i + 1; i2 < people.Count; i2++)
                {
                    var target = people[i2];

                    if (person.IsRelatedTo(target.Id) != null)
                        continue; // They already have a relationship, so skip it.

                    person.SetRelationship(target.Id, PersonRelationship.GetRandomRelationship(target.Id, gen));
                    target.SetRelationship(person.Id, PersonRelationship.GetRandomRelationship(person.Id, gen));
                }
            }
        }
    }
}
