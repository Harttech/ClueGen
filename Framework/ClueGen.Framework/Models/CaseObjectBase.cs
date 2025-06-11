using System;

namespace ClueGen.Framework.Models
{
    public abstract class CaseObjectBase : ICaseObject, ITextContent
    {
        protected CaseObjectBase(string displayName, params string[] descriptiveElements)
        {
            DisplayName = displayName;
            DescriptiveElements = descriptiveElements;
        }

        public Guid Id { get; } = Guid.NewGuid();
        public string DisplayName { get; }
        public string[] DescriptiveElements { get; }
    }
}
