namespace StudentSystem.Web.Common.Messages
{
    public class CommonMessages
    {
        public static string PropertyRequired(string propertyName) => $"{propertyName} not empty.";
        public static string PropertyNotExist(string propertyName) => $"{propertyName} not found.";
        public static string PropertyIsDuplicate(string propertyName) => $"{propertyName} duplicated.";
        public static string PropertyNoRight(string propertyName) => $"No permission to work with {propertyName}.";
        public static string PropertyWrongFormat(string propertyName, string format) => $"{propertyName} incorrect format. The format should be: {format}.";
        public static string PropertyGreaterThan(string propertyName1, string propertyName2) => $"{propertyName1} should be greater than to {propertyName2}";
        public static string PropertyLessThan(string propertyName1, string propertyName2) => $"{propertyName1} should be less than to {propertyName2}";
        public static string PropertyGreaterThanEqual(string propertyName1, string propertyName2) => $"{propertyName1} should be greater than equal to {propertyName2}";
        public static string PropertyLessThanEqual(string propertyName1, string propertyName2) => $"{propertyName1} should be less than equal to {propertyName2}";
    }
}