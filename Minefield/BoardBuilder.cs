using System;
using System.Collections.Generic;
using System.Linq;

namespace Minefield
{
    /// <summary>
    /// Implementation of IBoardBuilder that will create a number of mines within the board dimensions
    /// </summary>
    public class BoardBuilder : IBoardBuilder
    {
        private Random randomGenerator;
        private char baseLocation = 'A';

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

            if (width > 26 || height > 26)
            {
                throw new Exception("Board is too big");
            }

            // Create the number of mines required
            for (int mineInstance = 0; mineInstance < numberOfMines; mineInstance++)
            {
                var created = false;

                while (!created)
                {
                    // generate a position
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