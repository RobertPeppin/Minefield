﻿using System;
using System.Collections.Generic;

namespace Minefield
{
    public class Player : IPlayer
    {
        private readonly IGameBoard gameboard;
        private bool winner = false;
        private BoardLocation boardLocation;

        public Player(IGameBoard gameboard, int numberOfLives)
        {
            this.gameboard = gameboard;
            this.gameboard.Boom += Gameboard_Boom;
            this.gameboard.OtherSideReached += Gameboard_OtherSideReached;

            NumberOfLives = numberOfLives;

            CurrentBoardLocation = this.gameboard.GetStartLocation();
        }

        public List<BoardLocation> HistoricPositions { get; } = new List<BoardLocation>();

        public int NumberOfLives { get; private set; }

        public BoardLocation CurrentBoardLocation
        {
            get => boardLocation;
            set
            {
                boardLocation = value;
                HistoricPositions.Add(value);
            }
        }

        public bool CanPlay => NumberOfLives > 0 && !winner;

        private void Gameboard_OtherSideReached(object sender, EventArgs e)
        {
            winner = true;
            Console.WriteLine("WINNER!!!!");
            // write out the results
            GameSummary();
        }

        private void Gameboard_Boom(object sender, BoardLocation e)
        {
            Console.WriteLine("ARGH! a mine");
            Console.Beep();
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
        }
    }
}
