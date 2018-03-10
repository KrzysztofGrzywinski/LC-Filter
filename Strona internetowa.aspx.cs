using System;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using System.Numerics;
using System.Web.UI.WebControls;

public partial class Strona_internetowa : System.Web.UI.Page
{
    private double Am = 0; //amplituda napięcia zasilającego
    private double fs = 0; //częstotliwośc napięcia zasilającego
    private double L = 0; //wartość indukcyjności cewki w mH
    private double C = 0; //wartość pojemności kondensatora w uF
    private double R1 = 0; //wartość rezystancji rezystora R1 w Ohm
    private double R2 = 0; //wartość rezystancji rezystora R2 w Ohm
    private int fmin = 0; //dolna granica częstotliwośći pokazywanej
    private int fmax = 0; //górna granica częstotliwośći pokazywanej
    private int freq = 0;
    private double omega = 0, omega1=0;
    private Complex Z1;
    private Complex Z2;
    private Complex R1z;
    private Complex R2z;
    private Complex Xlz;
    private Complex Xcz;
    private Complex ku;
    private Complex Iwe;


    private double[,] Results; //tablica z wynikami


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            MultiView1.Visible = false;
        }
        else
        {

            MultiView1.Visible = true;
            messageLabel.Text = "";
        }

        MaintainScrollPositionOnPostBack = true;
    }


    protected void drawButton_Click(object sender, EventArgs e)
    {
        if (!Double.TryParse(txtVoltU1.Text, out Am))
        {
            messageLabel.Text = "Błedna wartość amplitudy";
            MultiView1.Visible = false;
            return;
        }
        else if (Am <= 0)
        {
            messageLabel.Text = "Wartość amplitudy nie moze byc ujemna";
            MultiView1.Visible = false;
            return;
        }
        if (!Double.TryParse(txtFreqU1.Text, out fs))
        {
            messageLabel.Text = "Błedna wartość czestotliwosci";
            MultiView1.Visible = false;
            return;
        }
        else if (fs <= 0)
        {
            messageLabel.Text = "Wartość częstotliwości nie moze byc ujemna";
            MultiView1.Visible = false;
            return;
        }
        if (!Double.TryParse(txtL.Text, out L))
        {
            messageLabel.Text = "Błedna wartość indukcyjności C";
            MultiView1.Visible = false;
            return;
        }
        else if (L < 0)
        {
            messageLabel.Text = "Wartość indukcyjności nie moze byc ujemna";
            MultiView1.Visible = false;
            return;
        }
        if (!Double.TryParse(txtC.Text, out C))
        {
            messageLabel.Text = "Błedna wartość pojemności C";
            MultiView1.Visible = false;
            return;
        }
        else if (C < 0)
        {
            messageLabel.Text = "Wartość pojemności nie moze byc ujemna";
            MultiView1.Visible = false;
            return;
        }
        if (!Double.TryParse(txtR1.Text, out R1))
        {
            messageLabel.Text = "Błedna wartość rezystancji R1";
            MultiView1.Visible = false;
            return;
        }
        else if (R1 < 0)
        {
            messageLabel.Text = "Wartość rezystancji R1 nie moze byc ujemna";
            MultiView1.Visible = false;
            return;
        }
        if (!Double.TryParse(txtR2.Text, out R2))
        {
            messageLabel.Text = "Błedna wartość rezystancji R2";
            MultiView1.Visible = false;
            return;
        }
        else if (R2 < 0)
        {
            messageLabel.Text = "Wartość rezystancji R2 nie moze byc ujemna";
            MultiView1.Visible = false;
            return;
        }
        if (!int.TryParse(txtMinRangeFreq.Text, out fmin))
        {
            messageLabel.Text = "Błedna wartość dolnego zakresu częstotliwości f";
            MultiView1.Visible = false;
            return;
        }
        else if (fmin <= 0)
        {
            messageLabel.Text = "Wartość częstotliwości nie moze byc ujemna lub równa 0";
            MultiView1.Visible = false;
            return;
        }
        if (!int.TryParse(txtMaxRangeFreq.Text, out fmax))
        {
            messageLabel.Text = "Błedna wartość górnego zakresu częstotliwości f";
            MultiView1.Visible = false;
            return;
        }
        else if (fmax <= 0)
        {
            messageLabel.Text = "Wartość częstotliwości nie moze byc ujemna lub równa 0";
            MultiView1.Visible = false;
            return;
        }

        Results = new double[fmax, 5];
        for (int i = 0; i <= fmax - fmin; i++)
        {
            freq = i + 1;

            omega = 2 * Math.PI * freq;
            omega1 = 2 * Math.PI * fs;
            R1z = new Complex(R1, 0);
            R2z = new Complex(R2, 0);
            Xlz = new Complex(0, omega * (L / 1000));
            Xcz = new Complex(0, -1 / (omega * (C / 1000000)));
            Z1 = Complex.Add(R2z, Xcz);
            Z2 = Complex.Divide(Complex.Multiply(R1z, Z1), Complex.Add(R1z, Z1));
            ku = Complex.Multiply(Complex.Divide(Z2, Complex.Add(Xlz, Z2)), Complex.Divide(R2z, Complex.Add(R2, Xcz)));
            Iwe = Complex.Divide(Am, Complex.Add(Z2, Xlz));

            Results[i, 0] = freq;
            Results[i, 1] = ku.Magnitude;
            Results[i, 2] = ku.Phase;
            Results[i, 3] = Iwe.Magnitude;
            Results[i, 4] = -Iwe.Magnitude;
        }

        Draw();

    }

    private void Draw()
    {
        DataTable table, table2, table3;
        DataView dView;
 
        table = new DataTable();
        table2 = new DataTable();
        table3 = new DataTable();
        DataColumn column;
        DataRow row;

        column = new DataColumn();
        column.DataType = System.Type.GetType("System.Double");
        column.ColumnName = "Częstotliwość";
        table.Columns.Add(column);

        column = new DataColumn();
        column.DataType = System.Type.GetType("System.Double");
        column.ColumnName = "Widmo amplitudowe";
        table.Columns.Add(column);

        column = new DataColumn();
        column.DataType = System.Type.GetType("System.Double");
        column.ColumnName = "Częstotliwość";
        table2.Columns.Add(column);

        column = new DataColumn();
        column.DataType = System.Type.GetType("System.Double");
        column.ColumnName = "Widmo fazowe";
        table2.Columns.Add(column);

        column = new DataColumn();
        column.DataType = System.Type.GetType("System.Double");
        column.ColumnName = "Częstotliwość";
        table3.Columns.Add(column);

        column = new DataColumn();
        column.DataType = System.Type.GetType("System.Double");
        column.ColumnName = "Obwiednia prądu";
        table3.Columns.Add(column);

        column = new DataColumn();
        column.DataType = System.Type.GetType("System.Double");
        column.ColumnName = "-Obwiednia prądu";
        table3.Columns.Add(column);


        for (int i = 0; i <= fmax - fmin; i++)
        {
            row = table.NewRow();
            row["Częstotliwość"] = Results[i, 0];
            row["Widmo amplitudowe"] = Results[i, 1];
            table.Rows.Add(row);
        }
        dView = new DataView(table);
        Chart1.Series.Clear();
        Chart1.DataBindTable(dView, "Częstotliwość");
        Chart1.Series["Widmo amplitudowe"].ChartType = SeriesChartType.Line;
        Chart1.ChartAreas[0].AxisX.LabelStyle.Format = "{#0}";
        Chart1.Height = 600;
        Chart1.Width = 1500;
        Chart1.ChartAreas[0].BackColor = System.Drawing.Color.Azure;
        Chart1.BackColor = System.Drawing.Color.SeaShell;
        Chart1.ChartAreas[0].AxisX.LineWidth = 3;
        Chart1.ChartAreas[0].AxisX.Title = "f [Hz]";
        Chart1.ChartAreas[0].AxisY.Title = "ku [dB]";
        Chart1.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Italic);
        Chart1.ChartAreas[0].AxisX.LogarithmBase = 10;
        Chart1.ChartAreas[0].AxisX.IsLogarithmic = true;
        Chart1.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
        Chart1.Titles.Add("Widmo amplitudowe");
        Chart1.Titles[0].Font = new System.Drawing.Font("Times New Roman", 16F, System.Drawing.FontStyle.Bold);
        Chart1.Titles[0].ForeColor = System.Drawing.Color.Black;
        Chart1.Legends[0].DockedToChartArea = Chart1.ChartAreas[0].Name;
        Chart1.Legends[0].Docking = Docking.Right;
        Chart1.ChartAreas[0].AxisX.Minimum = fmin;
        Chart1.ChartAreas[0].AxisX.Maximum = fmax;

        for (int i = 0; i <= fmax - fmin; i++)
        {
            row = table2.NewRow();
            row["Częstotliwość"] = Results[i, 0];
            row["Widmo fazowe"] = Results[i, 2];
            table2.Rows.Add(row);
        }
        dView = new DataView(table2);
        Chart2.Series.Clear();
        Chart2.DataBindTable(dView, "Częstotliwość");
        Chart2.Series["Widmo fazowe"].ChartType = SeriesChartType.Line;
        Chart2.ChartAreas[0].AxisX.LabelStyle.Format = "{#0}";
        Chart2.Height = 600;
        Chart2.Width = 1500;
        Chart2.ChartAreas[0].BackColor = System.Drawing.Color.Azure;
        Chart2.BackColor = System.Drawing.Color.SeaShell;
        Chart2.ChartAreas[0].AxisX.LineWidth = 3;
        Chart2.ChartAreas[0].AxisX.Title = "f [Hz]";
        Chart2.ChartAreas[0].AxisY.Title = "fi [rad]";
        Chart2.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Italic);
        Chart2.ChartAreas[0].AxisX.LogarithmBase = 10;
        Chart2.ChartAreas[0].AxisX.IsLogarithmic = true;
        Chart2.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
        Chart2.Titles.Add("Widmo fazowe");
        Chart2.Titles[0].Font = new System.Drawing.Font("Times New Roman", 16F, System.Drawing.FontStyle.Bold);
        Chart2.Titles[0].ForeColor = System.Drawing.Color.Black;
        Chart2.Legends[0].DockedToChartArea = Chart2.ChartAreas[0].Name;
        Chart2.Legends[0].Docking = Docking.Right;
        Chart2.ChartAreas[0].AxisX.Minimum = fmin;
        Chart2.ChartAreas[0].AxisX.Maximum = fmax;

        for (int i = 0; i <= fmax - fmin; i++)
        {
            row = table3.NewRow();
            row["Częstotliwość"] = Results[i, 0];
            row["Obwiednia prądu"] = Results[i, 3];
            row["-Obwiednia prądu"] = Results[i, 4];
            table3.Rows.Add(row);
        }
        dView = new DataView(table3);
        Chart3.Series.Clear();
        Chart3.DataBindTable(dView, "Częstotliwość");
        Chart3.Series["Obwiednia prądu"].ChartType = SeriesChartType.Line;
        Chart3.Series["-Obwiednia prądu"].ChartType = SeriesChartType.Line;
        Chart3.ChartAreas[0].AxisX.LabelStyle.Format = "{#0}";
        Chart3.Height = 600;
        Chart3.Width = 1500;
        Chart3.ChartAreas[0].BackColor = System.Drawing.Color.Azure;
        //Chart3.BackImage = "~/backround2.png";
        Chart3.ChartAreas[0].AxisX.LineWidth = 3;
        Chart3.ChartAreas[0].AxisX.Title = "f [Hz]";
        Chart3.ChartAreas[0].AxisY.Title = "I [A]";
        Chart3.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Italic);
        Chart3.ChartAreas[0].AxisX.LogarithmBase = 10;
        Chart3.ChartAreas[0].AxisX.IsLogarithmic = true;
        Chart3.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
        Chart3.Titles.Add("Obwiednia prądu zasilającego");
        Chart3.Titles[0].Font = new System.Drawing.Font("Times New Roman", 16F, System.Drawing.FontStyle.Bold);
        Chart3.Titles[0].ForeColor = System.Drawing.Color.Black;
        Chart3.Legends[0].DockedToChartArea = Chart2.ChartAreas[0].Name;
        Chart3.Legends[0].Docking = Docking.Right;
        Chart3.ChartAreas[0].AxisX.Minimum = fmin;
        Chart3.ChartAreas[0].AxisX.Maximum = fmax;

    }


    protected void multiView_Command(object sender, CommandEventArgs e)
    {
        switch ((string)e.CommandName)
        {
            case "WidmoAmplitudowe":
                {

                    MultiView1.SetActiveView(widmoAmplitudowe);
                    break;

                }
            case "WidmoFazowe":
                {
                    MultiView1.SetActiveView(widmoFazowe);
                    break;
                }
            case "ObwiedniaPradu":
                {
                    MultiView1.SetActiveView(obwiedniaPradu);
                    break;
                }

        }
    }
}