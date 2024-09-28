// #define TESTING
using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace Game{
    public class Program{
        public static void Main(string[] args){
            UI.Init(120, 40);
            bool gameRunning = true;
            string[] cmd;
            string[] lastCmd = "NONE".Split(' ');
            City city = new City();
            COMMANDS command;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.CursorVisible = false;
            Console.WriteLine("Press : to enter command");

            while(gameRunning){
                if (stopwatch.ElapsedMilliseconds > 500){
                    UI.TimeUpdate();
                    city.Update();

                    // int index = 0;
                    UI.DisplayShortList($"Citizens: {city.citizens.Count}/{city.citizenLimit}", line: 0);
                    UI.DisplayShortList($"Buildings: {city.buildings.Count}");
                    UI.DisplayShortList($"Mines: {city.mines.Count}");
                    UI.DisplayShortList($"Cash: {city.money}");
                    UI.DisplayShortList("--------------------");
                    city.DisplayItem();
                    UI.DisplayShortList("--------------------");
                    UI.DisplayShortList("Production Order:");
                    city.DisplayProductionOrder();
                    city.DisplaySellingOrder();
                    city.DisplayBuyingOrder();
                    
                    UI.ClearShortList();
                    stopwatch.Restart();
                }
                
                if (Console.GetCursorPosition().Top > UI.maxConsoleHeight)
                    UI.MoveUpDisplay();

                (command, cmd) = UI.ReadCmd();
                if (command == COMMANDS.LAST_COMMAND){
                    cmd = lastCmd;
                    command = UI.str2cmd(lastCmd[0]);
                } else if (command != COMMANDS.NONE){
                    lastCmd = cmd;
                }
                switch(command){
                    case COMMANDS.EXIT:
                        gameRunning = false;
                        break;
                    case COMMANDS.BUILD:
                        city.Build(cmd);
                        break;
                    case COMMANDS.NEXT:
                        city.Update();
                        break;
                    case COMMANDS.DISPLAY:
                        if (cmd.Length > 1)
                        switch(UI.str2cmd(cmd[1])){
                            case COMMANDS.CITIZEN:
                                city.DisplayCitizen();
                                break;
                            case COMMANDS.BUILDING:
                                city.DisplayBuilding();
                                break;
                            case COMMANDS.MINE:
                                city.DisplayMine();
                                break;
                            default:
                                break;
                        }
                        break;

                    case COMMANDS.MINE:
                        city.Mine(cmd);
                        break;

                    case COMMANDS.PRODUCE:
                        city.Produce(cmd);
                        break;


                    case COMMANDS.SELL:
                        city.Sell(cmd);
                        break;
                    case COMMANDS.BUY:
                        city.Buy(cmd);
                        break;

                    case COMMANDS.UPDATE:
                        city.Update();
                        break;
                    
                    default:
                        break;
                }

                System.Threading.Thread.Sleep(100);

            }
        }
    }
}