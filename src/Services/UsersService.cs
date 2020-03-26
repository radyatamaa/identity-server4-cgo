using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        public async Task<TestUser> FindByUsername(string username)
        {
            var getUserByusername = await GetByUsername(username);
            if(getUserByusername != null)
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
            return _dbContext.Set<Users>().Where(o => o.Id == id).FirstOrDefault();
        }

        public async Task<UsersDto> GetByIdUserTest(string id)
        {
            await Task.Yield();
            var guid = new Guid(id);
            var getUserById =_dbContext.Set<Users>().Where(o => o.Id == guid).FirstOrDefault();
            if (getUserById != null)
            {
                var user = new UsersDto(getUserById);

                return user;
            }
            return null;
        }
        public async Task<Users> GetByUsername(string username)
        {
            await Task.Yield();
            return _dbContext.Set<Users>().Where(o => o.Username == username).FirstOrDefault();
        }

        public Task<IQueryable<Users>> GetUsers()
        {
            return Task.FromResult(_dbContext.Set<Users>().AsNoTracking());
        }

        public async Task<UsersForm> Insert(UsersForm userForm)
        {
            try
            {

                var user = new Users(Guid.NewGuid())
                {
                    CreatedBy = "admin",
                    CreatedDate = DateTimeOffset.Now,
                    IsActive = true,
                    Username = userForm.Username,
                    Password = userForm.Password,
                    Name = userForm.Name,
                    GivenName = userForm.GivenName,
                    FamilyName = userForm.FamilyName,
                    Email = userForm.Email,
                    EmailVerified = userForm.EmailVerified,
                    WebSite = userForm.WebSite,
                    Address = userForm.Address
                };

                var result = await _dbContext.Set<Users>().AddAsync(user);
                await _dbContext.SaveChangesAsync();
                userForm.Id = user.Id.ToString();
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

            getUser.ModifiedBy = "admin";
            getUser.ModifiedDate = DateTimeOffset.Now;
            getUser.Username = userForm.Username;
            getUser.Password = userForm.Password;
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
    }
}
