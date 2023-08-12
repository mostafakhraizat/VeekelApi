using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using VeekelApi.Data;
using VeekelApi.Models.Authentication;
using VeekelApi.Models;
using Common.Data;

namespace SocialApi.Models.Authentication
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly string _tokenKey;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<JwtAuthenticationManager> _logger;
        private readonly ApplicationDbContext _context;

        public JwtAuthenticationManager(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration config, ILogger<JwtAuthenticationManager> logger)
        {
            this._userManager = userManager;
            _context = context;
            this._tokenKey = config.GetValue<string>("TokenKey");
            _logger = logger;

        }

        public async Task<object> Authenticate(AuthenticateRequest model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                _logger.LogWarning("Username or Password is null");
                return null;
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                _logger.LogInformation(model.Email + " is not confirm");
                return null;
            }
            if (user != null && user.User_type == 2)
            {
                _logger.LogInformation(model.Email + " it is admin");
                return "INVALID_USER";
            }
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                Authenticate a = new Authenticate();
                var token = GenerateJwtToken(user);
                a.Id = user.Id;
                a.Token = token;
                MobileSession session = new MobileSession();
                session.UserId = user.Id;
                session.FcmToken = model.FCMToken;
                session.DeviceType = "";
                if (!_context.MobileSessions.Select(e => e.UserId).ToList().Contains(user.Id))
                {
                    _context.MobileSessions.Add(session);
                    _context.SaveChanges();
                }
                else
                {
                    var updatedSession = _context.MobileSessions.Where(e => e.UserId == user.Id).FirstOrDefault();

                    updatedSession.DeviceType = session.DeviceType;
                    updatedSession.FcmToken = session.FcmToken;
                    _context.MobileSessions.Update(updatedSession);
                    _context.SaveChanges();
                }

                return a;
            }
            else
            {
                _logger.LogInformation(model.Email + " is invalid");
                return "INVALID_USER";
            }
        }

        public string GenerateCode(string email)
        {
            throw new NotImplementedException();
        }

        public string GenerateJwtToken(ApplicationUser user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_tokenKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            _logger.LogInformation("Token for " + user.UserName + "is" + tokenHandler.WriteToken(token));
            return tokenHandler.WriteToken(token);
        }
        public async Task<ApplicationUser> GetById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
        public struct RegisterReturn
        {
            public ApplicationUser User;
            public bool Success { get; set; }

                }
                public async Task<RegisterReturn> RegisterAsync(RegisterModel model)
                {
                    RegisterReturn r = new RegisterReturn();
                    Response response = new Response();
                    //validate
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        r.Success = false;
                        return r;
                    }
                    //map model to new user object
                    var rUser = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        CountryId = model.Country,
                         User_type = 1,
                        Status = 1,
                        Name = model.Name,
                        PhoneNumber = model.PhoneNumber,
 
                    };

                    r.User = rUser;
                    r.Success = true;
                    // hash password & save user 
                    await _userManager.CreateAsync(rUser, model.Password);
                    return r;
                }
                //public async Task<RegisterReturn> GoogleRegisterAsync(GoogleRegister model)
                //{
                //    RegisterReturn r = new RegisterReturn();
                //    Response response = new Response();
                //    //validate
                //    var user = await _userManager.FindByEmailAsync(model.Email);
                //    if (user != null)
                //    {
                //        r.Success = false;
                //        return r;
                //    }
                //    //map model to new user object
                //    var rUser = new ApplicationUser
                //    {
                //        UserName = model.Email,
                //        Email = model.Email,
                //        CountryId = model.Country,
                //        LanguageId = model.Language,
                //        User_type = 1,
                //        Status = 1,
                //        EmailConfirmed = true,
                //        Name = model.Name,
                //        PhoneNumber = model.PhoneNumber,
                //        NickName = model.NickName

                //    };

                //    r.User = rUser;
                //    r.Success = true;
                //    // hash password & save user 
                //    await _userManager.CreateAsync(rUser);
                //    return r;
                //}


                //public string GenerateCode(string email)
                //{
                //    byte[] hash;
                //    using (HMACSHA1 sha1 = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(_tokenKey)))
                //        hash = sha1.ComputeHash(UTF8Encoding.UTF8.GetBytes(email));
                //    string code = Convert.ToBase64String(hash);
                //    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                //    return code;
                //}



            //}    //    }

                ////public async Task<RegisterReturn> GoogleRegisterAsync(GoogleRegister model)
                //{
                //    RegisterReturn r = new RegisterReturn();
                //    Response response = new Response();
                //    //validate
                //    var user = await _userManager.FindByEmailAsync(model.Email);
                //    if (user != null)
                //    {
                //        r.Success = false;
                //        return r;
                //    }
                //    //map model to new user object
                //    var rUser = new ApplicationUser
                //    {
                //        UserName = model.Email,
                //        Email = model.Email,
                //        CountryId = model.Country,
                //        LanguageId = model.Language,
                //        User_type = 1,
                //        Status = 1,
                //        EmailConfirmed = true,
                //        Name = model.Name,
                //        PhoneNumber = model.PhoneNumber,
                //        NickName = model.NickName

                //    };

                //    r.User = rUser;
                //    r.Success = true;
                //    // hash password & save user 
                //    await _userManager.CreateAsync(rUser);
                //    return r;
                //}


             




        //    }


        //}
    }
}

