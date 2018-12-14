using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DIENMAYQUYETTIEN2.Models;
using System.Transactions;
using System.Net;
using System.Data.Entity;
using System.Data;
using System.Web.Security;

namespace DIENMAYQUYETTIEN2.Areas.Admin.Controllers
{
    public class ProductAdminController : Controller
    {
        DIENMAYQUYETTIENEntities db = new DIENMAYQUYETTIENEntities();
        //
        // GET: /Admin/ProductAdmin/
        public ActionResult Index()
        {
            var product = db.Products.OrderByDescending(x => x.ID).ToList();
            
            if (Session["Username"] != null)
            {
                return View(product);
            }
            else
            {
                return RedirectToAction("Login");
            }
            
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.ProductType = db.ProductTypes.OrderByDescending(x => x.ID).ToList();
            return View();
        }
        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product p)
        {
            if(ModelState.IsValid){
            var pro = new Product();
            
            pro.ProductCode = p.ProductCode;
            pro.ProductName = p.ProductName;
            pro.ProductType = p.ProductType;
            pro.ProductTypeID = p.ProductTypeID;
            pro.SalePrice = p.SalePrice;
            pro.Status = p.Status;
            pro.Quantity = p.Quantity;
            db.Products.Add(pro);
            db.SaveChanges();
            return RedirectToAction("Index");
            }
            //Tao điều kiện
            return View();

        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Login(Account acc)
        {
            if (ModelState.IsValid)
            {
                using (DIENMAYQUYETTIENEntities db = new DIENMAYQUYETTIENEntities())
                {
                    var obj = db.Accounts.Where(a => a.Username.Equals(acc.Username)&& a.Password.Equals(acc.Password)).FirstOrDefault();
                    
                        if (obj != null)  
                    {
                        Session["Username"] = obj.Username.ToString();
                        Session["FullName"] = obj.FullName.ToString();
                        return RedirectToAction("Index");  
                    }  
                }
            }
            return View(acc);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index", "ProductAdmin");
        }

    }
}