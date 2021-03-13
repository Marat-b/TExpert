using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using TExp.Models;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using System.Text;
using  TExp.Classes;

namespace TExp.App_Start
{

    
    public class MyUserManager:UserManager<t_User,int> 
    {
        private readonly MyUserStore _store;
        private  int _maxPeriodForChangePassword = 0;
        private  int _maxPeriodSignin = 0;
        private  bool _lockoutEnabled;
        private bool _checkUsedPasswordEnabled;
        private int _maxUsedPassword = 0;
        //public MyUserManager(IQueryableUserStore<t_User,int> store) : base(store)
        public MyUserManager(MyUserStore store) : base(store)
        {
             this._store = store;
           //this. _maxPeriodForChangePassword = 1;
            //this._lockoutEnabled = true;
            //this._maxPeriodSignin = 1;
            
        } 
        
        public static MyUserManager Create(IdentityFactoryOptions<MyUserManager> options,IOwinContext context)
        {
#if DEBUG
            Debug.WriteLine("MyUserManager Create");
#endif

            var manager=new MyUserManager(new MyUserStore(context.Get<MyDbContext>()));

#if DEBUG
            if (manager != null)
            {
                Debug.WriteLine("manager is not null!");
            }
            else
            {
                Debug.WriteLine("manager is NOT null");
            }
#endif

            manager.PasswordValidator=new PasswordValidator
            {
                RequiredLength = Convert.ToInt32(ConfigurationManager.AppSettings["RequiredLength"]),
                RequireNonLetterOrDigit = Convert.ToBoolean(ConfigurationManager.AppSettings["RequireNonLetterOrDigit"]),
                RequireDigit = Convert.ToBoolean(ConfigurationManager.AppSettings["RequireDigit"]),
                RequireLowercase = Convert.ToBoolean(ConfigurationManager.AppSettings["RequireLowercase"]),
                RequireUppercase = Convert.ToBoolean(ConfigurationManager.AppSettings["RequireUppercase"])
            };

            // Если истино, учётка блокируется по умолчанию после созданаия
            manager.UserLockoutEnabledByDefault = Convert.ToBoolean(ConfigurationManager.AppSettings["UserLockoutEnabledByDefault"]);
            // Максимальное число попыток ввода пароля, после чего учётка блокируется
            manager.MaxFailedAccessAttemptsBeforeLockout = Convert.ToInt32(ConfigurationManager.AppSettings["MaxFailedAccessAttemptsBeforeLockout"]);
            // Количество времени в минутах, когда учётка остаётся блокированной
            manager.DefaultAccountLockoutTimeSpan=TimeSpan.FromMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"]));
            // Признак будет ли блокирована учётка
            manager.LockoutEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["LockoutEnabled"]);
            // Максимальный период времени, после которого надо менять пароль
            manager.MaxPeriodForChangePassword = Convert.ToInt32(ConfigurationManager.AppSettings["MaxPeriodForChangePassword"]);
            // Максимальный период времени (в днях), когда учётка будет блокированной, если не было входа в систему
            manager.MaxPeriodSignin = Convert.ToInt32(ConfigurationManager.AppSettings["MaxPeriodSignin"]);
            //Проверка на использование старый паролей, при вводе нового пароля
            manager.CheckUsedPasswordEnabled =
                Convert.ToBoolean(ConfigurationManager.AppSettings["CheckUsedPasswordEnabled"]);
            // Глубина проверки старых паролей
            manager.MaxUsedPassword = Convert.ToInt32(ConfigurationManager.AppSettings["MaxUsedPassword"]);

            return manager;
        }

        
        /// <summary>
        /// Проверка , не блокирована ли учётка
        /// </summary>
        /// <param name="userId">userr's ID </param>
        /// <returns></returns>

