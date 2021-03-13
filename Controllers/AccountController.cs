using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TExp.App_Start;
using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
//using testTS.Models;

using TExp.Models;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using TExp.Classes;

namespace TExp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private MyUserManager _userManager;
        private MySignInManager _signInManager;
        private TExpEntities _db;


        public AccountController()
       {
            //_db=new CardPayEntities();
            //UserStore userStore=new UserStore(_db);
            _userManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<MyUserManager>();
            //_signInManager = HttpContext.GetOwinContext().Get<MySignInManager>();
            _signInManager = System.Web.HttpContext.Current.GetOwinContext().Get<MySignInManager>();
#if DEBUG
            if (_userManager != null)
            {
                Debug.WriteLine("_userManager is not null!");
            }
            else
            {
                Debug.WriteLine("_userManager is null!");
            }
#endif
        }


        //public AccountController(IUserStore<t_User,int> userStore)
        public AccountController(MyUserManager userManager, MySignInManager signInManager)
        {
#if DEBUG
            if (userManager != null)
            {
                Debug.WriteLine("userManager is not null!");
            }
            else
            {
                Debug.WriteLine("userManager is null!");
            }
#endif

            UserManager = userManager;
            //_userManager=new MyUserManager(userStore);
            SignInManager = signInManager;
        }


        public MySignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<MySignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }


        public MyUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<MyUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        } 

        /*
        public ActionResult Index()
        {
            return View();
        } */


        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
#if DEBUG
            Debug.WriteLine("HttpGet Login returnUrl="+ returnUrl);
#endif
            if (!String.IsNullOrWhiteSpace(returnUrl))
            {
                ViewBag.ReturnUrl = returnUrl;
            }
            
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel loginView, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!ModelState.IsValid)
            {
#if DEBUG
                Debug.WriteLine("Model is NOT valid!");
#endif
                return View();
            }
#if DEBUG
            Debug.WriteLine("Mode is valid");
#endif

            //_userManager.

            //var result =await SignInManager.PasswordSignInAsync(loginView.UserName, loginView.Password, false,shouldLockout: false);
            var result =
                await
                    _signInManager.PasswordSignInAsync(loginView.UserName, loginView.Password, false,
                        shouldLockout: true);
#if DEBUG
            Debug.WriteLine("PasswordSignInAsync result="+result.ToString());
            Debug.WriteLine("PasswordSignInAsync returnUrl=" + returnUrl);
