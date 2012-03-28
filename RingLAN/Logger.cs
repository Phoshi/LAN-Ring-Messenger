using System.IO;
using Extensions;
using System;

namespace RingLAN {
    /// <summary>
    /// Class to provide bespoke logging capabilities to either a file or stdout
    /// </summary>
    static class Logger {
        /// <summary>
        /// The path to log formatted output to
        /// </summary>
        private const string path = "output.log";
        /// <summary>
        /// The path to log packets recieved to
        /// </summary>
        private const string rawPath = "packets.log";

        /// <summary>
        /// Whether logging is enabled
        /// </summary>
        public static bool Logging = true;

        /// <summary>
        /// Initialises the logger 
        /// </summary>
        static Logger() {
            File.Delete(path);
            File.Delete(rawPath);
        }

        /// <summary>
        /// Logs a formatted string
        /// </summary>
        /// <param name="text">The message</param>
        /// <param name="section">The type of message</param>
        public static void Log(string text, string section = null) {
            if (!Logging){
                Console.WriteLine("{0}: {1}".With(section, text.Replace('\0', ' ')));
                return;
            }
            try {
                StreamWriter logger = new StreamWriter(path, true);
                if (section == null) {
                    section = "Global";
                }
                logger.WriteLine("{0}: {1}".With(section, text));
                logger.Close();
            }
            catch (IOException) {
                //Logger failed, better log that.
                //Wait...
            }
        }

        /// <summary>
        /// Logs a byte
        /// </summary>
        /// <param name="rawByte">The byte to log</param>
        public static void Log(byte rawByte)
        {
            if (!Logging){
                return;
            }
            try
            {
                StreamWriter logger = new StreamWriter(rawPath, true);
                logger.Write((char)rawByte);
                logger.Close();
            }
            catch (IOException)
            {
                //Logger failed, better log that.
                //Wait...
            }
        }
    }
}
