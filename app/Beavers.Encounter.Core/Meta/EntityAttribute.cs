using System;

namespace Beavers.Encounter.Core.Meta
{
    /// <summary>
    /// Базовый класс для атрибутов мета-описаний.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class EntityAttribute : Attribute
    {
    }
}
