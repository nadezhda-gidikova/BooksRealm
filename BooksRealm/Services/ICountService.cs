
using BooksRealm.Services.Mapping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksRealm.Services
{
    public interface ICountService
    {
        CountDto GetCounts();
    }
}
