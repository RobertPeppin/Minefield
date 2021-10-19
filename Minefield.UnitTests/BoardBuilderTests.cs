using System;
using System.Linq;
using NUnit.Framework;

namespace Minefield.UnitTests
{
    public class BoardBuilderTests
    {
        private char baseLocation = 'A';

        [Test]
        [TestCase(1, 1, 1)]
        [TestCase(10, 10, 10)]
        [TestCase(8, 8, 16)]
        [TestCase(2, 2, 1)]
        public void CreateBoardWithMines(int width, int height, int numberOfMines)
        {
            var boardBuilder = new BoardBuilder();

            var mines = boardBuilder.GenerateBoard(width, height, numberOfMines);

            Assert.AreEqual(numberOfMines, mines.Count);

            // check that all mines are within the bound of the board
            Assert.AreEqual(false, mines.Any(m => m.VerticalPosition > height || m.VerticalPosition < 1));
            Assert.AreEqual(false, mines.Any(m => m.HorizontalPosition > baseLocation.GetNextChar(height - 1) && m.HorizontalPosition < baseLocation.GetNextChar(width)));
        }

        [Test]
        [TestCase(26, 26, true)]
        [TestCase(27, 27, false)]
        public void CheckBoardWontBuildAtMoreThan26x26(int width, int height, bool doesBuild)
        {
            var boardBuilder = new BoardBuilder();
            try
            {
                var mines = boardBuilder.GenerateBoard(width, height, 1);

                Assert.AreEqual(doesBuild, mines.Any());
            }
            catch (Exception e)
            {
                if (e.Message == "Board is too big" && !doesBuild)
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail("Board build failed");
                }
            }
        }


        [Test]
        [TestCase('A', 1, 'B')]
        [TestCase('B', -1, 'A')]
        [TestCase('Z', -1, 'Y')]
        [TestCase('A', 7, 'H')]
        public void TestPositionConverter(char currentPosition, int delta, char result)
        {
            Assert.AreEqual(result, currentPosition.GetNextChar(delta));
        }
    }
}
