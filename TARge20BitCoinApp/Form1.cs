using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TARge20BitCoinApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGetRates_Click(object sender, EventArgs e)
        {
            AmountErrorTextBox.Visible = false;
            CurrencyErrorTextBox.Visible = false;
            int value;
            if(CurrencyCombo.Text == "Select Currency")
            {
                CurrencyErrorTextBox.Visible = true;
                CurrencyErrorTextBox.Text = "Select a currency!";
                HideResultBox();
                return;
            }

            if (Int32.TryParse(amountOfCoinBox.Text, out value)==false||Int32.Parse(amountOfCoinBox.Text)<= 0)
            {
                AmountErrorTextBox.Visible = true;
                AmountErrorTextBox.Text = "Enter correct coin amount!";
                HideResultBox();
                return;
            }
            if(CurrencyCombo.SelectedItem.ToString()== "EUR")
            {
                ShowResultBox();
                BitCoinRates bitcoin = GetRates();
                float result = Int32.Parse(amountOfCoinBox.Text) * bitcoin.bpi.EUR.rate_float;
                resultTextBox.Text = $"{result.ToString()} {bitcoin.bpi.EUR.code}";
            }

            if (CurrencyCombo.SelectedItem.ToString() == "USD")
            {
                ShowResultBox();
                BitCoinRates bitcoin = GetRates();
                float result = Int32.Parse(amountOfCoinBox.Text) * bitcoin.bpi.USD.rate_float;
                resultTextBox.Text = $"{result.ToString()} {bitcoin.bpi.USD.code}";
            }
            if (CurrencyCombo.SelectedItem.ToString() == "GBP")
            {
                ShowResultBox();
                BitCoinRates bitcoin = GetRates();
                float result = Int32.Parse(amountOfCoinBox.Text) * bitcoin.bpi.GBP.rate_float;
                resultTextBox.Text = $"{result.ToString()} {bitcoin.bpi.GBP.code}";
            }
        }
        private void ShowResultBox()
        {
            resultLabel.Visible = true;
            resultTextBox.Visible = true;
        }
        private void HideResultBox()
        {
            resultLabel.Visible = false;
            resultTextBox.Visible = false;
        }

        public static BitCoinRates GetRates()
        {
            string url = "https://api.coindesk.com/v1/bpi/currentprice.json";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            var webResponse = request.GetResponse();
            var webStream = webResponse.GetResponseStream();

            BitCoinRates bitcoin;
            using(var responseReader = new StreamReader(webStream))
            {
                var response = responseReader.ReadToEnd();
                bitcoin = JsonConvert.DeserializeObject<BitCoinRates>(response);
            }
            return bitcoin;
        }
    }
}
