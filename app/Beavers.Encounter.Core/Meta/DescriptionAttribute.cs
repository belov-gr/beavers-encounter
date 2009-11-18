namespace Beavers.Encounter.Core.Meta
{
    /// <summary>
    /// Описание свойства.
    /// </summary>
    public class DescriptionAttribute : EntityAttribute
    {
        public string Description { get; private set; }

        public DescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}
