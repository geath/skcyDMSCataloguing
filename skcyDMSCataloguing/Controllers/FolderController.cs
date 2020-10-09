using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using skcyDMSCataloguing.DAL.Repositories;
using skcyDMSCataloguing.Models;

namespace skcyDMSCataloguing.Controllers
{
    public class FolderController : Controller
    {
        private readonly IBaseAsyncRepo<Folder> baseAsyncRepo;

        public FolderController(IBaseAsyncRepo<Folder> baseAsyncRepo)
        {
            this.baseAsyncRepo = baseAsyncRepo;
        }


        // GET: FolderController
        public async Task<ActionResult> Index()
        {
            var query = await baseAsyncRepo.GetAllAsync(includeproperty: source=>source
                                .Include(cd=>cd.CustData));

            return View(query);
        }

        // GET: FolderController/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null) { return NotFound(); }

            var folder = await baseAsyncRepo.GetAllAsync(fl=>fl.ID==id, includeproperty: source => source
                                .Include(cd => cd.CustData));

            if (folder==null) { return NotFound(); }

            return View(folder.FirstOrDefault());
        }

        // GET: FolderController/Create
        public async Task<ActionResult> Create(int? boxid)
        {
            ViewData["BoxID"] = boxid;

            if (boxid != null)
            {
                var folderreltoboxid = await baseAsyncRepo.GetAllAsync(b => b.BoxID == boxid, orderBy: q => q.OrderByDescending(q => q.ID));

                if (folderreltoboxid.FirstOrDefault() == null)
                {
                    Folder foldertocreate = new Folder
                    {
                        BoxID = boxid ?? 1                        
                    };
                    return View("CreateFromBox", foldertocreate);
                }
                return View("CreateFromBox", folderreltoboxid.FirstOrDefault());
            }
            return View("Index");
        }

        // POST: FolderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("FolderName,FolderDescription,BoxID,CustDataID")] Folder folder) 
        {
            try
            {
                baseAsyncRepo.Insert(folder);
                await baseAsyncRepo.SaveAsync();

                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Box", action = "Index", id = folder.BoxID }));
            }
            catch
            {
                return View();
            }
        }

        // GET: FolderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FolderController/Edit/5
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

        // GET: FolderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FolderController/Delete/5
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
