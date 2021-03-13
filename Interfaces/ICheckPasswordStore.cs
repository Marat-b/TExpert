using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace TExp.Interfaces
{
    interface ICheckPasswordStore<TUser,in TKey> : IDisposable where TUser : class, IUser<TKey>
    {
        
        Task<bool> GetSetPasswordEnabledAsync(TUser user);
        /// <summary>
        /// Установка признака принудительный смены пароля
        /// </summary>
        /// <param name="user">Сущность</param>
        /// <param name="enabled">Истина или ложь</param>
        /// <returns>IdentityResult</returns>
        Task<IdentityResult> SetPasswordEnabledAsync(TUser user,bool enabled);
        Task<IdentityResult> SetChangePasswordEndDateAsync(TUser user);
        // Число дней прошедших после последнего изменения пароля
        Task<int> GetNumberDaysAfterLastChangePasswordAsync(TUser user);
    }
}
