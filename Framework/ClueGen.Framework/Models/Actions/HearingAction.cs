using System;

namespace ClueGen.Framework.Models.Actions
{
    internal class HearingAction : PersonActionBase<IPersonAction>
    {
        public HearingAction(string displayName, IPersonAction target, DateTime timeOfAction, Guid locationId, bool actionSuccessful, params string[] descriptiveElements) : base(displayName, target, PersonActionKind.Heard, timeOfAction, locationId, actionSuccessful, descriptiveElements)
        {
        }
    }
}
