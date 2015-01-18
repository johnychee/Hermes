using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebApp.Models;

namespace TestWebApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        public ActionResult Index(string msg = null)
        {
            ViewBag.Message = msg;

            HermesDBEntities hDesk = new HermesDBEntities();
            IEnumerable<Product> products = hDesk.Products;

            return View(products);
        }

        [Authorize(Roles = "SuperUser")]
        public ActionResult Create(string msg = null)
        {
            ViewBag.Message = msg;

            return View();
        }
        [HttpPost]
        [Authorize(Roles = "SuperUser")]
        public ActionResult Create(Product model, string msg = null)
        {
            ViewBag.Message = msg;
            HermesDBEntities hDesk = new HermesDBEntities();

            try
            {
                if (ModelState.IsValid)
                {
                    hDesk.Products.Add(model);
                    hDesk.SaveChanges();
                    return RedirectToAction("Details", new { @id = model.id, @msg = "Product successfully created" });
                }
                // invalid data
                else
                {
                    ViewBag.IsError = true;
                    return View(model);
                }
            }
            // some error
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View(model);
            }
        }

        public ActionResult Details(int id, string msg = null)
        {
            ViewBag.Message = msg;
            Product product = (new HermesDBEntities()).Products.Single(p => p.id == id);
            return View(product);
        }

        [Authorize(Roles = "SuperUser")]
        public ActionResult Edit(int id, string msg = null)
        {
            ViewBag.Message = msg;
            Product product = (new HermesDBEntities()).Products.Single(p => p.id == id);
            return View(product);
        }

        [HttpPost]
        [Authorize(Roles = "SuperUser")]
        public ActionResult Edit(int id, Product model, string msg = null)
        {
            ViewBag.Message = msg;
            HermesDBEntities hDesk = new HermesDBEntities();
            try
            {
                if (ModelState.IsValid)
                {
                    var product = hDesk.Products.Single(p => p.id == id);
                    product.name = model.name;
                    product.categoryId = model.categoryId;
                    product.description = model.description;
                    product.garant = model.garant;
                    product.supplier = model.supplier;

                    hDesk.SaveChanges();
                    return RedirectToAction("Details", new { @id = id, msg = "Product was successfully updated!" });
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View(model);
            }
        }

        [Authorize(Roles = "SuperUser")]
        public ActionResult Destroy(int id)
        {
            HermesDBEntities hDesk = new HermesDBEntities();
            try
            {
                hDesk.Products.Remove(hDesk.Products.Single(p => p.id == id));
                return RedirectToAction("Index", new { @msg = "Product was removed!" });
            }
            catch(Exception e)
            {
                return RedirectToAction("Details", new { @id = id, @msg = e.Message });
            }
        }

        [Authorize(Roles = "SuperUser")]
        public ActionResult AssignProducts()
        {
            HermesDBEntities hDesk = new HermesDBEntities();
            List<ProductsViewModel> model = new List<ProductsViewModel>();
            foreach (Product product in hDesk.Products)
            {
                ProductsViewModel pvm = new ProductsViewModel();
                pvm.Id = product.id;
                pvm.IsSelected = false;
                pvm.Name = product.name;
                pvm.Supplier = product.supplier;
                model.Add(pvm);
            }
            return View(model);
        }
     
        public JsonResult Get(string term)
        {
            HermesDBEntities hDesk = new HermesDBEntities();
            List<string> products = hDesk.Products.Where(p => p.name.StartsWith(term)).Select(y => y.name).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }
    }

}