using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace TExp.Interfaces
{
    interface ICheckAccountStore<TUser, in TKey> : IDisposable where TUser : class, IUser<TKey>
    {
        Task<int> GetNumberDaysAfterLastSigninAsync(TUser user);
        Task SetSigninEndDateAsync(TUser user);
        //Task<bool> ResetLastSignin(TUser user);
    }
}
