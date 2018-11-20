using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static ChartData.ChartData;

namespace WindowsFormsApp9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<MarketData> dataSource;
        private void Form1_Load(object sender, EventArgs e)
        {
            dataSource = ChartData.ChartData.GetData();
            Series series = new DevExpress.XtraCharts.Series("prices", ViewType.CandleStick);
            series.SetFinancialDataMembers("Date", "Low", "High", "Open", "Close");
            chartControl.Series.Add(series);
            //chartControl.CrosshairOptions.CommonLabelPosition = new CrosshairFreePosition() { DockCorner = DockCorner.LeftTop };

            chartControl.DataSource = dataSource;
            DevExpress.XtraCharts.XYDiagram diagram = (DevExpress.XtraCharts.XYDiagram)chartControl.Diagram;
            diagram.EnableAxisXScrolling = diagram.EnableAxisXZooming = true;

            diagram.AxisY.WholeRange.AlwaysShowZeroLevel = false;
            diagram.AxisY.GridLines.MinorVisible = true;

            diagram.AxisX.GridLines.Visible = true;
            diagram.AxisX.GridLines.MinorVisible = true;
            diagram.AxisX.Label.ResolveOverlappingOptions.AllowStagger = false;
            diagram.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Minute;
            diagram.AxisX.VisualRange.MaxValue = (DateTime)diagram.AxisX.VisualRange.MinValue + TimeSpan.FromHours(2);
            diagram.AxisX.DateTimeScaleOptions.WorkdaysOnly = true;
            diagram.AxisX.WholeRange.SideMarginsValue = 2;

            diagram.DependentAxesYRange = DevExpress.Utils.DefaultBoolean.True;
            var view = (XYDiagram2DSeriesViewBase)series.View;
            view.Indicators.Add(new BollingerBands());
            //{ CrosshairContentShowMode = CrosshairContentShowMode.Label, CrosshairEnabled = DevExpress.Utils.DefaultBoolean.True });
            //view.Indicators.Add(new TrendLine() { CrosshairContentShowMode = CrosshairContentShowMode.Label, CrosshairEnabled = DevExpress.Utils.DefaultBoolean.True });

        }
    }
}
