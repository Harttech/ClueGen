using System;

namespace ClueGen.Framework.Models.People
{
    public class PersonFamilyRelationship : PersonRelationship
    {
        public PersonFamilyRelationship(FamilyRelationshipKind familyKind, PersonRelationshipKind kind, Guid targetOfRelationship) : base(kind, targetOfRelationship)
        {
            FamilyKind = familyKind;
        }

        public FamilyRelationshipKind FamilyKind { get; }
    }
}
