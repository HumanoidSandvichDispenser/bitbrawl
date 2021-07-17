using System;

namespace BitBrawl.Client
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            GameCore.PauseOnFocusLost = false;
            using (var game = new GameCore(false, true))
                game.Run();
        }
    }
}
