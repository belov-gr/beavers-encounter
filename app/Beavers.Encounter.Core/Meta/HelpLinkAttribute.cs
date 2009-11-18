namespace Beavers.Encounter.Core.Meta
{
    /// <summary>
    /// Ссылка на раздел справки.
    /// </summary>
    public class HelpLinkAttribute : EntityAttribute
    {
        public string HelpLink { get; private set; }

        public HelpLinkAttribute(string helpLink)
        {
            HelpLink = helpLink;
        }
    }
}
