namespace Minefield
{
    /// <summary>
    /// Extensions to char type
    /// </summary>
    public static class CharExtensions
    {
        /// <summary>
        /// Given a character work out what the next one is based upon a delta
        /// This could have been done by using a dictionary lookup, or an enum.
        /// I chose to do this so that I didn't have to hard code that data.
        /// </summary>
        /// <param name="current">The current character</param>
        /// <param name="delta">How much to change it by</param>
        /// <returns>The next character to use</returns>
        public static char GetNextChar(this char current, int delta = 1)
        {
            int newValue = current + delta;

            char newChar = (char)newValue;

            if (char.IsLetter(newChar))
            {
                return newChar;
            }

            // Fail to get new value;
            return current;
        }
    }
}
