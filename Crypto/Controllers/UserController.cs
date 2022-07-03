using Crypto.Data;
using Crypto.Models;
using Crypto.Util;
using Microsoft.AspNetCore.Authorization;
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
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly crypto_web_app_dbContext db;

        public UserController(crypto_web_app_dbContext context)
        {
            db = context;
        }


        [HttpPost("login")]
        public async Task<ActionResult> LoginReq(LoginUser loginData)
        {
            bool flag = false;
            LoginResponse response = new LoginResponse();

            loginData.PasswordHash = Sha256HashGenerator.GenerateHash(loginData.PasswordHash);
            bool isValid = db.Logins.Any(x => x.Email == loginData.Email && x.PasswordHash == loginData.PasswordHash);
           
            if (isValid) {
                try
                {
                    Login login = db.Logins.Where(x => x.Email == loginData.Email).Single();
                    response.login_id= login.Id.ToString();
                    response.email = login.Email;
                    response.role = login.Role;
                    flag = true;
                }
                catch (Exception ex) {
                    flag = false;
                }
            }
            else
            {
                flag = false;
            }

            response.success = flag == true ? "1" : "0";
            await db.DisposeAsync();
            return Ok(response);
        }


        private bool isAccountExist(String email) {
            return db.Logins.Any(x => x.Email == email);
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterReq(Login userData)
        {
            bool flag = false;

            if (isAccountExist(userData.Email))
            {
                return Ok(new { success = flag, data="Account with this Email is already exists" });
            }

            userData.PasswordHash = Sha256HashGenerator.GenerateHash(userData.PasswordHash);
            userData.Role = "BIT-USER";
            try
            {
                db.Logins.Add(userData);
                await db.SaveChangesAsync();
                flag = true;
            }
            catch (Exception ex) {
                flag = false;
            }
            finally
            {
                db.Dispose();
            }
            return Ok(new { success = flag });
        }


        [HttpPost("getUserByMail")]
        public async Task<ActionResult> GetUserByMail(getUser userInf)
        {
            Login user = await db.Logins.Where(x => x.Email == userInf.email && x.Id == userInf.id).FirstOrDefaultAsync();
            if (user == null)
            {
                return Ok(new { success = "0" });
            }

            return Ok(new { success = "1", data=new { id =user.Id, fname=user.FirstName, lname=user.LastName,email=user.Email, aboutMe=user.AboutMe } });
        }


        [HttpPost("update")]
        public async Task<ActionResult> UpdateProfile(Login userData)
        {
            bool flag = false;

            try
            {
                Login data = db.Logins.Single(l => l.Id == userData.Id); //userData.ID

                if (data != null) {
                    data.FirstName = userData.FirstName;
                    data.LastName = userData.LastName;
                    data.AboutMe = userData.AboutMe;

                    await db.SaveChangesAsync();
                    flag = true;
                }                
            }
            catch (Exception ex)
            {
                flag = false;
            }
            finally
            {
                db.Dispose();
            }
            return Ok(new { success = userData });
        }
    }

    public class LoginUser
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }

    public class LoginResponse
    {
        public string success { get; set; }
        public string login_id { get; set; }
        public string email { get; set; }
        public string role { get; set; }
    }

    public class getUser
    {
        public int id { get; set; }
        public string email { get; set; }
    }
}