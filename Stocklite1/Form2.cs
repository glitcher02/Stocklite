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
        private HorizontalLineAnnotation currentPriceLine;
        private bool hasActiveBuy = false; // To track if there's an active buy
        private double buyPrice; // Stores the price at which the user bought
        private HorizontalLineAnnotation buyLine; // Annotation for the buy line
        private double leverage = 500;
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

            updateTimer = new Timer();
            updateTimer.Interval = 200; // 2 seconds
            updateTimer.Tick += UpdateChart;
            updateTimer.Start();

            currentPriceLine = new HorizontalLineAnnotation
            {
                AxisX = chart1.ChartAreas[0].AxisX,
                AxisY = chart1.ChartAreas[0].AxisY,
                IsInfinitive = true,
                LineColor = Color.Red,
                LineWidth = 2,
                Y = 0 // This will be updated with the latest price
            };
            chart1.Annotations.Add(currentPriceLine);




            chart1.Click += new EventHandler(chart1_Click);
        }

        /// <summary>
        /// Handles the chart click event to reset zoom.
        /// </summary>
        private void chart1_Click(object sender, EventArgs e)
        {
          
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
            stockDataList = rootObject.results;

            InitializeChart();
           
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
        private void InitializeChart()
        {
            chart1.Series.Clear();
            Series series = new Series("EUR/USD");
            series.ChartType = SeriesChartType.Candlestick;
            series["OpenCloseStyle"] = "Triangle";
            series["ShowOpenClose"] = "Both";
            series["PointWidth"] = "0.5";
            series["PriceUpColor"] = "Green";
            series["PriceDownColor"] = "Red";

            chart1.Series.Add(series);

            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd";
            chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
            chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;

            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.Size = 15;
            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart1.ChartAreas[0].AxisX.ScrollBar.BackColor = Color.Black;

            chart1.Invalidate();
        }
        private void UpdateChart(object sender, EventArgs e)
        {
            if (currentIndex >= stockDataList.Count)
            {
                updateTimer.Stop(); // Stop when all data is displayed
                return;
            }

            var data = stockDataList[currentIndex];
            DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(data.t).DateTime;

            currentPriceLine.Y = (double)data.c;

            DataPoint dataPoint = new DataPoint();
            dataPoint.XValue = dateTime.ToOADate();
            dataPoint.YValues = new double[] { (double)data.h, (double)data.l, (double)data.o, (double)data.c };
            chart1.Series[0].Points.Add(dataPoint);

            // Adjust visible points based on the track bar value
            int visiblePoints = Math.Max(1, trackBar1.Value); // Use the track bar value to set how many points are visible
            if (chart1.Series[0].Points.Count > visiblePoints)
            {
               // chart1.ChartAreas[0].AxisX.ScaleView.Size = visiblePoints;
            }
            else
            {
               // chart1.ChartAreas[0].AxisX.ScaleView.Size = chart1.Series[0].Points.Count;
            }

            // Adjust Y-axis dynamically based on the visible points
            var visibleDataPoints = chart1.Series[0].Points
                .Skip(Math.Max(0, chart1.Series[0].Points.Count - visiblePoints))
                .ToList();

            double minY = visibleDataPoints.Min(p => p.YValues[1]); // Minimum of the "low" values in the visible range
            double maxY = visibleDataPoints.Max(p => p.YValues[0]); // Maximum of the "high" values in the visible range

            // Add a margin to the Y-axis for better visualization
            double margin = (maxY - minY) * 0.1;
            chart1.ChartAreas[0].AxisY.Minimum = minY - margin;
            chart1.ChartAreas[0].AxisY.Maximum = maxY + margin;

            if (hasActiveBuy)
            {
                double currentPrice = (double)data.c;
                double profitLoss = (currentPrice - buyPrice) * leverage;
                textBox1.Text = $"Profit/Loss: {profitLoss}";
            }

            currentIndex++;
            chart1.Invalidate();

            




        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (updateTimer.Enabled)
            {
                updateTimer.Stop();
                button1.Text = "Start";
            }
            else
            {
                updateTimer.Start();
                button1.Text = "Pause";
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            // Ensure the value is at least 1 to avoid setting an invalid zoom level
            int zoomLevel = Math.Max(1, trackBar1.Value);

            // Apply the zoom level to the X-axis
            chart1.ChartAreas[0].AxisX.ScaleView.Size = zoomLevel;
        }

        private void Buy_button_Click(object sender, EventArgs e)
        {
            if (hasActiveBuy)
            {
                MessageBox.Show("You already have an active buy. Sell first before buying again.");
                return;
            }

            // Set buy price to the current price
            buyPrice = currentPriceLine.Y;
            hasActiveBuy = true;

            // Create an annotation line at the buy price
            buyLine = new HorizontalLineAnnotation
            {
                AxisX = chart1.ChartAreas[0].AxisX,
                AxisY = chart1.ChartAreas[0].AxisY,
                IsInfinitive = true,
                LineColor = Color.Green,
                LineWidth = 2,
                Y = buyPrice
            };
            chart1.Annotations.Add(buyLine);
        }

        private void Sell_button_Click(object sender, EventArgs e)
        {
            if (!hasActiveBuy)
            {
                MessageBox.Show("You need to buy before selling.");
                return;
            }

            // Calculate profit or loss
            double sellPrice = currentPriceLine.Y;
            double profitLoss = (sellPrice - buyPrice) * leverage;

            // Display profit or loss
            MessageBox.Show($"Sell Price: {sellPrice}\nBuy Price: {buyPrice}\nProfit/Loss: {profitLoss}");

            // Remove buy line and reset
            chart1.Annotations.Remove(buyLine);
            buyLine = null;
            hasActiveBuy = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }



}

