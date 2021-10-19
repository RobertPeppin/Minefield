using System.Collections.Generic;

namespace Minefield.UnitTests
{
    /// <summary>
    /// Stub board builder
    /// </summary>
    public class BoardBuilderStub : IBoardBuilder
    {
        /// <summary>
        /// Allow a test to set the locations of mines
        /// </summary>
        public List<BoardLocation> LocationsToUse { get; set; }

        /// <inheritdoc/>
        public List<BoardLocation> GenerateBoard(int width, int height, int numberOfMines)
        {
            return LocationsToUse;
        }
    }
}
