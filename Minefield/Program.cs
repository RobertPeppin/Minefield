using System;
using System.Threading.Tasks;

namespace Minefield
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Minefield");

            Game game = new Game();

            game.StartGame();
        }
    }
}
