
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;
using VeekelApi.Data;
using VeekelApi.Models;
using Microsoft.EntityFrameworkCore;
using VeekelApi.Models.Authentication;
using SocialApi.Models.Authentication;
using Common.Data;
using SocialApi.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace SocialApi.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Components.Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJwtAuthenticationManager _jWtAuthenticationManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        public AuthenticationController(ApplicationDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager, IJwtAuthenticationManager jWtAuthenticationManager)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
            _jWtAuthenticationManager = jWtAuthenticationManager;
        }

        [HttpPost("Authenticate")]
        public object Authenticate([Microsoft.AspNetCore.Mvc.FromBody] AuthenticateRequest model)
        {
            Response response = new Response();

            var result = _jWtAuthenticationManager.Authenticate(model);

            if (result != null && result.Result ==null)
            {
                response.Message = "INVALID_USER";
                response.Data = new object();
                return BadRequest(response);
            }
            else
            {
                if (result != null && result.Result == null)
                {
                    response.Message = "NOT_CONFIRMED";
                    response.Data = new object();
                    return BadRequest(response);
                }
                else
                {


                    Authenticate a = new Authenticate();
                    response.Message = "VALID_USER";
                    if (result != null)
                    {
                        Authenticate authModel = (Authenticate)result.Result;

                        var user = _context.Users.Where(e => e.Id == authModel.Id).FirstOrDefault();


                        if (user != null)
                        {
                            //if (user.Email2FAEnabled)
                            //{
                            //    bool sendOTPCode = _twoFactorAuthentication.SendTwoFactorOTP(user.Email);
                            //    response.Message = "EMAIL_2FA";
                            //    return Ok(response);
                            //}
                            //if (user.TwoFactorEnabled)
                            //{
                            //    response.Message = "APP_2FA";
                            //    return Ok(response);
                            //}
                            //else
                            //{
                            //}

                            response.Message = "VALID_USER";
                            response.Data = result.Result;
                            return Ok(response);

                        }
                        else
                        {
                            response.Message = "INVALID_USER";
                            response.Data = result.Result;
                            return Ok(response);
                        }
                    }
                    else
                    {
                        response.Message = "INVALID_USER";
                        response.Data = new object();
                        return BadRequest(response);
                    }
                }
            }

        }

        //[HttpPost]
        //[AllowAnonymous]
        //[Route("Authentication/SendOTP")]
        //public object SendOTP(string email)
        //{
        //    bool result = _twoFactorAuthentication.SendTwoFactorOTP(email);
        //    Response response = new Response();
        //    response.Message = result ? "DONE" : "FAILED";
        //    response.Data = new object { };
        //    return Ok(response);
        //}
        //[Microsoft.AspNetCore.Mvc.HttpPost("LoginByGoogle")]
        //public object LoginByGoogle([FromBody] string googleAccountId)
        //{
        //    Response response = new Response();
        //    Singleton.Instance.Logger.Log(LogType.INFO, "Login with google " + googleAccountId);
        //    try
        //    {
        //        var googleUser = _context.GoogleUsers.Where(x => x.GoogleAccountId == googleAccountId).FirstOrDefault();
        //        if (googleUser == null)
        //        {
        //            Singleton.Instance.Logger.Log(LogType.INFO, "LOGIN_WITH_GOOGLE: USER_NOT_FOUND");
        //            response.Data = new object();
        //            response.Message = "NOT_FOUND";
        //            return response;
        //        }


        //        var user = _context.Users.Where(x => x.Id == googleUser.UserId).FirstOrDefault();
        //        if (user == null)
        //        {
        //            Singleton.Instance.Logger.Log(LogType.INFO, "LOGIN_WITH_GOOGLE: USER_NOT_FOUND");
        //            response.Data = new Object();
        //            response.Message = "NOT_FOUND";
        //            return response;
        //        }
        //        //valid user 2fa
        //        if (user.Email2FAEnabled)
        //        {
        //            Singleton.Instance.Logger.Log(LogType.INFO, "LOGIN_WITH_GOOGLE: EMAIL_2FA");

        //            bool sendOTPCode = _twoFactorAuthentication.SendTwoFactorOTP(user.Email);
        //            response.Message = "EMAIL_2FA";
        //            return Ok(response);
        //        }
        //        if (user.TwoFactorEnabled)
        //        {
        //            Singleton.Instance.Logger.Log(LogType.INFO, "LOGIN_WITH_GOOGLE: APPL_2FA");

        //            response.Message = "APP_2FA";
        //            return Ok(response);
        //        }
        //        //valid user
        //        Singleton.Instance.Logger.Log(LogType.INFO, "LOGIN_WITH_GOOGLE: VALID_USER");

        //        Authenticate a = new Authenticate();
        //        var token = _jWtAuthenticationManager.GenerateJwtToken(user);
        //        a.Id = user.Id;
        //        a.Token = token;
        //        response.Data = a;
        //        response.Message = "Login success";
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        Singleton.Instance.Logger.Log(LogType.ERROR, "Login with google " + googleAccountId + " error " + ex.Message);
        //        response.Data = new Object();
        //        response.Message = "Login failed";
        //        return response;
        //    }
        //}


        //[AllowAnonymous]
        //[HttpPost("SignInWithGoogle/{googleProviderKey}/{email}")]
        //public object SignInWithGoogle(string googleProviderKey, string email)
        //{
        //    Response response = new Response();
        //    try
        //    {
        //        var userLogin = _context.UserLogins.Where(e => e.ProviderKey == googleProviderKey).FirstOrDefault();
        //        if (userLogin == null)
        //        {
        //            response.Data = new object()
        //            {
        //            };
        //            var emailExisting = _context.Users.Where(e => e.Email == email).FirstOrDefault();
        //            if (emailExisting != null)
        //            {
        //                Singleton.Instance.Logger.Log(LogType.INFO, "LOGIN_WITH_GOOGLE: CHANGE_EMAIL");

        //                response.Message = "CHANGE_EMAIL";
        //                return Ok(response);
        //            }
        //            else
        //            {
        //                Singleton.Instance.Logger.Log(LogType.INFO, "LOGIN_WITH_GOOGLE: REQGISTER");

        //                response.Message = "REQGISTER";
        //                return Ok(response);
        //            }
        //        }
        //        else
        //        {
        //            var user = _context.Users.Where(x => x.Id == userLogin.UserId).FirstOrDefault();
        //            if (user.TwoFactorEnabled)
        //            {
        //                Singleton.Instance.Logger.Log(LogType.INFO, "LOGIN_WITH_GOOGLE: 2FA");

        //                response.Message = "2FA";
        //                response.Data = new object();
        //                return Ok(response);
        //            }
        //            else
        //            {
        //                Singleton.Instance.Logger.Log(LogType.INFO, "LOGIN_WITH_GOOGLE: SUCCESS");

        //                Authenticate a = new Authenticate();
        //                var token = _jWtAuthenticationManager.GenerateJwtToken(user);
        //                a.Id = user.Id;
        //                a.Token = token;
        //                response.Data = a;
        //                response.Message = "SUCCESS";
        //                return Ok(response);
        //            }

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Singleton.Instance.Logger.Log(LogType.ERROR, "LOGIN_WITH_GOOGLE: " + e.Message);

        //        return BadRequest(e.Message);

        //    }
        //}






        [AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.HttpPost("Register")]
        public async Task<object> RegisterAsync([Microsoft.AspNetCore.Mvc.FromBody] RegisterModel user)
        {
            //Log.Logger.WriteToFileThreadSafe();
            var result = _jWtAuthenticationManager.RegisterAsync(user);
            var response = new Response();
            if (!result.Result.Success)
            {
                response.Message = "USER_ALREADY_EXISTS";
                response.Data = new object();
                return BadRequest(response);
            }
            var task = await _userManager.FindByEmailAsync(user.Email);
            var sourceContainerName = "media";
            var sourceFilePath = "ProfilePicture.png";
            //details of where we want to copy to
            var destContainerName = "profile-pictures";
            var destFilePath = task.Id + "/ProfilePicture.png";

            response.Message = "SUCCESSFULLY_REGISTERED";
            var appUser = _context.Users.Where(e => e.UserName == user.Email).FirstOrDefault();
            response.Data = new object();
            return Ok(response);
        }


        //[Authorize]
        //[HttpGet]
        //[Route("Authentication/checkAuthorization")]
        //public string checkAuthorization()
        //{
        //    return JsonConvert.SerializeObject(new
        //    {
        //        message = "Authorized"
        //    });
        //}



        //[AllowAnonymous]
        //[Microsoft.AspNetCore.Mvc.HttpPost("ResetPassword")]
        //public async Task<object> ResetPassword([Microsoft.AspNetCore.Mvc.FromBody] string email)
        //{
        //    Response response = new Response();
        //    var user = await _userManager.FindByEmailAsync(email);
        //    _logger.LogInformation("Reset password of email " + email);
        //    if (user == null)
        //    {
        //        response.Message = "INVALID_USER";
        //        _logger.LogInformation(email + " is invalid");
        //        response.Data = new object();
        //        return BadRequest(response);
        //    }
        //    else
        //    if (!(await _userManager.IsEmailConfirmedAsync(user)))
        //    {
        //        response.Message = "NOT_CONFIRMED";
        //        _logger.LogInformation(email + " is not confirm");
        //        response.Data = new object();
        //        return BadRequest(response);
        //    }
        //    else
        //    {
        //        string code = _jWtAuthenticationManager.GenerateCode(email);
        //        //string code = _jWtAuthenticationManager.GenerateJwtToken(user);
        //        var url = HttpUtility.UrlPathEncode(socialTradesURl + "Identity/Account/ResetPassword?code=" + code + "&Email=" + email + "&temp=1");
        //        string emailContent = _emailTemplates.ResetPasswordTemplate(user.Id, url);
        //        await _emailSender.SendEmailAsync(email.ToString(), "Reset Password", emailContent);
        //        response.Message = "SUCCESSFULLY_RESET";
        //        response.Data = new object();
        //        return Ok(response);
        //    }
        //}


        //[Authorize]
        //[Microsoft.AspNetCore.Mvc.HttpPost("Changepassword")]
        //public async Task<object> ChangePassword([Microsoft.AspNetCore.Mvc.FromBody] ChangePasswordModel data)
        //{
        //    Response response = new Response();
        //    var user = await _userManager.FindByEmailAsync(data.Email);
        //    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        //    {
        //        _logger.LogInformation(data.Email + " is invalid");
        //        response.Message = "INVALID_USER";
        //        response.Data = new object();
        //        return BadRequest(response);
        //    }
        //    if (await _userManager.CheckPasswordAsync(user, data.CurrentPassword))
        //    {
        //        var changePasswordResult = await _userManager.ChangePasswordAsync(user, data.CurrentPassword, data.NewPassword);
        //        if (changePasswordResult.Succeeded)
        //        {
        //            _logger.LogInformation(data.Email + " successfuly change");
        //            response.Message = "SUCCESSFULLY_CHANGE";
        //            response.Data = new object();
        //        }
        //    }
        //    else
        //    {
        //        _logger.LogInformation(data.CurrentPassword + " is incorrect");
        //        response.Message = "CURRENTPASSWORD_INCORRECT";
        //        response.Data = new object();
        //    }
        //    return Ok(response);
        //}

        //// [Authorize]
        //[Microsoft.AspNetCore.Mvc.HttpPost("enableNotifications/{userid}/{deviceType}")]
        //public object enableNotifications(string userid, string deviceType, [FromBody] string token)
        //{
        //    _logger.LogInformation("Enable Notications: " + userid);

        //    try
        //    {

        //        MobileSession session = new MobileSession();
        //        session = _context.MobileSessions.Where(e => e.UserId == userid).FirstOrDefault();

        //        if (session == null)
        //        {
        //            session = new MobileSession();
        //            session.UserId = userid;
        //            session.DeviceType = deviceType;
        //            session.LastSignIn = DateTime.Now;
        //            session.Token = token;
        //            _context.MobileSessions.Add(session);
        //            _context.SaveChanges();
        //        }
        //        else
        //        {
        //            session.UserId = userid;
        //            session.DeviceType = deviceType;
        //            session.LastSignIn = DateTime.Now;
        //            session.Token = token;
        //            _context.MobileSessions.Update(session);
        //            _context.SaveChanges();
        //            _logger.LogInformation("Enable Notications: ENABLED");
        //        }
        //        return Ok("Enabled");
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogInformation("Enable Notications EXCEPTION: " + e.Message);

        //        Console.WriteLine(e);

        //        return Ok(e.Message);
        //    }
        //}








        ////Google auth
        //[HttpPost]
        //[Route("Authentication/SignInUsingGoogle")]
        //public async Task<object> SignInUsingGoogle(string providerLogin, string providerKey)
        //{
        //    Response response = new Response();
        //    _logger.LogInformation("SignInUsingGoogle: " + providerKey);

        //    var result = await _signInManager.ExternalLoginSignInAsync(providerLogin, providerKey, isPersistent: false);
        //    if (result.Succeeded)
        //    {
        //        if (result.RequiresTwoFactor)
        //        {
        //            _logger.LogInformation("SignInUsingGoogle: 2FA");

        //            return "2FA";
        //        }
        //        else
        //        {
        //            var userLogin = _context.UserLogins.Where(e => e.ProviderKey == providerKey).FirstOrDefault();
        //            if (userLogin != null)
        //            {
        //                var user = _context.Users.Where(e => e.Id == userLogin.UserId).FirstOrDefault();
        //                if (user != null)
        //                {
        //                    _logger.LogInformation("SignInUsingGoogle: success");

        //                    Authenticate a = new Authenticate();
        //                    a.Token = _jWtAuthenticationManager.GenerateJwtToken(user);
        //                    a.Id = user.Id;
        //                    response.Message = "SUCCESS";
        //                    response.Data = a;
        //                    return Ok(response);
        //                }
        //            }
        //            response = new Response();
        //            response.Message = "SUCCESS";
        //            response.Data = new object();
        //            return BadRequest(response);
        //        }
        //    }
        //    if (result.RequiresTwoFactor)
        //    {
        //        _logger.LogInformation("SignInUsingGoogle: 2FA");

        //        //user need to also provide 2fa,still logged out
        //        return "2FA";

        //    }
        //    else
        //    {
        //        _logger.LogInformation("SignInUsingGoogle: Register");

        //        return "Register";
        //    }

        //    return "";
        //}






        ////Email Two factor authentication

        ///// <summary>
        ///// Authenticate2FAEmail
        ///// </summary>
        ///// <param name="email">User email trying to login</param>
        ///// <param name="code">Verification Code or OTP</param>
        ///// <returns></returns>
        //[HttpPost]
        //[AllowAnonymous]
        //[Route("Authentication/Authentication2FAEmail/{code}")]
        //public object Authenticate2FAEmail(string email, string code)
        //{
        //    _logger.LogInformation("Authentication2FAEmail: " + email);

        //    Response response = new Response();
        //    var user = _context.Users.Where(e => e.Email == email).FirstOrDefault();
        //    TwoFactorAuthentication twofactorAuth = null;
        //    if (user != null)
        //    {
        //        twofactorAuth = _context.TwoFactorAuthentications.Where(e => e.UserId == user.Id).FirstOrDefault();
        //    }
        //    if (twofactorAuth != null)
        //    {
        //        long timestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
        //        bool isCodeValid = twofactorAuth.Code == code && twofactorAuth.UserId == user.Id && twofactorAuth.ExpiryDate > timestamp;
        //        if (isCodeValid)
        //        {
        //            _logger.LogInformation("Authentication2FAEmail: VALID_CODE" + email);

        //            response.Message = "VALID_CODE";
        //            Authenticate a = new Authenticate();
        //            var token = _jWtAuthenticationManager.GenerateJwtToken(user);
        //            a.Id = user.Id;
        //            a.Token = token;
        //            response.Data = a;
        //            //clear code
        //            twofactorAuth.Code = null;
        //            _context.SaveChanges();
        //            return Ok(response);
        //        }
        //        else if (twofactorAuth.Code != code)
        //        {
        //            _logger.LogInformation("Authentication2FAEmail: INVALID_CODE" + email);

        //            response = new Response();
        //            response.Message = "INVALID_CODE";
        //            response.Data = new object { };
        //            return BadRequest(response);
        //        }
        //        else
        //        {
        //            _logger.LogInformation("Authentication2FAEmail: EXPIRED_CODE" + email);

        //            response = new Response();
        //            response.Message = "EXPIRED_CODE";
        //            response.Data = new object { };
        //            return BadRequest(response);
        //        }
        //    }
        //    _logger.LogInformation("Authentication2FAEmail: INVALID_CODE" + email);

        //    response = new Response();
        //    response.Message = "INVALID_CODE";
        //    response.Data = new object { };
        //    return BadRequest(response);

        //}

        ///// <summary>
        ///// CheckEmail2FAEnabled
        ///// </summary>
        ///// <param name="email">User email trying to login</param>
        ///// <returns></returns>

        //[HttpPost]
        //[Authorize]
        //[Route("Authentication/CheckEmail2FAEnabled")]
        //public object CheckEmail2FAEnabled(string email)
        //{
        //    Response response = new Response();
        //    response.Data = new object();
        //    try
        //    {
        //        _logger.LogInformation("CheckEmail2FAEnabled: " + email);

        //        response.Message = "DONE";
        //        if (_twoFactorAuthentication.IsTwoFactorAuthenticationUser(email))
        //        {
        //            response.Data = true;
        //        }
        //        else
        //        {
        //            response.Data = false;
        //        }
        //        _logger.LogInformation("CheckEmail2FAEnabled: DONE" + email);

        //        return Ok(response);

        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogInformation("CheckEmail2FAEnabled: ERROR" + e.Message);

        //        response.Message = "ERROR";
        //        return BadRequest(response);
        //    }
        //}

        ///// <summary>
        ///// SendTwoFactorActivationCodeEmail
        ///// </summary>
        ///// <param name="userid">user id tryiyng to enable the 2FA</param>
        ///// <returns></returns>

        //[HttpPost]
        //[Route("Authentication/SendTwoFactorActivationCodeEmail/{userid}")]
        //[Authorize]
        //public object SendTwoFactorActivationCodeEmail(string userid)
        //{
        //    _logger.LogInformation("SendTwoFactorActivationCodeEmail: " + userid);

        //    Response response = new Response();
        //    response.Data = new object();
        //    try
        //    {
        //        if (_twoFactorAuthentication.SendTwoFactorAcivationCode(userid))
        //        {
        //            _logger.LogInformation("SendTwoFactorActivationCodeEmail: DONE" + userid);

        //            response.Message = "DONE";
        //        }
        //        else
        //        {
        //            _logger.LogInformation("SendTwoFactorActivationCodeEmail: FAILED" + userid);

        //            response.Message = "FAILED";
        //        }
        //        return Ok(response);

        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogInformation("SendTwoFactorActivationCodeEmail: ERROR" + e.Message);

        //        response.Message = "ERROR";
        //        return BadRequest(response);
        //    }
        //}


        ///// <summary>
        ///// AddEmailTwoFactorAuthentication
        ///// Activate 2FA by matching the userid and code OTP
        ///// </summary>
        ///// <param name="userid"></param>
        ///// <param name="code"></param>
        ///// <returns></returns>

        //[HttpPost]
        //[Authorize]
        //[Route("Authentication/AddEmailTwoFactorAuthentication/{userid}/{code}")]
        //public object AddEmailTwoFactorAuthentication(string userid, string code)

        //{
        //    _logger.LogInformation("AddEmailTwoFactorAuthentication: " + userid);

        //    Response response = new Response();
        //    response.Data = new object();
        //    try
        //    {
        //        if (_twoFactorAuthentication.AddTwoFactorAuthentication(userid, code))
        //        {
        //            _logger.LogInformation("AddEmailTwoFactorAuthentication: DONE" + userid);

        //            response.Message = "DONE";
        //        }
        //        else
        //        {
        //            _logger.LogInformation("AddEmailTwoFactorAuthentication: FAILED" + userid);

        //            response.Message = "FAILED";
        //        }
        //        return Ok(response);

        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogInformation("AddEmailTwoFactorAuthentication: ERROR" + e.Message);

        //        response.Message = "ERROR";
        //        return BadRequest(response);
        //    }
        //}


        ///// <summary>
        ///// DisableEmailTwoFactorAuthentication
        ///// </summary>
        ///// <param name="userid"></param>
        ///// <returns></returns>

        //[Authorize]
        //[HttpPost]
        //[Route("Authentication/DisableEmailTwoFactorAuthentication/{userid}")]
        //public object DisableEmailTwoFactorAuthentication(string userid)
        //{
        //    _logger.LogInformation("DisableEmailTwoFactorAuthentication: " + userid);

        //    Response response = new Response();
        //    if (_twoFactorAuthentication.DisableTwoFactorAuthentication(userid))
        //    {
        //        _logger.LogInformation("DisableEmailTwoFactorAuthentication: DONE" + userid);

        //        response.Message = "DONE";
        //        response.Data = new object()
        //        {
        //        };
        //        return Ok(response);
        //    }
        //    else
        //    {
        //        _logger.LogInformation("DisableEmailTwoFactorAuthentication: FAILED" + userid);

        //        response.Message = "FAILED";
        //        response.Data = new object()
        //        {

        //        };
        //        return Ok(response);
        //    }

        //}





        ////APP 2FA

        ///// <summary>
        ///// Authenticate2FAApp
        ///// </summary>
        ///// <param name="email"></param>
        ///// <param name="code"></param>
        ///// <returns></returns>



        //[HttpPost]
        //[Route("Authentication/Authentication2FAApp/{code}")]
        //public async Task<object> Authenticate2FAApp(string email, string code)
        //{
        //    _logger.LogInformation("Authenticate2FAApp: FAILED" + email);

        //    var user = _context.Users.Where(u => u.Email == email).FirstOrDefault();

        //    Response response = new Response();
        //    bool validCode = await ValidateAppCode(user, code);
        //    if (validCode)
        //    {
        //        _logger.LogInformation("Authenticate2FAApp: VALID_CODE" + email);

        //        Authenticate authenticate = new Authenticate();
        //        var token = _jWtAuthenticationManager.GenerateJwtToken(user);
        //        var userid = user.Id;
        //        authenticate.Token = token;
        //        authenticate.Id = userid;
        //        response.Message = "VALID_CODE";
        //        response.Data = authenticate;
        //        return response;
        //    }
        //    _logger.LogInformation("Authenticate2FAApp: INVALID_CODE" + email);

        //    response.Message = "INVALID_CODE";
        //    response.Data = new object();
        //    return response;
        //}


        ///// <summary>
        ///// CheckApp2FAEnabled
        ///// </summary>
        ///// <param name="userid"></param>
        ///// <returns></returns>


        //[Authorize]
        //[HttpGet]
        //[Route("Authentication/CheckApp2FAEnabled/{userid}")]
        //public object CheckApp2FAEnabled(string userid)
        //{
        //    _logger.LogInformation("Authenticate2FAApp:" + userid);

        //    Response response = new Response();
        //    var user = _userManager.FindByIdAsync(userid).Result;
        //    var twofactorAuth = _userManager.GetTwoFactorEnabledAsync(user).Result;
        //    response.Message = (twofactorAuth) ? "ENABLED" : "DISABLED";
        //    return Ok(response);
        //}

        ///// <summary>
        ///// Disable2FAApp
        ///// </summary>
        ///// <param name="userid"></param>
        ///// <returns></returns>


        //[Authorize]
        //[HttpPost]
        //[Route("Authentication/DisableAppTwoFactorAuthentication/{userid}")]
        //public object DisableAppTwoFactorAuthentication(string userid)
        //{
        //    _logger.LogInformation("DisableAppTwoFactorAuthentication:" + userid);
        //    Response response = new Response();
        //    try
        //    {


        //        var user = _context.Users.Where(e => e.Id == userid).FirstOrDefault();
        //        if (user != null)
        //        {
        //            user.TwoFactorEnabled = false;
        //            _context.SaveChanges();
        //        }
        //        _logger.LogInformation("DisableAppTwoFactorAuthentication: DONE" + userid);

        //        response.Message = "DONE";
        //        return response;
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogInformation("DisableAppTwoFactorAuthentication: FAILED" + userid);

        //        response.Message = "FAILED";
        //        return response;
        //    }
        //}

        ///// <summary>
        ///// CheckApp2FACase
        ///// </summary>
        ///// <param name="userid"></param>
        ///// <returns></returns>

        //[Authorize]
        //[HttpGet]
        //[Route("Authentication/CheckApp2FACase/{userid}")]
        //public object CheckApp2FACase(string userid)
        //{
        //    _logger.LogInformation("CheckApp2FACase: " + userid);

        //    Response response = new Response();
        //    var userExists = _context.UserTokens.Any(e => e.UserId == userid);
        //    var user = _userManager.FindByIdAsync(userid).Result;
        //    var twofactorAuth = _userManager.GetTwoFactorEnabledAsync(user).Result;
        //    if (userExists)
        //    {
        //        if (!twofactorAuth)
        //        {
        //            _logger.LogInformation("CheckApp2FACase: SETUP_CODE" + userid);

        //            //Just Code verification
        //            response.Message = "SETUP_CODE";
        //            return Ok(response);
        //        }
        //        else
        //        {
        //            _logger.LogInformation("CheckApp2FACase: ENABLED" + userid);

        //            //Already enabled
        //            response.Message = "ENABLED";
        //            return Ok(response);
        //        }
        //    }
        //    else
        //    {
        //        _logger.LogInformation("CheckApp2FACase: SET_NEW" + userid);

        //        //Add new account to application
        //        response.Message = "SET_NEW";
        //        response.Data = LoadSharedKeyAndQrCodeUriAsync(user).Result;
        //        return Ok(response);
        //    }
        //}




        //[Authorize]
        //[HttpPost]
        //[Route("Authentication/ResetApp2FA/{userid}")]
        //public async Task<object> ResetApp2FA(string userid)
        //{
        //    _logger.LogInformation("ResetApp2FA: " + userid);

        //    var user = _userManager.FindByIdAsync(userid).Result;
        //    if (user != null)
        //    {
        //        _logger.LogInformation("ResetApp2FA: DONE");

        //        user.TwoFactorEnabled = false;
        //        _context.SaveChanges();

        //        var key = LoadSharedKeyAndQrCodeUriAsync(user).Result;
        //        return key;


        //    }
        //    _logger.LogInformation("ResetApp2FA: FAILED");

        //    return "FAILED";

        //}



        ///// <summary>
        ///// Setup2FAAppCode
        ///// </summary>
        ///// <param name="userid"></param>
        ///// <param name="code"></param>
        ///// <returns></returns>

        //[Authorize]
        //[HttpPost]
        //[Route("Authentication/Setup2FAAppCode/{userid}/{code}")]
        //public object Setup2FAAppCode(string userid, string code)
        //{
        //    _logger.LogInformation("Setup2FAAppCode: " + userid);

        //    Response response = new Response();

        //    var user = _userManager.FindByIdAsync(userid).Result;
        //    bool validCode = ValidateAppCode(user, code).Result;
        //    if (validCode)
        //    {
        //        _logger.LogInformation("Setup2FAAppCode: DONE" + userid);

        //        user.TwoFactorEnabled = true;
        //        _context.SaveChanges();
        //        response.Message = "DONE";
        //        return Ok(response);
        //    }
        //    _logger.LogInformation("Setup2FAAppCode: INVALID_CODE" + userid);

        //    response.Message = "INVALID_CODE";
        //    return BadRequest(response);
        //}

        ///// <summary>
        ///// App2FASetup
        ///// </summary>
        ///// <param name="userid"></param>
        ///// <returns></returns>
        //[Authorize]
        //[HttpPost]
        //[Route("Authentication/App2FASetup/{userid}")]
        //public async Task<object> App2FASetup(string userid)
        //{
        //    _logger.LogInformation("App2FASetup:" + userid);

        //    Response response = new Response();
        //    var user = await _userManager.FindByIdAsync(userid);
        //    _logger.LogInformation("App2FASetup DONE:" + userid);
        //    response.Message = "DONE";
        //    response.Data = LoadSharedKeyAndQrCodeUriAsync(user).Result;
        //    return Ok(response);

        //}




        ///// <summary>
        ///// EnableApp2FANew
        ///// </summary>
        ///// <param name="userid"></param>
        ///// <param name="code"></param>
        ///// <returns></returns>

        //[Authorize]
        //[HttpPost]
        //[Route("Authentication/AddApp2FANew/{userid}/{code}")]
        //public async Task<object> AddApp2FANew(string userid, string code)
        //{
        //    _logger.LogInformation("AddApp2FANew: " + userid);

        //    Response response = new Response();
        //    var user = await _userManager.FindByIdAsync(userid);
        //    if (user == null)
        //    {
        //        _logger.LogInformation("AddApp2FANew: NO_USER" + userid);

        //        response.Message =
        //        "NO_USER";
        //        return BadRequest(response);
        //    }

        //    // Strip spaces and hypens
        //    var verificationCode = code.Replace(" ", string.Empty).Replace("-", string.Empty);

        //    var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
        //        user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);
        //    if (!is2faTokenValid)
        //    {
        //        _logger.LogInformation("AddApp2FANew: INAVLID CODE" + userid);

        //        response.Message = "INAVLID CODE";
        //        return BadRequest(response);

        //    }
        //    else
        //    {
        //        await _userManager.SetTwoFactorEnabledAsync(user, true);


        //        if (await _userManager.CountRecoveryCodesAsync(user) == 0)
        //        {
        //            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
        //            _logger.LogInformation("AddApp2FANew: DONE" + userid);

        //            response.Message = "DONE";
        //            response.Data =
        //            recoveryCodes.ToArray();
        //            return Ok(response);
        //        }
        //        _logger.LogInformation("AddApp2FANew: DONE" + userid);

        //        response.Message = "DONE";
        //        return Ok(response);

        //    }

        //}



        ///// <summary>
        ///// TryAnotherAuthMethod
        ///// </summary>
        ///// <param name="email"></param>
        ///// <param name="method"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[AllowAnonymous]
        //[Route("Authentication/TryAnotherAuthMethod/{method}")]
        //public object TryAnotherAuthMethod(string email, string method)
        //{
        //    _logger.LogInformation("TryAnotherAuthMethod: " + email);

        //    var user = _userManager.FindByEmailAsync(email).Result;
        //    Response response = new Response();
        //    if (user != null)
        //    {
        //        if (method == "EMAIL")
        //        {
        //            if (user.TwoFactorEnabled)
        //            {
        //                _logger.LogInformation("TryAnotherAuthMethod: APP" + email);

        //                response.Message = "APP";
        //            }
        //            else
        //            {
        //                _logger.LogInformation("TryAnotherAuthMethod: NO_METHOD " + email);

        //                response.Message = "NO_METHOD";
        //            }
        //            return Ok(response);
        //        }
        //        else if (method == "APP" || method == "RECOVERY_CODE" /*code also available*/)
        //        {


        //            if (user.Email2FAEnabled)
        //            {
        //                _logger.LogInformation("TryAnotherAuthMethod: EMAIL" + email);

        //                response.Message = "EMAIL";
        //            }
        //            else
        //            {
        //                _logger.LogInformation("TryAnotherAuthMethod: NO_METHOD" + email);

        //                response.Message = "NO_METHOD";
        //            }
        //            return Ok(response);
        //        }
        //    }
        //    _logger.LogInformation("TryAnotherAuthMethod: NO_METHOD " + email);

        //    response.Message = "NO_METHOD";
        //    return BadRequest(response);
        //}

        ///// <summary>
        ///// AuthenticateRecoveryCode2FA
        ///// </summary>
        ///// <param name="email"></param>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[AllowAnonymous]
        //[Route("Authentication/AuthenticateRecoveryCode2FA/{code}")]
        //public object AuthenticateRecoveryCode2FA(string email, string code)
        //{
        //    _logger.LogInformation("AuthenticateRecoveryCode2FA: " + email);

        //    Response response = new Response();
        //    var user = _userManager.FindByEmailAsync(email).Result;
        //    var codes = _userManager.GetAuthenticationTokenAsync(user, "[AspNetUserStore]", "RecoveryCodes").Result;

        //    var codesString = codes.ToString().Split(';').ToList();
        //    if (codesString.Contains(code.ToString()))
        //    {
        //        codesString.Remove(code);
        //        string recoveryCodes = "";
        //        foreach (string rCode in codesString)
        //        {
        //            recoveryCodes += rCode + ";";
        //        }
        //        var userCodes = _context.UserTokens.Where(e => e.UserId == user.Id && e.Name == "RecoveryCodes").FirstOrDefault();
        //        if (userCodes != null)
        //        {
        //            _logger.LogInformation("AuthenticateRecoveryCode2FA: VALID_CODE" + email);

        //            userCodes.Value = recoveryCodes;
        //            _context.SaveChanges();
        //            response.Message = "VALID_CODE";
        //            Authenticate a = new Authenticate();
        //            a.Token = _jWtAuthenticationManager.GenerateJwtToken(user);
        //            a.Id = user.Id;
        //            response.Data = a;
        //            return Ok(response);

        //        }
        //    }
        //    _logger.LogInformation("AuthenticateRecoveryCode2FA: INVALID_CODE" + email);

        //    response.Message = "INVALID_CODE";
        //    return BadRequest(response);
        //}









        ///// <summary>
        ///// VerifyApp2FACodeExternal
        ///// </summary>
        ///// <param name="code"></param>
        ///// <param name="providerKey"></param>
        ///// <returns></returns>

        //[HttpPost]
        //[Route("Authentication/VerifyApp2FACodeExternal/{code}/{providerKey}")]
        //public async Task<object> VerifyApp2FACodeExternal(string code, string providerKey)
        //{


        //    _logger.LogInformation("VerifyApp2FACodeExternal: " + code);

        //    Response response = new Response();
        //    var userLogin = _context.UserLogins.Where(e => e.ProviderKey == providerKey).FirstOrDefault();
        //    if (userLogin != null)
        //    {
        //        var user = _context.Users.Where(e => e.Id == userLogin.UserId).FirstOrDefault();
        //        if (user != null)
        //        {
        //            var authenticatorCode = code.Replace(" ", string.Empty).Replace("-", string.Empty);
        //            var result = await _signInManager.TwoFactorSignInAsync(providerKey, authenticatorCode, false, false);

        //            if (result.Succeeded)
        //            {
        //                Authenticate a = new Authenticate();
        //                a.Token = _jWtAuthenticationManager.GenerateJwtToken(user);
        //                a.Id = user.Id;
        //                _logger.LogInformation("VerifyApp2FACodeExternal: SUCCESS" + code);

        //                response.Message = "SUCCESS";
        //                response.Data = a;
        //                return Ok(response);
        //            }
        //            else if (result.IsLockedOut)
        //            {
        //                _logger.LogInformation("VerifyApp2FACodeExternal: LOCKOUT" + code);

        //                response = new Response();

        //                response.Message = "LOCKOUT";
        //                return BadRequest(response);
        //            }
        //            else
        //            {
        //                _logger.LogInformation("VerifyApp2FACodeExternal: INVALID_CODE" + code);

        //                response = new Response();
        //                response.Message = "INVALID_CODE";
        //                return BadRequest(response);
        //            }
        //        }
        //        else
        //        {
        //            _logger.LogInformation("VerifyApp2FACodeExternal: USER_NOT_FOUND" + code);

        //            response = new Response();
        //            response.Message = "USER_NOT_FOUND";
        //            return BadRequest(response);
        //        }
        //    }
        //    else
        //    {
        //        _logger.LogInformation("VerifyApp2FACodeExternal: USER_LOGIN_NOT_FOUND" + code);

        //        response = new Response();
        //        response.Message = "USER_LOGIN_NOT_FOUND";
        //        return BadRequest(response);
        //    }
        //}





        //[HttpPost]
        //[Route("Authentication/ValidateEmail")]
        //public object ValidateEmail(string email)
        //{
        //    _logger.LogInformation("ValidateEmail: " + email);

        //    Response response = new Response();
        //    var emailExist = _context.Users.Where(e => e.Email == email).FirstOrDefault();
        //    if (emailExist == null)
        //    {
        //        _logger.LogInformation("ValidateEmail: REGISTER" + email);

        //        response.Message = "Register";
        //        return Ok(response);
        //    }
        //    else
        //    {
        //        _logger.LogInformation("ValidateEmail: INVALID_EMAIL" + email);

        //        response.Message = "INVALID_EMAIL";
        //        return BadRequest(response);
        //    }
        //}





        ///////////////////////////////////////////////////////////////////////////////////////////////////////////





        ///// <summary>
        ///// ValidateAppCode
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="code"></param>
        ///// <returns></returns>



        //private async Task<bool> ValidateAppCode(ApplicationUser user, string code)
        //{
        //    _logger.LogInformation("ValidateAppCode: INVALID_EMAIL" + user.Email);

        //    if (user != null)
        //    {
        //        var authenticatorKey = await _userManager.GetAuthenticationTokenAsync(user, "[AspNetUserStore]", "AuthenticatorKey");
        //        if (authenticatorKey != null)
        //        {
        //            _logger.LogInformation("ValidateAppCode: DONE" + user.Email);

        //            var totp = new Totp(Base32Encoding.ToBytes(authenticatorKey));
        //            return totp.VerifyTotp(code, out _);
        //        }
        //    }
        //    return false;
        //}


        ///// <summary>
        ///// LoadSharedKeyAndQrCodeUriAsync
        ///// </summary>
        ///// <param name="user"></param>
        ///// <returns></returns>
        //private async Task<string> LoadSharedKeyAndQrCodeUriAsync(ApplicationUser user)
        //{

        //    // Load the authenticator key & QR code URI to display on the form
        //    var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
        //    if (string.IsNullOrEmpty(unformattedKey))
        //    {
        //        await _userManager.ResetAuthenticatorKeyAsync(user);
        //        unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
        //    }

        //    return FormatKey(unformattedKey);
        //}



        ///// <summary>
        ///// FormatKey
        ///// </summary>
        ///// <param name="unformattedKey"></param>
        ///// <returns></returns>
        //private string FormatKey(string unformattedKey)
        //{
        //    var result = new StringBuilder();
        //    int currentPosition = 0;
        //    while (currentPosition + 4 < unformattedKey.Length)
        //    {
        //        result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
        //        currentPosition += 4;
        //    }
        //    if (currentPosition < unformattedKey.Length)
        //    {
        //        result.Append(unformattedKey.Substring(currentPosition));
        //    }

        //    return result.ToString().ToLowerInvariant();
        //}

        //[HttpPost]
        //[Route("Authentication/GoogleAuthentication")]
        //public async Task<object> GoogleAuthentication(string providerKey)
        //{

        //    Response response = new Response();
        //    var checkuser = await _userManager.FindByLoginAsync("Google", providerKey);
        //    if (checkuser == null)
        //    {
        //        //google data fill
        //        response.Data = new object();
        //        response.Message = "DATA_FILL";
        //        return Ok(response);
        //    }

        //    //check if email authrntication is enabled
        //    if (checkuser != null && checkuser.EmailConfirmed)
        //    {
        //        if (checkuser.Email2FAEnabled)
        //        {
        //            SendOTP(checkuser.Id);
        //            response.Data = new object();
        //            response.Message = "Email2FA";
        //            return Ok(response);
        //        }

        //        if (checkuser.TwoFactorEnabled)
        //        {
        //            response.Data = new object();
        //            response.Message = "App2FA";
        //            return Ok(response);
        //        }
        //        Authenticate a = new Authenticate();
        //        var token = _jWtAuthenticationManager.GenerateJwtToken(checkuser);
        //        a.Id = checkuser.Id;
        //        a.Token = token;
        //        response.Data = a;
        //        response.Message = "VALID_USER";
        //        return Ok(response);
        //    }
        //    response.Data = new object();
        //    response.Message = "NOT_CONFIRMED";
        //    return Ok(response);
        //}


        //[HttpPost("GoogleDataFill")]
        //public async Task<object> GoogleDataFill([FromBody] GoogleRegister user, string googleAccountId, string token, string deviceType)
        //{
        //    Response response = new Response();
        //    //var info = await _signInManager.GetExternalLoginInfoAsync();
        //    //if (info == null)
        //    //{
        //    //    response.Data = new object();
        //    //    response.Message = "PROVIDER_NOT_VALID";
        //    //    return BadRequest(response);
        //    //}
        //    var registerUser = new ApplicationUser
        //    {
        //        UserName = user.Email,
        //        Email = user.Email,
        //        CountryId = user.Country,
        //        LanguageId = user.Language,
        //        User_type = 1,
        //        Status = 1,
        //        ImageFile = null,
        //        Name = user.Name,
        //        NickName = user.NickName,
        //        PhoneNumber = user.PhoneNumber
        //        ,
        //        EmailConfirmed = true
        //    };


        //    var checkNickname = _user.IfExistNickName(user.NickName);
        //    if (checkNickname)
        //    {
        //        response.Data = new object();
        //        response.Message = "INVALID_NICKNAME";
        //        return BadRequest(response);
        //    }

        //    var result = await _userManager.CreateAsync(registerUser);
        //    if (result.Succeeded)
        //    {
        //        UserLoginInfo info = new UserLoginInfo("Google", googleAccountId, "Google");

        //        _context.UserLogins.Add(new IdentityUserLogin<string>
        //        {
        //            ProviderDisplayName = "Google",
        //            ProviderKey = googleAccountId,
        //            LoginProvider = "Google",
        //            UserId = registerUser.Id
        //        });
        //        _context.SaveChanges();
        //        _ = _graph.QueryAsync("BursaPlus-SocialGraph", "Create (u:User{ userid:'" + registerUser.Id + "', name:'" + registerUser.Name + "', about :'New Account', nickname:'" + registerUser.NickName + "', image :'" + "', coverImage:'',countryid:" + registerUser.CountryId + "} )");
        //        _logger.LogInformation("User created a new account with password.");

        //        Authenticate a = new Authenticate();
        //        var regToken = _jWtAuthenticationManager.GenerateJwtToken(registerUser);
        //        a.Id = registerUser.Id;
        //        a.Token = regToken;
        //        response.Data = a;
        //        response.Message = "VALID_USER";
        //        return Ok(response);
        //    }
        //    response.Data = new object();
        //    response.Message = "FAILED_CREATE_USER";
        //    return Ok(response);


        //}


    }
}