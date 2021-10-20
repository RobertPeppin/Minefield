using System;
using DryIoc;

namespace Minefield
{
    public class Game
    {
        /// <summary>
        /// Starts a game and manages it to completion
        /// </summary>
        /// <param name="width">Width of board required - default 8</param>
        /// <param name="height">Height of board required - default 8</param>
        /// <param name="mines">Number of mines to use - default 16</param>
        /// <param name="numberOfLives">Number of lives a user starts with - default 3</param>
        public void StartGame(int width = 8, int height = 8, int mines = 16, int numberOfLives = 3)
        {
            bool playing = true;

            Console.WriteLine("Starting game");

            // Create a board
            var currentBoard = Container.Current.Resolve<IGameBoard>();
            var currentPlayer = Container.Current.Resolve<IPlayer>();
            currentPlayer.SetMaxLives(numberOfLives);

            while (playing)
            {
                Console.Clear();
                Console.WriteLine("Creating board ...");
                currentBoard.HeightOfBoard = height;
                currentBoard.WidthOfBoard = width;
                currentBoard.NumberOfMines = mines;

                currentBoard.CreateBoard();
                Console.WriteLine("Mines shuffled and board built.");
                currentPlayer.GetStartPosition();

                Console.WriteLine("Your move, Punk!");

                while (currentPlayer.CanPlay)
                {
                    // Get an input
                    Console.WriteLine($"You are in square {currentPlayer.CurrentBoardLocation} and you have {currentPlayer.NumberOfLives} lives left");
                    Console.WriteLine("Where next? U,D,L or R");

                    ConsoleKeyInfo direction = Console.ReadKey(true);
                    Console.Clear();

                    char keyPressed = (char)direction.Key;
                    Direction moveDirection = Direction.None;
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
                        default:
                            break;
                    }

                    if (moveDirection == Direction.None)
                    {
                        Console.WriteLine("Invalid move!");
                    }
                    else
                    {
                        currentPlayer.CurrentBoardLocation = currentBoard.Move(currentPlayer.CurrentBoardLocation, moveDirection);
                    }
                }

                Console.WriteLine("Would you like to play again? Y/N");
                ConsoleKeyInfo playAgain = Console.ReadKey(true);

                if (playAgain.Key == ConsoleKey.N)
                {
                    playing = false;
                }
                else
                {
                    currentPlayer.Reset();
                }
            }

            Console.WriteLine("Game over...");
        }
    }
}