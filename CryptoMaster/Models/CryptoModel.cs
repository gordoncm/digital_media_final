namespace CryptoMaster.Models
{
    public class CryptoModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public double? Price { get; set; }

        public int CMCRank { get; set; }

        public string Ticker {  get; set; }

        public int TotalSupply { get; set; }
    }
}
