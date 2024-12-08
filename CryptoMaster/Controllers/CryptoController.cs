using CryptoMaster.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CryptoMaster.Controllers
{
    public class CryptoController : Controller
    {
        public IActionResult Index()
        {
            using var db = new CryptoDbContex();

            var coins = db.Coins
    .OrderBy(b => b.CMCRank).ToList<CryptoModel>();

            return View("Index", coins);
        }
         
        public IActionResult AddCoins()
        {
            FetchCMC.saveCMCData(); 

            return View("AddCoins"); 
        } 
    }
}
