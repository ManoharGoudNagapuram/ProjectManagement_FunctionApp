using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAccountFunction.Interfaces
{
    public interface IEntraUserService
    {
        Task<User> CreateUserAsync(
        string displayName,
        string upn,
        string mailNickname,
        string password);
    }
}
