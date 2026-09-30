using EmployeeAccountFunction.Interfaces;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAccountFunction.Services
{
    public class EntraUserService: IEntraUserService
    {
        private readonly GraphServiceClient _graph;

        public EntraUserService(GraphServiceClient graph)
        {
            _graph = graph;
        }
        public async Task<User> CreateUserAsync(
        string displayName,
        string upn,
        string mailNickname,
        string password)
        {
            var user = new User
            {
                AccountEnabled = true,
                DisplayName = displayName,
                MailNickname = mailNickname,
                UserPrincipalName = upn,
                PasswordProfile = new PasswordProfile
                {
                    Password = password,
                    ForceChangePasswordNextSignIn = true
                }
            };

            return await _graph.Users.PostAsync(user)
                ?? throw new InvalidOperationException(
                    "Microsoft Graph returned no user.");
        }
    }
}
