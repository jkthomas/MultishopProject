using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Multishop.Data.DAL.Services.Repository;
using Multishop.Entities.Accounts;
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

        public InventoryController()
        {
            _storedProductRepository = new StoredProductRepository(new Data.DAL.Context.ApplicationDbContext());
        }

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
        public ApplicationUser CurrentUser { get; set; }

        // GET: Inventory
        public ActionResult Index()
        {
            CurrentUser = UserManager.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            IEnumerable<StoredProduct> storedProducts = _storedProductRepository.GetEntities().Where(p => p.UserId == CurrentUser.Id);

            return View(storedProducts.ToList());
        }

        // POST: Cart/Add
        [HttpPost, ActionName("Add")]
        public void Add(List<OrderProduct> products)
        {
            CurrentUser = UserManager.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            foreach (OrderProduct product in products)
            {
                StoredProduct storedProduct = _storedProductRepository.GetEntities()
                    .Where(p => p.UserId == this.CurrentUser.Id && p.ProductId == product.ProductId)
                    .FirstOrDefault();
                try
                {
                    storedProduct.Quantity += product.Quantity;
                    _storedProductRepository.Update(storedProduct);
                    _storedProductRepository.Save();
                }
                catch (NullReferenceException e)
                {
                    storedProduct = new StoredProduct()
                    {
                        UserId = CurrentUser.Id,
                        ProductId = product.ProductId,
                        Quantity = product.Quantity
                    };
                    _storedProductRepository.Insert(storedProduct);
                    _storedProductRepository.Save();
                }
            }
        }
    }
}