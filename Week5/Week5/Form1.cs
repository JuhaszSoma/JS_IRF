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


namespace Week5
{
    public partial class Form1 : Form
    {
        MNBArfolyamServiceSoapClient mnbService = new MNBArfolyamServiceSoapClient();
        GetExchangeRatesRequestBody request = new GetExchangeRatesRequestBody()
        {
            currencyNames = "EUR",
            startDate = "2020-01-01",
            endDate = "2020-06-30"
        };


        public Form1()
        {
            InitializeComponent();
            var request = mnbService.GetExchangeRates(request);
        }

        public void GetExchangeRates()
        {
            
        }
    }
}
