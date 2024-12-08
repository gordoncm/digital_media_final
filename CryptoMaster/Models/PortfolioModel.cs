using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoMaster.Models
{
    [Table("Portfolio")]
    public class PortfolioModel
    {
        public int Id { get; set; }

        public string? TokenName { get; set; }

        public double? Balance { get; set; }

        public string? Username {  get; set; }
    }
}
