using System.Collections.Generic;
using System.Threading;
using DryIoc;
using Moq;
using NUnit.Framework;

namespace Minefield.UnitTests
{
    /// <summary>
    /// These game board test use a Moq of the IBoardBuilder
    /// </summary>
    public class GameBoardTests
    {
        [SetUp]
        public void Setup()
        {
            // Create a Moq builder (instead of using a Stub)
            var moqBuilder = new Mock<IBoardBuilder>();

            moqBuilder.Setup(u => u.GenerateBoard(8, 8, 0))
                .Returns(new List<BoardLocation>());

            moqBuilder.Setup(u => u.GenerateBoard(4, 4, 0))
                .Returns(new List<BoardLocation>());

            moqBuilder.Setup(u => u.GenerateBoard(8, 8, 8))
                .Returns(
                new List<BoardLocation>()
                    {
                        new BoardLocation (){HorizontalPosition = 'D', VerticalPosition = 1},
                        new BoardLocation (){HorizontalPosition = 'D', VerticalPosition = 2},
                        new BoardLocation (){HorizontalPosition = 'D', VerticalPosition = 3},
                        new BoardLocation (){HorizontalPosition = 'D', VerticalPosition = 4},
                        new BoardLocation (){HorizontalPosition = 'D', VerticalPosition = 5},
                        new BoardLocation (){HorizontalPosition = 'D', VerticalPosition = 6},
                        new BoardLocation (){HorizontalPosition = 'D', VerticalPosition = 7},
                        new BoardLocation (){HorizontalPosition = 'D', VerticalPosition = 8},
                    }
                );
            var builder = moqBuilder.Object;

            Container.Current.RegisterInstance(builder);
            Container.Current.Register<IGameBoard, GameBoard>();
        }

        [Test]
        [TestCase(8, 8)]
        [TestCase(4, 4)]
        public void SetupBoard(int width, int height)
        {
            // no mines
            IGameBoard board = Container.Current.Resolve<IGameBoard>();

            board.HeightOfBoard = height;
            board.WidthOfBoard = width;
            board.NumberOfMines = 0;

            board.CreateBoard();
        }

        /// <summary>
        /// Test the board will allow a user to get to the other side
        /// </summary>
        /// <param name="width">Width of board to use</param>
        /// <param name="height">Height of Board to use</param>
        [Test]
        [TestCase(8, 8)]
        public void MoveAccrossBoard(int width, int height)
        {
            // use to track an event was raised
            ManualResetEvent eventRaised = new ManualResetEvent(false);

            IGameBoard board = Container.Current.Resolve<IGameBoard>();

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

        /// <summary>
        /// When a player attempts to move onto a mine, an event should be raised
        /// </summary>
        [Test]
        public void MoveAcrossBoardAndHitMine()
        {
            // use to track an event was raised
            ManualResetEvent eventRaised = new ManualResetEvent(false);

            IGameBoard board = Container.Current.Resolve<IGameBoard>();

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
            IGameBoard board = Container.Current.Resolve<IGameBoard>();

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
            IGameBoard board = Container.Current.Resolve<IGameBoard>();

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

            // this player should not move as they are at the limit of the board
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

        [TearDown]
        public void TearDown()
        {
            Container.Current.Dispose();
        }
    }
}