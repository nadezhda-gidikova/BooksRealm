using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksRealm.Services.Mapping.Models
{
    public class PersonDto:IMapFrom<Person>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
        public string Profesion { get; set; }
        public DateTime RegistrationDate { get; set; }

    }
}
