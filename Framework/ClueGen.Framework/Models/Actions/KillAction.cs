using System;
using ClueGen.Framework.Models.People;

namespace ClueGen.Framework.Models.Actions
{
    internal class KillAction : PersonActionBase<Person>
    {
        public KillAction(string displayName, Person target, DateTime timeOfAction, Guid locationId, bool actionSuccessful, params string[] descriptiveElements) : base(displayName, target, PersonActionKind.Killed, timeOfAction, locationId, actionSuccessful, descriptiveElements)
        {
        }
    }
}
