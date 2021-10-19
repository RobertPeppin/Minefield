using System;
namespace Minefield
{
    public static class CharExtensions
    {
        public static char GetNextChar(this char current, int delta = 1)
        {
            var valueOfCurrent = (int)current;

            var newValue = valueOfCurrent + delta;

            var newChar = (char)newValue;

            if (char.IsLetter(newChar))
            {
                return newChar;
            }

            // Fail to get new value;
            return current;
        }
    }
}
