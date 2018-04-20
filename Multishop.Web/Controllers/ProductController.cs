using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Multishop.Data.DAL.Context;
using Multishop.Data.DAL.Services.Repository;
using Multishop.Entities.ShopEntities;
using Multishop.Web.Models.CartViewModels;

namespace Multishop.Web.Controllers
{
    public class ProductController : Controller
    {
        private IProductionRepository<Product> _productRepository;
        private IProductionRepository<Category> _categoryRepository;
        private IProductionRepository<OrderProduct> _orderProductRepository;

        public ProductController()
        {
            this._productRepository = new ProductRepository(new ApplicationDbContext());
            this._categoryRepository = new CategoryRepository(new ApplicationDbContext());
            this._orderProductRepository = new OrderProductRepository(new ApplicationDbContext());
        }

        // GET: Product
        public ActionResult Index()
        {
            ProductIndexViewModel productIndexViewModel = new ProductIndexViewModel()
            {
                Products = _productRepository.GetEntities(),
                IsAdmin = User.IsInRole("Admin") //bool: IsAdmin = false if current user is not an Admin
            };

            return View(productIndexViewModel);
        }

        // GET: Product/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _productRepository.GetDetails(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Product/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ProductViewModel model = new ProductViewModel()
            {
                Categories = _categoryRepository.GetEntities().ToList()
            };
            return View(model);
        }

        // POST: Product/Create
        // TODO: Bind include only required Product entity properties
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(/*[Bind(Include = "ProductId,Name,UnitPrice,Category,Description")]*/ ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                productViewModel.Categories = _categoryRepository.GetEntities().ToList();
                return View(productViewModel);
            }
            productViewModel.Product.Category = _categoryRepository.GetEntities().Where(c => c.CategoryId == productViewModel.CategoryId).FirstOrDefault();
            Product product = new Product()
            {
                CategoryId = productViewModel.Product.Category.CategoryId,
                Name = productViewModel.Product.Name,
                UnitPrice = productViewModel.Product.UnitPrice,
                Description = productViewModel.Product.Description
            };
            _productRepository.Insert(product);
            _productRepository.Save();

            return RedirectToAction("Index");
        }

        // GET: Product/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _productRepository.GetDetails(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ProductId,Name,UnitPrice,Category,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.Update(product);
                _productRepository.Save();

                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Product/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _productRepository.GetDetails(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = _productRepository.GetDetails(id);
            _productRepository.Delete(id);
            _productRepository.Save();

            return RedirectToAction("Index");
        }

        // GET: Product/Buy/5
        [Authorize]
        public ActionResult Buy(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            Product product = _productRepository.GetDetails(id);
            if (product.UnitPrice > user.Balance)
            {
                TempData["Error"] = "Insufficient credits!";
                return RedirectToAction("Index");
            }

            user.Balance -= product.UnitPrice;
            OrderProduct orderProduct = _orderProductRepository.GetEntities().FirstOrDefault(p => p.ProductId == product.ProductId);
            try
            {
                orderProduct.Quantity += 1;
                _orderProductRepository.Update(orderProduct);
                _orderProductRepository.Save();
            } catch (NullReferenceException e)
            {
                orderProduct = new OrderProduct()
                {
                    UserId = user.Id,
                    ProductId = product.ProductId,
                    Quantity = 1
                };
                _orderProductRepository.Insert(orderProduct);
                _orderProductRepository.Save();
            }

            TempData["Success"] = "Bought successfully!";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _productRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
