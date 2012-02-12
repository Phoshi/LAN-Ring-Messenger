using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Extensions;

namespace RingLAN {
    static class Logger {
        private static string path = "output.log";

        static Logger() {
            File.Delete(path);
        }

        public static void Log(string text, string section = null) {
            try {
                StreamWriter logger = new StreamWriter(path, true);
                if (section == null) {
                    section = "Global";
                }
                logger.WriteLine("{0}: {1}".With(section, text));
                logger.Close();
            }
            catch (IOException ex) {
                //Logger failed, better log that.
            }
        }
    }
}
