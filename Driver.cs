using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
/// <summary>
/// RIKER QUINTANA
/// 816823248
/// </summary>
namespace TicTacToe
{
    class Driver
    {
        static void Main()
        {

            bool continuing = false;
            char decisionYOrN;

            if (File.Exists("game.txt".ToLower()))
            {
                bool simulate = true;
                string[] lines = System.IO.File.ReadAllLines("game.txt");
                var board = new TicTacToe();
                board.GameMaster(lines,simulate);
            }
            else
            {
                var board = new TicTacToe();
                do
                {
                    board.GameMaster();
                    Console.WriteLine("Play Again? y/n");
                    decisionYOrN = Console.ReadLine()[0];
                    continuing = (decisionYOrN == 'y');
                }
                while (continuing);
                Console.ReadLine();
            }
        }
    }
}
