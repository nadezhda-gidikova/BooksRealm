using BooksRealm.Data.Common.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace BooksRealm.Data.Models
{
    public class BooksRealmRole : IdentityRole, IAuditInfo, IDeletableEntity
    {
        public BooksRealmRole()
            : this(null)
        {
        }

        public BooksRealmRole(string name)
            : base(name)
        {

        }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
