using System;
namespace Minefield.UnitTests
{
    public class GameBoardStub : IGameBoard
    {
        public int WidthOfBoard { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int HeightOfBoard { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int NumberOfMines { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
    }
}
