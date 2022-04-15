using BooksRealm.Data.Common.Repositories;
using BooksRealm.Data.Models;
using BooksRealm.Messaging;
using BooksRealm.Models.ContactForm;
using BooksRealm.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BooksRealm.Controllers
{
   
    public class ContactsController : Controller
    {
        private readonly IContactService contactsService;

        public ContactsController(IContactService contactsService)
        {
            this.contactsService = contactsService;
        }

        public IActionResult Index()
        {
            return this.View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Index(ContactFormViewModel contactFormViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(contactFormViewModel);
            }

            await this.contactsService.SendContactToAdmin(contactFormViewModel);

            return this.RedirectToAction("ThankYou");
        }

        public IActionResult ThankYou()
        {
            return this.View();
        }
    }
}
