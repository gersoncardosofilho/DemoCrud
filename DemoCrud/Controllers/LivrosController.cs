using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoCrud.LivrosDAL;
using DemoCrud.Models;

namespace DemoCrud.Controllers
{
    public class LivrosController : Controller
    {
        private LivrosContext db = new LivrosContext();

        public PartialViewResult Listar(Livro livro, int pagina = 1, int registros = 5)
        {
            var livros = db.Livros.Include(l => l.Genero);

            if (!String.IsNullOrWhiteSpace(livro.Titulo))
            {
                livros = livros.Where(l => l.Titulo.Contains(livro.Titulo));
            }

            if (!String.IsNullOrWhiteSpace(livro.Autor))
            {
                livros = livros.Where(l => l.Autor.Contains(livro.Autor));
            }

            if (livro.AnoEdicao != 0)
            {
                livros = livros.Where(l => l.AnoEdicao.Equals(livro.AnoEdicao));
            }

            if (livro.Valor != decimal.Zero)
            {
                livros = livros.Where(l=>l.Valor.Equals(livro.Valor));
            }

            var livrosPaginados = livros.OrderBy(l => l.Titulo).Skip((pagina-1) * registros).Take(registros);

            return PartialView("_Listar", livrosPaginados.ToList());
        }

        // GET: Livros
        public ActionResult Index()
        {
            var livros = db.Livros.Include(l => l.Genero);
            return View(livros.ToList());
        }

        // GET: Livros/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Livro livro = db.Livros.Find(id);
            if (livro == null)
            {
                return HttpNotFound();
            }
            return View(livro);
        }

        // GET: Livros/Create
        public ActionResult Create()
        {
            ViewBag.GeneroId = new SelectList(db.Generos, "Id", "Nome");
            return View();
        }

        // POST: Livros/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Titulo,AnoEdicao,Valor,Autor,GeneroId")] Livro livro)
        {
            if (ModelState.IsValid)
            {
                db.Livros.Add(livro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GeneroId = new SelectList(db.Generos, "Id", "Nome", livro.GeneroId);
            return View(livro);
        }

        // GET: Livros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Livro livro = db.Livros.Find(id);
            if (livro == null)
            {
                return HttpNotFound();
            }
            ViewBag.GeneroId = new SelectList(db.Generos, "Id", "Nome", livro.GeneroId);
            return View(livro);
        }

        // POST: Livros/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Titulo,AnoEdicao,Valor,Autor,GeneroId")] Livro livro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(livro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GeneroId = new SelectList(db.Generos, "Id", "Nome", livro.GeneroId);
            return View(livro);
        }

        // GET: Livros/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Livro livro = db.Livros.Find(id);
            if (livro == null)
            {
                return HttpNotFound();
            }
            return View(livro);
        }

        // POST: Livros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Livro livro = db.Livros.Find(id);
            db.Livros.Remove(livro);
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
