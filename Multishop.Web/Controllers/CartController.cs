using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Multishop.Data.DAL.Services.Repository;
using Multishop.Entities.Accounts;
using Multishop.Entities.ShopEntities;
using Multishop.Service.Operations;
using Multishop.Web.Models.CartViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Multishop.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private IProductionRepository<OrderProduct> _orderProductRepository;
        private ApplicationUserManager _userManager;
        
        public CartController()
        {
            _orderProductRepository = new OrderProductRepository(new Data.DAL.Context.ApplicationDbContext());
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

        // GET: Cart
        public ActionResult Index()
        {
            CurrentUser = UserManager.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            List<OrderProduct> orderedProducts = _orderProductRepository.GetEntities().Where(p => p.UserId == this.CurrentUser.Id).ToList();
            CartViewModel model = new CartViewModel()
            {
                OrderProducts = orderedProducts,
                Price = BalanceOperations.GetPrice(orderedProducts)
            };

            return View(model);
        }

        // POST: Cart/Add
        [HttpPost, ActionName("Add")]
        public void Add(Product product)
        {
            CurrentUser = UserManager.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            OrderProduct orderProduct = _orderProductRepository.GetEntities()
                .Where(p => p.UserId == this.CurrentUser.Id && p.ProductId == product.ProductId)
                .FirstOrDefault();
            try
            {
                orderProduct.Quantity += 1;
                _orderProductRepository.Update(orderProduct);
                _orderProductRepository.Save();
            }
            catch (NullReferenceException e)
            {
                orderProduct = new OrderProduct()
                {
                    UserId = CurrentUser.Id,
                    ProductId = product.ProductId,
                    Quantity = 1
                };
                _orderProductRepository.Insert(orderProduct);
                _orderProductRepository.Save();
            }
        }

        // GET: Cart/Return
        public ActionResult Return(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _orderProductRepository = new OrderProductRepository(new Data.DAL.Context.ApplicationDbContext());
            OrderProduct orderProduct = _orderProductRepository.GetDetails(id);
            orderProduct.Quantity -= 1;

            if(orderProduct.Quantity == 0)
            {
                _orderProductRepository.Delete(id.GetValueOrDefault());
            } else
            {
                _orderProductRepository.Update(orderProduct);
            }
            _orderProductRepository.Save();

            return RedirectToAction("Index");
        }

        //GET: Cart/Checkout
        public ActionResult Checkout()
        {
            return View();
        }

        //POST: Cart/Checkout
        [HttpPost, ActionName("Checkout")]
        [ValidateAntiForgeryToken]
        public ActionResult CheckoutConfirmed()
        {
            CurrentUser = UserManager.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            List<OrderProduct> orderProducts = _orderProductRepository.GetEntities()
                .Where(p => p.UserId == this.CurrentUser.Id).ToList();
            if (!BalanceOperations.CanBuy(orderProducts, CurrentUser.Balance))
            {
                TempData["Error"] = "Insufficient credits!";
                return RedirectToAction("Index");
            }
            CurrentUser.Balance -= BalanceOperations.GetPrice(orderProducts);
            UserManager.Update(CurrentUser);

            var inventoryController = DependencyResolver.Current.GetService<InventoryController>();
            inventoryController.ControllerContext = new ControllerContext(this.Request.RequestContext, inventoryController);

            inventoryController.Add(orderProducts);

            foreach(OrderProduct product in orderProducts)
            {
                _orderProductRepository.Delete(product.OrderProductId);
            }
            _orderProductRepository.Save();
            

            return RedirectToAction("Index", "Inventory");
        }
    }
}