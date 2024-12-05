using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.IO;
using System.Net;
using System.Security.Policy;

namespace Stocklite1
{
    static class Program
    {
        private static readonly HttpClient client = new HttpClient();
        /// <summary>
        /// The main starting point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //string appleapiUrl = "https://api.polygon.io/v2/aggs/ticker/AAPL/range/1/day/2024-07-01/2024-10-06?adjusted=true&sort=asc&apiKey=ea0K7Sr2HDr2rmNNq3rR2W9siLWxSoG5";
            //string appleapiUrl = "https://api.polygon.io/v2/aggs/ticker/C:EURUSD/range/1/day/2023-01-09/2024-10-10?adjusted=true&sort=asc&apiKey=IuP6zivLRuSGEb2N_nPt2XRde0pOH3_m";
            string appleapiUrl = "https://api.polygon.io/v2/aggs/ticker/AAPL/range/1/day/2023-01-09/2024-11-20?adjusted=true&sort=asc&apiKey=IuP6zivLRuSGEb2N_nPt2XRde0pOH3_m";



            Stocklite1.LoadingPage loadingForm = new Stocklite1.LoadingPage();

            loadingForm.Show();
            Console.WriteLine("Loading page shown");
            WebClient wc = new WebClient();
            string content = wc.DownloadString(appleapiUrl);

            Console.WriteLine(content);
                string filePath = "C:\\Users\\archie\\source\\repos\\Stocklite1\\Stocklite1\\Resources\\appledata.json";
            

                
            File.WriteAllText(filePath, content);

            Console.WriteLine("Data saved to " + filePath);
            //}
            //else
            //{
            //    Console.WriteLine("Failed to fetch data. Status code: " + response.StatusCode);
            //}

            //System.Threading.Tasks.Task.Delay(3000).Wait(); // 3 seconds

            loadingForm.Close();
            Console.WriteLine("Loading page closed");
            Application.Run(new Form1());
            Console.WriteLine("Form1 Opened");
        }
    }
}
