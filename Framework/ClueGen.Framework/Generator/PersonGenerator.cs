using System;
using System.Collections.Generic;
using ClueGen.Framework.Models.People;

namespace ClueGen.Framework.Generator
{
    internal class PersonGenerator
    {
        private readonly INumberGenerator _generator;

        public PersonGenerator(INumberGenerator generator)
        {
            _generator = generator;
        }

        public Person GenerateRelativePerson(string firstname, string lastname, Role role, Person relative, FamilyRelationshipKind familyRelationshipKind)
        {
            var newPerson = GeneratePerson(role, firstname, lastname);

            var relationship1 = new PersonFamilyRelationship(GetGenderedFamilyRelationshipKind(familyRelationshipKind, relative.Gender), PersonRelationship.GetRandomRelationshipKind(_generator), newPerson.Id);
            var relationship2 = new PersonFamilyRelationship(GetOpposingFamilyRelationshipKind(familyRelationshipKind, newPerson.Gender), PersonRelationship.GetRandomRelationshipKind(_generator), relative.Id);

            newPerson.SetRelationship(relative.Id, relationship1);
            relative.SetRelationship(newPerson.Id, relationship2);

            return newPerson;
        }

        public Person[] GenerateFamily(int size, string familyName = null)
        {
            if (string.IsNullOrWhiteSpace(familyName))
                familyName = CaseGenerator.LastNames[_generator.GetInt(0, CaseGenerator.LastNames.Length - 1)];

            var family = new Person[size];

            // Before generating the actual people, generate only the relationships and ensure they are valid. This way we can generate the people with the correct relationships in one go instead of having to update them after generating them.
            // Identify "people" simply by index in the family array.
            var relationships = new List<Dictionary<int, FamilyRelationshipKind>>();

            // First generate random relationships
            for (var i = 0; i < size; i++)
                relationships.Add(new Dictionary<int, FamilyRelationshipKind>());

            var maxFamilyRelationshipValue = Enum.GetValues(typeof(FamilyRelationshipKind)).Length - 1;

            for (var i = 0; i < size; i++)
            {
                var relationshipToOthers = relationships[i];

                for (var i2 = i + 1; i2 < family.Length; i2++)
                {
                    var theirRelationshipToOthers = relationships[i2];

                    var relationship = (FamilyRelationshipKind)_generator.GetInt(0, maxFamilyRelationshipValue);
                    var opposingRelationship = GetOpposingFamilyRelationshipKind(relationship, false);

                    relationshipToOthers.Add(i2, relationship);
                    theirRelationshipToOthers.Add(i, opposingRelationship);
                }
            }

            // Then validate the relationships. For example, if person A is the mother of person B and person C is the brother of person B, then person A must be the mother of person C as well.

            // Validate and normalize relationship graph so derived constraints hold (local function kept here for clarity).
            ValidateRelationships(relationships);

            // Does not have to be the most important person of the family. Just a person to build the family around.
            var centralFigure = GeneratePerson(Role.Witness, null, familyName);
            family[0] = centralFigure;

            // Generate the other people and their relationships to the central figure.
            for (var i = 1; i < size; i++)
                family[i] = GenerateRelativePerson(null, familyName, Role.Witness, centralFigure, relationships[i][0]);

            // Finally, generate the relationships between the other family members.
            for (var i = 1; i < family.Length; i++)
            {
                var self = family[i];

                for (var i2 = i + 1; i2 < family.Length; i2++)
                {
                    var other = family[i2];
                    other.SetRelationship(self.Id, new PersonFamilyRelationship(relationships[i2][i], PersonRelationship.GetRandomRelationshipKind(_generator), self.Id));
                    self.SetRelationship(self.Id, new PersonFamilyRelationship(relationships[i][i2], PersonRelationship.GetRandomRelationshipKind(_generator), other.Id));
                }
            }

            return family;
        }
        
