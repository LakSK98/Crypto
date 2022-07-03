using Crypto.Util;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Crypto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        // GET: Currency
        [HttpGet]
        public async Task<ActionResult> GetCurrentBTCPrice()
        {
            bool isSuccess = false;
            Double btcPrice = 0.0;
            string getUrl = "https://api.coinranking.com/v2/coins?symbols[]=BTC";
            try
            {
                using (var client = new System.Net.WebClient())
                {
                    string response = client.DownloadString(getUrl);
                    CoinData coinData = JsonConvert.DeserializeObject<CoinData>(response);

                    btcPrice = Double.Parse(coinData.data.coins.ElementAt(0).price);
                    btcPrice = Math.Round(btcPrice, 2);

                    isSuccess= true;
                    return Ok(new { success = isSuccess, data = btcPrice });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { success = isSuccess });
            }
        }
    }
}