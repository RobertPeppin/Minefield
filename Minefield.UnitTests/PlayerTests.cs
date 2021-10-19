using System;
using NUnit.Framework;

namespace Minefield.UnitTests
{
    public class PlayerTests
    {
        [Test]
        public void CheckLivesDecrementOnMine()
        {
            GameBoardStub board = new GameBoardStub();

            Player player = new Player(board, 3);
            Assert.AreEqual(3, player.NumberOfLives);

            board.SetOffMine();

            Assert.AreEqual(2, player.NumberOfLives);
        }

        [Test]
        public void CheckGameIsOverOnLivesGone()
        {
            GameBoardStub board = new GameBoardStub();

            Player player = new Player(board, 3);
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
    }
}
