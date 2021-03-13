using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Globalization;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Resources;
using System.Diagnostics;
using System.Web.UI.WebControls;
//using testTS.Classes;
using TExp.Interfaces;


namespace TExp.Models
{
    public class MyUserStore : IQueryableUserStore<t_User,int>,
        IUserRoleStore<t_User,int>,
    IUserPasswordStore<t_User, int>,
        IUserLockoutStore<t_User,int> ,
        IUserTwoFactorStore<t_User,int>,
        ICheckPasswordStore<t_User,int>,
        ICheckAccountStore<t_User,int>,
        IUserUsedPasswordStore<t_User,int>


    {
        private readonly MyDbContext _db;

        public MyUserStore(MyDbContext db )
        {
            /*if (db == null)
            {
                throw new ArgumentNullException("db");
            } */

            _db = db;
            
            AutoSaveChanges = true;
        }

        /// <summary>
        ///     If true will call SaveChanges after CreateAsync/UpdateAsync/DeleteAsync
        /// </summary>
        public bool AutoSaveChanges { get; set; }

        // IQueryableUserStore<User, int>
        /*
        public IQueryable<t_User> Users
        {
            get {  }
        }
        */
      

        public IQueryable<t_User> Users
        {
            get
            {
                return _db.User;
                //throw new NotImplementedException();
            }
        }

        // IUserStore<User, Key>

        public Task CreateAsync(t_User user)
        {
            /* if (user == null)
            {
                throw new ArgumentNullException("user");
            }*/

            t_User userDb = user; // ToUser(user);

            return Task.Factory.StartNew(() => { SaveUser(userDb); });
        }

        public Task DeleteAsync(t_User user)
        {
            /*if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            _db.t_User.Remove(user);
            SaveChanges();
            return Task.FromResult<Object>(null);*/
                 throw new System.NotImplementedException();
        }
        /*
        public Task<t_User> FindByIdAsync(string userId)
        {
            return _db.t_User
                .Include(u => u.t_UserRole)
                .FirstOrDefaultAsync(u => u.UserID.Equals(userId));
            //return Task.Factory.StartNew<t_User>(() => FindUser(userId, FindKeyType.Id));
        }*/

        public Task<t_User> FindByIdAsync(int userId)
        {
            return _db.User
                .Include(u => u.t_UserRole)
                .FirstOrDefaultAsync(u => u.UserID.Equals(userId));
            //return Task.Factory.StartNew<t_User>(() => FindUser(userId.ToString(), FindKeyType.Id));
        }

        public Task<t_User> FindByNameAsync(string userName)
        {
            return _db.User
                .Include(u => u.t_UserRole)
                .FirstOrDefaultAsync(u => u.UserName == userName);
            //return Task.Factory.StartNew<t_User>(() => FindUser(userName, FindKeyType.Name));
        }

        public Task UpdateAsync(t_User user)
        {
            /*if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            _db.Entry(user).State = EntityState.Modified;
            return SaveChanges();*/
            return Task.Factory.StartNew(() =>
            {
                
                    t_User userDbOrig = _db.User.Find(user.Id);
                    userDbOrig.UserName = user.UserName;
                    userDbOrig.PasswordHash = user.PasswordHash;
                    //userDbOrig.Email = user.Email;
                    _db.Entry(userDbOrig).State = EntityState.Modified;
                    _db.SaveChanges();
                
            });
        }

        ////////////  IUserPasswordStore \\\\\\\\\\\\\\\\\\\\\\\\\\

        public Task<string> GetPasswordHashAsync(t_User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(t_User user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetPasswordHashAsync(t_User user, string passwordHash)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        /////////////////////// IUserLoginStore<User, Key>  \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        /*
        public Task AddLoginAsync(t_User user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            var userLogin = Activator.CreateInstance<t_User>();
            userLogin.UserID = user.Id;
            userLogin.LoginProvider = login.ProviderKey;
            userLogin.ProviderKey = login.ProviderKey;
            user.Logins.Add(userLogin);
            return Task.FromResult(0);
        }

        public async Task<t_User> FindAsync(UserLoginInfo login)
        {
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            var provider = login.LoginProvider;
            var key = login.ProviderKey;

            var userLogin = await _db.UserLogins.FirstOrDefaultAsync(l => l.LoginProvider == provider && l.ProviderKey == key);

            if (userLogin == null)
            {
                return default(t_User);
            }

            return await _db.t_User
                .Include(u => u.Logins).Include(u => u.Roles).Include(u => u.Claims)
                .FirstOrDefaultAsync(u => u.Id.Equals(userLogin.UserId));
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(t_User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult<IList<UserLoginInfo>>(user.Logins.Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey)).ToList());
        }

        public Task RemoveLoginAsync(t_User user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            var provider = login.LoginProvider;
            var key = login.ProviderKey;

            var item = user.Logins.SingleOrDefault(l => l.LoginProvider == provider && l.ProviderKey == key);

            if (item != null)
            {
                user.Logins.Remove(item);
            }

            return Task.FromResult(0);
        }*/

        // IUserClaimStore<User, int>
/*
        public Task AddClaimAsync(t_User user, Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }

            var item = Activator.CreateInstance<UserClaim>();
            item.UserId = user.Id;
            item.ClaimType = claim.Type;
            item.ClaimValue = claim.Value;
            user.Claims.Add(item);
            return Task.FromResult(0);
        }

        public Task<IList<Claim>> GetClaimsAsync(t_User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult<IList<Claim>>(user.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList());
        }

        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }

            foreach (var item in user.Claims.Where(uc => uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type).ToList())
            {
                user.Claims.Remove(item);
            }

            foreach (var item in _db.UserClaims.Where(uc => uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type).ToList())
            {
                _db.UserClaims.Remove(item);
            }

            return Task.FromResult(0);
        }*/

        // IUserRoleStore<User, int>

        public IQueryable<t_UserRole> UserRoles
        {
            get { return _db.UserRole; }
        }

        public Task AddToRoleAsync(t_User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("ValueCannotBeNullOrEmpty", "roleName");
            }

            var userRole = _db.UserRole.SingleOrDefault(r => r.Name == roleName);

            if (userRole == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Resources.RoleNotFound", new object[] { roleName }));
            }
            
            user.t_UserRole.Add(userRole);
            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(t_User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult<IList<string>>(user.t_UserRole.Join(_db.UserRole, ur => ur.RoleID, r => r.RoleID, (ur, r) => r.Name).ToList());
        }


        

        public Task<bool> IsInRoleAsync(t_User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Resources.ValueCannotBeNullOrEmpty", "roleName");
            }

            return Task.FromResult(_db.UserRole.Any(r => r.Name == roleName && r.t_User.Any(u => u.Id.Equals(user.Id))));
        }

        public Task RemoveFromRoleAsync(t_User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Resources.ValueCannotBeNullOrEmpty", "roleName");
            }

            var userRole = user.t_UserRole.SingleOrDefault(r => r.Name == roleName);

            if (userRole != null)
            {
                user.t_UserRole.Remove(userRole);
            }

            return Task.FromResult(0);
        }

