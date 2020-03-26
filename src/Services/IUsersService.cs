using IdentityServer4.Models;
using IdentityServer4.Models.ViewModels;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Serivces
{
    public interface IUsersService
    {
        Task<IQueryable<Users>> GetUsers();
        Task<UsersDto> GetByIdUserTest(string id);
        Task<Users> GetById(Guid id);
        Task<Users> GetByUsername(string username);
        Task<UsersForm> Insert(UsersForm entity);
        Task Insert(IEnumerable<Users> entity);
        Task<UsersForm> Update(UsersForm entity,string id);
        Task Update(IEnumerable<Users> entities);
        Task Delete(Users entity);
        Task<bool> ValidateCredentials(string username, string password);
        Task<TestUser> FindBySubjectId(string subjectId);
        Task<TestUser> FindByUsername(string username);
        TestUser FindByExternalProvider(string provider, string userId);
        TestUser AutoProvisionUser(string provider, string userId, List<Claim> claims);
    }
}
