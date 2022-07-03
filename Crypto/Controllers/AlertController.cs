using Crypto.Data;
using Crypto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Crypto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertController : ControllerBase
    {
        private readonly crypto_web_app_dbContext _context;
        public AlertController(crypto_web_app_dbContext context)
        {
            _context = context;
        }
        // GET: Alert
        [HttpGet]
        public async Task<ActionResult> PackageListing()
        {
            return Ok(await _context.AlertPackages.ToListAsync());
        }


        [HttpPost]
        public async Task<ActionResult> BuyPackage(UserAlertPackage reqData)
        {
            bool flag = false;
            String msg = "You have successfully purchase";

            if (_context.UserAlertPackages.Any(p=> p.AlertPackageId == reqData.AlertPackageId && p.BitUserId == reqData.BitUserId)) {
                return Ok(new { success = true, data="You Already bought this pack" });
            }

            if (_context.UserAlertPackages.Any(p => p.BitUserId == reqData.BitUserId))
            {
                //Delete previose package
                try
                {
                    UserAlertPackage pack = _context.UserAlertPackages.Single(p => p.BitUserId == reqData.BitUserId);
                    _context.UserAlertPackages.Remove(pack);
                    msg = "You have successfuly Upgrade your package";
                }
                catch (Exception ex)
                {
                    //throw ex.InnerException;
                }
            }

            reqData.Status = "active";
            try
            {
                //Find no of alerts in selected alertPackage
                AlertPackage selectedPackage = _context.AlertPackages.Find(reqData.AlertPackageId);
               
                if (selectedPackage != null)
                {
                    //Add no of alerts to object
                    reqData.RemainingAlerts = selectedPackage.NoOfAlerts;
                    _context.UserAlertPackages.Add(reqData);
                    _context.SaveChanges();
                    flag = true;
                }                
            }
            catch (Exception ex)
            {
                flag = false;
            }
            finally
            {
                _context.Dispose();
            }
            return Ok(new { success = flag, data = msg });
        }

        [HttpPost("getSelectedPackage")]

        public async Task<ActionResult> GetSelectedPackage(getUser userInf)
        {
            bool isPackageSelected = false;

            if (_context.UserAlertPackages.Any(x => x.BitUserId == userInf.id))
            {
                try
                {
                    UserAlertPackage package = _context.UserAlertPackages.Single(p => p.BitUserId == userInf.id);
                    isPackageSelected = true;
                    return Ok(
                        new
                        {
                            success = isPackageSelected,
                            package = new
                            {
                                ID = package.AlertPackage.Id,
                                PackageName = package.AlertPackage.PackageName,
                                NoOfAlerts = package.AlertPackage.NoOfAlerts,
                                NoOfRemainingAlerts = package.RemainingAlerts,
                                Description = package.AlertPackage.Description,
                                DurationInDays = package.AlertPackage.DurationInDays
                            }
                        });
                }
                catch (Exception ex)
                {
                    return Ok(new { success = isPackageSelected });
                }
                finally
                {
                    _context.Dispose();
                }
            }
            else
            {
                return Ok(new { success = isPackageSelected });
            }
        }


        [HttpPost("setPrice")]
        public async Task<ActionResult> SetPrice(setPriceReq setPriceReq)
        {
            bool flag = false;

            if (_context.PriceTracks.Any(p => p.BitUserId == setPriceReq.loginId))
            {
                //User has already set => then update current value
                PriceTrack priceTrack = _context.PriceTracks.Single(p=> p.BitUserId == setPriceReq.loginId);
                if (priceTrack != null)
                {
                    priceTrack.Price = setPriceReq.price;
                    _context.SaveChanges();
                    flag = true;
                }
            }
            else
            {
                PriceTrack newPriceTrack = new PriceTrack() { 
                    BitUserId = setPriceReq.loginId,
                    Price = setPriceReq.price
                };
                _context.PriceTracks.Add(newPriceTrack);
                _context.SaveChanges();
                flag = true;
            }

            return Ok(new { success = flag });
        }

        [HttpPost("getPrice")]
        public async Task<ActionResult> getPrice(setPriceReq setPriceReq)
        {
            bool flag = false;

            if (_context.PriceTracks.Any(p => p.BitUserId == setPriceReq.loginId))
            {
                //User has already set
                PriceTrack priceTrack = _context.PriceTracks.Single(p => p.BitUserId == setPriceReq.loginId);
                if (priceTrack != null)
                {
                    PriceTrack pt = new PriceTrack() { 
                        Id= priceTrack.Id,
                        BitUserId = priceTrack.BitUserId,
                        Price = priceTrack.Price
                    };
                    flag = true;
                    return Ok(new { success = flag, data = pt });                    
                }
            }

            return Ok(new { success = flag });
        }


        
        [HttpGet("getAlertHistory/{currentUserId}")]
        public async Task<ActionResult> GetAlertHistory(int? currentUserId)
        {
            bool isSuccess = false;

            if (_context.UserAlertPackages.Any(x => x.BitUserId == currentUserId))
            {
                try
                {
                    List<AlertHistory> list = _context.AlertHistories.Where(a => a.BitUserId == currentUserId).ToList();
                    
                    //Avoid Reccursion while Serializing
                    List<AlertHistory> alertHistoreis = new List<AlertHistory>();
                    foreach (AlertHistory alert in list)
                    {
                        AlertHistory obj = new AlertHistory()
                        {
                            Id = alert.Id,
                            BitUserId = alert.BitUserId,
                            Price= alert.Price,
                            SentAt = alert.SentAt,
                            PackageName = alert.PackageName
                        };
                        alertHistoreis.Add(obj);                        
                    }
                    isSuccess = true;
                    return Ok(new { success = isSuccess, data = alertHistoreis });
                }
                catch (Exception ex)
                {
                    return Ok(new { success = isSuccess });
                }
                finally
                {
                    _context.Dispose();
                }
            }
            else
            {
                return Ok(new { success = isSuccess });
            }
        }
    }
    public class setPriceReq
    {
        public int loginId { get; set; }
        public string price { get; set; }
    }
}