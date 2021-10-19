using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace Minefield.UnitTests
{
    public class GameBoardTests
    {
        private BoardBuilderStub builder;

        [SetUp]
        public void Setup()
        {
            builder = new BoardBuilderStub();
        }

        [Test]
        [TestCase(8, 8)]
        [TestCase(4, 4)]
        public void SetupBoard(int width, int height)
        {
            // no mines
            builder.LocationsToUse = new List<BoardLocation>();

            GameBoard board = new GameBoard(builder);

            board.HeightOfBoard = height;
            board.WidthOfBoard = width;
            board.NumberOfMines = 0;

            board.CreateBoard();
        }

        [Test]
        [TestCase(8, 8)]
        [TestCase(4, 4)]
        public void MoveAccrossBoard(int width, int height)
        {
            // use to track an event was raised
            ManualResetEvent eventRaised = new ManualResetEvent(false);

            // no mines
            builder.LocationsToUse = new List<BoardLocation>();

            GameBoard board = new GameBoard(builder);

            board.HeightOfBoard = height;
            board.WidthOfBoard = width;
            board.NumberOfMines = 0;
            board.OtherSideReached += (sender, e) => { eventRaised.Set(); };

            board.CreateBoard();

            var location = board.GetStartLocation();

            for (int move = 0; move < width; move++)
            {
                location = board.Move(location, Direction.Right);
            }

            // check the event was raised
            Assert.IsTrue(eventRaised.WaitOne(100));
        }

        [Test]
        public void MoveAcrossBoardAndHitMine()
        {
            // use to track an event was raised
            ManualResetEvent eventRaised = new ManualResetEvent(false);

            // generate mines down the board in the 4th column
            var mines = new List<BoardLocation>();

            for (int index = 1; index <= 8; index++)
            {
                mines.Add(new BoardLocation()
                {
                    HorizontalPosition = 'D',
                    VerticalPosition = index
                });
            }

            builder.LocationsToUse = mines;

            GameBoard board = new GameBoard(builder);

            board.HeightOfBoard = 8;
            board.WidthOfBoard = 8;
            board.NumberOfMines = 8;
            board.Boom += (sender, e) => { eventRaised.Set(); };

            board.CreateBoard();

            var location = board.GetStartLocation();

            // we should be able to make 2 moves and then boom
            location = board.Move(location, Direction.Right);
            Assert.AreEqual('B', location.HorizontalPosition);

            location = board.Move(location, Direction.Right);
            Assert.AreEqual('C', location.HorizontalPosition);
            // this should go bang
            location = board.Move(location, Direction.Right);

            // check the event was raised
            Assert.IsTrue(eventRaised.WaitOne(100));
        }

        [Test]
        public void MoveAroundAndCheckPosition()
        {
            // no mines
            builder.LocationsToUse = new List<BoardLocation>();

            GameBoard board = new GameBoard(builder);

            board.HeightOfBoard = 8;
            board.WidthOfBoard = 8;
            board.NumberOfMines = 0;

            board.CreateBoard();

            var location = new BoardLocation()
            {
                HorizontalPosition = 'A',
                VerticalPosition = 8
            };

            // go right, down, left and up
            location = board.Move(location, Direction.Right);
            Assert.AreEqual('B', location.HorizontalPosition);
            Assert.AreEqual(8, location.VerticalPosition);
            Assert.AreEqual("B:8", location.ToString());

            location = board.Move(location, Direction.Down);
            Assert.AreEqual('B', location.HorizontalPosition);
            Assert.AreEqual(7, location.VerticalPosition);
            Assert.AreEqual("B:7", location.ToString());

            location = board.Move(location, Direction.Left);
            Assert.AreEqual('A', location.HorizontalPosition);
            Assert.AreEqual(7, location.VerticalPosition);
            Assert.AreEqual("A:7", location.ToString());

            location = board.Move(location, Direction.Up);
            Assert.AreEqual('A', location.HorizontalPosition);
            Assert.AreEqual(8, location.VerticalPosition);
            Assert.AreEqual("A:8", location.ToString());
        }

        [Test]
        public void MoveAndAttemptToMoveOffBoard()
        {
            // no mines
            builder.LocationsToUse = new List<BoardLocation>();

            GameBoard board = new GameBoard(builder);

            board.HeightOfBoard = 8;
            board.WidthOfBoard = 8;
            board.NumberOfMines = 0;

            board.CreateBoard();

            var location = new BoardLocation()
            {
                HorizontalPosition = 'A',
                VerticalPosition = 8
            };

            // go right, down, left and up
            location = board.Move(location, Direction.Left);
            // this should not move
            Assert.AreEqual('A', location.HorizontalPosition);
            Assert.AreEqual(8, location.VerticalPosition);
            Assert.AreEqual("A:8", location.ToString());

            location = board.Move(location, Direction.Up);
            Assert.AreEqual('A', location.HorizontalPosition);
            Assert.AreEqual(8, location.VerticalPosition);
            Assert.AreEqual("A:8", location.ToString());

            // try the bottom right now
            location = new BoardLocation()
            {
                HorizontalPosition = 'H',
                VerticalPosition = 1
            };


            location = board.Move(location, Direction.Right);
            Assert.AreEqual('H', location.HorizontalPosition);
            Assert.AreEqual(1, location.VerticalPosition);
            Assert.AreEqual("H:1", location.ToString());

            location = board.Move(location, Direction.Down);
            Assert.AreEqual('H', location.HorizontalPosition);
            Assert.AreEqual(1, location.VerticalPosition);
            Assert.AreEqual("H:1", location.ToString());
        }
    }
}