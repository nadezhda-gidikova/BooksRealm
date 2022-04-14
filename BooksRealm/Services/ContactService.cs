using BooksRealm.Data.Common.Repositories;
using BooksRealm.Data.Models;
using BooksRealm.Messaging;
using BooksRealm.Models.ContactForm;
using System.Threading.Tasks;


namespace BooksRealm.Services
{
   
    public class ContactService:IContactService
    {
        private readonly IRepository<ContactFormEntry> contactsRepository;
        private readonly IEmailSender emailSender;

        public ContactService(IRepository<ContactFormEntry> contactsRepository,
            IEmailSender emailSender)
        {
            this.contactsRepository = contactsRepository;
            this.emailSender = emailSender;
        }
        public async Task SendContactToAdmin(ContactFormViewModel contactFormViewModel)
        {
            
            var contactFormEntry = new ContactFormEntry
            {
                Name=contactFormViewModel.Name,
                Email = contactFormViewModel.Email,
                Subject = contactFormViewModel.Subject,
                Content = contactFormViewModel.Content,
            };

            await this.contactsRepository.AddAsync(contactFormEntry);
            await this.contactsRepository.SaveChangesAsync();

            await this.emailSender.SendEmailAsync(
                contactFormViewModel.Email,
                string.Concat(contactFormViewModel.Name),
                WebConstants.SystemEmail,
                contactFormViewModel.Subject,
                contactFormViewModel.Content);
        }
    }
}
