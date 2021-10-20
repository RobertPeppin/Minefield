using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minefield
{
    /// <summary>
    /// Implementation of IPlayer
    /// Manages the life span of a player and the moves they make
    /// </summary>
    public class Player : IPlayer
    {
        private readonly IGameBoard gameboard;
        private bool winner = false;
        private BoardLocation boardLocation;
        private int maxLives = 3;
        private List<BoardLocation> foundMines = new List<BoardLocation>();
        private const char BaseChar = 'A';

        /// <summary>
        /// Create a new player using the gameboard with a number of lives
        /// </summary>
        /// <param name="gameboard">The game board to play on</param>
        public Player(IGameBoard gameboard)
        {
            this.gameboard = gameboard;
            this.gameboard.Boom += Gameboard_Boom;
            this.gameboard.OtherSideReached += Gameboard_OtherSideReached;

            NumberOfLives = maxLives;
        }

        /// <inheritdoc/>
        public List<BoardLocation> HistoricPositions { get; } = new List<BoardLocation>();

        /// <inheritdoc/>
        public int NumberOfLives { get; set; }

        /// <inheritdoc/>
        public BoardLocation CurrentBoardLocation
        {
            get => boardLocation;
            set
            {
                boardLocation = value;
                HistoricPositions.Add(value);
            }
        }

        /// <inheritdoc/>
        public bool CanPlay => NumberOfLives > 0 && !winner;

        /// <inheritdoc/>
        public void Reset()
        {
            NumberOfLives = maxLives;
            HistoricPositions.Clear();
            foundMines.Clear();
        }

        public void GetStartPosition()
        {
            CurrentBoardLocation = gameboard.GetStartLocation();
        }

        /// <inheritdoc/>
        public void SetMaxLives(int maxLives)
        {
            this.maxLives = maxLives;
            NumberOfLives = maxLives;
        }

        private void Gameboard_OtherSideReached(object sender, EventArgs e)
        {
            winner = true;
            Console.WriteLine("WINNER!!!!");
            // write out the results
            GameSummary();
        }

        private void Gameboard_Boom(object sender, BoardLocation e)
        {
            Console.WriteLine($"ARGH! a mine in {e}");
            foundMines.Add(e);
            NumberOfLives--;

            if (NumberOfLives == 0)
            {
                Console.WriteLine("Out of lives");
                GameSummary();
            }
        }

        private void GameSummary()
        {
            Console.WriteLine($"You made {HistoricPositions.Count - 1} moves");

            Console.WriteLine();

            Console.WriteLine("Your board");

            StringBuilder sb = new StringBuilder();

            // max space needed = length of max heigh string
            if (gameboard.HeightOfBoard > 9)
            {
                sb.Append("   ");
            }
            else
            {
                // single digits
                sb.Append("  ");
            }

            for (int left = 0; left < gameboard.WidthOfBoard; left++)
            {
                sb.Append($" {BaseChar.GetNextChar(left)} ");
            }

            Console.WriteLine(sb.ToString());

            for (int up = gameboard.HeightOfBoard; up > 0; up--)
            {
                sb.Clear();

                if (up < 10)
                {
                    sb.Append($" {up}-");
                }
                else
                {
                    sb.Append($"{up}-");
                }

                for (int left = 0; left < gameboard.WidthOfBoard; left++)
                {
                    if (foundMines.Any(l => l.VerticalPosition == up && l.HorizontalPosition == BaseChar.GetNextChar(left)))
                    {
                        sb.Append(" M ");
                    }
                    else
                    {
                        if (HistoricPositions.Any(l => l.VerticalPosition == up && l.HorizontalPosition == BaseChar.GetNextChar(left)))
                        {
                            sb.Append(" > ");
                        }
                        else
                        {
                            sb.Append(" - ");
                        }
                    }
                }
                Console.WriteLine(sb.ToString());
            }

            Console.WriteLine();
        }
    }
}