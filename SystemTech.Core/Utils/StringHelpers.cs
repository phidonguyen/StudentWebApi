using System.Text.RegularExpressions;

namespace SystemTech.Core.Utils
{
    public static class StringHelpers
    {
        public static string GetFileName(string filePath) => string.IsNullOrEmpty(filePath) ? filePath : filePath.Split("/").Last();
        
        public static bool SlugHasSpecialChar(this string input)
        {
            Regex regex = new Regex(@"^[a-z\d](?:[a-z\d_-]*[a-z\d])?$");

            return regex.IsMatch(input);
        } 
    }
}