namespace SystemTech.Core.Constant
{
    public static class Criteria
    {
        public const string FieldNameDef = "Latest";
        // Giá trị của các fields này phải đúng với tên fields của CriteriaAlias bên dưới
        // Declare các constant này để validation thôi
        public const string DefaultLatest = "Latest";
        public const string DefaultEarliest = "Earliest";
        public const string DefaultRecentlyChange = "RecentlyChange";
        public const string DefaultPriority = "Priority";
    }
    
    public class DefaultCriteriaAlias
    {
        public const string Latest = "-CreatedDate";
        public const string Earliest = "CreatedDate";
        public const string RecentlyChange = "-UpdatedDate";
        public const string Priority = "-Order";
    }
}