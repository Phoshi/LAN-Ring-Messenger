using System.IO;
using Extensions;
using System;

namespace RingLAN {
    static class Logger {
        private const string path = "output.log";
        private const string rawPath = "packets.log";
        public static bool Logging = true;

        static Logger() {
            File.Delete(path);
            File.Delete(rawPath);
        }

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
