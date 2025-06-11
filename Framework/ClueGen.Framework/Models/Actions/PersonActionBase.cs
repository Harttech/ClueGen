using System;

namespace ClueGen.Framework.Models.Actions
{
    public abstract class PersonActionBase<TTarget> : CaseObjectBase, IPersonAction<TTarget> where TTarget : ICaseObject
    {
        protected PersonActionBase(string displayName, TTarget target, PersonActionKind kind, DateTime timeOfAction, Guid locationId, bool actionSuccessful, params string[] descriptiveElements) : base(displayName, descriptiveElements)
        {
            TargetOfAction = target;
            TargetOfActionId = target.Id;
            LocationId = locationId;
            Kind = kind;
            TimeOfAction = timeOfAction;
            ActionSuccessful = actionSuccessful;
        }

        public Guid TargetOfActionId { get; }
        public TTarget TargetOfAction { get; }
        public PersonActionKind Kind { get; }
        public bool ActionSuccessful { get; }
        public DateTime TimeOfAction { get; }
        public Guid LocationId { get; }
    }
}
