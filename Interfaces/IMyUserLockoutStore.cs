using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.DynamicData;
using Microsoft.AspNet.Identity;

namespace TExp.Interfaces
{
    public interface IMyUserLockoutStore<TUser, in TKey> : IUserLockoutStore<TUser, TKey>, IDisposable where TUser : class, IUser<TKey>
    {
        Task<bool> CheckNotActiveAccountAsync(TUser user);
        Task<bool> CheckLastSetPasswordAsync(TUser user);
    }
}
