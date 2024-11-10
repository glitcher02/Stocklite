using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Stocklite1.Form2;
using System.Windows.Forms.DataVisualization.Charting;
using OpenTK.Graphics.OpenGL;

namespace Stocklite1
{

    /// <summary>
    /// Represents the second form in the application.
    /// </summary>
    public partial class Form2 : Form
    {
        private List<StockData> stockDataList;
        private int currentIndex = 0;
        private Timer updateTimer;
        /// <summary>
        /// Initializes a new instance of the <see cref="Form2"/> class.
        /// </summary>
        public Form2()
        {
            InitializeComponent();
            LoadChartData();

            chart1.Click += new EventHandler(chart1_Click);
        }

        /// <summary>
        /// Handles the chart click event to reset zoom.
        /// </summary>
        private void chart1_Click(object sender, EventArgs e)
        {
            ///chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset();
            ///chart1.ChartAreas[0].AxisY.ScaleView.ZoomReset();
        }

        /// <summary>
        /// Handles the form closing event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
            }
        }

        private void LoadChartData()
        {
            string jsonFilePath = "C:\\Users\\archie\\source\\repos\\Stocklite1\\Stocklite1\\Resources\\appledata.json";
            string jsonData = File.ReadAllText(jsonFilePath);

            RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(jsonData);
            List<StockData> stockDataList = rootObject.results;

            chart1.Series.Clear();
            Series series = new Series("EUR/USD");
            series.ChartType = SeriesChartType.Candlestick;

            series["OpenCloseStyle"] = "Triangle"; //type of opening and closing price
            series["ShowOpenClose"] = "Both"; //shows open and close price
            series["PointWidth"] = "0.5"; //width of candle
            series["PriceUpColor"] = "Green"; // Color for price increase
            series["PriceDownColor"] = "Red"; // Color for price decrease

            double minY = double.MaxValue;
            double maxY = double.MinValue;

            foreach (var data in stockDataList)
            {
                DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(data.t).DateTime;

                DataPoint dataPoint = new DataPoint();
                dataPoint.XValue = dateTime.ToOADate();
                dataPoint.YValues = new double[] { (double)data.h, (double)data.l, (double)data.o, (double)data.c };
                series.Points.Add(dataPoint);

                minY = Math.Min(minY, (double)data.l);
                maxY = Math.Max(maxY, (double)data.h);
            }

            chart1.Series.Add(series);

            chart1.ChartAreas[0].AxisY.Minimum = minY - (maxY - minY) * 0.1;
            chart1.ChartAreas[0].AxisY.Maximum = maxY + (maxY - minY) * 0.1;

            //Preseting correct time
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd"; // Display dates in a readable format
            chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days; // Use days as the interval type
            chart1.ChartAreas[0].AxisX.Interval = 2; // Set interval to 1 day to make it more readable
                                                     //chart1.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;

            //Time Customisation
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            //Scale
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;

            chart1.ChartAreas[0].AxisX.ScaleView.Size = 30;
            

            chart1.ChartAreas[0].AxisX.ScaleView.MinSize = 1;
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = 5;

            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.Size = 15;
            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart1.ChartAreas[0].AxisX.ScrollBar.BackColor = Color.Black;

            chart1.Invalidate();
        }

        /// <summary>
        /// Represents stock data.
        /// </summary>
        public class StockData
        {
            /// <summary>
            /// Gets or sets the timestamp.
            /// </summary>
            public long t { get; set; }

            /// <summary>
            /// Gets or sets the opening price.
            /// </summary>
            public decimal o { get; set; }

            /// <summary>
            /// Gets or sets the highest price.
            /// </summary>
            public decimal h { get; set; }

            /// <summary>
            /// Gets or sets the lowest price.
            /// </summary>
            public decimal l { get; set; }

            /// <summary>
            /// Gets or sets the closing price.
            /// </summary>
            public decimal c { get; set; }
        }

        /// <summary>
        /// Represents the root object for JSON deserialization.
        /// </summary>
        public class RootObject
        {
            /// <summary>
            /// Gets or sets the ticker symbol.
            /// </summary>
            public string ticker { get; set; }

            /// <summary>
            /// Gets or sets the query count.
            /// </summary>
            public int queryCount { get; set; }

            /// <summary>
            /// Gets or sets the results count.
            /// </summary>
            public int resultsCount { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the data is adjusted.
            /// </summary>
            public bool adjusted { get; set; }

            /// <summary>
            /// Gets or sets the list of stock data results.
            /// </summary>
            public List<StockData> results { get; set; }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Your initialization code for the form goes here
        }


    }

      

    }

