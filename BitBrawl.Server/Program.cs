using System;

namespace BitBrawl.Server
{
    class Program
    {
        static System.Threading.Thread CommandLineThread;
        
        static void Main()
        {
            CommandLineThread = new System.Threading.Thread(ReadEvalPrintLoop);
            CommandLineThread.Start();

            using (var game = new GameCore(true, false))
            {
                game.Run();
            }
        }

        static void ReadEvalPrintLoop()
        {
            string cmd;
            while ((cmd = Console.ReadLine()) != "exit")
            {
                Console.WriteLine("yo lesgo");
            }
        }
    }
}
