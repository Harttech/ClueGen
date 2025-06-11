using System;

namespace ClueGen.Framework.Models.Actions
{
    public interface IPersonAction : ICaseObject
    {
        Guid TargetOfActionId { get; }
        PersonActionKind Kind { get; }
        bool ActionSuccessful { get; }
        DateTime TimeOfAction { get; }
        Guid LocationId { get; }
    }

    public interface IPersonAction<out TTarget> : IPersonAction where TTarget: ICaseObject
    {
        TTarget TargetOfAction { get; }
    }
}
