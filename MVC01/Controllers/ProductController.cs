﻿using MVC01.Models.Northwind;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVC01.Controllers
{
    public class ProductController : Controller
    {
        private NorthwindEntities ctx = new NorthwindEntities();
        //
        // GET: /Forms/
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("Kool")]
        public string Bogus()
        {
            return "It is cool to use ActionName";
        }


        public ActionResult Search(string q)
        {
            var products = ctx.Products
                              .Include("Category")
                              .Where(c => c.ProductName.Contains(q))
                              .Take(10);
            return View(products);
        }

        // GET: /Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = ctx.Products.Find(id);
            if (product == null) {
                return HttpNotFound();
            }

            ViewBag.Categories = new SelectList(
                ctx.Categories
                .OrderBy(c => c.CategoryName),
                "CategoryID", "CategoryName", product.CategoryID);

            ViewBag.Suppliers = new SelectList(
                ctx.Suppliers
                .OrderBy(c => c.CompanyName),
                "SupplierID", "CompanyName", product.SupplierID);

            return View(product);
        }

        // POST: /Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,UnitPrice,CategoryID")] Product product)
        {
            if (ModelState.IsValid) {
                ctx.Entry(product).State = EntityState.Modified;
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: /Product/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(ctx.Categories, "CategoryID", "CategoryName");
            ViewBag.SupplierID = new SelectList(ctx.Suppliers, "SupplierID", "CompanyName");
            return View();
        }

        // POST: /Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
        {
            if (ModelState.IsValid)
            {
                ctx.Products.Add(product);
                ctx.SaveChanges();
                return View();
            }

            ViewBag.CategoryID = new SelectList(ctx.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.SupplierID = new SelectList(ctx.Suppliers, "SupplierID", "CompanyName", product.SupplierID);
            return View(product);
        }
    }
}