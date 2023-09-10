using System;
using System.Net;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

class ExchangeRateService
{
    private const string ApiUrl = "https://api.exchangerate-api.com/v4/latest/USD";

    public static decimal GetExchangeRate(string fromCurrency, string toCurrency)
    {
        try
        {
            using (var webClient = new WebClient())
            {
                string json = webClient.DownloadString(ApiUrl);
                JObject exchangeRates = JObject.Parse(json);

                decimal fromRate = exchangeRates["rates"][fromCurrency].Value<decimal>();
                decimal toRate = exchangeRates["rates"][toCurrency].Value<decimal>();

                decimal exchangeRate = toRate / fromRate;

                return exchangeRate;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return -1;
        }
    }
}

