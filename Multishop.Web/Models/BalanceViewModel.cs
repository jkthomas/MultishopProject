using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Multishop.Entities.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multishop.Web.Models
{
    public class BalanceViewModel
    {
        public BalanceViewModel()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            if (user == null)
                Balance = 0;
            else
                Balance = user.Balance;

        }
        public decimal Balance { get; set; }
    }
}