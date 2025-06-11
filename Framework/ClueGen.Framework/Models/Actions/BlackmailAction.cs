using System;
using ClueGen.Framework.Models.People;

namespace ClueGen.Framework.Models.Actions
{
    internal class BlackmailAction : PersonActionBase<Person>
    {
        public BlackmailAction(string displayName, Person target, DateTime timeOfAction, Guid locationId, bool actionSuccessful, params string[] descriptiveElements) : base(displayName, target, PersonActionKind.Blackmailed, timeOfAction, locationId, actionSuccessful, descriptiveElements)
        {
        }
    }
}
