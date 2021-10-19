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
        private Random randomGenerator;
        private char baseLocation = 'A';

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
            mines = builder.GenerateBoard(WidthOfBoard, HeightOfBoard, NumberOfMines);
        }

        /// <inheritdoc/>
        public BoardLocation GetStartLocation()
        {
            BoardLocation startLocation = null;

            while (startLocation == null)
            {
                /// generate a random start location - we always start on the left (A)
                var verticalStart = randomGenerator.Next(1, HeightOfBoard);

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
                var nextPosition = GetNextPosition(currentPosition, moveDirection);

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
            switch (moveDirection)
            {
                case Direction.Down:
                    return new BoardLocation()
                    {
                        VerticalPosition = currentPosition.VerticalPosition - 1,
                        HorizontalPosition = currentPosition.HorizontalPosition
                    };

                case Direction.Up:
                    return new BoardLocation()
                    {
                        VerticalPosition = currentPosition.VerticalPosition + 1,
                        HorizontalPosition = currentPosition.HorizontalPosition
                    };

                case Direction.Right:
                    return new BoardLocation()
                    {
                        VerticalPosition = currentPosition.VerticalPosition,
                        HorizontalPosition = currentPosition.HorizontalPosition.GetNextChar()
                    };

                case Direction.Left:
                    return new BoardLocation()
                    {
                        VerticalPosition = currentPosition.VerticalPosition,
                        HorizontalPosition = currentPosition.HorizontalPosition.GetNextChar(-1)
                    };
            }

            return currentPosition;
        }

        private bool ValidateMove(BoardLocation currentPosition, Direction moveDirection)
        {
            // check the sanity of the move
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
            }

            return false;
        }
    }
}