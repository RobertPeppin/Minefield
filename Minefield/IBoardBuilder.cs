using System.Collections.Generic;

namespace Minefield
{
    /// <summary>
    /// Interface to define the operations of a Board Builder.
    /// </summary>
    public interface IBoardBuilder
    {
        /// <summary>
        /// Generate Mine locations for a board and return them in a list
        /// </summary>
        /// <param name="width">How wide is your board</param>
        /// <param name="height">How tall is your board</param>
        /// <param name="numberOfMines">How many mines you would like to generate</param>
        /// <returns>A list of mine locations</returns>
        List<BoardLocation> GenerateBoard(int width, int height, int numberOfMines);
    }
}