using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ClueGen.Framework.Models
{
    public interface ICaseContainerObject<TChild> where TChild : CaseObjectBase
    {
        ReadOnlyCollection<TChild> Children { get; }
    }

    public interface ICaseContainerChild<out TParent> where TParent : CaseObjectBase
    {
        Guid ParentId { get; }
        TParent Parent { get; }
    }

    public abstract class CaseContainerBase<TChild> : CaseObjectBase, ICaseContainerObject<TChild>
    {
        protected readonly List<TChild> _children = new List<TChild>();

        protected CaseContainerBase(string displayName, params string[] descriptiveElements) : base(displayName, descriptiveElements)
        {
            Children = new ReadOnlyCollection<TChild>(_children);
        }

        public ReadOnlyCollection<TChild> Children { get; }
    }

    public abstract class CaseContainerChildBase<TParent> : CaseObjectBase, ICaseContainerChild<TParent> where TParent : CaseObjectBase
    {
        protected CaseContainerChildBase(string displayName, TParent parent, params string[] descriptiveElements) : base(displayName, descriptiveElements)
        {
            Parent = parent;
            ParentId = parent.Id;
        }

        public Guid ParentId { get; }
        public TParent Parent { get; }
    }

    public abstract class CaseContainerChildContainerBase<TParent, TChild> : CaseObjectBase, ICaseContainerObject<TChild>, ICaseContainerChild<TParent> where TParent : CaseObjectBase where TChild : CaseObjectBase
    {
        private protected List<TChild> _children = new List<TChild>();

        protected CaseContainerChildContainerBase(string displayName, TParent parent, params string[] descriptiveElements) : base(displayName, descriptiveElements)
        {
            Children = new ReadOnlyCollection<TChild>(_children);
            Parent = parent;
            ParentId = parent.Id;
        }

        public ReadOnlyCollection<TChild> Children { get; }
        public Guid ParentId { get; }
        public TParent Parent { get; }
    }
}
