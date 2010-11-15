namespace Beavers.Encounter.Common
{
    public class Breadcrumb
    {
        public string Text { get; set; }
        public string Link { get; set; }
        public int Level { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Breadcrumb)) return false;
            return Equals((Breadcrumb) obj);
        }

        public bool Equals(Breadcrumb other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Link, Link);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Link != null ? Link.GetHashCode() : 0);
            }
        }
    }
}
