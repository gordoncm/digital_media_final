using Newtonsoft.Json;
using static CryptoMaster.FetchCMC;
using System.Net;
using CryptoMaster.Models;

namespace CryptoMaster
{
    public class GetPortfolioAPI
    {
        public static List<PortfolioModel> GetFromDB(string userName)
        {
            using var db = new CryptoDbContex(); 

            var portfolios = db.Portfolio.Where(p => p.Username == userName).ToList();

            return portfolios;
        }
        public static List<PortfolioModel> GetPortfolioTokens(string tokenAddress, string userName)
        {
            using var db = new CryptoDbContex();

            foreach (var coin in db.Portfolio)
            {
                db.Remove(coin);
                db.SaveChanges(); 
            }

            var URL = new UriBuilder("https://api.bscscan.com/api?module=account&action=tokentx&address="+tokenAddress+"&page=1&offset=5&startblock=0&endblock=999999999&sort=asc&apikey=YJSVURH74MR9MZWDUBN4EUXXU2W4ZK2BYW");

            var client = new WebClient(); 
            string result = client.DownloadString(URL.ToString());

            var data = JsonConvert.DeserializeObject<Root>(result).result;

            List<PortfolioModel> models = new(); 

            foreach (var coin in data)
            {
                string tokenSymbol = coin.tokenSymbol;
                string tokenName = coin.tokenName;
                double coinValue = 0;

                double.TryParse(coin.value, out coinValue);

                var portfolioToken = new PortfolioModel(); 

                portfolioToken.Balance = coinValue;
                portfolioToken.TokenName = tokenName;
                portfolioToken.Username = userName;

                db.Add(portfolioToken);
                db.SaveChanges();

                models.Add(portfolioToken); 
            }

            return models; 
        }
    }

    public class Result
    {
        public string blockNumber { get; set; }
        public string timeStamp { get; set; }
        public string hash { get; set; }
        public string nonce { get; set; }
        public string blockHash { get; set; }
        public string from { get; set; }
        public string contractAddress { get; set; }
        public string to { get; set; }
        public string? value { get; set; }
        public string tokenName { get; set; }
        public string tokenSymbol { get; set; }
        public string tokenDecimal { get; set; }
        public string transactionIndex { get; set; }
        public string gas { get; set; }
        public string gasPrice { get; set; }
        public string gasUsed { get; set; }
        public string cumulativeGasUsed { get; set; }
        public string input { get; set; }
        public string confirmations { get; set; }
    }

    public class Root
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<Result> result { get; set; }
    }



}
