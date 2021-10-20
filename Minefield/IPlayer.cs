using System.Collections.Generic;

namespace Minefield
{
    /// <summary>
    /// The operations a player must implement
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Gets a list of all the positions the user has tried
        /// </summary>
        List<BoardLocation> HistoricPositions { get; }

        /// <summary>
        /// Gets or Sets the number of lives the current user has left
        /// </summary>
        int NumberOfLives { get; set; }

        /// <summary>
        /// Gets or Sets the players current position on the board
        /// </summary>
        BoardLocation CurrentBoardLocation { get; set; }

        /// <summary>
        /// Gets whether the player is currenty able to still play
        /// </summary>
        bool CanPlay { get; }

        /// <summary>
        /// Resets this user ready for a new game
        /// </summary>
        void Reset();

        /// <summary>
        /// Sets the maximum number of lives a player has
        /// </summary>
        /// <param name="maxNumberOfLives">Number of lives a player has at the beginning of the game</param>
        void SetMaxLives(int maxNumberOfLives);

        /// <summary>
        /// The player asks the board for a starting position that isnt a mine
        /// </summary>
        void GetStartPosition();
    }
}