        private void ValidateRelationships(List<Dictionary<int, FamilyRelationshipKind>> relationships)
        {
            /*
             * This attempts the following:
             * Connect children with the same parent as siblings.
             * Connect parents with the same child as married. It doesn't matter if the parent is mother and father. Mother and mother and father and father does also work because the gender is still unknown at this point. (Plus progressivity, yay.)
             * If already a married set of parents exists, all other mothers and fathers turn into aunts and uncles.
             * The parent of a parent should become a grandparent for the child-parent's children.
             * The children of one's uncle should become cousins.
             *
             * The mutations go in sequential order. But since a mutation can cause other mutations to be necessary, the process is repeated until no more mutations are necessary.
             */

            var priorities = new Dictionary<FamilyRelationshipKind, int>
            {
                { FamilyRelationshipKind.Mother, 1 },
                { FamilyRelationshipKind.Father, 2 },
                { FamilyRelationshipKind.Grandfather, 3 },
                { FamilyRelationshipKind.Grandmother, 4 },
                { FamilyRelationshipKind.Son, 5 },
                { FamilyRelationshipKind.Daughter, 6 },
                { FamilyRelationshipKind.Uncle, 7 },
                { FamilyRelationshipKind.Aunt, 8 },
                { FamilyRelationshipKind.Nephew, 9 },
                { FamilyRelationshipKind.Niece, 10 },
                { FamilyRelationshipKind.Grandson, 6 },
                { FamilyRelationshipKind.Granddaughter, 6 },
                { FamilyRelationshipKind.Brother, 7 },
                { FamilyRelationshipKind.Sister, 7 },
                { FamilyRelationshipKind.Cousin, 8 },
                { FamilyRelationshipKind.Married, 9 }
            };

            for (var selfIndex = 0; selfIndex < relationships.Count; selfIndex++)
            {
                var selfRelationships = relationships[selfIndex];

                foreach (var (otherIndex, relationship) in selfRelationships)
                {
                    // ToDo
                }
            }
        }

        public Person GeneratePerson(Role role, string firstname = null, string lastname = null)
        {
            var gender = _generator.GetInt(0, 1) == 0;

            if (string.IsNullOrWhiteSpace(firstname))
                firstname = gender
                    ? CaseGenerator.FemaleFirstNames[_generator.GetInt(0, CaseGenerator.FemaleFirstNames.Length - 1)]
                    : CaseGenerator.MaleFirstNames[_generator.GetInt(0, CaseGenerator.MaleFirstNames.Length - 1)];

            if (string.IsNullOrWhiteSpace(lastname))
                lastname = CaseGenerator.LastNames[_generator.GetInt(0, CaseGenerator.LastNames.Length - 1)];

            // Don't generate personality traits on a scale but rather a deviation from the middle.
            var personality = new Personality(
                openness: (byte)(50 + _generator.GetInt(-50, 50)),
                consciountiousness: (byte)(50 + _generator.GetInt(-50, 50)),
                extraversion: (byte)(50 + _generator.GetInt(-50, 50)),
                agreeableness: (byte)(50 + _generator.GetInt(-50, 50)),
                neuroticism: (byte)(50 + _generator.GetInt(-50, 50)));

            var person = new Person(firstname, lastname, gender, personality, role, Guid.NewGuid());
            return person;
        }

