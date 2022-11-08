using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Week5.MnbServiceReference;
using Week5.Entities;
using System.Xml;
using System.Windows.Forms.DataVisualization.Charting;

namespace Week5
{
    public partial class Form1 : Form
    {
        MNBArfolyamServiceSoapClient mnbService = new MNBArfolyamServiceSoapClient();
        BindingList<RateData> Rates = new BindingList<RateData>();
        BindingList<string> Currencies = new BindingList<string>();

        public Form1()
        {
            InitializeComponent();
            GetCurrencies();
            comboBox1.DataSource = Currencies;
            RefreshData();

        }

        public string GetExchangeRates()
        {
            GetExchangeRatesRequestBody request = new GetExchangeRatesRequestBody()
            {
                currencyNames = (string)comboBox1.SelectedItem,
                startDate = dateTimePicker1.Value.ToString(),
                endDate = dateTimePicker2.Value.ToString()
            };

            var response = mnbService.GetExchangeRates(request);
            var result = response.GetExchangeRatesResult;
            return result;

        }

        public void Kitoltes(string xmlfajl)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlfajl);

            foreach (XmlElement elem in xml.DocumentElement)
            {
                RateData valtoInfo = new RateData();
                Rates.Add(valtoInfo);
                valtoInfo.Date = DateTime.Parse(elem.GetAttribute("date"));
                var childElement = (XmlElement)elem.ChildNodes[0];
                if (childElement == null)
                    continue;
                valtoInfo.Currency = childElement.GetAttribute("curr");
                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit != 0)
                {
                    valtoInfo.Value = value / unit / 100;
                }
            }
        }
        private void DiagramAdatok()
        {
            var chartElsoElem = chartRateData.Series[0];
            chartElsoElem.ChartType = SeriesChartType.Line;
            chartElsoElem.XValueMember = "Date";
            chartElsoElem.YValueMembers = "Value";
            chartElsoElem.BorderWidth = 2;

            var legend = chartRateData.Legends[0];
            legend.Enabled = false;

            var chartArea = chartRateData.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;
        }
        private void RefreshData()
        {
            Rates.Clear();
            Kitoltes(GetExchangeRates());
            dataGridView1.DataSource = Rates;
            chartRateData.DataSource = Rates;
            DiagramAdatok();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
        private void GetCurrencies()
        {
            GetCurrenciesRequestBody requestBodyC = new GetCurrenciesRequestBody();
            var response = mnbService.GetCurrencies(requestBodyC);
            var result = response.GetCurrenciesResult;
            //
            //
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);


            XmlElement elem = xml.DocumentElement;
            foreach (XmlElement child in elem.ChildNodes[0])
            {
                string actualCurrency = child.InnerText;
                Currencies.Add(actualCurrency);
            }
        }
    }
}
