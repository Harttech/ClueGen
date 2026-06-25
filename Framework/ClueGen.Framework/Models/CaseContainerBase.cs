using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ClueGen.Framework.Models
{
    /// <summary>
    /// Interface for objects that contain child objects of type <typeparamref name="TChild"/>.
    /// </summary>
    /// <typeparam name="TChild">The type of the child objects.</typeparam>
    public interface ICaseContainerObject<TChild> : ICaseObject where TChild : ICaseObject
    {
        ReadOnlyCollection<TChild> Children { get; }
        TChild CreateChild(string displayName, params string[] descriptiveElements);
        void RemoveChild(TChild child);
    }

    /// <summary>
    /// Interface for objects that are children of a <see cref="ICaseContainerObject{TChild}"/> object.
    /// </summary>
    /// <typeparam name="TParent">The type of the parent object.</typeparam>
    public interface ICaseContainerChild<out TParent> : ICaseObject
    {
        Guid ParentId { get; }
        TParent Parent { get; }
    }

    /// <summary>
    /// Base class for objects that contain child objects of type <typeparamref name="TChild"/>.
    /// </summary>
    /// <typeparam name="TChild">The type of the child objects.</typeparam>
    public abstract class CaseContainerBase<TChild> : CaseObjectBase, ICaseContainerObject<TChild> where TChild: CaseObjectBase
    {
        protected readonly List<TChild> _children = new List<TChild>();

        protected CaseContainerBase(string displayName, params string[] descriptiveElements) : base(displayName, descriptiveElements)
        {
            Children = new ReadOnlyCollection<TChild>(_children);
        }

        public ReadOnlyCollection<TChild> Children { get; }

        public virtual TChild CreateChild(string displayName, params string[] descriptiveElements)
        {
            var child = (TChild)Activator.CreateInstance(typeof(TChild), displayName, this, descriptiveElements);
            _children.Add(child);
            return child;
        }

        public virtual void RemoveChild(TChild child)
        {
            _children.Remove(child);
        }
    }

    /// <summary>
    /// Base class for objects that are children of a <see cref="CaseContainerBase{TChild}"/> object.
    /// </summary>
    /// <typeparam name="TParent">The type of the parent object.</typeparam>
    public abstract class CaseContainerChildBase<TParent> : CaseObjectBase where TParent: CaseObjectBase
    {
        protected CaseContainerChildBase(string displayName, TParent parent, params string[] descriptiveElements) : base(displayName, descriptiveElements)
        {
            Parent = parent;
            ParentId = parent.Id;
        }

        public Guid ParentId { get; }
        public TParent Parent { get; }
    }

    /// <summary>
    /// Base class for objects that are children of a <see cref="CaseContainerBase{TChild}"/> or another CaseContainerChildBase instance and can have child objects of its own.
    /// </summary>
    /// <typeparam name="TParent">The type of the parent object.</typeparam>
    /// <typeparam name="TChild">The type of the child object.</typeparam>
    public abstract class CaseContainerChildBase<TParent, TChild> : CaseContainerBase<TChild> where TParent: CaseObjectBase where TChild: CaseObjectBase
    {
        protected CaseContainerChildBase(string displayName, TParent parent, params string[] descriptiveElements) : base(displayName, descriptiveElements)
        {
            Parent = parent;
            ParentId = parent.Id;
        }

        public Guid ParentId { get; }
        public TParent Parent { get; }
    }
}
