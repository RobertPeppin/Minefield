using System;
using System.Threading.Tasks;

namespace Minefield
{
    public class Game
    {
        public Game()
        {
        }

        /// <summary>
        /// Gets the current board being played
        /// </summary>
        public IGameBoard CurrentBoard { get; private set; }

        /// <summary>
        /// Gets the current player playing
        /// </summary>
        public IPlayer CurrentPlayer { get; private set; }

        /// <summary>
        /// Starts a game and manages it to completion
        /// </summary>
        public void StartGame()
        {
            Console.WriteLine("Starting game");

            // Create a board
            CurrentBoard = new GameBoard(new BoardBuilder());

            Console.WriteLine("Creating board");
            CurrentBoard.HeightOfBoard = 8;
            CurrentBoard.WidthOfBoard = 8;
            CurrentBoard.NumberOfMines = 16;

            CurrentBoard.CreateBoard();
            Console.WriteLine("Mines shuffled and board built");

            // Create a player
            CurrentPlayer = new Player(CurrentBoard, numberOfLives: 3);

            Console.WriteLine("Your move, Punk!");

            while (CurrentPlayer.CanPlay)
            {
                // Get an input
                Console.WriteLine($"You are in square {CurrentPlayer.CurrentBoardLocation} and you have {CurrentPlayer.NumberOfLives} lives left");
                Console.WriteLine("Where next? U,D,L or R");
                var direction = Console.ReadKey(true);

                var keyPressed = (char)direction.Key;
                var moveDirection = Direction.None;
                switch (keyPressed)
                {
                    case 'U':
                        moveDirection = Direction.Up;
                        break;
                    case 'D':
                        moveDirection = Direction.Down;
                        break;
                    case 'L':
                        moveDirection = Direction.Left;
                        break;
                    case 'R':
                        moveDirection = Direction.Right;
                        break;
                }

                if (moveDirection == Direction.None)
                {
                    Console.WriteLine("Invalid move!");
                }
                else
                {
                    CurrentPlayer.CurrentBoardLocation = CurrentBoard.Move(CurrentPlayer.CurrentBoardLocation, moveDirection);
                }
            }

            Console.WriteLine("Game over...");

        }
    }
}
