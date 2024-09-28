using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Game
{
    public enum COMMANDS{
        NONE,
        EXIT,
        BUILD,
        NEXT,
        DISPLAY,
        UPDATE,

        MINE,


        HOUSE,
        BLACKSMITH,
        STORE,
        SHOP,

        CITIZEN,
        BUILDING,
        MATERIAL,

        LAST_COMMAND,
    
        ADD,
        REMOVE,
        ITEM,
        SELL,
        BUY,

        FOREST,
        CAVE,
        QUARRY,


        PRODUCE,
    }

    public static class UI{
        private static int width = 10;
        private static int height = 10;
        public static int maxConsoleHeight {get; private set;} = 10;

        private static DateTime time;
        private static void SetWindowSize(){
            Console.Clear();
            if(OperatingSystem.IsWindows()){
                Console.SetWindowSize(width, height);
            } else if (OperatingSystem.IsLinux()){

                bool status;
                do{
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine($"Set console size to {width}x{height} manually.");
                    Console.WriteLine($"Current settings: {Console.WindowWidth}x{Console.WindowHeight}");
                    status = Console.WindowWidth == width & Console.WindowHeight == height;

                    Console.WriteLine($"Status: {status}");
                    System.Threading.Thread.Sleep(10);
                } while (!status);
                Console.WriteLine("Done");
            }
        }
        public static void Init(int width, int height){
            UI.width = width;
            UI.height = height;
            UI.maxConsoleHeight = height - 2*height/10;
            #if DEBUG
            Console.WriteLine("Debug mode");
            #else
            SetWindowSize();
            #endif
            Console.Clear();
            Console.SetCursorPosition(0, 0);

            time = new DateTime(2021, 1, 1, 0, 0, 0);
        }

        private static int nextLine = 0;
        public static void DisplayShortList(string str, int line = -1, int width = -1){
            (int w, int h) = Console.GetCursorPosition();
            if (line == -1) line = Console.CursorTop;
            else nextLine = line;

            if (width == -1)Console.SetCursorPosition(UI.width*3/4, nextLine);
            else Console.SetCursorPosition(width, nextLine);
            nextLine++;
            ClearLine();

            Console.Write(str);
            Console.SetCursorPosition(w, h);
        }
        public static void ClearLine(){
            (int w, int h) = Console.GetCursorPosition();
            Console.Write(new string(' ', width-w));
            Console.SetCursorPosition(w, h);
        }

        public static void ClearShortList(){
            for (int i = nextLine; i < height-1; i++){
                UI.DisplayShortList(new string(' ', width/4));
            }
            nextLine = 0;
        }

        public static COMMANDS str2cmd(string cmd){
            (int w, int h) = Console.GetCursorPosition();
            if (cmd == "!!")
                return COMMANDS.LAST_COMMAND;
            if (!Enum.TryParse<COMMANDS>(cmd.ToUpper(), out COMMANDS command)){
                command = COMMANDS.NONE;
                Console.SetCursorPosition(1, Console.CursorTop);
                UI.ClearLine();
                Console.Write("Invalid command");
            }
            #if !DEBUG
            if (h == 0)
                h ++;
            Console.SetCursorPosition(w, h);
            #endif
            return command;
        }

        public static ItemType str2tool(string cmd){
            (int w, int h) = Console.GetCursorPosition();
            if (!Enum.TryParse<ItemType>(cmd.ToUpper(), out ItemType command)){
                command = ItemType.NONE;
                Console.SetCursorPosition(1, Console.CursorTop);
                UI.ClearLine();
                Console.Write("Invalid command");
            }
            #if !DEBUG
            if (h == 0)
                h ++;
            Console.SetCursorPosition(w, h);
            #endif
            return command;
        }

        public static void MoveUpDisplay(){
            for (int i = 0; i < height -2; i++){
                // Console.SetCursorPosition(0, height-1);
                Console.WriteLine(new string(' ', width));
                System.Threading.Thread.Sleep(10);
            }

            Console.SetCursorPosition(0, 1);
        }

        private static string ReadLine(){
            string str = "";
            ConsoleKeyInfo c;

            do {
                c = Console.ReadKey(true);
                if (c.Key is ConsoleKey.Backspace){
                    if (str.Length > 0){
                        str = str.Remove(str.Length-1);
                        Console.Write("\b \b");
                    }
                } else if (c.Key is ConsoleKey.Enter){
                    break;
                } else {
                    str += c.KeyChar;
                    Console.Write(c.KeyChar);
                }
            } while (c.Key is not ConsoleKey.Enter);
            
            return str;
        }

        public static (COMMANDS, string[]) ReadCmd(){
            string? cmd = "";
            COMMANDS command = COMMANDS.NONE;
            Console.CursorVisible = true;
            #if DEBUG
                if (true){
            #else
                if (Console.KeyAvailable && Console.ReadKey(true).KeyChar == ':'){
            #endif
                (int w, int h) = Console.GetCursorPosition();
                Console.SetCursorPosition(0, height-1);
                ClearLine();
                Console.Write(":");
            #if DEBUG
                cmd = Console.ReadLine();
                if (cmd is not null)
                    cmd = cmd.ToLower();
                else
                    cmd = "";
            #else
                cmd = UI.ReadLine().ToLower();
            #endif
                command = str2cmd(cmd.Split()[0]);
            #if !DEBUG
                if (h == 0)
                    h ++;
                Console.SetCursorPosition(w, h);
            #endif
            }
            Console.CursorVisible = false;
            return (command, cmd.Split());
        }


        public static void TimeUpdate(){
            time = time.AddMinutes(1);
            UI.DisplayShortList(time.ToString("HH:mm"), line: UI.height-1, width: UI.width-6);

        }
    }
}
