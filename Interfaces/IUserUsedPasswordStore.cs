using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace TExp.Interfaces
{
    interface IUserUsedPasswordStore<TUser, in TKey> : IDisposable where TUser : class, IUser<TKey>
    {
        Task<bool> IsUsedPassword(TKey userId,string hashPassword, int maxCountUsedPassword);
        Task SetUsedPassword(TKey userId, string hashPassword);
        Task<IEnumerable<string>> GetUsedPassword(TKey userId, int maxCountUsedPassword);
    }
}
