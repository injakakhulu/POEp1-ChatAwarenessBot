using POEp1.Forms;
using System;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System;
using System.Windows.Forms;
using POEp1.Forms;

using System;
using System.Windows.Forms;
using POEp1.Forms;

namespace POEp1
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Force the WinForms Application explicitly
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            // Fully qualify the form
            System.Windows.Forms.Application.Run(new POEp1.Forms.MainForm());
        }
    }
}