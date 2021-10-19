using System;

namespace Minefield
{
    /// <summary>
    /// Defines a board that can be used to play on
    /// </summary>
    public interface IGameBoard
    {
        /// <summary>
        /// Event to indicate that the player just landed on a mine
        /// </summary>
        event EventHandler<BoardLocation> Boom;

        /// <summary>
        /// Event to tell the game that the player reached the other side
        /// </summary>
        event EventHandler OtherSideReached;

        /// <summary>
        /// Gets or Sets how wide this board is
        /// </summary>
        int WidthOfBoard { get; set; }

        /// <summary>
        /// Gets or Sets how tall this board is
        /// </summary>
        int HeightOfBoard { get; set; }

        /// <summary>
        /// Gets or Sets how many mines are on the board
        /// </summary>
        int NumberOfMines { get; set; }

        /// <summary>
        /// Create a new game board
        /// </summary>
        void CreateBoard();

        /// <summary>
        /// Move the player in a particular position
        /// </summary>
        /// <param name="currentPosition">Where the player is now</param>
        /// <param name="moveDirection">Which direction does the player want to move in</param>
        /// <returns>Where the player is after the move</returns>
        BoardLocation Move(BoardLocation currentPosition, Direction moveDirection);

        /// <summary>
        /// Once the board has been built, ask it where to start
        /// </summary>
        /// <returns>The start location</returns>
        BoardLocation GetStartLocation();
    }
}