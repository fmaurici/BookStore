using CRUD3.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Entities.Enums;

namespace CRUD3.Models
{
    public class BookOperationViewModel : BaseViewModel
    {
        public UserViewModel User { get; set; }
        public BookViewModel Book { get; set; }
        public BookOperations Type { get; set; }
        public DateTime Date { get; set; }
    }
}