        public virtual async Task<bool> IsLockedOutAsync(int userId)
        {
            //ThrowIfDisposed();
            //var store = GetUserLockoutStore();
#if DEBUG
            Debug.WriteLine("** IsLockedOutAsync **");
            Debug.WriteLine("userId="+ userId);
#endif
            var user = await _store.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                throw new InvalidOperationException("UserIdNotFound");
            }
            //if (await _store.GetLockoutEnabledAsync(user).ConfigureAwait(false))
            //{
                var lockoutTime = await _store.GetLockoutEndDateAsync(user).ConfigureAwait((false));
                return lockoutTime >= DateTimeOffset.UtcNow;
            //}
           // return false;
        }

   
    /*
        public virtual async Task<bool> CheckPasswordAsync(MyUser user, string password)
        {
            //ThrowIfDisposed();
            //var passwordStore = GetPasswordStore();
            if (user == null)
            {
                return false;
            }
            return await VerifyPasswordAsync(user, password).ConfigureAwait(false);
        }
        */
        /*
        protected virtual async Task<bool> VerifyPasswordAsync( MyUser user,string password)
        {
            var hash = await _store.GetPasswordHashAsync(user).ConfigureAwait(false);
            var result = PasswordHasher.VerifyHashedPassword(hash, password);
#if DEBUG
            Debug.WriteLine("* VerifyPasswordAsync *");
            Debug.WriteLine("hash="+hash);
            Debug.WriteLine("password="+password);
            Debug.WriteLine("result=" + result.ToString());
#endif
            return PasswordHasher.VerifyHashedPassword(hash, password) != PasswordVerificationResult.Failed;
        }
        */
        
            /// <summary>
            /// Проверка на количесто дней после последней установки пароля, а также
            /// на признак принудельной установки пароля.
            /// False - надо менять пароль
            /// 
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
        public async Task<bool> CheckLastSetPasswordAsync(t_User user)
        {
            int numberDays=0;

            if (await _store.GetSetPasswordEnabledAsync(user))
            {
                //надо обязательно менять пароль
                return false;
            }

             numberDays =await _store.GetNumberDaysAfterLastChangePasswordAsync(user);
            if (numberDays < MaxPeriodForChangePassword || MaxPeriodForChangePassword==0)
            {
                
                return true;
                //return false; //на время теста
            }

           

            //пора менять пароль
            return false;
        }

        /// <summary>
        /// Проверка на количество дней прошедших после 
        /// последнего входа в систему.
        /// false - учётку надо блокировать
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> CheckLastSigninAsync(t_User user)
        {
            int numberDays = 0;
            numberDays = await _store.GetNumberDaysAfterLastSigninAsync(user);
            if (numberDays < MaxPeriodSignin || MaxPeriodSignin==0)
            {

                return true;
            }

            return false;
        }


        /// <summary>
        /// Установить дату последнего входа в систему
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> SetSigninEndDateAsync(t_User user)
        {
             await _store.SetSigninEndDateAsync(user); //.ConfigureAwait(true);
            if (await UpdateAsync(user)==IdentityResult.Success)
            {
                return true;
            }
            return false;
        }

        /*
        /// <summary>
        /// Сброс признака принудительного смены пароля
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> ResetLastSignin(t_User user)
        {
            if (await _store.ResetLastSignin(user))
            {
                if (await UpdateAsync(user) == IdentityResult.Success)
                {
                    return true;
                }
            }
            return false;
        }*/


        /// <summary>
        /// Установка текущей даты смены пароля
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IdentityResult> SetChangePasswordEndDateAsync(t_User user)
        {
            if (await _store.SetChangePasswordEndDateAsync(user)==IdentityResult.Success)
            {
                if (await UpdateAsync(user) == IdentityResult.Success)
                {
                    return IdentityResult.Success;

                }

            }

            return IdentityResult.Failed();
        }

        /// <summary>
        /// Установка признака принудительной смены пароля
        /// </summary>
        /// <param name="user">Сущность</param>
        /// <param name="enabled">Ложь или истина</param>
        /// <returns></returns>
        public async Task<IdentityResult> SetPasswordEnabledAsync(t_User user, bool enabled)
        {
            if (await _store.SetPasswordEnabledAsync(user, enabled) == IdentityResult.Success)
            {
                if (await UpdateAsync(user) == IdentityResult.Success)
                {
                    return IdentityResult.Success;
                }
            }
            return IdentityResult.Failed();
        }

        /// <summary>
        /// Получение списка пользователей
        /// </summary>
        /// <returns>IEnumerable AccountViewModel</returns>
        public IEnumerable<AccountViewModel> GetListUsers()
        {
            return _store.Users.Select(s => new AccountViewModel() {Id = s.UserID, UserName = s.UserName, FIO = s.FIO});
        }

        /// <summary>
        /// Получение identity пользователя для смены пароля
        /// </summary>
        /// <param name="userId">ИД пользователя</param>
        /// <returns>Модель ResetPasswordInputModel</returns>
        public ResetPasswordInputModel GetUserForResetPassword(int userId)
        {
            return
                _store.Users.Select(s => new ResetPasswordInputModel() {UserName = s.UserName, Id = s.UserID})
                    .Where(w => w.Id == userId)
                    .FirstOrDefault();
        }

        /// <summary>
        /// Get user ID by user name
        /// </summary>
        /// <param name="userName">user name</param>
        /// <returns>User ID</returns>
        public int GetIdByName(string userName)
        {
            return _store.Users.Where(w => w.UserName==userName).Select(s =>  s.UserID).FirstOrDefault();
        }


        /// <summary>
        /// Get list of user roles
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserRoleViewModel> GetListUserRoles()
        {
            return _store.UserRoles.Select(s => new UserRoleViewModel() {RoleId  = s.RoleID, Name = s.Name});
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="model">RegisterInputModel - usser's model for registration</param>
        /// <param name="selectedRole">Selected role for user </param>
        /// <returns></returns>
        public async Task<IdentityResult> RegisterUserAsync(RegisterInputModel model, string selectedRole)
        {
            var user = Activator.CreateInstance<t_User>();
            user.UserName = model.UserName;
            user.FIO = model.FIO;
            var result = await CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
#if DEBUG
                Debug.WriteLine("resulult is secceeded");
#endif
                if (selectedRole != null)
                {
                    //var result2 = 
                    await _store.AddToRoleAsync(user, selectedRole);
                    //if (result2.Succeeded)
                   // {
#if DEBUG
                        Debug.WriteLine("Roles is added");
#endif
                        return IdentityResult.Success;
                }
            }
            return IdentityResult.Failed();
        }

        /// <summary>
        /// Создание нового пользователя
        /// </summary>
        /// <param name="model"> модель AccountViewModel</param>
        /// <param name="selectedRole">Название роли</param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateUserAsync(AccountViewModel model)
        {
            var user = Activator.CreateInstance<t_User>();
            user.UserName = model.UserName;
            user.FIO = model.FIO;
            user.Position = model.Position;
            user.LockoutEnabled= model.LockoutEnabled;
            user.SetPasswordEnabled= model.SetPasswordEnabled;
            var result = await CreateAsync(user, "~!#");
            if (result.Succeeded)
            {
#if DEBUG
                Debug.WriteLine("resulult is secceeded");
#endif
                if (model.UserRole != null)
                {
                    //var result2 = 
                    await _store.AddToRoleAsync(user, model.UserRole);
                    await _store.UpdateAsync(user);
                    //if (result2.Succeeded)
                    // {
#if DEBUG
                    Debug.WriteLine("Roles is added");
#endif
                    return IdentityResult.Success;
                }
            }
            return IdentityResult.Failed();
        }


        /// <summary>
        /// Сохранить вновь введённый пароль, как использованный
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="password">User password (plain text)</param>
        /// <returns></returns>
        public async Task<IdentityResult> SetUsedPasswordAsync(int userId, string password)
        {
            if (_checkUsedPasswordEnabled)
            {
                string hashPassword = PasswordHasher.HashPassword(password);
                await _store.SetUsedPassword(userId, hashPassword);
            }
            return IdentityResult.Success;
        }

        /// <summary>
        /// Проверка на уже использованный пароль.
        /// Истина - пароль уже был использован
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="password">Plain password</param>
        /// <returns>Boolean</returns>
        public async Task<bool> IsUsedPasswordAsync(int userId, string password)
        {
            bool ret = false;
            //string hashPassword = PasswordHasher.HashPassword(password);
            if (_checkUsedPasswordEnabled)
            {
                foreach (string usedPasswordHash in await _store.GetUsedPassword(userId, _maxUsedPassword))
                {
#if DEBUG
                    Debug.WriteLine("+++ MyUserManager.IsUsedPasswordAsync usedPasswordHash=" + usedPasswordHash);
#endif
                    if (PasswordHasher.VerifyHashedPassword(usedPasswordHash, password) ==
                        PasswordVerificationResult.Success)
                    {
#if DEBUG
                        Debug.WriteLine("++++++ MyUserManager.IsUsedPasswordAsync  PasswordVerificationResult.Success");
#endif
                        ret = true;
                        break;
                    }
                }
            }
            //return await _store.IsUsedPassword(userId, hashPassword, _maxUsedPassword);
            return ret;
        }

        /// <summary>
        /// Сброс пароля
        /// </summary>
        /// <param name="userId">ИД пользователя</param>
        /// <param name="password">пароль пользователя</param>
        /// <returns></returns>
        public async Task<bool> SetPasswordAsync(int userId, string password)
        {
            var user =await _store.Users.Where(w => w.UserID == userId).FirstOrDefaultAsync();
            if (user != null)
            {
                string passwordHash = PasswordHasher.HashPassword(password);
               await _store.SetPasswordHashAsync(user, passwordHash);
                if (await UpdateAsync(user) == IdentityResult.Success)
                {
                    return true;
                }
                return false;

               
            }
            return false;
        }

        
        /// <summary>
        /// Добавить пользователю соотвествующую роль
        /// </summary>
        /// <param name="userId">ИД пользователя</param>
        /// <param name="roleName">Наименование роли</param>
        /// <returns></returns>
        public async Task<bool> AddUserToRoleAsync(int userId, string roleName)
        {
            var user = await _store.Users.Where(w => w.UserID == userId).FirstOrDefaultAsync();
            if (user != null)
            {
                await _store.AddToRoleAsync(user, roleName);
                if (await UpdateAsync(user) == IdentityResult.Success)
                {
                    return true;
                }
                return false;
                //return true;
            }
            return false;
        }


        /// <summary>
        /// Убрать пользователя из соответствующей роли
        /// </summary>
        /// <param name="userId">ИД пользователя</param>
        /// <param name="roleName">Наименование роли</param>
        /// <returns></returns>
        public async Task<bool> RemoveUserFromRoleAsync(int userId, string roleName)
        {
            var user = await _store.Users.Where(w => w.UserID == userId).FirstOrDefaultAsync();
            if (user != null)
            {
                await _store.RemoveFromRoleAsync(user, roleName);
                if (await UpdateAsync(user) == IdentityResult.Success)
                {
                    return true;
                }
                return false;
                //return true;
            }
            return false;
        }

        /// <summary>
        /// Убрать пользователя из текующей роли
        /// </summary>
        /// <param name="userId">ИД пользователя</param>
        /// <returns></returns>
        public async Task<bool> RemoveUserFromRoleAsync(int userId)
        {
            var user = await _store.Users.FirstOrDefaultAsync(f => f.UserID==userId);
            if (user != null)
            {
#if DEBUG
                Debug.WriteLine("^^^^^  user is not null!");
#endif
                string roleName = _store.GetRolesAsync(user).Result.FirstOrDefault();
                if (roleName != null)
                {
#if DEBUG
                    Debug.WriteLine("^^^^ roleName is not null");
#endif
                    await _store.RemoveFromRoleAsync(user, roleName);
                    if (await UpdateAsync(user) == IdentityResult.Success)
                {
#if DEBUG
                        Debug.WriteLine("^^^^ user update is true");
#endif
                        return true;
                }
                return false;
                    //return true;
                }
#if DEBUG
                else
                {
                    Debug.WriteLine("^^^^ roleName null");
                }
#endif
            }
#if DEBUG
            else
            {
                Debug.WriteLine("^^^^^  user is null");
            }
#endif
            return false;
        }



        ////////////////////////////////////////////////////// \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        /// <summary>
        /// Блокировка учётки
        /// </summary>
        public bool LockoutEnabled
        {
            get { return _lockoutEnabled; }
            set { _lockoutEnabled = value; }
        }

        /// <summary>
        /// Максимальный период времени, после которого надо
        /// менять пароль
        /// Если 0 - период бесконечен
        /// </summary>
        public int MaxPeriodForChangePassword
        {
            get { return _maxPeriodForChangePassword; }
            set { _maxPeriodForChangePassword = value; }
        }

        /// <summary>
        /// Максимальный период времени , после которого
        /// учётка блокируется, если не было входа в систему
        /// Если 0 - период бесконечен
        /// </summary>
        public int MaxPeriodSignin
        {
            get { return _maxPeriodSignin; }
            set { _maxPeriodSignin = value; }
        }

        /// <summary>
        /// Проверка использование старого пароля, при вводе нового пароля
        /// </summary>
        public bool CheckUsedPasswordEnabled
        {
            get { return _checkUsedPasswordEnabled; }
            set { _checkUsedPasswordEnabled = value; }
        }

        /// <summary>
        /// Глубина проверки использованный паролей
        /// </summary>
        public int MaxUsedPassword
        {
            get { return _maxUsedPassword; }
            set { _maxUsedPassword = value; }
        }

    }


    /// <summary>
    /// //////////////////////// MySignInManager  \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    /// </summary>


    public class MySignInManager : SignInManager<t_User,int>
    {
        private readonly MyUserManager _myUserManager;

        public MySignInManager(MyUserManager myUserManager, IAuthenticationManager authenticationManager)
            : base(myUserManager, authenticationManager)
        {
            this._myUserManager = myUserManager;
        }

        public static MySignInManager Create(IdentityFactoryOptions<MySignInManager> options, IOwinContext context)
        {
            return new MySignInManager(context.GetUserManager<MyUserManager>(), context.Authentication);
        }


        /// <summary>
        /// Проверка и регистрация пользователя
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="isPersistent"></param>
        /// <param name="shouldLockout"></param>
        /// <returns></returns>
        
        public virtual async Task<SignInCode> PasswordSignInAsync(string userName, string password, bool isPersistent,
            bool shouldLockout)
        {
#if DEBUG
            Debug.WriteLine("**** Begin PasswordSignInAsync *****");
#endif
            if (UserManager == null)
            {
                return SignInCode.Failure;
            }

            var user = await _myUserManager.FindByNameAsync(userName); //.WithCurrentCulture();
            if (user == null)
            {
                return SignInCode.Failure;
            }
#if DEBUG
            else
            {
                Debug.WriteLine("user is not null");
            }
#endif
            // должны ли учётки блокироваться
            // await _myUserManager.SetLockoutEnabledAsync(user.UserID, shouldLockout);

         

            //Проверка пароля
            if (await _myUserManager.CheckPasswordAsync(user, password)) //.WithCurrentCulture())
            {
#if DEBUG
                Debug.WriteLine("* CheckPasswordAsync *");
                Debug.WriteLine("SignInCode.Success!!");
#endif



                // Проверка на постоянную блокировку
                if (await _myUserManager.GetLockoutEnabledAsync(user.UserID))
                {
                    return SignInCode.LockedOut;
                }

                // Активизирована ли функция  блокировки учётки в системе
                if (_myUserManager.LockoutEnabled)
                {
                    // Блокирована ли учётка?
                    if (await _myUserManager.IsLockedOutAsync(user.UserID)) //.WithCurrentCulture())
                    {
#if DEBUG
                        Debug.WriteLine("user is lockedout");
#endif
                        return SignInCode.LockedOut;
                    }
#if DEBUG
                    else
                    {
                        Debug.WriteLine("user is not lockedout");
                    }
#endif

                    // Проверка как давно не было входа в систему
                    if (!await _myUserManager.CheckLastSigninAsync(user))
                    {
                        // Вход в систему был больше положенного срока
                        await _myUserManager.SetLockoutEnabledAsync(user.UserID, true);
                        return SignInCode.LockedOut;
                    }
                }


                

                // Пароль верный

                // Проверка даты последнего изменения пароля
                if (await _myUserManager.CheckLastSetPasswordAsync(user))
                {
#if DEBUG
                    Debug.WriteLine("+++ CheckLastSetPasswordAsync is true");
#endif
                    // Регистрация в системе
                    AuthenticationManager.SignOut();
                    var identity = await _myUserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignIn(identity);
                    if (_myUserManager.LockoutEnabled)
                    {
                        await _myUserManager.ResetAccessFailedCountAsync(user.UserID);
                        await _myUserManager.SetSigninEndDateAsync(user);
                    }
                    return SignInCode.Success;
                }
                else
                {
                    // Настал период изменения пароля
#if DEBUG
                    Debug.WriteLine("+++ CheckLastSetPasswordAsync is false");
#endif
                    return SignInCode.ChangePassword;
                }

                
                //return await SignInOrTwoFactor(user, isPersistent).WithCurrentCulture();
            }

            else
            {
                // Пароль неверный
#if DEBUG
                Debug.WriteLine("* CheckPasswordAsync *");
                Debug.WriteLine("SignInCode.Failure!");
#endif

                // Активизирована ли функция  блокировки учётки в системе
                if (_myUserManager.LockoutEnabled)
                {

#if DEBUG
                    Debug.WriteLine("++++ GetLockoutEnabledAsync is enabled");
#endif
                    // Увелечение счётчика неудачных попыток ввода пароля
                    if (await _myUserManager.AccessFailedAsync(user.UserID) == IdentityResult.Success)
                    {
#if DEBUG
                        Debug.WriteLine("~~~ AccessFailedAsync is working!");
#endif
                        if (await _myUserManager.IsLockedOutAsync(user.UserID))
                        {
                            //Заблокировать учётку
                            return SignInCode.LockedOut;
                        }
                            

                    }

             
                }
#if DEBUG
                else
                {
                    Debug.WriteLine("++++ GetLockoutEnabledAsync is disabled");
                }
#endif
               
            }


            return SignInCode.Failure;
        }
        

    }
}