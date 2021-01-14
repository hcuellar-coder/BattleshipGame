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
        private static GameLogic _gameLogic;
        static void Main(string[] args)
        {
            _gameGrid = new GameGrid();
            _prompts = new Prompts(_gameGrid);
            _gameLogic = new GameLogic(_gameGrid);

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
                        PlayAgain = _gameLogic.PlayBattleShip();
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
    }
}