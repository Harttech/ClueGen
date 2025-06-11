using System;

namespace ClueGen.Framework.Models.Actions
{
    internal class SearchForAction : PersonActionBase<IPersonAction>
    {
        public SearchForAction(string displayName, IPersonAction target, DateTime timeOfAction, Guid locationId, bool actionSuccessful, params string[] descriptiveElements) : base(displayName, target, PersonActionKind.SearchedFor, timeOfAction, locationId, actionSuccessful, descriptiveElements)
        {
        }
    }
}
