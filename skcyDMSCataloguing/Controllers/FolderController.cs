using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using skcyDMSCataloguing.DAL.Repositories;
using skcyDMSCataloguing.Models;
using skcyDMSCataloguing.ViewModels;

namespace skcyDMSCataloguing.Controllers
{
    public class FolderController : Controller
    {
        private readonly IBaseAsyncRepo<Folder> baseAsyncFolderRepo;
        private readonly IBaseAsyncRepo<CustData> baseAsyncCustDataRepo;
        private readonly IBaseAsyncRepo<CustAccData> baseAsyncCustAccRepo;
        private readonly IBaseAsyncRepo<CustRelData> baseAsyncCustRelRepo;

        public FolderController(IBaseAsyncRepo<Folder> baseAsyncFolderRepo,
                                IBaseAsyncRepo<CustData> baseAsyncCustDataRepo,
                                IBaseAsyncRepo<CustAccData> baseAsyncCustAccRepo,
                                IBaseAsyncRepo<CustRelData> baseAsyncCustRelRepo
                               )
        {
            this.baseAsyncFolderRepo = baseAsyncFolderRepo;
            this.baseAsyncCustDataRepo = baseAsyncCustDataRepo;
            this.baseAsyncCustAccRepo = baseAsyncCustAccRepo;
            this.baseAsyncCustRelRepo = baseAsyncCustRelRepo;
        }


        // GET: FolderController
        public async Task<ActionResult> Index()
        {
            var query = await baseAsyncFolderRepo.GetAllAsync(includeproperty: source=>source
                                .Include(cd=>cd.CustData)
                                    .ThenInclude(hl=>hl.PrjHelixes1)
                                 .Include(cd => cd.CustData)
                                    .ThenInclude(hl => hl.PrjVelocities1)
                                  .Include(cd => cd.CustData)
                                    .ThenInclude(hl => hl.PrjVelocities2)
                                    );

            return View(query);
        }


      

            // GET: FolderController/Details/5
            public async Task<ActionResult> Details(int? id)
        {
            if (id == null) { return NotFound(); }

            var folder = await baseAsyncFolderRepo.GetAllAsync(fl=>fl.ID==id, includeproperty: source => source
                                .Include(cd => cd.CustData)
                                    );

            if (folder==null) { return NotFound(); }

            return View(folder.FirstOrDefault());
        }


       
        public async Task<ActionResult> GetRelatedCif(string accountno)
        {
            if (accountno == null || accountno == "") { return NotFound(); }

            var allcifs = await baseAsyncCustDataRepo.GetAllAsync();

            var acc = await baseAsyncCustAccRepo.GetByConditionAsync(filter: ac => ac.CustAccountNo == accountno, includeProperties: "CustRelDataEntries");

            var accountcifs = new HashSet<int>(acc.CustRelDataEntries.Select(cf => cf.CustDataID));

            ViewData["AccountNo"] = acc.CustAccountNo;

            var viewModel = new List<AssignedAccountData>();

            foreach (var cif in allcifs)
            {
                viewModel.Add(new AssignedAccountData
                {
                    CIFID = cif.ID,
                    CIFNo = cif.CIFNo,
                    CIFCustomerName = cif.CustomerName,
                    Assigned = accountcifs.Contains(cif.ID)
                });
            }

            ViewData["BoxID"] = TempData["BoxID"];

            return View("GetRelatedCif", viewModel.Where(a => a.Assigned == true).ToList());
        }




        // GET: FolderController/Create
        public async Task<ActionResult> Create(int? boxid, string? accno, string? cifno)
        {
            ViewData["BoxID"] = boxid;
            TempData["BoxID"]= boxid;            

            if (boxid != null && (accno == null || accno ==""))
            {
                var folderreltoboxid = await baseAsyncFolderRepo.GetAllAsync(b => b.BoxID == boxid, orderBy: q => q.OrderByDescending(q => q.ID));

                if (folderreltoboxid.FirstOrDefault() != null)
                {
                    Folder foldertocreate = new Folder
                    {
                        BoxID = boxid ?? 0,
                        FolderName = "",
                        FolderDescription="",
                        CustDataCIFNo=""
                        
                    };
                    return View("CreateFromBox", foldertocreate);
                }
                return View("CreateFromBox", folderreltoboxid.FirstOrDefault());            
            }                      

            return View();
        }

        // POST: FolderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("FolderName,FolderDescription,BoxID,CustDataCIFNo")] Folder folder) 
        {
            try
            {

                baseAsyncFolderRepo.Insert(folder);
                await baseAsyncFolderRepo.SaveAsync();

                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Box", action = "Index", id = folder.BoxID }));
            }
            catch
            {
                return View();
            }
        }

        // GET: FolderController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id==null) { return NotFound(); }

            var folderToEdit = await baseAsyncFolderRepo.GetByConditionAsync(filter: i => i.ID == id);

            ViewData["BoxID"] = folderToEdit.BoxID;

            return View(folderToEdit);
        }

        // POST: FolderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("ID,FolderName,BoxID,CustDataCIFNo")] Folder folder)
        {
            if (id != folder.ID)
            {
                return NotFound();
            }
            try
            {
                baseAsyncFolderRepo.Update(folder);
                await baseAsyncFolderRepo.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }

             
            return View(folder);
        }

        // GET: FolderController/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null) { return NotFound(); }

            var folderToDelete = await baseAsyncFolderRepo.GetByConditionAsync(fld => fld.ID == id, includeProperties:"Box,CustData");
          
            if (folderToDelete == null) { return NotFound(); }
            
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(folderToDelete);
        }

        // POST: FolderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Folder folder)
        {
            var folderToDelete = await baseAsyncFolderRepo.GetByConditionAsync(fld => fld.ID == id);

            ViewData["FolderBoxID"] = folderToDelete.BoxID;

            if (folder == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {                 

                baseAsyncFolderRepo.Delete(folder);
                await baseAsyncFolderRepo.SaveAsync();
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Box", action = "Index", id = ViewData["FolderBoxID"]??1 }));
            }
            catch (DbUpdateException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                return RedirectToAction("Delete", new { ID = id, saveChangesError = true });
            }
        }
    }
}
