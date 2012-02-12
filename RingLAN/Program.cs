using System;
using System.Windows.Forms;

namespace RingLAN {
    /// <summary>
    /// Author Name:    Andrew Heaford
    /// Date:           2012/02/12
    /// Description:    IM Client that supports communications over a serial ring network.
    /// Advice:         Try not to die.
    /// </summary>
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Logger.Log("Startup.");
            Application.Run(new Startup());
        }
    }
}
