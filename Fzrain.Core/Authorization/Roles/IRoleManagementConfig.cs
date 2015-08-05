using System.Collections.Generic;

namespace Fzrain.Authorization.Roles
{
    public interface IRoleManagementConfig
    {
        List<StaticRoleDefinition> StaticRoles { get; }
    }
}