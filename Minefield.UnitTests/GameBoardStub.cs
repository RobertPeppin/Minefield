using System;

namespace Minefield.UnitTests
{
    public class GameBoardStub : IGameBoard
    {
        public int WidthOfBoard { get; set; }
        public int HeightOfBoard { get; set; }
        public int NumberOfMines { get; set; }

        public event EventHandler<BoardLocation> Boom;
        public event EventHandler OtherSideReached;

        public void CreateBoard()
        {
            throw new NotImplementedException();
        }

        public BoardLocation GetStartLocation()
        {
            return new BoardLocation() { HorizontalPosition = 'A', VerticalPosition = 4 };
        }

        public BoardLocation Move(BoardLocation currentPosition, Direction moveDirection)
        {
            throw new NotImplementedException();
        }

        public void SetOffMine()
        {
            Boom?.Invoke(this, new BoardLocation() { HorizontalPosition = 'B', VerticalPosition = 1 });
        }

        public void ReachTheOtherSide()
        {
            OtherSideReached?.Invoke(this, new());
        }
    }
}
