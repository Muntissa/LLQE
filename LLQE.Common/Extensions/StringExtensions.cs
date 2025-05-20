namespace LLQE.Common.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string text, int wordLimit = 8)
        {
            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return string.Join(' ', words.Take(wordLimit)) + (words.Length > wordLimit ? "..." : "");
        }
    }
}
