// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlantopiaForum.Data;
using PlantopiaForum.Models;

namespace PlantopiaForum.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /////////////////////////////////////////
            /// BEGIN: ApplicationUser custom fields
            /////////////////////////////////////////            

            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Display(Name = "Location")]
            public string Location { get; set; }

            [Display(Name = "Photo Name")]
            public string ImageFilename { get; set; }

            // This is for uploading a photo, not for storing in the database.
            [NotMapped]  // This prevents it from being mapped to the database
            [Display(Name = "Photo")]
            public IFormFile? ImageFile { get; set; }

            /////////////////////////////////////////
            /// END: ApplicationUser custom fields
            /////////////////////////////////////////
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel

            {
                /////////////////////////////////////////
                /// BEGIN: ApplicationUser custom fields
                /////////////////////////////////////////  

                Name = user.Name,
                Location = user.Location,
                ImageFilename = user.ImageFilename,
                PhoneNumber = phoneNumber
                /////////////////////////////////////////
                /// END: ApplicationUser custom fields
                /////////////////////////////////////////
            };

            
        }
        
       
        

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }



        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            /////////////////////////////////////////
            /// BEGIN: ApplicationUser custom fields
            /////////////////////////////////////////

            if (Input.ImageFile != null)
            {
                // Generate a unique filename for the new image
                var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(Input.ImageFile.FileName);

                // Save the uploaded image to the server
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", newFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.ImageFile.CopyToAsync(fileStream);
                }

                // Update the user's ImageFilename in the database
                user.ImageFilename = newFileName;
            }
            else
            {
                // If no new image is uploaded, retain the old image filename
                user.ImageFilename = user.ImageFilename;  // Ensure that ImageFilename is not null
            }

            if (Input.Name != user.Name)
            {
                user.Name = Input.Name;
            }

            // Update Location
            if (Input.Location != user.Location)
            {
                user.Location = Input.Location;
            }


            await _userManager.UpdateAsync(user);

            /////////////////////////////////////////
            /// END: ApplicationUser custom fields
            /////////////////////////////////////////

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
