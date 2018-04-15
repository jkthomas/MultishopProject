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
        private IRepository<Product> productRepository;
        private IRepository<Category> categoryRepository;
        private IRepository<OrderProduct> orderProductRepository;

        public ProductController()
        {
            this.productRepository = new ProductRepository(new ApplicationDbContext());
            this.categoryRepository = new CategoryRepository(new ApplicationDbContext());
        }

        // GET: Product
        public ActionResult Index()
        {
            ProductIndexViewModel productIndexViewModel = new ProductIndexViewModel()
            {
                Products = productRepository.GetEntities(),
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
            Product product = productRepository.GetDetails(id);
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
                Categories = categoryRepository.GetEntities().ToList()
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
                productViewModel.Categories = categoryRepository.GetEntities().ToList();
                return View(productViewModel);
            }
            productViewModel.Product.Category = categoryRepository.GetEntities().Where(c => c.CategoryId == productViewModel.CategoryId).FirstOrDefault();
            Product product = new Product()
            {
                CategoryId = productViewModel.Product.Category.CategoryId,
                Name = productViewModel.Product.Name,
                UnitPrice = productViewModel.Product.UnitPrice,
                Description = productViewModel.Product.Description
            };
            productRepository.Insert(product);
            productRepository.Save();

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
            Product product = productRepository.GetDetails(id);
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
                productRepository.Update(product);
                productRepository.Save();

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
            Product product = productRepository.GetDetails(id);
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
            Product product = productRepository.GetDetails(id);
            productRepository.Delete(id);
            productRepository.Save();

            return RedirectToAction("Index");
        }

        //TODO: Implement this
        // GET: Product/Buy/5
        [Authorize]
        public ActionResult Buy(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            this.orderProductRepository = new OrderProductRepository(new ApplicationDbContext());
            var user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            Product product = productRepository.GetDetails(id);
            OrderProduct orderProduct = new OrderProduct()
            {
                CartId = user.Cart.CartId,
                ProductId = product.ProductId,
                Quantity = 1
            };
            orderProductRepository.Insert(orderProduct);
            orderProductRepository.Save();

            TempData["Success"] = "Added Successfully!";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                productRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
