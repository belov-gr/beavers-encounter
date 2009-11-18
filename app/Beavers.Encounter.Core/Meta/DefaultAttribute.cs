namespace Beavers.Encounter.Core.Meta
{
    /// <summary>
    /// Значение свойства по умолчанию.
    /// </summary>
    public class DefaultAttribute : EntityAttribute
    {
        public object Value { get; private set; }

        public DefaultAttribute(object value)
        {
            Value = value;
        }
    }
}
