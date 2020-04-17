using IdentityServer4.Data;
using IdentityServer4.Models;
using IdentityServer4.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer4.Services
{
    public class RolesService : IRolesService
    {
        private readonly IDbContext _dbContext;

        public RolesService(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<RolesForm>> GetRolesByType(int roleType)
        {
            var result = _dbContext.Set<Roles>().Where(o => o.RoleType == roleType)
                .ToList()
                .Select(o => new RolesForm(o))
                .ToList();

            return result;
        }

        public async Task<RolesForm> Insert(RolesForm rolesForm)
        {
            var user = new Roles(Guid.NewGuid())
            {
                CreatedBy = "admin",
                CreatedDate = DateTimeOffset.Now,
                IsActive = true,
                RoleName = rolesForm.RoleName,
                RoleType = rolesForm.RoleType
            };
            var result = await _dbContext.Set<Roles>().AddAsync(user);
            await _dbContext.SaveChangesAsync();
            rolesForm.Id = user.Id.ToString();
            return rolesForm;
        }

        public async Task<RolesForm> Update(RolesForm rolesForm)
        {
            var id = new Guid(rolesForm.Id);
            var role = _dbContext.Set<Roles>().Where(o => o.Id == id).FirstOrDefault();
            role.RoleName = rolesForm.RoleName;
            role.RoleType = rolesForm.RoleType;
             _dbContext.Set<Roles>().Update(role);
            await _dbContext.SaveChangesAsync();
            return rolesForm;
        }
    }
}
