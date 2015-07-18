using ContactBookAPIWebClient.Filters;
using ContactBookAPIWebClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using ContactBookAPIWebClient.DataAccess;
using System.Web.Security;

namespace ContactBookAPIWebClient.Views
{
    //[InitializeSimpleMembership]
    public class AccountController : Controller
    {
        private UsersContext _context;
        public AccountController()
        {
            _context = new UsersContext();
        }
        public ActionResult Login()
        {
            LoginViewModel viewModel = new LoginViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            bool modelValid = true;
            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                ModelState.AddModelError("UserName", "Please provide user name");
                modelValid = false;
            }

            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError("Password", "Please provide password");
                modelValid = false;
            }

            if (!modelValid)
                return View(model);

            try
            {
                bool authenticated = WebSecurity.Login(model.UserName, model.Password);
                if (authenticated)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Notification"] = new Notification("Incorrect username/password pair.", Nature.warning);
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Notification = new Notification("Error occured when processing your request", Nature.danger);
                return View("Error");
            }
        }

        public ActionResult Logout()
        {
            WebSecurity.Logout();
            return RedirectToAction("Index", "Contact");
        }

        public ActionResult SignUp()
        {
            SignUpViewModel viewModel = new SignUpViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SignUp(SignUpViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords don't match");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    UserEndpoint ue = new UserEndpoint();
                    ue.Register(model.UserName);
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new { PrivateKey = string.Empty, PublicKey = string.Empty });
                    TempData["Notification"] = new Notification("Please check your e-mail, we sent you access keys.", Nature.success);

                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    return View("Error");
                }
            }
            else
            {
                return View(model);
            }
        }

        [Authorize]
        public ActionResult Settings()
        {
            int id = WebSecurity.GetUserId(WebSecurity.CurrentUserName);
            UserProfile profile = _context.UserProfiles.First(x => x.UserId == id);

            SettingsViewModel viewModel = new SettingsViewModel();
            viewModel.PrivateKey = profile.PrivateKey;
            viewModel.PublicKey = profile.PublicKey;

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Settings(SettingsViewModel model)
        {
            try
            {

                if (!string.IsNullOrWhiteSpace(model.OldPassword) &&
                    !string.IsNullOrWhiteSpace(model.NewPassword) &&
                    !string.IsNullOrWhiteSpace(model.ConfirmNewPassword))
                {


                    if (model.NewPassword == model.ConfirmNewPassword)
                    {
                        if (!WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                        {
                            ModelState.AddModelError("OldPassword", "Incorrect password");
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("NewPassword", "");
                        ModelState.AddModelError("ConfirmNewPassword", "Passwords don't match");
                        return View(model);
                    }

                }
                int id = WebSecurity.GetUserId(WebSecurity.CurrentUserName);
                UserProfile profile = _context.UserProfiles.First(x => x.UserId == id);
                profile.PublicKey = model.PublicKey;
                profile.PrivateKey = model.PrivateKey;
                _context.SaveChanges();

            }
            catch (Exception ex)
            {

                return View("Error");
            }

            TempData["Notification"] = new Notification("Changes have been saved successfuly.", Nature.success);
            return View(model);

        }
    }
}