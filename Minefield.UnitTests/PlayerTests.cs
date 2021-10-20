using DryIoc;
using NUnit.Framework;

namespace Minefield.UnitTests
{
    /// <summary>
    /// These tests use a stub version of the GameBoard.
    /// </summary>
    public class PlayerTests
    {
        [SetUp]
        public void Setup()
        {
            // Add the required dependencies into the container
            Container.Current.Register<IGameBoard, GameBoardStub>();
            Container.Current.Register<IPlayer, Player>();
        }

        [Test]
        public void CheckLivesDecrementOnMine()
        {
            GameBoardStub board = (GameBoardStub)Container.Current.Resolve<IGameBoard>();

            IPlayer player = Container.Current.Resolve<IPlayer>();
            Assert.AreEqual(3, player.NumberOfLives);

            board.SetOffMine();

            Assert.AreEqual(2, player.NumberOfLives);
        }

        [Test]
        public void CheckMaxLivesAffectsLifeCount()
        {
            IPlayer player = Container.Current.Resolve<IPlayer>();
            GameBoardStub board = (GameBoardStub)Container.Current.Resolve<IGameBoard>();

            Assert.AreEqual(3, player.NumberOfLives);

            player.SetMaxLives(5);

            Assert.AreEqual(5, player.NumberOfLives);

            board.SetOffMine();

            Assert.AreEqual(4, player.NumberOfLives);
        }

        [Test]
        public void CheckGameIsOverOnLivesGone()
        {
            IPlayer player = Container.Current.Resolve<IPlayer>();
            GameBoardStub board = (GameBoardStub)Container.Current.Resolve<IGameBoard>();

            Assert.AreEqual(3, player.NumberOfLives);
            Assert.AreEqual(true, player.CanPlay);

            board.SetOffMine();
            Assert.AreEqual(2, player.NumberOfLives);
            Assert.AreEqual(true, player.CanPlay);

            board.SetOffMine();
            Assert.AreEqual(1, player.NumberOfLives);
            Assert.AreEqual(true, player.CanPlay);

            board.SetOffMine();
            Assert.AreEqual(0, player.NumberOfLives);
            Assert.AreEqual(false, player.CanPlay);
        }

        [TearDown]
        public void TearDown()
        {
            Container.Current.Dispose();
        }
    }
}
