using ContactBookAPIWebClient.DataAccess;
using ContactBookAPIWebClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ContactBookAPIWebClient.Controllers
{
    public class GroupController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.SiteRoot = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            base.OnActionExecuted(filterContext);
        }

        private UsersContext _userContext;
        public GroupController()
        {
            _userContext = new UsersContext();
        }

        // GET: Group
        [Authorize]
        public ActionResult Index(string searchQuery, string searchScope, int? pageNumber, int pageSize = 12)
        {
            int id = WebSecurity.GetUserId(WebSecurity.CurrentUserName);
            var userProfile = _userContext.UserProfiles.First(x => x.UserId == id);
            searchScope = "all";
            if (string.IsNullOrWhiteSpace(userProfile.PrivateKey) || string.IsNullOrWhiteSpace(userProfile.PublicKey))
            {
                TempData["Notification"] = new Notification("Please provide access keys that have been sent you by email", Nature.warning);
                return RedirectToAction("Settings", "Account");
            }

            pageNumber = pageNumber ?? 1;

            GroupEndpoint g = new GroupEndpoint();
            UserData userData = new UserData();
            userData.PublicKey = userProfile.PublicKey;
            userData.Timestamp = DateTime.Now;

            List<Group> result;
            if (string.IsNullOrWhiteSpace(searchQuery) || searchScope == null)
            {
                userData.GenerateAuthenticationHash(userProfile.PrivateKey + userProfile.PublicKey + "GET/contact/" + pageNumber.Value + "/" + pageSize+"/true"+ userData.Timestamp + userProfile.PrivateKey);
                result = g.GetGroups(pageNumber.Value, pageSize, userData);

            }
            else
            {
                userData.GenerateAuthenticationHash(userProfile.PrivateKey + userProfile.PublicKey + "GET/contact/" + searchScope + "/" + searchQuery + "/" + pageNumber.Value + "/" + pageSize+"/true" + userData.Timestamp + userProfile.PrivateKey);
                result =g.GetFilteredGroups(searchScope, searchQuery, pageNumber.Value, pageSize, userData);
            }

            ViewBag.SearchQuery = searchQuery;
            return View(result);
        }

        [Authorize]
        public ActionResult Create()
        {
            Group g = new Group();
            return View(g);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Group model)
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
                userData.GenerateAuthenticationHash(userProfile.PrivateKey + userProfile.PublicKey + "POST/contact" + userData.Timestamp + userProfile.PrivateKey);

                GroupEndpoint g = new GroupEndpoint();
                model.isContactGroup = true;
                string message = g.CreateGroup(model, userData);

                TempData["Notification"] = new Notification("Group has been added" + message, Nature.success);
                Thread.Sleep(2500);
                return RedirectToAction("Index");

            }
            else
            {
                return View(model);
            }
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

            TempData["Notification"] = new Notification("Group has been removed" + message, Nature.success);

            return RedirectToAction("Index");

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

            GroupEndpoint g = new GroupEndpoint();


            var group = g.GetGroup(id, userData);

            return View(group);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(GroupViewModel model)
        {
            int userId = WebSecurity.GetUserId(WebSecurity.CurrentUserName);
            var userProfile = _userContext.UserProfiles.First(x => x.UserId == userId);
            model.group.isContactGroup = false;

            UserData userData = new UserData();
            userData.PublicKey = userProfile.PublicKey;
            userData.Timestamp = DateTime.Now;
            userData.GenerateAuthenticationHash(userProfile.PrivateKey + userProfile.PublicKey + "POST/contact/" + model.group._id + userData.Timestamp + userProfile.PrivateKey);

            GroupEndpoint g = new GroupEndpoint();
            string message = g.UpdateGroup(model.group, userData);

            TempData["Notification"] = new Notification("Group details has been saved" + message, Nature.success);
            Thread.Sleep(2500);

            return RedirectToAction("Index");
        }
    }
}