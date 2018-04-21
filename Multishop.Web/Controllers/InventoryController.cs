using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Multishop.Data.DAL.Services.Repository;
using Multishop.Entities.ShopEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multishop.Web.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        private IProductionRepository<StoredProduct> _storedProductRepository;
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Inventory
        public ActionResult Index()
        {
            _storedProductRepository = new StoredProductRepository(new Data.DAL.Context.ApplicationDbContext());
            var user = UserManager.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            IEnumerable<StoredProduct> storedProducts = _storedProductRepository.GetEntities().Where(p => p.UserId == user.Id);

            return View(storedProducts.ToList());
        }
    }
}