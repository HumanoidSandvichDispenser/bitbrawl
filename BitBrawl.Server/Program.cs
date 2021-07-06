using System;

namespace BitBrawl.Server
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new GameCore(true))
                game.Run();
        }
    }
}
