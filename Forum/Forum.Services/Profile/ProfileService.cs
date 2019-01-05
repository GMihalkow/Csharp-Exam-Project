using AutoMapper;
using CloudinaryDotNet;
using Forum.Services.Common;
using Forum.Services.Interfaces.Db;
using Forum.Services.Interfaces.Profile;
using Forum.ViewModels.Interfaces.Profile;
using Forum.ViewModels.Profile;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;

namespace Forum.Services.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly IMapper mapper;
        private readonly IDbService dbService;
        private readonly IOptions<CloudConfiguration> cloudConfig;

        public ProfileService(IMapper mapper, IDbService dbService, IOptions<CloudConfiguration> CloudConfig)
        {
            this.mapper = mapper;
            this.dbService = dbService;
            cloudConfig = CloudConfig;
        }

        public IProfileInfoViewModel GetProfileInfo(ClaimsPrincipal principal)
        {
            var user =
                this.dbService
                .DbContext
                .Users
                .Include(u => u.Posts)
                .Where(u => u.UserName == principal.Identity.Name)
                .FirstOrDefault();

            var model = this.mapper.Map<ProfileInfoViewModel>(user);

            return model;
        }
        
        public void UploadProfilePicture(IFormFile image, string username)
        {
            var user = this.dbService.DbContext.Users.FirstOrDefault(u => u.UserName == username);

            CloudinaryDotNet.Account cloudAccount = new CloudinaryDotNet.Account(this.cloudConfig.Value.CloudName, this.cloudConfig.Value.ApiKey, this.cloudConfig.Value.ApiSecret);

            Cloudinary cloudinary = new Cloudinary(cloudAccount);

            var stream = image.OpenReadStream();

            CloudinaryDotNet.Actions.ImageUploadParams uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams()
            {
                File = new FileDescription(image.FileName, stream),
                PublicId = $"{username}_profile_pic"
            };

            CloudinaryDotNet.Actions.ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);

            string url = cloudinary.Api.UrlImgUp.BuildUrl($"{username}_profile_pic");

            var updatedUrl = cloudinary.GetResource(uploadParams.PublicId).Url;

            user.ProfilePicutre = updatedUrl;

            this.dbService.DbContext.Entry(user).State = EntityState.Modified;
            this.dbService.DbContext.SaveChanges();
        }

        public bool IsImageExtensionValid(string fileName)
        {
            int counter = 0;

            foreach (var extension in ServicesConstants.AllowedImageExtensions)
            {
                if (fileName.EndsWith(extension))
                {
                    counter++;
                }
            }

            if (counter == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}