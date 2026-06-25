using System;
using System.Collections.Generic;
using ClueGen.Framework.Generator;

namespace ClueGen.Framework.Models.People
{
    public class PersonRelationship
    {
        public PersonRelationship(PersonRelationshipKind kind, Guid targetOfRelationship)
        {
            Kind = kind;
            TargetOfRelationship = targetOfRelationship;
        }

        public PersonRelationshipKind Kind { get; }
        public Guid TargetOfRelationship { get; }

        public static PersonRelationship GetRandomRelationship(Guid targetOfRelationship, INumberGenerator generator) => new PersonRelationship(GetRandomRelationshipKind(generator), targetOfRelationship);

        public static PersonRelationshipKind GetRandomRelationshipKind(INumberGenerator generator)
        {
            var rand = generator.GetInt(1, 100);

            // Select relationship based on weight. For now, just use hardcoded weights.

            /*
             * Loves: 5%
             * Hates: 5%
             * Likes: 20%
             * Dislikes: 20%
             * IsAcquainted: 50%
             */

            if (rand <= 5)
                return generator.GetInt(0, 1) == 0 ? PersonRelationshipKind.Loves : PersonRelationshipKind.Hates;

            if (rand <= 20)
                return generator.GetInt(0, 1) == 0 ? PersonRelationshipKind.Likes : PersonRelationshipKind.Dislikes;

            return PersonRelationshipKind.IsAcquainted;
        }
    }
}