#endif
            
            
            switch (result)
            {
                    case SignInCode.Success:
#if DEBUG
                    Debug.WriteLine("++ ready to RedirectToLocal");
#endif
                    return RedirectToLocal(returnUrl);
                    case SignInCode.LockedOut:
#if DEBUG
                    Debug.WriteLine("Redirect to Lockout");
#endif
                    return View("Lockout");
                    case SignInCode.ChangePassword:
                    return View("ChangePassword");
                default:
                    ModelState.AddModelError("","Пароль или логин неверны");
//                    ViewBag.ReturnUrl = returnUrl;
                    return View(loginView);
            } 
            /*var user = await _userManager.FindByNameAsync(loginView.UserName);
            if (user != null)
            {
                Debug.WriteLine("user is NOT null");

                if (await _userManager.CheckPasswordAsync(user, loginView.Password))
                {
                    Debug.WriteLine("redirect!!!");
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                    ClaimsIdentity identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    authenticationManager.SignIn(identity);
                    return RedirectToAction("Index", "Home");
                }

            }
#if DEBUG
            else
            {
                Debug.WriteLine("user is NULL!");
            }
#endif*/
            //return View();

        }
        /*
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            int userId = _userManager.GetIdByName(HttpContext.User.Identity.Name.ToString());
#if DEBUG
            Debug.WriteLine("RsetPassword UserName=" + HttpContext.User.Identity.Name.ToString());
#endif 
            ResetPasswordInputModel resetPasswordInput = _userManager.GetUserForResetPassword(userId);
            return View(resetPasswordInput);

        }
        */


        [AllowAnonymous]
        public  ActionResult ResetPassword(int Id)
        {
            if (Id == 0 || Id==null)
            {
                Id= _userManager.GetIdByName(HttpContext.User.Identity.Name.ToString());
            }

            ResetPasswordInputModel resetPasswordInput = _userManager.GetUserForResetPassword(Id);
                return View(resetPasswordInput);
            
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPasswordInputModel model)
        {
            //model.OldPassword = "";
#if DEBUG
            Debug.WriteLine("UserName="+model.UserName);
            Debug.WriteLine("Password="+model.Password);
            Debug.WriteLine("ConfirmPassword=" + model.ConfirmPassword);
            //Debug.WriteLine("OldPassword="+model.OldPassword);
            Debug.WriteLine("Id="+model.Id.ToString());
#endif
            if (ModelState.IsValid)
            {
#if DEBUG
                Debug.WriteLine("Model is valid");
#endif
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
#if DEBUG
                    Debug.WriteLine("user is not null");
#endif  
                    /*
                    var checkOldPassword =await _userManager.CheckPasswordAsync(user, model.OldPassword);

                    if (checkOldPassword)
                    { */
                        var result = await _userManager.RemovePasswordAsync(user.UserID);
                        if (result.Succeeded)
                        {
#if DEBUG
                            Debug.WriteLine("Password is removed to succeeded");
#endif
                            var result2 = await _userManager.AddPasswordAsync(user.UserID, model.Password);
                            if (result2.Succeeded)
                            {
#if DEBUG
                                Debug.WriteLine("Password is added to succeeded");
#endif
                                var result3 = await _userManager.SetSigninEndDateAsync(user);
#if DEBUG
                                if (result3)
                                {
                                    Debug.WriteLine("SetSigninEndDateAsync is  well done");
                                }


#endif
                                var result4 = await _userManager.ResetAccessFailedCountAsync(user.UserID);
#if DEBUG
                                if (result4.Succeeded)
                                {
                                    Debug.WriteLine("ResetAccessFailedCountAsync is executed.");
                                }
#endif
                                /*
                                result4 = await _userManager.SetChangePasswordEndDateAsync(user);
#if DEBUG
                                if (result4.Succeeded)
                                {
                                    Debug.WriteLine("SetChangePasswordEndDateAsync is executed.");
                                }
#endif

                                result4 = await _userManager.SetPasswordEnabledAsync(user, false);
#if DEBUG
                                if (result4.Succeeded)
                                {
                                    Debug.WriteLine("SetPasswordEnabledAsync is executed.");
                                }
#endif
                                */
                                return RedirectToAction("List");
                            }
                        }

              //      }
              /*      else
                    {
                        // Активизирована ли функция  блокировки учётки в системе
                        if (_userManager.LockoutEnabled)
                        {

#if DEBUG
                            Debug.WriteLine("++++ GetLockoutEnabledAsync is enabled");
#endif
                            // Увелечение счётчика неудачных попыток ввода пароля
                            if (await _userManager.AccessFailedAsync(user.UserID) == IdentityResult.Success)
                            {
#if DEBUG
                                Debug.WriteLine("~~~ AccessFailedAsync is working!");
#endif
                                if (await _userManager.IsLockedOutAsync(user.UserID))
                                {
#if DEBUG
                                    Debug.WriteLine("Account is locked out!");
#endif
                                    //Заблокировать учётку
                                    return View("Lockout");
                                }


                            }


                        }

                    } */

                   
                }
            }
#if DEBUG
            Debug.WriteLine("Error is occured in ResetPassword");
#endif
            return View(model);
        }
       

        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordInputModel model)
        {
            if (ModelState.IsValid)
            {
#if DEBUG
                Debug.WriteLine("Model is valid");
#endif
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
#if DEBUG
                    Debug.WriteLine("user is not null");
#endif
                    if (!(await _userManager.IsUsedPasswordAsync(user.UserID, model.Password)))
                    {
#if DEBUG
                        Debug.WriteLine("Password is NOT used.");
#endif

                        var result2 =
                            await _userManager.ChangePasswordAsync(user.UserID, model.OldPassword, model.Password);
                        if (result2.Succeeded)
                        {
#if DEBUG
                            Debug.WriteLine("Password is changed to succeeded");

#endif
                            var resultSetUsedPassword =
                                await
                                    _userManager.SetUsedPasswordAsync(user.UserID, model.Password).ConfigureAwait(true);

#if DEBUG
                            Debug.WriteLine("Used password is saved!");
#endif

                            var result3 = await _userManager.SetSigninEndDateAsync(user); //.ConfigureAwait(false);
#if DEBUG
                            if (result3)
                            {
                                Debug.WriteLine("SetSigninEndDateAsync is  well done");
                            }


#endif
                            var result4 = await _userManager.ResetAccessFailedCountAsync(user.UserID);
#if DEBUG
                            if (result4.Succeeded)
                            {
                                Debug.WriteLine("ResetAccessFailedCountAsync is executed.");
                            }
#endif
                            result4 = await _userManager.SetChangePasswordEndDateAsync(user);
#if DEBUG
                            if (result4.Succeeded)
                            {
                                Debug.WriteLine("SetChangePasswordEndDateAsync is executed.");
                            }
#endif

                            result4 = await _userManager.SetPasswordEnabledAsync(user, false);
#if DEBUG
                            if (result4.Succeeded)
                            {
                                Debug.WriteLine("SetPasswordEnabledAsync is executed.");
                            }
#endif

                            return RedirectToAction("ResetPasswordConfirmation");
                        }
                        else
                        {
                            // Активизирована ли функция  блокировки учётки в системе
                            if (_userManager.LockoutEnabled)
                            {

#if DEBUG
                                Debug.WriteLine("++++ GetLockoutEnabledAsync is enabled");
#endif
                                // Увелечение счётчика неудачных попыток ввода пароля
                                if (await _userManager.AccessFailedAsync(user.UserID) == IdentityResult.Success)
                                {
#if DEBUG
                                    Debug.WriteLine("~~~ AccessFailedAsync is working!");
#endif
                                    if (await _userManager.IsLockedOutAsync(user.UserID))
                                    {
#if DEBUG
                                        Debug.WriteLine("Account is locked out!");
#endif
                                        //Заблокировать учётку
                                        return View("Lockout");
                                    }


                                }


                            }
                            ModelState.AddModelError("","Ввод старого пароля не верен!");

                        }
                    }
                    else
                    {
#if DEBUG
                        Debug.WriteLine("Password is used!");

#endif
                        ModelState.AddModelError("", "Пароль уже использовался");
                        return View();
                    }
                }
            }
          
