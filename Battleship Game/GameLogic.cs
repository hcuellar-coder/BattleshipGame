using System;
using System.Collections.Generic;
using System.Text;

namespace Battleship_Game
{
    public class GameLogic
    {
        public int ShotsFired { get; private set; } = 0;
        public int NumberOfHits { get; private set; } = 0;
        public int NumberOfMisses { get; private set; } = 0;

        private GameGrid _gameGrid;
        private const int MAX_GUESSES = 8;

        public GameLogic(GameGrid gameGrid)
        {
            _gameGrid = gameGrid;
        }

        public bool PlayBattleShip()
        {
            int XValue;
            int YValue;

            var playAgain = false;

            while (true)
            {
                Console.Clear();
                XValue = QueryForXFiringPosition();
                YValue = QueryForYFiringPosition(XValue);



                if (GameDecision.DetermineIfGuessed(XValue, YValue, ShotsFired))
                {
                    Console.Write("\nRookie mistake ensign! Choose a spot you haven't already shot at! Press Enter to resume!");
                    Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    if (GameDecision.DetermineHit(XValue, YValue))
                    {
                        NumberOfHits++;
                        Console.WriteLine("\nHit!\n");
                        Console.Write("Press Enter to resume!");
                        Console.ReadLine();
                    }
                    else
                    {
                        NumberOfMisses++;
                        Console.WriteLine("\nMiss!\n");
                        Console.Write("Press Enter to resume!");
                        Console.ReadLine();
                    }
                    _gameGrid.EditGrid(XValue, YValue, GameDecision.DetermineHit(XValue, YValue));
                    ShotsFired++;
                }


                if (NumberOfHits == (MAX_GUESSES / 2)+1)
                {
                    playAgain = false;
                    Console.Clear();
                    _gameGrid.PrintGrid();
                    Console.WriteLine("\nCongratulations!! Youve Sunk the Battleship!");
                    break;
                }
                else if (ShotsFired == MAX_GUESSES)
                {

                    playAgain = false;
                    Console.Clear();
                    PrintSummary();
                    Console.WriteLine("\nYou Lost!");
                    Console.WriteLine("Better Luck Next Time!");
                    break;
                }
                else if ((ShotsFired == (MAX_GUESSES / 2)) && (NumberOfMisses == (MAX_GUESSES / 2)))
                {
                    bool restart = false;
                    while (true)
                    {
                        Console.Clear();
                        Console.Write("\nListen kid, I'm going to be real with you...");
                        Console.Write("You're not going to win. Do you just want to restart? (Y or N) : ");
                        var PlayerInput = Console.ReadLine();
                        if (PlayerInput == "y" || PlayerInput == "Y")
                        {
                            restart = true;
                            break;
                        }
                        else if (PlayerInput == "n" || PlayerInput == "N")
                        {
                            restart = false;
                            break;
                        }
                        else
                        {
                            Console.Write("\nIncorrect Input press Enter to resume.");
                            Console.ReadLine();
                        }
                    }

                    if (restart)
                    {
                        playAgain = true;
                        break;
                    }
                }
            }
            ShotsFired = 0;
            NumberOfHits = 0;
            NumberOfMisses = 0;
            return playAgain;
        }

        public int QueryForXFiringPosition()
        {
            PrintSummary();
            Console.Write("\n(X-axis) - Select a spot [1-10] to fire upon : ");

            int xValue;
            bool isValid = int.TryParse(Console.ReadLine(), out xValue) && GameDecision.InputValidation(xValue);

            while (isValid == false)
            {
                if (GameDecision.InputValidation(xValue) == false)
                {
                    Console.Clear();
                    Console.WriteLine("\nIncorrect Input ensign! Numbers from 1 - 10!!!! Press Enter to resume!\n");
                    Console.ReadLine();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("---------Input needs to be a number!!---------\n");
                    
                }
                PrintSummary();
                Console.Write("\n(X-axis) - Select a spot [1-10] to fire upon : ");
                isValid = int.TryParse(Console.ReadLine(), out xValue) && GameDecision.InputValidation(xValue);
            }
            return xValue;
        }

        public int QueryForYFiringPosition(int xValue)
        {
            Console.Write("\n(Y-axis) - Select a spot [1-10] to fire upon : ");

            int yValue;
            bool isValid = int.TryParse(Console.ReadLine(), out yValue) && GameDecision.InputValidation(yValue);

            while (isValid == false)
            {
                if (GameDecision.InputValidation(yValue) == false)
                {
                    Console.Clear();
                    Console.WriteLine("\nIncorrect Input ensign! Numbers from 1 - 10!!!! Press Enter to resume!\n");
                    Console.ReadLine();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("---------Input needs to be a number!!---------\n");
                }
                PrintSummary();
                Console.Write("\n(X-axis) - Select a spot [1-10] to fire upon : " + xValue + "\n");
                Console.Write("\n(Y-axis) - Select a spot [1-10] to fire upon : ");
                isValid = int.TryParse(Console.ReadLine(), out yValue) && GameDecision.InputValidation(yValue);
            }

            return yValue;
        }

        public void PrintSummary()
        {
            Console.Write("Shots Remaining = " + (MAX_GUESSES - ShotsFired));
            Console.Write(" Hits = " + NumberOfHits);
            Console.Write(" Misses = " + NumberOfMisses + "\n");
            _gameGrid.PrintGrid();
        }


    }
}
