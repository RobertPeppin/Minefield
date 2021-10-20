using System;
using System.Linq;
using DryIoc;

namespace Minefield
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Setup a simple container using DryIoC (simple and small)
            Container.Current.Register<IBoardBuilder, BoardBuilder>();
            Container.Current.Register<IGameBoard, GameBoard>();
            Container.Current.Register<IPlayer, Player>();

            Console.WriteLine("Welcome to Minefield");
            PrepareGame(args);

            Container.Current.Dispose();
        }

        private static void PrepareGame(string[] args)
        {
            if (args.Any())
            {
                StartCustomGame(args);
            }
            else
            {
                Game game = new();
                game.StartGame();
            }
        }

        private static void StartCustomGame(string[] args)
        {
            int height = 8;
            int width = 8;
            int mines = 16;
            int lives = 3;

            try
            {
                for (int index = 0; index < args.Count(); index = index + 2)
                {
                    var name = args[index];
                    var value = args[index + 1];

                    switch (name)
                    {
                        case "-w":
                            width = Convert.ToInt32(value);
                            break;
                        case "-h":
                            height = Convert.ToInt32(value);
                            break;
                        case "-m":
                            mines = Convert.ToInt32(value);
                            break;
                        case "-l":
                            lives = Convert.ToInt32(value);
                            break;
                    }
                }
            }
            catch
            {
                Console.WriteLine("Invalid arguments.");
                Console.WriteLine("-h integer sets the height of the board");
                Console.WriteLine("-w integer sets the width of the board");
                Console.WriteLine("-m integer sets the nnumber of mines on the board");
                Console.WriteLine("-l integer sets the number of lives to start with");
                Console.WriteLine("e.g. -w 16 -h 16 -m 64 -l 6");
                Console.WriteLine("Will create a 16 x 16 board with 64 mines and a player with 6 lives");

                return;
            }

            Game game = new();
            game.StartGame(width, height, mines, lives);
        }
    }
}