using IdentityServer4.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer4.Services
{
    public interface IRolesService
    {
        Task<List<RolesDto>> GetRolesByType(int roleType);
        Task<RolesForm> Insert(RolesForm rolesForm);
        Task<RolesForm> Update(RolesForm rolesForm);

    }
}
