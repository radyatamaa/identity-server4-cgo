using IdentityServer4.Data;
using IdentityServer4.Models;
using IdentityServer4.Models.ViewModels;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public async Task<List<RolesDto>> GetRolesByType(int roleType)
        {
            var result = new List<RolesDto>();
            var query = _dbContext.Set<Roles>().Where(o => o.RoleType == roleType)
                .ToList();
            query.ForEach(o =>
            {
                var queryPermissionRolesIds =
                _dbContext.Set<PermissionRoleMapping>()
                .Where(c => c.RoleId == o.Id).Select(c => c.PermissionId);

                var permissions = _dbContext.Set<PermissionRecord>()
                .Where(c => queryPermissionRolesIds.Contains(c.Id)).ToList();

                var dto = new RolesDto(o, permissions);
                result.Add(dto);
                
            });
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
                RoleType = rolesForm.RoleType,
                Description = rolesForm.Description
            };
            var result = await _dbContext.Set<Roles>().AddAsync(user);

            if (rolesForm.PermissionId.Count != 0)
            {
                foreach(var permissionId in rolesForm.PermissionId)
                {
                    var permissionRoleMap = new PermissionRoleMapping()
                    {
                        PermissionId = permissionId,
                        RoleId = user.Id
                    };
                    await _dbContext.Set<PermissionRoleMapping>().AddAsync(permissionRoleMap);
                }
            }

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
            role.Description = rolesForm.Description;

             _dbContext.Set<Roles>().Update(role);

            if (rolesForm.PermissionId.Count != 0)
            {

                var delete = _dbContext.Set<PermissionRoleMapping>().Where(o => o.RoleId == id).ToList();
                if (delete.Count != 0)
                {
                    _dbContext.Set<PermissionRoleMapping>().RemoveRange(delete);
                }

                foreach (var permissionId in rolesForm.PermissionId)
                {
                    var permissionRoleMap = new PermissionRoleMapping()
                    {
                        PermissionId = permissionId,
                        RoleId = id
                    };
                    await _dbContext.Set<PermissionRoleMapping>().AddAsync(permissionRoleMap);
                }
            }
            else
            {

                var delete = _dbContext.Set<PermissionRoleMapping>().Where(o => o.RoleId == id).ToList();
                if (delete.Count != 0)
                {
                    _dbContext.Set<PermissionRoleMapping>().RemoveRange(delete);
                }
            }

            await _dbContext.SaveChangesAsync();
            return rolesForm;
        }
    }
}
