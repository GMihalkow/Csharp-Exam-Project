﻿using Forum.Web.Areas.AccountPanel.Services.Settings.Contracts;
using Forum.Web.Areas.Profile.Services.Account.Contracts;
using Forum.Web.Attributes.CustomValidationAttributes;
using Forum.Web.Services.Account.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Forum.Web.Areas.AccountPanel.Controllers.Profile
{
    [Area("AccountPanel")]
    public class ProfileController : Controller
    {
        private readonly IAccountService accountService;
        private readonly ISettingsService settingsService;
        private readonly IProfileService profileService;

        public ProfileController(IAccountService accountService, ISettingsService settingsService, IProfileService profileService)
        {
            this.accountService = accountService;
            this.settingsService = settingsService;
            this.profileService = profileService;
        }

        public IActionResult Details(string id)
        {
            var user = this.accountService.GetUserById(id);

            this.ViewData["profilePicUrl"] = user.ProfilePicutre;

            this.ViewData["userId"] = user.Id;

            this.ViewData["username"] = user.UserName;

            var model = this.profileService.GetProfileInfo(this.User);

            return this.View(model);
        }

        public PartialViewResult MyProfile()
        {
            var model = this.profileService.GetProfileInfo(this.User);

            return this.PartialView("_MyProfilePartial", model);
        }

        [HttpPost("UploadProfilePicture")]
        public IActionResult UploadProfilePicture([AllowedImageExtensions] IFormFile image)
        {
            if (image == null)
            {
                return this.BadRequest();
            }
            if (ModelState.IsValid)
            {
                this.profileService.UploadProfilePicture(image, this.User.Identity.Name);
                return this.Redirect("/Account/Profile");
            }
            else
            {
                var result = this.View("Error", this.ModelState);
                result.StatusCode = (int)HttpStatusCode.BadRequest;

                return result;
            }
        }
    }
}