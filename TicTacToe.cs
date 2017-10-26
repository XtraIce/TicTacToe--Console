using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
/// <summary>
/// RIKER QUINTANA
/// 816823248
/// </summary>
namespace TicTacToe
{///
    class TicTacToe
    {
        private int playerRowSel;
        /// custom set; to check fro valid input
        public int PlayerRowSel
        {
            get
            { return this.playerRowSel; }
            set
            {
                if (value <= 2)
                {
                    this.playerRowSel = value;
                    validInput = true;
                }
                else
                {
                    validInput = false;
                }
            }
        }
        private int playerColSel;
        /// custom set; to check fro valid input
        public int PlayerColSel
        {
            get
            { return this.playerColSel; }
            set
            {
                if (value >= 0 && value <= 2)
                {
                    this.playerColSel = value;
                    validInput = true;
                }
                else
                {
                    validInput = false;
                }
            }
        }
        bool player1Go = true;
        bool player2Go = true;
        bool playerWin1 = false;
        bool playerWin2 = false;
        bool AICasual = false;
        bool gameDraw = false;
        bool validInput = true;
        int[,] theBoard = new int[3, 3];
        /// <summary>
        /// default constructor
        /// </summary>
        public TicTacToe()
        {
            /// initialize tictactoe board to zero in all positions
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                    theBoard[i, j] = 0;
            }
        }
        /// <summary>
        /// Clear the board of all player inputs and reset with 0's
        /// </summary>
        private void ClearBoard()
        {
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    theBoard[i, j] = 0;
                }
            }
            player1Go = true;
            player2Go = true;
        }
        /// <summary>
        /// No variable-passed method to translate the string input > int data entry on the board
        /// Used for Player vs. Player/AI
        /// </summary>
        public void MakeMove()
        {

            do
            {
                Console.WriteLine("Make Your Move");
                string playerInput = Console.ReadLine();
                //Console.WriteLine($"debug: Playerinput {playerInput}");
                if (playerInput.Length != 2)
                {
                    Console.WriteLine("Invalid Input\n");
                    validInput = false;
                }
                else
                {
                    //Console.WriteLine($"Debug: {playerInput[0]}");
                    PlayerRowSel = rowConverter(playerInput[0]);
                    //Console.WriteLine($"Debug: {playerRowSel}");
                    PlayerColSel = (int)Char.GetNumericValue(playerInput, 1);
                }
            }
            while (!validInput);
            //AI is on
            if (AICasual)
            {
                if (theBoard[playerRowSel, playerColSel] == 0)
                {
                    if (player1Go)
                        theBoard[playerRowSel, playerColSel] = 1;
                }
                // if index already has a player input, call the function again for new input
                else
                {
                    Console.WriteLine("Already filled.\n");
                    MakeMove();
                }
            }
            else
            {
                // assigns game board at index a value based on who's turn it is
                if (theBoard[playerRowSel, playerColSel] == 0)
                {
                    if (player1Go)
                        theBoard[playerRowSel, playerColSel] = 1;
                    else
                        theBoard[playerRowSel, playerColSel] = 2;
                }
                // if index already has a player input, call the function again for new input
                else
                {
                    Console.WriteLine("Already filled.\n");
                    MakeMove();
                }

            }

        }
        /// <summary>
        /// Simulates games with text file input from the base directory
        /// </summary>
        /// <param name="line"></param>
        public void GameMaster(string[] line, bool simulate)
        {
            //Paul helped with Json StreamWriter
            Newtonsoft.Json.JsonSerializer jsonSer = new JsonSerializer();
            System.IO.StreamWriter sw = new System.IO.StreamWriter("GameResults.txt");
            Newtonsoft.Json.JsonTextWriter jsonText = new JsonTextWriter(sw);

            int inputLastLine = line.Count();
            for (int currentLine = 0; currentLine < inputLastLine; currentLine++)
            {
                string currentString = line[currentLine];
                bool IsValid = InputValidator(line, currentLine);
                if (IsValid)
                {
                    if (currentString.StartsWith("0000000000"))
                    {
                        //JSON
                        jsonSer.Serialize(jsonText, theBoard);
                        sw.WriteLine();
                        //
                        ClearBoard();
                    }
                    else
                    {
                        MakeMove(line, currentLine);
                        CheckWin(simulate, jsonSer, sw, jsonText);
                    }
                }
                else { continue; }
            }
            sw.Close();

        }
        /// <summary>
        /// Method runs the game until either player wins
        /// </summary>
        public void GameMaster()
        {
            if (mainMenu())
            {
                AICasual = true;
                    while ((!playerWin1) && (!playerWin2))
                    {
                        Console.Clear();
                        Console.WriteLine(this);
                        MakeMove();
                        CheckWin();
                    if (!playerWin1)
                    {
                        CasualAI();
                        CheckWin();
                    }
                    }
                    if (playerWin1)
                    {
                        Console.WriteLine(this);
                        Console.WriteLine("Congratulations, You are victorious!");
                        playerWin1 = false;
                    }
                    else
                    {
                        Console.WriteLine(this);
                        Console.WriteLine("How could you lose to this!?");
                        playerWin2 = false;
                    }
            }
            else
            {
                deterPlayer();
                while ((!playerWin1) && (!playerWin2))
                {
                    Console.Clear();
                    Console.WriteLine(this);
                    MakeMove();
                    CheckWin();
                }
                if (playerWin1)
                {
                    Console.WriteLine(this);
                    Console.WriteLine("Congratulations, Player 1 is victorious!");
                    playerWin1 = false;
                }
                else
                {
                    Console.WriteLine(this);
                    Console.WriteLine("Congratulations, Player 2 is victorious!");
                    playerWin2 = false;
                }
            }
            

        }
        /// <summary>
        /// variable-passed method to translate the text file input > int data entry on the board
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        public void MakeMove(string[] input, int index)
        {
            string currentString = input[index];
            PlayerRowSel = rowConverter(currentString[4]);
            PlayerColSel = (int)Char.GetNumericValue(currentString, 5);
            if (theBoard[playerRowSel, playerColSel] == 0)
            {
                if (player1Go)
                    theBoard[playerRowSel, playerColSel] = 1;
                else
                    theBoard[playerRowSel, playerColSel] = 2;
            }
        }
        /// <summary>
        /// Method  used for a file simulated game
        ///         checks all possible win conditions
        ///         checks for a draw
        ///         outputs to JSON string and txt file
        /// </summary>
        /// <param name="simulate"></param>
        /// <param name="jsonSer"></param>
        /// <param name="sw"></param>
        /// <param name="jsonText"></param>
        private void CheckWin(bool simulate, JsonSerializer jsonSer, System.IO.StreamWriter sw, JsonTextWriter jsonText)
        {
            int checkWinner;
            bool winner;
            if (player1Go)
                checkWinner = 1;
            else checkWinner = 2;

            var boardValues = new List<int>();
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    boardValues.Add(theBoard[i, j]);
                }
            }
            //Win Scenarios
            if (boardValues[0] == checkWinner && boardValues[3] == checkWinner && boardValues[6] == checkWinner)
                winner = true;
            else if (boardValues[1] == checkWinner && boardValues[4] == checkWinner && boardValues[7] == checkWinner)
                winner = true;
            else if (boardValues[2] == checkWinner && boardValues[5] == checkWinner && boardValues[8] == checkWinner)
                winner = true;
            else if (boardValues[0] == checkWinner && boardValues[1] == checkWinner && boardValues[2] == checkWinner)
                winner = true;
            else if (boardValues[3] == checkWinner && boardValues[4] == checkWinner && boardValues[5] == checkWinner)
                winner = true;
            else if (boardValues[6] == checkWinner && boardValues[7] == checkWinner && boardValues[8] == checkWinner)
                winner = true;
            else if (boardValues[0] == checkWinner && boardValues[4] == checkWinner && boardValues[8] == checkWinner)
                winner = true;
            else if (boardValues[6] == checkWinner && boardValues[4] == checkWinner && boardValues[0] == checkWinner)
                winner = true;
            else
                winner = false;
            //Draw Condition
            if ((boardValues[0] != 0) && (boardValues[1] != 0) && (boardValues[2] != 0) && (boardValues[3] != 0) && (boardValues[4] != 0) &&
                (boardValues[5] != 0) && (boardValues[6] != 0) && (boardValues[7] != 0) && (boardValues[8] != 0))
            {
                gameDraw = true;
            }
            //IF winner declared, WHO
            if (checkWinner == 1 & winner)
            {
                playerWin1 = true;
                playerWin2 = !playerWin1;
                //JSON
                jsonSer.Serialize(jsonText, theBoard);
                sw.WriteLine();
                //
                ClearBoard();
            }
            if (checkWinner == 2 & winner)
            {
                playerWin2 = true;
                playerWin1 = !playerWin2;
                //JSON
                jsonSer.Serialize(jsonText, theBoard);
                sw.WriteLine();
                //
                ClearBoard();
            }
            // IF DRAW
            if (gameDraw)
            {
                //JSON
                jsonSer.Serialize(jsonText, theBoard);
                sw.WriteLine();
                ClearBoard();
                gameDraw = false;
            }
        }

        /// <summary>
        /// Method  used for a player vs. player/AI
        ///         checks all possible win conditions
        ///         checks for a draw
        /// </summary>
        /// <returns></returns>
        private void CheckWin()
        {
            int checkWinner;
            bool winner;
            if (player1Go)
                checkWinner = 1;
            else checkWinner = 2;

            var boardValues = new List<int>();
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    boardValues.Add(theBoard[i, j]);
                }
            }
            //Win Scenarios
            if (boardValues[0] == checkWinner && boardValues[3] == checkWinner && boardValues[6] == checkWinner)
                winner = true;
            else if (boardValues[1] == checkWinner && boardValues[4] == checkWinner && boardValues[7] == checkWinner)
                winner = true;
            else if (boardValues[2] == checkWinner && boardValues[5] == checkWinner && boardValues[8] == checkWinner)
                winner = true;
            else if (boardValues[0] == checkWinner && boardValues[1] == checkWinner && boardValues[2] == checkWinner)
                winner = true;
            else if (boardValues[3] == checkWinner && boardValues[4] == checkWinner && boardValues[5] == checkWinner)
                winner = true;
            else if (boardValues[6] == checkWinner && boardValues[7] == checkWinner && boardValues[8] == checkWinner)
                winner = true;
            else if (boardValues[0] == checkWinner && boardValues[4] == checkWinner && boardValues[8] == checkWinner)
                winner = true;
            else if (boardValues[6] == checkWinner && boardValues[4] == checkWinner && boardValues[2] == checkWinner)
                winner = true;
            else
                winner = false;
            //Draw Condition
            if ((boardValues[0] != 0) && (boardValues[1] != 0) && (boardValues[2] != 0) && (boardValues[3] != 0) && (boardValues[4] != 0) &&
                (boardValues[5] != 0) && (boardValues[6] != 0) && (boardValues[7] != 0) && (boardValues[8] != 0))
            {
                gameDraw = true;
            }
            //IF winner declared, who
            if (checkWinner == 1 & winner)
            {
                playerWin1 = true;
                playerWin2 = false;
                winner = false;
                ClearBoard();
            }
            if (checkWinner == 2 & winner)
            {
                playerWin2 = true;
                playerWin1 = false;
                winner = false;
                ClearBoard();
            }
            //IF DRAW
            if (gameDraw)
            {
                Console.WriteLine("DRAW GAME");
                Console.ReadLine();
                ClearBoard();
                gameDraw = false;
                player1Go = true;
                player2Go = true;
            }
            player1Go = !player1Go;
            player2Go = !player2Go;
        }
        /// <summary>
        /// To String method for an object of type TicTacToe
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var listOfStrings = new List<string>();
            listOfStrings.Add("\n   0  1  2");
            listOfStrings.Add("  ________");
            listOfStrings.Add($"A| {theBoard[0, 0]}  {theBoard[0, 1]}  {theBoard[0, 2]}|");
            listOfStrings.Add($"B| {theBoard[1, 0]}  {theBoard[1, 1]}  {theBoard[1, 2]}|");
            listOfStrings.Add($"C| {theBoard[2, 0]}  {theBoard[2, 1]}  {theBoard[2, 2]}|");
            listOfStrings.Add("  --------");
            string returnedValue = String.Join("\n", listOfStrings);
            return returnedValue;
        }
        private int rowConverter(char rowInput)
        {
            rowInput = char.ToUpper(rowInput);
            switch (rowInput)
            {
                case 'A': return 0;
                case 'B': return 1;
                case 'C': return 2;
                default: return 3;
            }
        }
        /// <summary>
        /// Determine which player goes first via a "flip of a coin"
        /// </summary>
        private void deterPlayer()
        {
            Console.WriteLine("A coin is flipped. Player 1: Heads, Player 2: Tails\n");
            Thread.Sleep(500);
            Console.WriteLine(". ");
            Thread.Sleep(500);
            Console.WriteLine(". ");
            Thread.Sleep(500);
            Console.WriteLine(". ");
            Random coinFlip = new Random();
            int coin = coinFlip.Next(1, 500);
            if (coin % 2 == 1)
            {
                player1Go = true;
                player2Go = false;
                Console.WriteLine("Heads, Player 1 goes first;");
                Console.ReadLine();
            }
            else
            {
                player1Go = false;
                player2Go = true;
                Console.WriteLine("Tails, Player 2 goes first.");
                Console.ReadLine();
            }
        }
        /// <summary>
        /// Check if the input line from a text file is valid, if not, skip it
        /// </summary>
        /// <param name="line"></param>
        /// <param name="lineNum"></param>
        /// <returns></returns>
        private bool InputValidator(string[] line, int lineNum)
        {
            string currentInput = line[lineNum];
            if (currentInput.StartsWith("P1:") || currentInput.StartsWith("P2:"))
            {
                if (currentInput.Substring(4) == "A2" || currentInput.Substring(4) == "A1" || currentInput.Substring(4) == "A0" ||
                    currentInput.Substring(4) == "B2" || currentInput.Substring(4) == "B1" || currentInput.Substring(4) == "B0" ||
                    currentInput.Substring(4) == "C2" || currentInput.Substring(4) == "C1" || currentInput.Substring(4) == "C0")
                {
                    if (currentInput[1] == '1' && player2Go)
                    {
                        player1Go = true;
                        player2Go = false;
                        return true;
                    }
                    else if (currentInput[1] == '2' && player1Go)
                    {
                        player2Go = true;
                        player1Go = false;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            else if (currentInput.StartsWith("0000000000"))
            {
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// Main Menu of Game. Ask player which game mode desired
        /// </summary>
        /// <returns></returns>
        public bool mainMenu()
        {
            int playerDecision;
            do
            {
                Console.Clear();
                Console.WriteLine("Welcome to Tic-Tac-Toe!");
                Console.WriteLine("PLAYER VS. PLAYER: press [1]");
                Console.WriteLine("PLAY VS. AI: press [2]");
                playerDecision = int.Parse(Console.ReadLine());
                if (playerDecision !=0 && playerDecision != 1 && playerDecision != 2)
                {
                    continue;
                }
                if (playerDecision == 1) { return false; }
                if (playerDecision == 2) { return true; }
            }
            while (true);
        }
        /// <summary>
        /// A Simple AI that determines moves randomly and checks if theyre valid
        /// </summary>
        private void CasualAI()
        {
            Console.WriteLine("I'm making my move..");
            Thread.Sleep(500);
            Console.WriteLine(". ");
            Thread.Sleep(500);
            Console.WriteLine(". ");
            Thread.Sleep(500);
            Console.WriteLine(". ");
            Random choiceRow = new Random();
            Random choiceCol = new Random();
            do
            {
                int lolAIRow = choiceRow.Next(1, 1000) % 3;
                int lolAICol = choiceCol.Next(1, 5000) % 3;

                if (theBoard[lolAIRow, lolAICol] == 0)
                {
                    theBoard[lolAIRow, lolAICol] = 2;
                    Console.WriteLine("THERE!");
                    Thread.Sleep(1000);
                    break;
                }
            }
            while (true);
        }
    }
}
