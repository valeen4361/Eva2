using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Prueba2;

namespace Prueba2.Controllers
{
    public class ProductoesController : Controller
    {
        private SupermercadoEntities db = new SupermercadoEntities();

        // GET: Productoes
        public ActionResult Index()
        {
            var producto = db.Producto.Include(p => p.Categoria);
            return View(producto.ToList());
        }

        // GET: Productoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: Productoes/Create
        public ActionResult Create()
        {
            ViewBag.idCategoria = new SelectList(db.Categoria, "idCategoria", "nombre");
            return View();
        }

        // POST: Productoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idProducto,nombre,descripcion,precio,stock,idCategoria,estado")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Producto.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idCategoria = new SelectList(db.Categoria, "idCategoria", "nombre", producto.idCategoria);
            return View(producto);
        }

        // GET: Productoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCategoria = new SelectList(db.Categoria, "idCategoria", "nombre", producto.idCategoria);
            return View(producto);
        }

        // POST: Productoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProducto,nombre,descripcion,precio,stock,idCategoria,estado")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index"); //me esta arojando error al guardar un cambio. 
            }
            ViewBag.idCategoria = new SelectList(db.Categoria, "idCategoria", "nombre", producto.idCategoria);
            return View(producto);
        }

        // GET: Productoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Productoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Producto.Find(id);
            db.Producto.Remove(producto);
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


        public ActionResult Estadisticas ()
        {

            var cateInfoListque = new List<Categoria>();
            var produInfoListque = new List<Producto>();


            //Iterar productos
            var groupPr = produInfoListque.GroupBy(n => n.stock)
        .Select(n => new
        {
            Producto= n.Key,
            Counts = n.Count(),
            
            
        });
            var groups = cateInfoListque.GroupBy(n => n.idCategoria)
                         .Select(n => new
                         {
                        Categoria = n.Key,
                             Counts = n.Count(),
                             

                         });



            return View(groups);
            

        }


    }
}
