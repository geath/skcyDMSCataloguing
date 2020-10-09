using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using skcyDMSCataloguing.DAL;
using skcyDMSCataloguing.DAL.Repositories;
using skcyDMSCataloguing.Models;
using skcyDMSCataloguing.ViewModels;

namespace skcyDMSCataloguing.Controllers
{
    public class BoxController : Controller
    {
        private readonly IBaseAsyncRepo<Box> baseAsyncRepo;

        public BoxController(IBaseAsyncRepo<Box>  baseAsyncRepo)
        {
            this.baseAsyncRepo = baseAsyncRepo;
        }


        // GET: BookController
        public async Task<ActionResult> Index(int? id, int? folderid)
        {
            var viewmodel = new BoxFolderDocIndexData();


            viewmodel.Boxes = await baseAsyncRepo.GetAllAsync(includeproperty: source => source
                                .Include(bc => bc.BoxCreator)
                                .Include(fl => fl.Folders)
                                .ThenInclude(ct=>ct.CustData));
            if (id != null) {

                ViewData["BoxID"] = id.Value;
                viewmodel.Folders = viewmodel.Boxes
                                    .Where(b => b.ID == id)
                                    .Single().Folders;
            }
            if (folderid !=null)
            {
                ViewData["FolderID"] = folderid.Value;
            }

            return View(viewmodel);
        }

        // GET: BookController/Create
        public async Task<ActionResult> Create()
        {
            Box box = new Box();
            var maxid = (from boc in await baseAsyncRepo.GetAllAsync()
                         select boc.ID).Max();

            box.BoxDescription = "Box." + maxid.ToString();
            box.DateBoxCreated = DateTime.Now;
            return View(box);
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("BoxDescription,DateBoxCreated,BoxCreatorID")] Box box)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    baseAsyncRepo.Insert(box);
                    await baseAsyncRepo.SaveAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
                
            }
            return RedirectToAction("Index");
        }




        // GET: BookController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
      


        // GET: BookController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
