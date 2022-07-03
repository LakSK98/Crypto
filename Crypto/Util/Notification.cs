using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Newtonsoft.Json;
using Crypto.Data;
using Crypto.Models;

namespace Crypto.Util
{
    public class Notification
    {
        private string getUrl = "https://api.coinranking.com/v2/coins?symbols[]=BTC";

        public void StartProcessing(CancellationToken cancellationToken = default(CancellationToken))
        {
            while (true)
            {
                System.Diagnostics.Debug.WriteLine("=========== Notification service ===============");

               using(crypto_web_app_dbContext db = new crypto_web_app_dbContext())
                {
                    try
                    {
                        List<PriceTrack> priceTracks = db.PriceTracks.ToList();
                        using (var client = new System.Net.WebClient())
                        {
                            string response = client.DownloadString(getUrl);
                            if (!string.IsNullOrEmpty(response))
                            {
                                //System.Diagnostics.Debug.WriteLine(response);
                                CoinData coinData = JsonConvert.DeserializeObject<CoinData>(response);

                                String btcPrice_text = coinData.data.coins.ElementAt(0).price;
                                System.Diagnostics.Debug.WriteLine("BTC Price: " + btcPrice_text);

                                Double btcPrice = Double.Parse(btcPrice_text);
                                btcPrice = Math.Round(btcPrice, 2);                                

                                //Check matching records from price track table
                                foreach (PriceTrack priceTrack in priceTracks)
                                {
                                    Double userPrice = Double.Parse(priceTrack.Price);
                                    if (userPrice == btcPrice)
                                    {
                                        System.Diagnostics.Debug.WriteLine("===========<<< Match Found >>>===============");
                                        System.Diagnostics.Debug.WriteLine("BTC Price: " + btcPrice_text + " user price: " + userPrice);

                                        //Check users has enough Remaining Alerts
                                        UserAlertPackage userAlertPackage = db.UserAlertPackages.Single(p => p.BitUserId == priceTrack.BitUserId);
                                        if(userAlertPackage.RemainingAlerts > 0)
                                        {
                                            //Send Notification Email
                                            if (!priceTrack.BitUser.Email.Equals("")) SendEmail.sendNotificationEmail(priceTrack.BitUser.Email, btcPrice);

                                            if(userAlertPackage.RemainingAlerts == 1)
                                            {
                                                SendEmail.sendInformationMail(priceTrack.BitUser.Email, "package expired");
                                                //expired
                                                userAlertPackage.Status = "expired";
                                            }

                                            //Insert this item to notification History table
                                            AlertPackage alertPackage = db.AlertPackages.Find(userAlertPackage.AlertPackageId);
                                            AlertHistory alertHistory = new AlertHistory() {
                                                BitUserId = priceTrack.BitUserId,
                                                PackageName = alertPackage.PackageName,
                                                Price = "$"+btcPrice_text,
                                                SentAt = DateTime.Now.ToString()
                                            };
                                            db.AlertHistories.Add(alertHistory);
                                            db.SaveChanges();

                                            //Update Remailning Alert
                                            userAlertPackage.RemainingAlerts -= 1;
                                            db.SaveChanges();

                                            //Remove PriceTrack Row
                                            db.PriceTracks.Remove(priceTrack);
                                            db.SaveChanges();
                                        }
                                            

                                    }
                                }

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("===========ERROR: Notification service ===============");
                    }
                } 

                Thread.Sleep(1000 * 12);
            }
        }

        private void ProcessCancellation()
        {
            Thread.Sleep(10000);
        }

    }
}