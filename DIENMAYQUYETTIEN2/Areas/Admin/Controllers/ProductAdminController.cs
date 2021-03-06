﻿using System;
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
        //create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.ProductType = db.ProductTypes.OrderByDescending(x => x.ID).ToList();
            if (Session["Username"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product p)
        {
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                { 
                var pro = new Product();

                pro.ProductCode = p.ProductCode;
                pro.ProductName = p.ProductName;
                pro.ProductType = p.ProductType;
                pro.ProductTypeID = p.ProductTypeID;
                pro.OriginPrice = p.OriginPrice;
                pro.SalePrice = p.SalePrice;
                pro.Status = p.Status;
                pro.Quantity = p.Quantity;
                pro.InstallmentPrice = p.InstallmentPrice;
                pro.Avatar = p.Avatar;
                db.Products.Add(pro);
                db.SaveChanges();
                var path = Server.MapPath("~/App_Data");
                path = path + "/" + pro.ID;
                    if (Request.Files["Avatar"] != null && Request.Files["Avatar"].ContentLength > 0)
                    {
                        Request.Files["Avatar"].SaveAs(path);
                        scope.Complete();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("Avatar", "Chưa có hình ảnh");
                    }
                }

            }
            return View();
        }
        public FileResult Details(int id)
        {
            var path = Server.MapPath("~/App_Data/" + id);
            return File(path, "images");
        }
        //Login
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
                    var obj = db.Accounts.Where(a => a.Username.Equals(acc.Username) && a.Password.Equals(acc.Password)).FirstOrDefault();

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
        //Logout
        [HttpPost]
        public ActionResult Logout()
        {   
            Session.Clear();
            Session.Abandon(); // it will clear the session at the end of request
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Edit(int ID)
        {
<<<<<<< HEAD
            Product model = db.Products.Find(ID);
            if (model == null)
=======
            
                if (ID == null)
                {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product bangsanpham = db.Products.Find(ID);
                if (bangsanpham == null)
                {
                return HttpNotFound();
                }
            if (Session["Username"] != null)
            {
                ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ID", "ProductTypeName", bangsanpham.ProductTypeID);
                return View(bangsanpham);
            }
            else
>>>>>>> f4e2eda2ef465cdcb917c61c5b94c9653aaf87c8
            {
                return RedirectToAction("Login");
            }
<<<<<<< HEAD
            //ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ID", "ProducTypeName", model.ProductTypeID);
            ViewBag.ProductType = db.ProductTypes.OrderByDescending(x => x.ID).ToList();
            return View(model);
=======
>>>>>>> f4e2eda2ef465cdcb917c61c5b94c9653aaf87c8
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model)
        {

            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();

                    var path = Server.MapPath("~/App_Data");
                    path = path + "/" + model.ID;
                    if (Request.Files["HinhAnh"] != null && Request.Files["HinhAnh"].ContentLength > 0)
                    {
                        Request.Files["HinhAnh"].SaveAs(path);


                    }
                    scope.Complete();
                    return RedirectToAction("Index");

                }
            }
            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ID", "ProductTypeName", model.ProductTypeID);
            return View(model);

        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product bangsanpham = db.Products.Find(id);
            if (bangsanpham == null)
            {
                return HttpNotFound();
            }
            if (Session["Username"] != null)
            {
                return View(bangsanpham);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product bangsanpham = db.Products.Find(id);
            db.Products.Remove(bangsanpham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
       
    }
}
