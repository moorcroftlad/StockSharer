using System;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using StockSharer.Web.Authentication;
using StockSharer.Web.Cache;
using StockSharer.Web.Data;
using StockSharer.Web.Email;
using StockSharer.Web.ViewModels;

namespace StockSharer.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository = new UserRepository();
        private readonly RedisCache _redisCache = new RedisCache();
        private readonly ISendEmail _emailSender = new SesEmailSender();
        private readonly AuthenticationHelper _authenticationHelper = new AuthenticationHelper();

        public ActionResult Login(string returnUrl = "")
        {
            var loginViewModel = new LoginViewModel
                {
                    ReturnUrl = returnUrl
                };
            return User.Identity.IsAuthenticated ? LogOut() : View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ValidateUser(loginViewModel.Email, loginViewModel.Password))
            {
                _authenticationHelper.SetFormsAuthenticationCookie(Response, loginViewModel.Email);
                if (!string.IsNullOrEmpty(loginViewModel.ReturnUrl) 
                    && Url.IsLocalUrl(loginViewModel.ReturnUrl) 
                    && loginViewModel.ReturnUrl.StartsWith("/")
                    && !loginViewModel.ReturnUrl.StartsWith("//") 
                    && !loginViewModel.ReturnUrl.StartsWith("/\\"))
                {
                    return Redirect(loginViewModel.ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            loginViewModel.LoginError = true;
            return View(loginViewModel);
        }

        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            if (registerViewModel.Password != registerViewModel.ConfirmPassword)
            {
                registerViewModel.Error = "Passwords must match";
                return View(registerViewModel);
            }
            var email = registerViewModel.Email.ToLower();
            var userId = _userRepository.CreateUser(email, registerViewModel.Forename, registerViewModel.Surname, PasswordHash.CreateHash(registerViewModel.Password));
            if (userId > 0)
            {
                SendActivationEmail(email, CreateTemporaryAuthGuid(userId));
                return RedirectToAction("RegistrationSuccessful", "User");
            }
            registerViewModel.Error = "An account with your email address already exists";
            return View(registerViewModel);
        }

        public ActionResult RegistrationSuccessful()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

        public ActionResult Activate(Guid id)
        {
            int userId;
            int.TryParse(_redisCache.Get(id.ToString()), out userId);
            if (userId > 0)
            {
                _userRepository.ActivateUser(userId);
                _authenticationHelper.SetFormsAuthenticationCookie(Response, userId);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ActivateAccount(string email)
        {
            var user = _userRepository.RetrieveUser(email);
            if (user != null && !user.Active)
            {
                var temporaryAuthGuid = CreateTemporaryAuthGuid(user.UserId);
                SendActivationEmail(user.Email, temporaryAuthGuid);
            }
            return RedirectToAction("RegistrationSuccessful");
        }

        public ActionResult ResetPassword()
        {
            var warningMessageViewModel = new WarningMessageViewModel
                {
                    Message = TempData["Message"] == null ? null : TempData["Message"].ToString()
                };
            return View(warningMessageViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string email)
        {
            var user = _userRepository.RetrieveUser(email);
            if (user != null && user.Active)
            {
                var temporaryAuthGuid = CreateTemporaryAuthGuid(user.UserId);
                SendPasswordResetEmail(user.Email, temporaryAuthGuid);
            }
            return RedirectToAction("ResetPasswordSuccess");
        }

        public ActionResult ChangePassword(Guid id)
        {
            int userId;
            int.TryParse(_redisCache.Get(id.ToString()), out userId);
            if (userId == 0)
            {
                TempData["Message"] = "Your password reset link has expired, please enter your email again for a new link";
                return RedirectToAction("ResetPassword");
            }

            var changePasswordViewModel = new ChangePasswordViewModel
                {
                    ChangePassword = new ChangePassword
                        {
                            UserId = userId
                        }
                };
            return View(changePasswordViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePassword changePassword)
        {
            if (changePassword.Password != changePassword.ConfirmPassword)
            {
                return View(new ChangePasswordViewModel
                    {
                        ChangePassword = changePassword,
                        Message = "Your passwords do not match"
                    });
            }
            _userRepository.UpdatePassword(changePassword.UserId, PasswordHash.CreateHash(changePassword.Password));
            _authenticationHelper.SetFormsAuthenticationCookie(Response, changePassword.UserId);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ResetPasswordSuccess()
        {
            return View();
        }

        private bool ValidateUser(string email, string password)
        {
            var passwordHash = _userRepository.RetrievePasswordHash(email);
            return passwordHash != null && PasswordHash.ValidatePassword(password, passwordHash);
        }

        private Guid CreateTemporaryAuthGuid(int userId)
        {
            var authGuid = Guid.NewGuid();
            _redisCache.Set(authGuid.ToString(), userId.ToString(CultureInfo.InvariantCulture), new TimeSpan(1, 0, 0, 0));
            return authGuid;
        }

        private void SendActivationEmail(string email, Guid temporaryAuthGuid)
        {
            var bodyText = string.Format("Thank you for registering an account with StockSharer.  To complete your registration please click on the link: http://www.stocksharer.com/user/activate/{0}", temporaryAuthGuid);
            var bodyHtml = string.Format("Thank you for registering an account with StockSharer.  To complete your registration please click on the link below:<br /><br /><a href=\"http://www.stocksharer.com/user/activate/{0}\">http://www.stocksharer.com/user/activate/{0}</a>", temporaryAuthGuid);
            _emailSender.SendEmail(email, "Welcome to StockSharer", bodyText, bodyHtml);
        }

        private void SendPasswordResetEmail(string email, Guid temporaryAuthGuid)
        {
            var bodyText = string.Format("We have received a request to reset your StockSharer password, if this was not from you please ignore this email.  To change your password please click on the link and follow the instructions: http://www.stocksharer.com/user/changepassword/{0}", temporaryAuthGuid);
            var bodyHtml = string.Format("We have received a request to reset your StockSharer password, if this was not from you please ignore this email.  To change your password please click on the link and follow the instructions:<br /><br /><a href=\"http://www.stocksharer.com/user/changepassword/{0}\">http://www.stocksharer.com/user/changepassword/{0}</a>", temporaryAuthGuid);
            _emailSender.SendEmail(email, "StockSharer password reset", bodyText, bodyHtml);
        }
    }

    public class WarningMessageViewModel
    {
        public string Message { get; set; }
    }

    public class ChangePasswordViewModel : WarningMessageViewModel
    {
        public ChangePassword ChangePassword { get; set; }
    }

    public class ChangePassword
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}