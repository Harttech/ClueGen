using System;

namespace ClueGen.Framework.Models.Actions
{
    public class SeeingAction : PersonActionBase<IPersonAction>
    {
        public SeeingAction(string displayName, IPersonAction target, DateTime timeOfAction, Guid locationId, bool actionSuccessful, params string[] descriptiveElements) : base(displayName, target, PersonActionKind.Saw, timeOfAction, locationId, actionSuccessful, descriptiveElements)
        {
        }
    }
}
