using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stocklite1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            
            Stocklite1.LoadingPage loadingForm = new Stocklite1.LoadingPage();
            loadingForm.Show();

            
            System.Threading.Tasks.Task.Delay(3000).Wait(); // 3 seconds

            
            loadingForm.Close();

            
            Application.Run(new Form1());
        }
    }
}
