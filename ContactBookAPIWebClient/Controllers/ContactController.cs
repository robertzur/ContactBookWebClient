using ContactBookAPIWebClient.DataAccess;
using ContactBookAPIWebClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ContactBookAPIWebClient.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.SiteRoot = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            base.OnActionExecuted(filterContext);
        }

        // GET: Contact
        private UsersContext _userContext;
        public ContactController()
        {
            _userContext = new UsersContext();
        }

        [Authorize]
        public ActionResult Index(int? pageNumber, int pageSize = 12)
        {
            int id = WebSecurity.GetUserId(WebSecurity.CurrentUserName);
            var userProfile = _userContext.UserProfiles.First(x => x.UserId == id);

            if (string.IsNullOrWhiteSpace(userProfile.PrivateKey) || string.IsNullOrWhiteSpace(userProfile.PublicKey))
            {
                TempData["Notification"] = new Notification("Please provide access keys that have been sent you by email", Nature.warning);
                return RedirectToAction("Settings", "Account");
            }

            pageNumber = pageNumber ?? 1;

            ContactEndpoint c = new ContactEndpoint();
            UserData userData = new UserData();
            userData.PublicKey = userProfile.PublicKey;
            userData.Timestamp = DateTime.Now;
            userData.GenerateAuthenticationHash(userProfile.PrivateKey+userProfile.PublicKey+"GET/contact/"+pageNumber.Value+"/"+pageSize+userData.Timestamp+userProfile.PrivateKey);

            var result = c.GetContacts(pageNumber.Value, pageSize, userData);
            return View(result);
        }

        [Authorize]
        public ActionResult Create()
        {
            Contact c = new Contact();
            return View(c);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Contact model)
        {
            int id = WebSecurity.GetUserId(WebSecurity.CurrentUserName);
            var userProfile = _userContext.UserProfiles.First(x => x.UserId == id);

            if (string.IsNullOrWhiteSpace(userProfile.PrivateKey) || string.IsNullOrWhiteSpace(userProfile.PublicKey))
            {
                TempData["Notification"] = new Notification("Please provide access keys that have been sent you by email", Nature.warning);
                return RedirectToAction("Account", "Settings");
            }

            if (ModelState.IsValid)
            {
                UserData userData = new UserData();
                userData.PublicKey = userProfile.PublicKey;
                userData.Timestamp = DateTime.Now;
                userData.GenerateAuthenticationHash(userProfile.PrivateKey + userProfile.PublicKey + "POST/contact"+ userData.Timestamp + userProfile.PrivateKey);

                ContactEndpoint c = new ContactEndpoint();
                string message = c.CreateContact(model, userData);
                
                TempData["Notification"] = new Notification("Contact has been added" + message, Nature.success);
                return RedirectToAction("Index");

            } else
            {
                return View(model);
            }
        }

        [Authorize]
        public ActionResult Edit(string id)
        {
            int userId = WebSecurity.GetUserId(WebSecurity.CurrentUserName);
            var userProfile = _userContext.UserProfiles.First(x => x.UserId == userId);

            UserData userData = new UserData();
            userData.PublicKey = userProfile.PublicKey;
            userData.Timestamp = DateTime.Now;
            userData.GenerateAuthenticationHash(userProfile.PrivateKey + userProfile.PublicKey + "GET/contact/" + id + userData.Timestamp + userProfile.PrivateKey);

            ContactEndpoint c = new ContactEndpoint();
            var contact = c.GetContact(id, userData);

            return View(contact);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(Contact model)
        {
            int userId = WebSecurity.GetUserId(WebSecurity.CurrentUserName);
            var userProfile = _userContext.UserProfiles.First(x => x.UserId == userId);
            model.isContactGroup = false;

            UserData userData = new UserData();
            userData.PublicKey = userProfile.PublicKey;
            userData.Timestamp = DateTime.Now;
            userData.GenerateAuthenticationHash(userProfile.PrivateKey + userProfile.PublicKey + "POST/contact/" + model._id + userData.Timestamp + userProfile.PrivateKey);

            ContactEndpoint c = new ContactEndpoint();
            string message = c.UpdateContact(model, userData);

            TempData["Notification"] = new Notification("Contact has been added" + message, Nature.success);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            int userId = WebSecurity.GetUserId(WebSecurity.CurrentUserName);
            var userProfile = _userContext.UserProfiles.First(x => x.UserId == userId);

            UserData userData = new UserData();
            userData.PublicKey = userProfile.PublicKey;
            userData.Timestamp = DateTime.Now;
            userData.GenerateAuthenticationHash(userProfile.PrivateKey + userProfile.PublicKey + "DELETE/contact/" + id + userData.Timestamp + userProfile.PrivateKey);

            ContactEndpoint c = new ContactEndpoint();

            string message = c.DeleteContact(id, userData);

            TempData["Notification"] = new Notification("Contact has been removed" + message, Nature.success);

            return RedirectToAction("Index");

        }

        public JsonResult GetContactDetailsByAjax(string id)
        {
            int userId = WebSecurity.GetUserId(WebSecurity.CurrentUserName);
            var userProfile = _userContext.UserProfiles.First(x => x.UserId == userId);

            UserData userData = new UserData();
            userData.PublicKey = userProfile.PublicKey;
            userData.Timestamp = DateTime.Now;
            userData.GenerateAuthenticationHash(userProfile.PrivateKey + userProfile.PublicKey + "GET/contact/"+id+ userData.Timestamp + userProfile.PrivateKey);

            ContactEndpoint c = new ContactEndpoint();
            var contact = c.GetContact(id, userData);

            return Json(contact, JsonRequestBehavior.AllowGet);

        }

    }
}