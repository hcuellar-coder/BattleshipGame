using System;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Xml.Schema;

namespace Battleship_Game
{
    class Program
    {
        static bool Alive = true;
        static bool PlayAgain = false;
        static bool PlayDecision = false;

        private static GameGrid _gameGrid;
        private static Prompts _prompts;
        static void Main(string[] args)
        {
            _gameGrid = new GameGrid();
            _prompts = new Prompts(_gameGrid);

            while(Alive)
            { 
                _gameGrid.InitializeGrid();
                Battleships.createBattleship();               
                if (!PlayAgain)
                {
                    PlayDecision = _prompts.GameIntro();
                }
                if (PlayDecision || PlayAgain)
                {
                    bool tutorialDecision = _prompts.Tutorial();
                    if (tutorialDecision)
                    {
                        PlayBattleShip();
                        if (!PlayAgain)
                        {
                            Alive = _prompts.PlayAgainPrompt();
                            PlayAgain = Alive;
                        } 
                        else
                        {
                            Alive = PlayAgain;
                        }
                        Console.Clear();
                    }
                }
            }

        }

        public static int QueryForXFiringPosition(int shotsFired, int numberOfHits, int numberOfMisses)
        {
            Console.Write("Shots Remaining = " + (8 - shotsFired));
            Console.Write(" Hits = " + numberOfHits);
            Console.Write(" Misses = " + numberOfMisses + "\n");
            _gameGrid.PrintGrid();
            Console.Write("\n(X-axis) - Select a spot [1-10] to fire upon : ");

            int xValue;
            bool isValid = int.TryParse(Console.ReadLine(), out xValue) 
                                && GameDecision.InputValidation(xValue);

            while (isValid == false)
            {
                if (GameDecision.InputValidation(xValue) == false)
                {
                    Console.Clear();
                    Console.WriteLine("\nIncorrect Input ensign! Numbers from 1 - 10!!!! Press Enter to resume!\n");
                    Console.ReadLine();
                } else
                {
                    Console.Clear();
                    Console.WriteLine("---------Input needs to be a number!!---------\n");
                    Console.Write("Shots Remaining = " + (8 - shotsFired));
                    Console.Write(" Hits = " + numberOfHits);
                    Console.Write(" Misses = " + numberOfMisses + "\n");
                    _gameGrid.PrintGrid();
                    Console.Write("\n(X-axis) - Select a spot [1-10] to fire upon : ");
                }
                isValid = int.TryParse(Console.ReadLine(), out xValue)
                                && GameDecision.InputValidation(xValue);
            }
            return xValue;
        }

        public static int QueryForYFiringPosition(int shotsFired, int numberOfHits, int numberOfMisses, int xValue)
        {
            Console.Write("\n(Y-axis) - Select a spot [1-10] to fire upon : ");

            int yValue;
            bool isValid = int.TryParse(Console.ReadLine(), out yValue)
                                && GameDecision.InputValidation(yValue);

            while (isValid == false)
            {
                if (GameDecision.InputValidation(yValue) == false)
                {
                    Console.Clear();
                    Console.WriteLine("\nIncorrect Input ensign! Numbers from 1 - 10!!!! Press Enter to resume!\n");
                    Console.ReadLine();
                } else 
                {
                    Console.Clear();
                    Console.WriteLine("---------Input needs to be a number!!---------\n");
                    Console.Write("Shots Remaining = " + (8 - shotsFired));
                    Console.Write(" Hits = " + numberOfHits);
                    Console.Write(" Misses = " + numberOfMisses + "\n");
                    _gameGrid.PrintGrid();
                    Console.Write("\n(X-axis) - Select a spot [1-10] to fire upon : " + xValue + "\n");
                    Console.Write("\n(Y-axis) - Select a spot [1-10] to fire upon : ");
                }

                isValid = int.TryParse(Console.ReadLine(), out yValue)
                                && GameDecision.InputValidation(yValue);
            }
            
            return yValue;
        }


        public static void PlayBattleShip()
        {
            int shotsFired = 0;
            int numberOfHits = 0;
            int numberOfMisses = 0;
            int XValue;
            int YValue;
            
            while (true)
            {
                Console.Clear();
                XValue = QueryForXFiringPosition(shotsFired, numberOfHits, numberOfMisses);
                YValue = QueryForYFiringPosition(shotsFired, numberOfHits, numberOfMisses, XValue);
                

             
                if (GameDecision.DetermineIfGuessed(XValue, YValue, shotsFired))
                {
                    Console.Write("\nRookie mistake ensign! Choose a spot you haven't already shot at! Press Enter to resume!");
                    Console.ReadLine();
                    Console.Clear();
                } 
                else
                {
                    if (GameDecision.DetermineHit(XValue, YValue))
                    {
                        numberOfHits++;
                        Console.WriteLine("\nHit!\n");
                        Console.Write("Press Enter to resume!");
                        Console.ReadLine();
                    }
                    else
                    {
                        numberOfMisses++;
                        Console.WriteLine("\nMiss!\n");
                        Console.Write("Press Enter to resume!");
                        Console.ReadLine();
                    }
                    _gameGrid.EditGrid(XValue, YValue, GameDecision.DetermineHit(XValue, YValue));
                    shotsFired++;
                }
                

                if (numberOfHits == 5)
                {
                    PlayAgain = false;
                    Console.Clear();
                    _gameGrid.PrintGrid();
                    Console.WriteLine("\nCongratulations!! Youve Sunk the Battleship!");
                    break;
                }
                else if (shotsFired == 8)
                {
                    
                    PlayAgain = false;
                    Console.Clear();
                    Console.Write("Shots Remaining = " + (8 - shotsFired));
                    Console.Write(" Hits = " + numberOfHits);
                    Console.Write(" Misses = " + numberOfMisses + "\n");
                    _gameGrid.PrintGrid();
                    Console.WriteLine("\nYou Lost!");
                    Console.WriteLine("Better Luck Next Time!");
                    break;
                }
                else if (shotsFired == 4 && numberOfMisses == 4)
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
                        PlayAgain = true;
                        break;
                    }
                }
            }
        }
    }
}