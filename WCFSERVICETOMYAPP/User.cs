using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WCFSERVICETOMYAPP
{
    public class User
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public string TaxId { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
    }
    public enum UserOperationResult
    {
        Good,
        NameExists,
        EmailExists,
        TaxIdExists,
        PhoneNumberExists
    }
}