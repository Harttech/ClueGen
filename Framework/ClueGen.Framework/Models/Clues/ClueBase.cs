using System;

namespace ClueGen.Framework.Models.Clues
{
    public abstract class ClueBase : CaseObjectBase
    {
        protected ClueBase(string displayName, ClueKind kind, params string[] descriptiveElements) : base(displayName, descriptiveElements)
        {
            Kind = kind;
        }

        public ClueKind Kind { get; }
    }

    public abstract class ClueBase<T> : ClueBase where T: ICaseObject
    {
        protected ClueBase(string displayName, ClueKind kind, T clueHolder, params string[] descriptiveElements) : base(displayName, kind, descriptiveElements)
        {
            ClueHolder = clueHolder;
        }

        public T ClueHolder { get; }
    }
}