#if DEBUG
            Debug.WriteLine("Error is occured in ChangePassword");
#endif
            return View(model);
        }


        [Authorize]
        public ActionResult Register()
        {
            RegisterInputModel model = new RegisterInputModel();
            model.userRoles = _userManager.GetListUserRoles();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Register(RegisterInputModel model, string SelectedRole)
        {
            if (ModelState.IsValid)
            {
#if DEBUG
                Debug.WriteLine("Model is valid");
#endif
                var user = new t_User();
                user.UserName = model.UserName;
                user.FIO = model.FIO;
                user.Position = model.Position;
                user.SetPasswordEnabled = model.SetPasswordEnabled;
                
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
#if DEBUG
                    Debug.WriteLine("resulult is secceeded");
#endif
                    if (SelectedRole != null)
                    {
                        var result2 = await _userManager.AddToRoleAsync(user.Id, SelectedRole);
                        if (result2.Succeeded)
                        {
#if DEBUG
                            Debug.WriteLine("Roles is added");
#endif
                            return RedirectToAction("List");
                        }
                    }
                }/*
                var result = await _userManager.RegisterUserAsync(model, SelectedRole);
                if (result.Succeeded)
                {
#if DEBUG
                    Debug.WriteLine("User is added!");
#endif
                    return RedirectToAction("List");
                }*/
            }
            model.userRoles = _userManager.GetListUserRoles();
            return View(model);
        }



        [Authorize]
        public async Task<ActionResult> Edit(int Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user != null)
            {

                AccountViewModel model = new AccountViewModel();
                model.UserName = user.UserName;
                model.FIO = user.FIO;
                model.Position = user.Position;
                model.SetPasswordEnabled = user.SetPasswordEnabled;
                model.LockoutEnabled = user.LockoutEnabled;

                //await userManager.UpdateAsync(user);
                IEnumerable<string> Roles = await _userManager.GetRolesAsync(Id);
                if (Roles != null)
                {
                    model.UserRole = Roles.FirstOrDefault();
                }
                else
                {
                    model.UserRole = "";
                }
                return View(model);
            }
            return RedirectToAction("List");
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Edit(AccountViewModel model, string SelectedRole)
        {
            if (ModelState.IsValid)
            {
#if DEBUG
                Debug.WriteLine("Model is valid");
#endif
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
#if DEBUG
                    Debug.WriteLine("User is not null");
#endif

                    user.FIO = model.FIO;
                    user.Position = model.Position;
                    user.SetPasswordEnabled = model.SetPasswordEnabled;
                    user.LockoutEnabled = model.LockoutEnabled;
                    await _userManager.UpdateAsync(user);
                    
                    var roles = await _userManager.GetRolesAsync(model.Id);
                    await _userManager.RemoveFromRolesAsync(model.Id, roles.ToArray<string>());
                    var result = await _userManager.AddToRoleAsync(model.Id, SelectedRole);
                    if (result.Succeeded)
                    {
#if DEBUG
                        Debug.WriteLine("Role is added");
#endif
                        return RedirectToAction("List");
                    }
                }
            }
            return View(model);
        }


        [Authorize]
        public ActionResult LogOut()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            ViewBag.ReturnUrl="/";
            return View("Login");
            //return RedirectToAction("Login", "Account",routeValues:"/");
        }



        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        public ActionResult List()
        {

            IEnumerable<AccountViewModel> accountViewModel = _userManager.GetListUsers();   //_db.t_User.Select(s => new AccountViewModel() { Id = s.UserID, UserName = s.UserName, FIO = s.FIO });
            return View(accountViewModel);
        }





        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (!Url.IsLocalUrl(returnUrl))
            {
#if DEBUG
                Debug.WriteLine("** RedirectToLocal IsLocalUrl");
                Debug.WriteLine("returnUrl="+ returnUrl);
#endif
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

    }
}