        // IUserSecurityStampStore<User, int>
        /*
        public Task<string> GetSecurityStampAsync(t_User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(t_User user, string stamp)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        // IUserEmailStore<User, int>

        public Task<t_User> FindByEmailAsync(string email)
        {
            return _db.Users
                .Include(u => u.Logins).Include(u => u.Roles).Include(u => u.Claims)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public Task<string> GetEmailAsync(t_User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(t_User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailAsync(t_User user, string email)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.Email = email;
            return Task.FromResult(0);
        }

        public Task SetEmailConfirmedAsync(t_User user, bool confirmed)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        // IUserPhoneNumberStore<User, int>

        public Task<string> GetPhoneNumberAsync(t_User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberAsync(User user, string phoneNumber)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        // IUserTwoFactorStore<User, int>

        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }*/

        /////////// IUserLockoutStore<User, int> \\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public Task<int> GetAccessFailedCountAsync(t_User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(t_User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(t_User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(
                user.LockoutEndDate.HasValue ?
                    new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDate.Value.UtcDateTime, DateTimeKind.Utc)) :
                    new DateTimeOffset());
        }

        public Task<int> IncrementAccessFailedCountAsync(t_User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.AccessFailedCount++;
            //SaveUser(user);
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(t_User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.AccessFailedCount = 0;
            return Task.FromResult(0);
        }

        public Task SetLockoutEnabledAsync(t_User user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.LockoutEnabled = enabled;
            //SaveUser(user);
            return Task.FromResult(0);
        }

        public Task SetLockoutEndDateAsync(t_User user, DateTimeOffset lockoutEnd)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.LockoutEndDate = lockoutEnd == DateTimeOffset.MinValue ? null : new DateTime?(lockoutEnd.UtcDateTime);
            //SaveUser(user);
            return Task.FromResult(0);
        }

        
        // ///////////////////////// ICheckPasswordStore

       

        public Task<IdentityResult> SetPasswordEnabledAsync(t_User user, bool enabled)
        {
            if (user != null)
            {
                user.SetPasswordEnabled = enabled;
                return Task.FromResult(IdentityResult.Success);
            }
            
            return Task.FromResult(IdentityResult.Failed());
        }

        public Task<IdentityResult> SetChangePasswordEndDateAsync(t_User user)
        {
            if (user != null)
            {
                user.SetPasswordEndDate = DateTimeOffset.UtcNow;
                return Task.FromResult(IdentityResult.Success);
            }
            
            return Task.FromResult(IdentityResult.Failed());
        }

        
        public Task<bool> GetSetPasswordEnabledAsync(t_User user)
        {
            //throw new NotImplementedException();
            return Task.FromResult(user.SetPasswordEnabled);
        }

        public Task<int> GetNumberDaysAfterLastChangePasswordAsync(t_User user)
        {
            DateTimeOffset dateTimeOffset;
            if (user.SetPasswordEndDate == null)
            {
                dateTimeOffset=DateTimeOffset.UtcNow;
            }
            else
            {
                dateTimeOffset =(DateTimeOffset) user.SetPasswordEndDate;
            }
            return Task.FromResult((DateTimeOffset.UtcNow - dateTimeOffset).Days);
        }



        // /////////////////// ICheckAccountStore

        public Task<int> GetNumberDaysAfterLastSigninAsync(t_User user)
        {
            DateTimeOffset dateTimeOffset;
            if (user.SigninEndDate == null)
            {
                dateTimeOffset = DateTimeOffset.UtcNow;
            }
            else
            {
                dateTimeOffset = (DateTimeOffset)user.SigninEndDate;
            }
            return Task.FromResult((DateTimeOffset.UtcNow - dateTimeOffset).Days);
        }

        public Task SetSigninEndDateAsync(t_User user)
        {
            if (user != null)
            {
                user.SigninEndDate=DateTimeOffset.UtcNow;
                return Task.FromResult(0);
            }
            return Task.FromResult(1);
        }

       


        public void Dispose()
        {
            _db.Dispose();
        }

        private Task SaveChanges()
        {
            return AutoSaveChanges ? _db.SaveChangesAsync() : Task.FromResult(0);
        }


        ///////////////// My routins \\\\\\\\\\\\\\

        private t_User ToUser(MyUser user)
        {
            return new t_User
            {
                
                PasswordHash = user.PasswordHash,
                
                UserName = user.UserName
                //SecurityStamp = Guid.Parse(user.SecurityStamp)
            };
        }

        private MyUser ToMyUser(t_User userDb)
        {
#if DEBUG
            Debug.WriteLine("* ToMyUser *");
            Debug.WriteLine(String.Format("userDB.Id={0}, userDB.Name={1}",userDb.UserID.ToString(),userDb.UserName) );
#endif
            return new MyUser
            {
                UserID = userDb.UserID,
                //Email = userDb.Email,
                UserName = userDb.UserName,
                PasswordHash = userDb.PasswordHash
                //PasswordResetToken = userDb.PasswordResetToken,
                //SecurityStamp = userDb.SecurityStamp.ToString()
            };
        }

        private void SaveUser(t_User userDb)
        {

            t_User user = _db.User.Find(userDb.UserID);
            if (user == null)
            {
                try
                {

                    _db.User.Add(userDb);
                    _db.SaveChanges();

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
            {
                _db.Entry(userDb).State = EntityState.Modified;
                _db.SaveChanges();
            }
            
        }

        private MyUser FindUser(string key, FindKeyType keyType)
        {
            t_User userDb = null;
            MyUser user = null;
            
                switch (keyType)
                {
                    case FindKeyType.Id:
                        int id = 0;
                        if (!int.TryParse(key, out id))
                        {
                            throw new Exception();
                        }

                        userDb = _db.User.Find(id);
                        break;
                    case FindKeyType.Name:
                        userDb = _db.User.First(u => u.UserName == key);
                        break;
                    /*case FindKeyType.Email:
                        userDb = _db.t_User.First(u => u.Email == key);
                        break;*/
                    default:
                        throw new NotImplementedException();
                }
                if (userDb != null)
                {
                    user = ToMyUser(userDb);
#if DEBUG
                Debug.WriteLine("user.UserID="+user.UserID.ToString());
#endif
            }
            

            return user;
        }

        /*
        Task IUserStore<MyUser, string>.CreateAsync(MyUser user)
        {
            throw new NotImplementedException();
        }

        Task IUserStore<MyUser, string>.UpdateAsync(MyUser user)
        {
            throw new NotImplementedException();
        }

        Task IUserStore<MyUser, string>.DeleteAsync(MyUser user)
        {
            throw new NotImplementedException();
        }

        Task<MyUser> IUserStore<MyUser, string>.FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        Task<MyUser> IUserStore<MyUser, string>.FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
        */
        public Task SetTwoFactorEnabledAsync(t_User user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetTwoFactorEnabledAsync(t_User user)
        {
            //throw new NotImplementedException();
            return Task.FromResult(false);
        }


        /////////////////////// IUserUsedPassword \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        /// <summary>
        /// It is used password ?
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="hashPassword">Hashed password</param>
        /// <param name="maxCountUsedPassword">Amount used paswords for counting</param>
        /// <returns>true is usedpassword</returns>
        public Task<bool> IsUsedPassword(int userId, string hashPassword, int maxCountUsedPassword)
        {
            /*return  Task.FromResult( _db.User.Include(i=>i.t_UsedPassword.OrderByDescending(o=>o.UserId).Take(maxCountUsedPassword)
            .Where(w=>w.PasswordHash==hashPassword)).Where(w=>w.UserID==userId).Count()>0 ?
             true : false);*/
#if DEBUG
            Debug.WriteLine("*** UserStore.IsUsedPassword hashPassword="+ hashPassword);
#endif
            return Task.FromResult( _db.UsedPassword.Where(w => w.UserId == userId)
                .OrderByDescending(o => o.UsedPasswordId)
                .Take(maxCountUsedPassword)
                .Where(w => w.PasswordHash == hashPassword).Count() > 0
                ? true
                : false);
           

        }

        public Task SetUsedPassword(int userId, string hashPassword)
        {
            t_UsedPassword usedPassword=new t_UsedPassword();
            usedPassword.PasswordHash = hashPassword;
            usedPassword.UserId = userId;
            _db.UsedPassword.Add(usedPassword);
            _db.SaveChanges();
            
            return Task.FromResult(0);
        }

        public Task<IEnumerable<string>> GetUsedPassword(int userId, int maxCountUsedPassword)
        {
            return Task.FromResult(_db.UsedPassword.Where(w => w.UserId == userId)
               .OrderByDescending(o => o.UsedPasswordId)
               .Take(maxCountUsedPassword).Select(s=>s.PasswordHash).AsEnumerable()
               
               );
        }
    }

    internal enum FindKeyType
    {
        Id,
        Name,
        Email
    }
}