namespace BooksRealm.Models.ContactForm
{
    using BooksRealm.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

   
        public class ContactFormViewModel
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Please input your names")]
            [Display(Name = "Your names")]
            public string Name { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Please input your email")]
            [EmailAddress(ErrorMessage = "Please input valid email")]
            [Display(Name = "Your email address")]
            public string Email { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Please input message title")]
            [StringLength(100, ErrorMessage = "Title must be atleast {2} and not more than {1} symbol.", MinimumLength = 5)]
            [Display(Name = "Message title")]
            public string Title { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Please input message content")]
            [StringLength(10000, ErrorMessage = "Message must be atleast {2} symbols.", MinimumLength = 20)]
            [Display(Name = "Message content")]
            public string Content { get; set; }

            [GoogleReCaptchaValidation]
            public string RecaptchaValue { get; set; }
        }
    }

