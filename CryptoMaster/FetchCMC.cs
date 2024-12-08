namespace CryptoMaster
{
    using System.Web;
    using System.Net;
    using System;
    using Newtonsoft.Json;
    using CryptoMaster.Models;

    public class FetchCMC
    {
        public static void saveCMCData()
        {
            using var dbContext = new CryptoDbContex();

            var coinsToRemove = dbContext.Coins.ToList(); 

            foreach (var coin in coinsToRemove)
            {
                dbContext.Remove(coin);
                dbContext.SaveChanges(); 
            }

            var URL = new UriBuilder("https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest");

            var CMC_API_KEY = "c846b350-4645-4bcd-81c1-b71119b8bcb7";
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["start"] = "1";
            queryString["limit"] = "100";
            queryString["convert"] = "USD";

            URL.Query = queryString.ToString();

            var client = new WebClient();
            client.Headers.Add("X-CMC_PRO_API_KEY", CMC_API_KEY);
            client.Headers.Add("Accepts", "application/json");
            string result =  client.DownloadString(URL.ToString());

            var data = JsonConvert.DeserializeObject<CryptoApiResponse>(result);

            if (data != null)
            {
                foreach (var coin in data.Data)
                {
                    var name = coin.Name;
                    var cmcRank = coin.CMC_Rank;
                    var usdQuote = coin.Quote.Usd;
                    var symbol = coin.Symbol; 

                    var savedCoin = new CryptoModel();

                    savedCoin.Name = name;
                    savedCoin.Ticker = symbol;
                    savedCoin.TotalSupply = 0;
                    savedCoin.Price = usdQuote.Price;

                    dbContext.Add(savedCoin);
                    dbContext.SaveChanges(); 
                }
            }
        }
        public class CryptoApiResponse
        {
            public CryptoCoin[] Data { get; set; }
        }

        public class CryptoCoin
        {
            public int CMC_Rank { get; set; }
            public string Name { get; set; }
            public string Symbol { get; set; }
            public string Slug { get; set; }
            public CryptoQuote Quote { get; set; }
        }

        public class CryptoQuote
        {
            [JsonProperty("USD")]
            public UsdQuote Usd { get; set; }
        }

        public class UsdQuote
        {
            public double? Price { get; set; }
        }
    }
}
