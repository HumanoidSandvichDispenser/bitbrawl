using RedGrin.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitBrawl.Network
{
    /// <summary>
    /// Manages network logs
    /// </summary>
    public class Logger : ILogger
    {
        public LogLevels Level { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Log(string message)
        {
            if (GameCore.IsServer)
            {
                Console.WriteLine(message);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(message);
                //Nez.Console.DebugConsole.Instance.Log(message);
            }
        }

        public void Debug(string message)
        {
            Console.WriteLine(message);
        }

        public void Error(string message)
        {
            message = $"[{DateTime.Now}] **ERROR** {message}";
            Log(message);
        }

        public void Info(string message)
        {
            message = $"[{DateTime.Now}] {message}";
            Log(message);
        }

        public void Warning(string message)
        {
            message = $"[{DateTime.Now}] **WARN** {message}";
            Log(message);
        }
    }
}
