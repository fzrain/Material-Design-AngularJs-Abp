using Abp.Collections;

namespace Fzrain.Authorization.Users
{
    public class UserManagementConfig : IUserManagementConfig
    {
        public ITypeList<object> ExternalAuthenticationSources { get; set; }

        public UserManagementConfig()
        {
            ExternalAuthenticationSources = new TypeList();
        }
    }
}