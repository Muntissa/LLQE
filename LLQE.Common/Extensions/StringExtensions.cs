namespace LLQE.Common.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string text, int wordLimit = 8, bool symbol = true)
        {
            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var truncated = string.Join(' ', words.Take(wordLimit));
            if (words.Length > wordLimit)
            {
                return symbol ? truncated + "..." : truncated;
            }
            return truncated;
        }
    }
}
