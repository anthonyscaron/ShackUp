using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShackUp.UI.Models
{
    public class UpdateAccountViewModel
    {
        public string StateId { get; set; }
        public string EmailAddress { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
    }
}