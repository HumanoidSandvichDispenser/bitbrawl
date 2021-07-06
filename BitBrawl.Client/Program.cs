using System;

namespace BitBrawl.Client
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new GameCore(false))
                game.Run();
        }
    }
}