        private FamilyRelationshipKind GetOpposingFamilyRelationshipKind(FamilyRelationshipKind familyRelationshipKind, bool gender)
        {
            return familyRelationshipKind switch
            {
                FamilyRelationshipKind.Mother => gender ? FamilyRelationshipKind.Daughter : FamilyRelationshipKind.Son,
                FamilyRelationshipKind.Father => gender ? FamilyRelationshipKind.Daughter : FamilyRelationshipKind.Son,
                FamilyRelationshipKind.Grandfather => gender ? FamilyRelationshipKind.Granddaughter : FamilyRelationshipKind.Grandson,
                FamilyRelationshipKind.Grandmother => gender ? FamilyRelationshipKind.Granddaughter : FamilyRelationshipKind.Grandson,
                FamilyRelationshipKind.Son => gender ? FamilyRelationshipKind.Mother : FamilyRelationshipKind.Father,
                FamilyRelationshipKind.Daughter => gender ? FamilyRelationshipKind.Mother : FamilyRelationshipKind.Father,
                FamilyRelationshipKind.Uncle => gender ? FamilyRelationshipKind.Niece : FamilyRelationshipKind.Nephew,
                FamilyRelationshipKind.Aunt => gender ? FamilyRelationshipKind.Niece : FamilyRelationshipKind.Nephew,
                FamilyRelationshipKind.Nephew => gender ? FamilyRelationshipKind.Aunt : FamilyRelationshipKind.Uncle,
                FamilyRelationshipKind.Niece => gender ? FamilyRelationshipKind.Aunt : FamilyRelationshipKind.Uncle,
                FamilyRelationshipKind.Grandson => gender ? FamilyRelationshipKind.Grandmother : FamilyRelationshipKind.Grandfather,
                FamilyRelationshipKind.Granddaughter => gender ? FamilyRelationshipKind.Grandmother : FamilyRelationshipKind.Grandfather,
                FamilyRelationshipKind.Brother => gender ? FamilyRelationshipKind.Sister : FamilyRelationshipKind.Brother,
                FamilyRelationshipKind.Sister => gender ? FamilyRelationshipKind.Sister : FamilyRelationshipKind.Brother,
                FamilyRelationshipKind.Cousin => FamilyRelationshipKind.Cousin,
                FamilyRelationshipKind.Married => FamilyRelationshipKind.Married
            };
        }

        private FamilyRelationshipKind GetGenderedFamilyRelationshipKind(FamilyRelationshipKind kind, bool gender)
        {
            return kind switch
            {
                FamilyRelationshipKind.Mother => gender ? FamilyRelationshipKind.Mother : FamilyRelationshipKind.Father,
                FamilyRelationshipKind.Father => gender ? FamilyRelationshipKind.Mother : FamilyRelationshipKind.Father,
                FamilyRelationshipKind.Grandfather => gender ? FamilyRelationshipKind.Grandmother : FamilyRelationshipKind.Grandfather,
                FamilyRelationshipKind.Grandmother => gender ? FamilyRelationshipKind.Grandmother : FamilyRelationshipKind.Grandfather,
                FamilyRelationshipKind.Son => gender ? FamilyRelationshipKind.Daughter : FamilyRelationshipKind.Son,
                FamilyRelationshipKind.Daughter => gender ? FamilyRelationshipKind.Daughter : FamilyRelationshipKind.Son,
                FamilyRelationshipKind.Uncle => gender ? FamilyRelationshipKind.Aunt : FamilyRelationshipKind.Uncle,
                FamilyRelationshipKind.Aunt => gender ? FamilyRelationshipKind.Aunt : FamilyRelationshipKind.Uncle,
                FamilyRelationshipKind.Nephew => gender ? FamilyRelationshipKind.Niece : FamilyRelationshipKind.Nephew,
                FamilyRelationshipKind.Niece => gender ? FamilyRelationshipKind.Niece : FamilyRelationshipKind.Nephew,
                FamilyRelationshipKind.Grandson => gender ? FamilyRelationshipKind.Granddaughter : FamilyRelationshipKind.Grandson,
                FamilyRelationshipKind.Granddaughter => gender ? FamilyRelationshipKind.Granddaughter : FamilyRelationshipKind.Grandson,
                FamilyRelationshipKind.Brother => gender ? FamilyRelationshipKind.Sister : FamilyRelationshipKind.Brother,
                FamilyRelationshipKind.Sister => gender ? FamilyRelationshipKind.Sister : FamilyRelationshipKind.Brother,
                FamilyRelationshipKind.Cousin => FamilyRelationshipKind.Cousin,
                FamilyRelationshipKind.Married => FamilyRelationshipKind.Married
            };
        }
    }
}
