using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MemLogLib.Diagnostic
{
    public class SimpleLogger
    {
        protected static SimpleLogger instance;
        /// <summary>
        /// True - логи будут дублироваться в консол
        /// </summary>
        protected bool writeConsole;
        #region Синглтон
        protected SimpleLogger(string projectVersion = "", bool writeConsole = false)
        {
            this.writeConsole = writeConsole;
            try
            {
                string msg = new string('-', 10) + '\n';
                if (!string.IsNullOrEmpty(projectVersion))
                    msg += projectVersion + '\n';
                File.AppendAllText(LogPath, msg);
            }
            catch (IOException)
            {

            }
        }

        public static SimpleLogger GetInstance(string projectVersion = "", bool writeConsole = false)
        {
            if (instance == null)
                instance = new SimpleLogger(projectVersion, writeConsole);
            return instance;
        }
        #endregion


        /// <summary>
        /// количество попыток записи в файл
        /// </summary>
        public static int maxAttempts = 2;
        public static string LogPath { get; set; }
            = Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\..\\", "logs.log");

        public void Log(string message)
        {
            if (!File.Exists(LogPath))
                File.Create(LogPath);

            //добавляем время лога
            string time = DateTime.Now.ToString("dd.MM.y HH:mm:ss") + " ";
            if (message[message.Length - 1] != '\n')
                message = time + message + '\n';
            else
                message = time + message;

            int curAttempt = 0;

            while (curAttempt < maxAttempts)
                try
                {
                    curAttempt++;
                    TryWrite(message);
                    break;
                }
                catch (IOException e)
                {
                    Thread.Sleep(500);
                }
        }

        /// <summary>
        /// попытка записать в файл
        /// </summary>
        private void TryWrite(string message)
        {
            File.AppendAllText(LogPath, message);
            if (this.writeConsole)
                Console.WriteLine(message);
        }
    }
}
