using System;
using System.Collections.Generic;
using System.Linq;

namespace Minefield
{
    /// <summary>
    /// Implementation of IGameBoard that manages the state of the board
    /// </summary>
    public class GameBoard : IGameBoard
    {
        private readonly IBoardBuilder builder;
        private List<BoardLocation> mines;
        private readonly Random randomGenerator;
        private readonly char baseLocation = 'A';

        /// <summary>
        /// Create a new instance of the game board
        /// </summary>
        /// <param name="builder">An instance of a board builder</param>
        public GameBoard(IBoardBuilder builder)
        {
            this.builder = builder;
            randomGenerator = new Random(DateTime.Now.Millisecond);
        }

        /// <inheritdoc/>
        public int WidthOfBoard { get; set; }

        /// <inheritdoc/>
        public int HeightOfBoard { get; set; }

        /// <inheritdoc/>
        public int NumberOfMines { get; set; }

        /// <inheritdoc/>
        public event EventHandler<BoardLocation> Boom;

        /// <inheritdoc/>
        public event EventHandler OtherSideReached;

        /// <inheritdoc/>
        public void CreateBoard()
        {
            // create a new set of mines for this board
            mines = builder.GenerateBoard(WidthOfBoard, HeightOfBoard, NumberOfMines);
        }

        /// <inheritdoc/>
        public BoardLocation GetStartLocation()
        {
            BoardLocation startLocation = null;

            while (startLocation == null)
            {
                /// generate a random start location - we always start on the left (A)
                int verticalStart = randomGenerator.Next(1, HeightOfBoard);

                // and make sure it isnt a mine
                if (!mines.Any(m => m.VerticalPosition == verticalStart && m.HorizontalPosition == 'A'))
                {
                    startLocation = new BoardLocation()
                    {
                        HorizontalPosition = 'A',
                        VerticalPosition = verticalStart
                    };
                }
            }

            return startLocation;
        }

        /// <inheritdoc/>
        public BoardLocation Move(BoardLocation currentPosition, Direction moveDirection)
        {
            if (ValidateMove(currentPosition, moveDirection))
            {
                BoardLocation nextPosition = GetNextPosition(currentPosition, moveDirection);

                // check the mines
                if (mines.Any(m => m.VerticalPosition == nextPosition.VerticalPosition && m.HorizontalPosition == nextPosition.HorizontalPosition))
                {
                    // BOOOOM!
                    Boom?.Invoke(this, nextPosition);

                    return currentPosition;
                }

                if (nextPosition.HorizontalPosition == 'A'.GetNextChar(WidthOfBoard - 1))
                {
                    OtherSideReached?.Invoke(this, new EventArgs());
                }

                return nextPosition;
            }
            else
            {
                return currentPosition;
            }
        }

        private BoardLocation GetNextPosition(BoardLocation currentPosition, Direction moveDirection)
        {
            return moveDirection switch
            {
                Direction.Down => new BoardLocation()
                {
                    VerticalPosition = currentPosition.VerticalPosition - 1,
                    HorizontalPosition = currentPosition.HorizontalPosition
                },
                Direction.Up => new BoardLocation()
                {
                    VerticalPosition = currentPosition.VerticalPosition + 1,
                    HorizontalPosition = currentPosition.HorizontalPosition
                },
                Direction.Right => new BoardLocation()
                {
                    VerticalPosition = currentPosition.VerticalPosition,
                    HorizontalPosition = currentPosition.HorizontalPosition.GetNextChar()
                },
                Direction.Left => new BoardLocation()
                {
                    VerticalPosition = currentPosition.VerticalPosition,
                    HorizontalPosition = currentPosition.HorizontalPosition.GetNextChar(-1)
                },
                Direction.None => currentPosition,
                _ => currentPosition,
            };
        }

        private bool ValidateMove(BoardLocation currentPosition, Direction moveDirection)
        {
            // check the sanity of the move (i.e. it doesnt go off the board)
            switch (moveDirection)
            {
                case Direction.Down:
                    if (currentPosition.VerticalPosition > 1)
                    {
                        return true;
                    }
                    break;

                case Direction.Up:
                    if (currentPosition.VerticalPosition < HeightOfBoard)
                    {
                        return true;
                    }
                    break;

                case Direction.Left:
                    if (currentPosition.HorizontalPosition != 'A')
                    {
                        return true;
                    }
                    break;

                case Direction.Right:
                    if (currentPosition.HorizontalPosition != (char)(baseLocation + WidthOfBoard - 1))
                    {
                        return true;
                    }
                    break;
                case Direction.None:
                    break;
                default:
                    break;
            }

            return false;
        }
    }
}