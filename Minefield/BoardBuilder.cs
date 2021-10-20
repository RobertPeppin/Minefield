using System;
using System.Collections.Generic;
using System.Linq;

namespace Minefield
{
    /// <summary>
    /// Implementation of IBoardBuilder that will create a number of mines within the board dimensions
    /// This allows for a board between 1 and 26 high and 1 and 26 wide (so I dont have to worry about
    /// characters beyond Z for now.
    /// This can then be used to tune the game.
    /// </summary>
    public class BoardBuilder : IBoardBuilder
    {
        private const int MaxWidth = 26;
        private const int MaxHeight = 26;
        private readonly Random randomGenerator;

        // This should be a constant somewhere as I have now used it twice
        private readonly char baseLocation = 'A';

        /// <summary>
        /// Create a new instance of the Board builder
        /// </summary>
        public BoardBuilder()
        {
            // Init the RNG
            randomGenerator = new Random(DateTime.Now.Millisecond);
        }

        /// <inheritdoc/>
        public List<BoardLocation> GenerateBoard(int width, int height, int numberOfMines)
        {
            List<BoardLocation> mineLocations = new List<BoardLocation>();

            // Check the request size isnt beyond my limits
            if (width > MaxWidth || height > MaxHeight)
            {
                throw new Exception("Board is too big");
            }

            // Create the number of mines required
            for (int mineInstance = 0; mineInstance < numberOfMines; mineInstance++)
            {
                bool created = false;

                while (!created)
                {
                    // generate a position using the RNG
                    int vertical = randomGenerator.Next(1, height);

                    // we need to base the horizontal from 0 to convert to a char
                    int horizontal = randomGenerator.Next(0, width - 1);

                    char horizontalChar = baseLocation.GetNextChar(horizontal);

                    // check mineLocations to ensure this is unqiue
                    if (!mineLocations.Any(m => m.HorizontalPosition == horizontalChar && m.VerticalPosition == vertical))
                    {
                        mineLocations.Add(new BoardLocation()
                        {
                            VerticalPosition = vertical,
                            HorizontalPosition = horizontalChar
                        });

                        created = true;
                    }
                }
            }

            // send the mines back
            return mineLocations;
        }
    }
}