using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Data;
using IdentityServer4.Models;
using IdentityServer4.Models.ViewModels;
using IdentityServer4.Test;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer4.Serivces
{
    public class UsersService : IUsersService
    {
        private readonly IDbContext _dbContext;
        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";
        bool isDecrypt = false; 
        public UsersService(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public TestUser AutoProvisionUser(string provider, string userId, List<Claim> claims)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(Users entity)
        {
            var check = GetById(entity.Id).Result;
            check.IsDeleted = true;
            check.DeletedDate = DateTime.Now;
            check.DeleteBy = "System";
            check.IsActive = false;
            //await Update(check);
        }

        public TestUser FindByExternalProvider(string provider, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<TestUser> FindBySubjectId(string subjectId)
        {
            var guid = new Guid(subjectId);
            var getUserByusername = await GetById(guid);
            var user = new TestUser
            {
                SubjectId = getUserByusername.Id.ToString(),
                Username = getUserByusername.Username,
                Password = getUserByusername.Password,
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, getUserByusername.Name),
                    new Claim(JwtClaimTypes.GivenName, getUserByusername.GivenName),
                    new Claim(JwtClaimTypes.FamilyName, getUserByusername.FamilyName),
                    new Claim(JwtClaimTypes.Email, getUserByusername.Email),
                    new Claim(JwtClaimTypes.EmailVerified, getUserByusername.EmailVerified.ToString(), ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, getUserByusername.WebSite),
                    new Claim(JwtClaimTypes.Address, getUserByusername.Address)
                }
            };
            return user;

        }
        public string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }
        public string Decrypt(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            isDecrypt = true;
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }
        public async Task<TestUser> FindByUsername(string username)
        {
            var getUserByusername = await GetByUsername(username);
          
            if (getUserByusername != null)
            {
                var user = new TestUser
                {
                    SubjectId = getUserByusername.Id.ToString(),
                    Username = getUserByusername.Username,
                    Password = getUserByusername.Password,
                    Claims =
                {
                    new Claim(JwtClaimTypes.Name, getUserByusername.Name),
                    new Claim(JwtClaimTypes.GivenName, getUserByusername.GivenName),
                    new Claim(JwtClaimTypes.FamilyName, getUserByusername.FamilyName),
                    new Claim(JwtClaimTypes.Email, getUserByusername.Email),
                    new Claim(JwtClaimTypes.EmailVerified, getUserByusername.EmailVerified.ToString(), ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, getUserByusername.WebSite),
                    new Claim(JwtClaimTypes.Address, getUserByusername.Address)
                }
                };

                return user;
            }
            return null;
        }

        public async Task<Users> GetById(Guid id)
        {
            await Task.Yield();
            var user =  _dbContext.Set<Users>().Where(o => o.Id == id).FirstOrDefault();
            if(user != null)
            {
                if(isDecrypt == false)
                {
                    var password = this.Decrypt(user.Password);
                    user.Password = password;
                }
            }
            return user;
        }

        public async Task<UsersDto> GetByIdUserTest(string id)
        {
            await Task.Yield();
            var guid = new Guid(id);
            var getUserById =_dbContext.Set<Users>().Where(o => o.Id == guid).FirstOrDefault();
            if (getUserById != null)
            {
                if(isDecrypt == false)
                {
                    var password = this.Decrypt(getUserById.Password);
                    getUserById.Password = password;
                }
                var user = new UsersDto(getUserById);

                return user;
            }
            return null;
        }
        public async Task<Users> GetByUsername(string username)
        {
            await Task.Yield();
            var user =  _dbContext.Set<Users>().Where(o => o.Username == username).FirstOrDefault();
            if (user != null)
            {
                if(isDecrypt == false) {
                    var password = this.Decrypt(user.Password);
                    user.Password = password;
                }
            }
            return user;
        }

        public Task<IQueryable<Users>> GetUsers()
        {
            return Task.FromResult(_dbContext.Set<Users>().AsNoTracking());
        }

        public async Task<UsersForm> Insert(UsersForm userForm,string otpCode)
        {
            try
            {
                var password = this.Encrypt(userForm.Password);
                var user = new Users(Guid.NewGuid())
                {
                    CreatedBy = "admin",
                    CreatedDate = DateTimeOffset.Now,
                    IsActive = true,
                    Username = userForm.Username,
                    Password = password,
                    Name = userForm.Name,
                    GivenName = userForm.GivenName,
                    FamilyName = userForm.FamilyName,
                    Email = userForm.Email,
                    EmailVerified = userForm.EmailVerified,
                    WebSite = userForm.WebSite,
                    Address = userForm.Address,
                    CurrentOTPCode = otpCode,
                    UserType = (int)userForm.UserType,
                    PhoneNumber = userForm.PhoneNumber
                };

                var result = await _dbContext.Set<Users>().AddAsync(user);
                await _dbContext.SaveChangesAsync();
                userForm.Id = user.Id.ToString();
                userForm.OTP = otpCode;
                return userForm;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Task Insert(IEnumerable<Users> entity)
        {
            throw new NotImplementedException();
        }

        public async Task<UsersForm> Update(UsersForm userForm,string id)
        {
            var guid = new Guid(id);
            var getUser = _dbContext.Set<Users>().Where(o => o.Id == guid).FirstOrDefault();

            var password = this.Encrypt(userForm.Password);

            getUser.ModifiedBy = "admin";
            getUser.ModifiedDate = DateTimeOffset.Now;
            getUser.Username = userForm.Username;
            getUser.Password = password;
            getUser.Name = userForm.Name;
            getUser.GivenName = userForm.GivenName;
            getUser.FamilyName = userForm.FamilyName;
            getUser.Email = userForm.Email;
            getUser.EmailVerified = userForm.EmailVerified;
            getUser.WebSite = userForm.WebSite;
            getUser.Address = userForm.Address;

            _dbContext.Set<Users>().Update(getUser);
            await _dbContext.SaveChangesAsync();
            return userForm;
        }


        public Task Update(IEnumerable<Users> entities)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidateCredentials(string username, string password)
        {
            var user = await FindByUsername(username);
            if (user != null)
            {
                return user.Password.Equals(password);
            }

            return false;
        }

        public async Task<UsersDto> VerifiedEmail(VerifiedOTP verifiedOTP)
        {
            var user = _dbContext.Set<Users>().Where(o => o.Email == verifiedOTP.Email && o.CurrentOTPCode == verifiedOTP.CodeOTP).FirstOrDefault();
            try
            {

                user.EmailVerified = true;

                _dbContext.Set<Users>().Update(user);
                await _dbContext.SaveChangesAsync();
                var userDto = new UsersDto(user);
                return userDto;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<Users> GetByPhoneNumberOTP(string phoneNumber, string oTP)
        {
            await Task.Yield();
            var now = DateTime.Now;
            var user = _dbContext.Set<Users>().Where(o => o.PhoneNumber == phoneNumber && o.CurrentOTPCode == oTP  && o.ExpiredOTP >= now).FirstOrDefault();
            if (user != null)
            {
                if (isDecrypt == false)
                {
                    var password = this.Decrypt(user.Password);
                    user.Password = password;
                }
            }
            return user;

        }

        public async Task<Users> GenerateOTP(string phoneNumber, string otp,DateTime expiredDate)
        {
            await Task.Yield();
            var user = _dbContext.Set<Users>().Where(o => o.PhoneNumber == phoneNumber).FirstOrDefault();
            if (user != null)
            {
                user.CurrentOTPCode = otp;
                user.ExpiredOTP = expiredDate;
                _dbContext.Set<Users>().Update(user);
                await _dbContext.SaveChangesAsync();
            }
          
            return user;
        }
    }
}
