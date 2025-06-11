using System;

namespace ClueGen.Framework.Models
{
    public interface ICaseObject
    {
        Guid Id { get; }
        string DisplayName { get; }
    }
}
