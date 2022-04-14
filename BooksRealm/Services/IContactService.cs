namespace BooksRealm.Services
{
    using BooksRealm.Models.ContactForm;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface  IContactService
    {
        public Task SendContactToAdmin(ContactFormViewModel contactFormViewModel);
    }
}
