namespace BooksRealm.Controllers
{
    using BooksRealm.Models.ContactForm;
    using BooksRealm.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class ContactsController : BaseController
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
