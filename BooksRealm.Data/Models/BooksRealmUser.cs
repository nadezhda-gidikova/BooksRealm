namespace BooksRealm.Data.Models
{
    using BooksRealm.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;

    public class BooksRealmUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public BooksRealmUser()
        {
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.Votes = new HashSet<Vote>();
            this.Reviews = new HashSet<Review>();

        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

    }
}
