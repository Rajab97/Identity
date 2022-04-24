using MyBankIdentityService.Data.Entities;
using MyBankIdentityService.Models.ConfigModels;
using MyBankIdentityService.Services.Interfaces;
using Novell.Directory.Ldap;
using Novell.Directory.Ldap.Controls;
using Serilog;
using System;
using System.Linq;

namespace MyBankIdentityService.Services
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        private readonly ActiveDirectoryConfig activeDirectoryConfig;

        public ActiveDirectoryService(ActiveDirectoryConfig activeDirectoryConfig)
        {
            this.activeDirectoryConfig = activeDirectoryConfig;
        }

        public User GetUserFromAD(string username)
        {
            Log.Information($"GetUseFromAD called with username {username}");
            try
            {
                using (var connection = new LdapConnection { SecureSocketLayer = false })
                {
                    connection.Connect(activeDirectoryConfig.Host, LdapConnection.DefaultPort);
                    connection.Bind(activeDirectoryConfig.DistinguishedName, activeDirectoryConfig.Password);
                    var constraints = new LdapSearchConstraints();

                    constraints.SetControls(new LdapControl[] {
                         new LdapSortControl(new LdapSortKey("cn=config"), true)
                    });

                    var results = connection.Search(
                       $"OU={activeDirectoryConfig.OrganizationUnit},dc=accessbank,dc=local",
                        LdapConnection.ScopeSub,
                        $"(userPrincipalName={username})",
                        null,
                        false
                        );

                    var nextEntry = results.FirstOrDefault();
                    var user = new User
                    {
                        Username = nextEntry.GetAttribute("sAMAccountName").StringValue,
                        Fullname = nextEntry.GetAttribute("displayname").StringValue,
                        Email = nextEntry.GetAttribute("mail").StringValue,
                        HrCode = nextEntry.GetAttribute("st").StringValue,
                        UserPrincipal = nextEntry.GetAttribute("userPrincipalName").StringValue,
                        IsEnabled = nextEntry.GetAttribute("userAccountControl").StringValue == "512" ? true : false //512 means true, 514 means false
                    };

                    try
                    {
                        user.BranchCode = nextEntry.GetAttribute("postalCode").StringValue;
                    }catch{}

                    return user;
                }
            }
            catch (Exception ex)
            {
                Log.Information($"User {username} not found in Active Directory: {ex}");
                return null;
            }
        }

        public bool ValidateCredentials(string username, string password)
        {
            try
            {
                Log.Information($"Validate credentials called with username {username}");
                using (var connection = new LdapConnection { SecureSocketLayer = false })
                {
                    connection.Connect(activeDirectoryConfig.Host, LdapConnection.DefaultPort);
                    connection.Bind(username, password);
                    var result = connection.Bound;

                    return result;
                }
            }
            catch (Exception ex)
            {
                Log.Information(ex.ToString());
                return false;
            }
        }
    }
}
