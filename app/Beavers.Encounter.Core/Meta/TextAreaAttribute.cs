namespace Beavers.Encounter.Core.Meta
{
    /// <summary>
    /// Многострочное свойство с поддержкой BBCode.
    /// </summary>
    public class TextAreaAttribute : EntityAttribute
    {
        public int Cols { get; private set; }
        public int Rows { get; private set; }

        public TextAreaAttribute(int cols, int rows)
        {
            Cols = cols;
            Rows = rows;
        }
    }
}
