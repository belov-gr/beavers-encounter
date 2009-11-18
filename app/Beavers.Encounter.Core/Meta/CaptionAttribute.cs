namespace Beavers.Encounter.Core.Meta
{
    /// <summary>
    /// Русское наименование свойства.
    /// </summary>
    public class CaptionAttribute : EntityAttribute
    {
        public string Caption { get; private set; }

        public CaptionAttribute(string caption)
        {
            Caption = caption;
        }
    }
}
