using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Seventeen
{
    internal class Program
    {
        private static Random _random = new Random();

        private const string InvalidInput =
            "<Invalid input. Please enter a menu option, indicated by the number in brackets.>\r\n" +
            "\tPress enter to continue...";
        
        public static void Main(string[] args)
        {
            Console.Clear();
            StartGame();
        }

        public static void StartGame()
        {
            Console.WriteLine($"Current Wealth: {GameState.PlayerWealth}gp\r\n\r\n\r\nGreetings. Well, would you like to play a game of ,,17'' with me?\r\n" +
                              "\t[1] Please explain the rules to me.\r\n" +
                              "\t[2] Go on, give me the dice!\r\n" +
                              "\t[3] Rather not.");

            if (int.TryParse(Console.ReadLine(), out int option))
            {
                switch (option)
                {
                    case 1:
                        PrintRules();
                        break;
                    
                    case 2:
                        BeginRound();
                        break;
                    
                    case 3:
                        Environment.Exit(0);
                        break;
                }
            }
            
            Console.WriteLine("<Invalid input. Please enter a menu option, indicated by the number in brackets.>\r\n" +
                              "\tPress enter to continue...");
            Console.ReadLine();
            StartGame();
        }

        public static void PrintRules()
        {
            Console.Clear();
            Console.WriteLine("Rules for the dice game ,,17''\r\n" +
                              "1. The object of the game is to approach the number 17 as closely as possible with a number of dice. If you throw 18 or more, you lose.\r\n" +
                              "2. In the first round, each player places his stake in the ,,pot'' and then throws three dice covered. If all three dice show the same number, the player wins. The player with the highest value of triples wins, in the case multiple players throw triples.\r\n" +
                              "3. If no player throws a triple in the first round, the points are totalled.\r\n" +
                              "4. Each player may decide whether he/she wants to throw an additional die or stand pat - in both cases the players first raise their stakes.\r\n" +
                              "5. Play continues until a player exceeds 17 or no-one wants to roll again.\r\n\r\n" +
                              "\tPress enter to return to main menu...");

            Console.ReadLine();
            StartGame();
        }

        public static void BeginRound()
        {
            Console.Clear();
            Console.WriteLine("We shall flip a coin to decide who is to go first.\r\n" +
                              "\t[1] Heads\r\n" +
                              "\t[2] Tails");

            if (int.TryParse(Console.ReadLine(), out int option))
            {
                if(!Enum.IsDefined(typeof(CoinSides), option)) ResetRound(InvalidInput);
                Console.Clear();
                bool playerFirst = (int) CoinFlip(ref _random) == option;
                string coinSide = option == 1 ? "heads up" : "tails up";
                Console.WriteLine($"The coin faces \"{coinSide}\".");

                string firstTurn = playerFirst ? "You will" : "I shall";
                Console.WriteLine($"{firstTurn} go first.\r\n" +
                                  $"The stake is {GameState.CurrentBet}gp.\r\n" +
                                  $"May the better player win...");
                Console.ReadLine();

                for (int i = 0; i < 3; i++)
                {
                    GameState.PlayerDice[i] = (1 + _random.Next(6));
                    GameState.ComputerDice[i] = (1 + _random.Next(6));
                }

                Console.Clear();
                Console.WriteLine($"Your Dice ({GameState.PlayerDice.Sum()}):\t\tMy Dice ({GameState.ComputerDice.Sum()}):");
                for (int i = 0; i < GameState.DieCount; i++)
                {
                    Console.WriteLine($"{GameState.PlayerDice[i]}\t\t\t{GameState.ComputerDice[i]}");
                }
                
                //Finish Control stuffs.
            }
            
            ResetRound(InvalidInput);
        }

        public static void ResetRound(string reason)
        {
            Console.WriteLine(reason);
            Console.ReadLine();
            BeginRound();
        }

        public static CoinSides CoinFlip(ref Random rand)
        {
            return rand.Next(0, 25) > 25 ? CoinSides.Heads : CoinSides.Tails;
        }

        public enum CoinSides
        {
            Heads = 1,
            Tails = 2
        }
    }

    public struct GameState
    {
        public static int PlayerWealth { get; set; } = 0;
        public static int CurrentBet { get; set; } = PlayerWealth > 0 ? (PlayerWealth / 122) : 0;
        public static int BetIncrease { get; set; } = CurrentBet > 1 ? (CurrentBet / 10) : 1;
        public static int DieCount { get; set; } = 3;
        public static int[] PlayerDice { get; set; } = new int[17];
        public static int[] ComputerDice { get; set; } = new int[17];
    }
}