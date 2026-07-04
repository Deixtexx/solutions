namespace task01
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            input = input.ToLower();
            input = new string(input.Where(c => !Char.IsPunctuation(c) && !Char.IsWhiteSpace(c)).ToArray());
            Console.WriteLine(input);
            return input.Equals(new string(input.Reverse().ToArray()));
        }
    }
}
