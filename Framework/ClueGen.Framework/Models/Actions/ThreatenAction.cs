using System;
using ClueGen.Framework.Models.People;

namespace ClueGen.Framework.Models.Actions
{
    internal class ThreatenAction : PersonActionBase<Person>
    {
        public ThreatenAction(string displayName, Person target, DateTime timeOfAction, Guid locationId, bool actionSuccessful, params string[] descriptiveElements) : base(displayName, target, PersonActionKind.Threatened, timeOfAction, locationId, actionSuccessful, descriptiveElements)
        {
        }
    }
}
