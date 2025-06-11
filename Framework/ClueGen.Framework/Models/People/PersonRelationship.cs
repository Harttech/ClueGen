using System;

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
    }
}
