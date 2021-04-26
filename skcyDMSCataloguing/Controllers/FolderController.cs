using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IBaseAsyncRepo<PrjHelix1> baseAsyncHelix1;
        private readonly IBaseAsyncRepo<PrjVelocity1> baseAsyncVelocity1;
        private readonly IBaseAsyncRepo<PrjVelocity2> baseAsyncVelocity2;

        public FolderController(IBaseAsyncRepo<Folder> baseAsyncFolderRepo,
                                IBaseAsyncRepo<CustData> baseAsyncCustDataRepo,
                                IBaseAsyncRepo<CustAccData> baseAsyncCustAccRepo,
                                IBaseAsyncRepo<CustRelData> baseAsyncCustRelRepo,
                                IBaseAsyncRepo<PrjHelix1> baseAsyncHelix1,
                                IBaseAsyncRepo<PrjVelocity1> baseAsyncVelocity1,
                                IBaseAsyncRepo<PrjVelocity2> baseAsyncVelocity2
                               )
        {
            this.baseAsyncFolderRepo = baseAsyncFolderRepo;
            this.baseAsyncCustDataRepo = baseAsyncCustDataRepo;
            this.baseAsyncCustAccRepo = baseAsyncCustAccRepo;
            this.baseAsyncCustRelRepo = baseAsyncCustRelRepo;
            this.baseAsyncHelix1 = baseAsyncHelix1;
            this.baseAsyncVelocity1 = baseAsyncVelocity1;
            this.baseAsyncVelocity2 = baseAsyncVelocity2;
        }


        // GET: FolderController
        //[AllowAnonymous]
        [Authorize(Roles = "Administrators,WebAppAdmins,WebAppPowerUsers,WebAppEditors,WebAppContributors")]
        public async Task<IActionResult> Index()
        {
            var query = await baseAsyncFolderRepo.GetAllAsync(includeproperty: source => source
                                .Include(cd => cd.CustData)
                                    .ThenInclude(hl => hl.PrjHelixes1)
                                 .Include(cd => cd.CustData)
                                    .ThenInclude(hl => hl.PrjVelocities1)
                                  .Include(cd => cd.CustData)
                                    .ThenInclude(hl => hl.PrjVelocities2)
                                    );

            return View(query);
        }

        // GET: FolderController/Details/5
        //[AllowAnonymous]
        [Authorize(Roles = "Administrators,WebAppAdmins,WebAppPowerUsers,WebAppEditors,WebAppContributors")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) { return NotFound(); }

            var folder = await baseAsyncFolderRepo.GetAllAsync(fl => fl.ID == id, includeproperty: source => source
                                    .Include(cd => cd.CustData)
                                    );

            if (folder == null) { return NotFound(); }

            return View(folder.FirstOrDefault());
        }


        [HttpGet]
        //[AllowAnonymous]
        [Authorize(Roles = "Administrators,WebAppAdmins,WebAppPowerUsers,WebAppEditors,WebAppContributors")]
        public async Task<IActionResult> GetRelatedCif(int boxid, string accountno, string custidn)
        {
            int a = boxid;

            if (String.IsNullOrEmpty(accountno) && String.IsNullOrEmpty(custidn)) { return NotFound(); }
            if (!String.IsNullOrEmpty(accountno) && String.IsNullOrEmpty(custidn))
            {
                var acc = await baseAsyncCustAccRepo.GetByConditionAsync(filter: ac => ac.CustAccountNo == accountno, includeProperties: "CustRelDataEntries");
                var allcifs = await baseAsyncCustDataRepo.GetAllAsync(filter: a=>a.CIFNo.Contains(accountno));
                var accountcifs = new HashSet<string>(acc.CustRelDataEntries.Select(cf => cf.CustCIFNo));
                ViewData["AccountNo"] = acc.CustAccountNo;
                var viewModel = new List<AssignedAccountData>();
                foreach (var cif in allcifs)
                {
                    viewModel.Add(new AssignedAccountData
                    {
                        CIFNo = cif.CIFNo.Trim(),
                        CIFCustomerName = cif.CustomerName ?? "",
                        Assigned = accountcifs.Contains(cif.CIFNo),
                        BoxID = boxid,

                        OldCIFNo = cif.OldCIFNo,
                        CardTypePr = cif.CardType.ToString(),
                        CardActivePr = cif.CardActive.ToString()
                        //,ManagedBy = ((cif.PrjHelixes1.ToString() ?? cif.PrjVelocities1.ToString()) ?? cif.PrjVelocities2.ToString()) ?? "BOC"
                    });
                }
                var model = viewModel.Where(a => a.Assigned == true).ToList();
                return View("GetRelatedCif", model);
            }

            if (String.IsNullOrEmpty(accountno) && !String.IsNullOrEmpty(custidn))
            {
                var allcifs = await baseAsyncCustDataRepo.GetAllAsync(filter: cd => 
                                        (cd.CustomerIDN.Contains(custidn) || cd.CustomerName.Contains(custidn))
                                        ||
                                        (cd.OldCIFNo.Contains(custidn) || cd.OldCIFNo.Contains(custidn)));

                var viewModel = new List<AssignedAccountData>();
                foreach (var cif in allcifs)
                {
                    viewModel.Add(new AssignedAccountData
                    {
                        CIFNo = cif.CIFNo.Trim(),
                        CIFCustomerName = cif.CustomerName ?? "",
                        BoxID = boxid,

                        OldCIFNo = cif.OldCIFNo,
                        CardTypePr = cif.CardType.ToString()??"Unknown",
                        CardActivePr= cif.CardActive.ToString() ?? "Unknown"                        
                       ,// ManagedBy = (cif.PrjHelixes1.Any()?"Helix":(cif.PrjVelocities1.Any()?"Velocity 1" :(cif.PrjVelocities2.Any()?"Velocity 2":"BOC")))
                    });
                }
                var model = viewModel.ToList();
                return View("GetRelatedCif", model);
            }

            return View();
        }


        // GET: FolderController/Create
        //[AllowAnonymous]
        [Authorize(Roles = "Administrators,WebAppAdmins,WebAppPowerUsers,WebAppEditors,WebAppContributors")]
        public async Task<IActionResult> Create(int? boxid, string CIFNo)
        {
            ViewData["BoxID"] = boxid;
            TempData["BoxID"] = boxid;


            if (boxid != null && string.IsNullOrEmpty(CIFNo))
            {
                var folderreltoboxid = await baseAsyncFolderRepo.GetAllAsync(b => b.BoxID == boxid, orderBy: q => q.OrderByDescending(q => q.ID));

                if (folderreltoboxid.FirstOrDefault() != null)
                {
                    Folder foldertocreate = new Folder
                    {
                        BoxID = boxid ?? 0,
                        FolderName = "",      //Folder's Barcode
                        FolderDescription = "",
                        CustDataCIFNo = ""  //CIF Number

                    };
                    return View("CreateFromBox", foldertocreate);
                }
                return View("CreateFromBox", folderreltoboxid.FirstOrDefault());
            }

            else
            {
                if (boxid != null && (CIFNo != null || CIFNo != ""))
                {
                    var folderreltoboxid = await baseAsyncFolderRepo.GetAllAsync(b => b.BoxID == boxid, orderBy: q => q.OrderByDescending(q => q.ID));

                    if (folderreltoboxid.FirstOrDefault() != null)
                    {
                        Folder foldertocreate = new Folder
                        {
                            BoxID = boxid ?? 0,
                            FolderName = "",      //Folder's Barcode
                            FolderDescription = "",
                            CustDataCIFNo = CIFNo.Trim()  //CIF Number

                        };
                        return View("CreateFromBox", foldertocreate);
                    }
                    return View("CreateFromBox" , folderreltoboxid.FirstOrDefault() );
                }
            }


            return View();
        }

        // POST: FolderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AllowAnonymous]
        [Authorize(Roles = "Administrators,WebAppAdmins,WebAppPowerUsers,WebAppEditors,WebAppContributors")]
        public async Task<IActionResult> Create([Bind("FolderName,FolderDescription,BoxID,CustDataCIFNo")] Folder folder)
        {
            var fex = await baseAsyncFolderRepo.GetAllAsync(m => m.BoxID == folder.BoxID);
            var awe = new HashSet<string>(fex.Select(c => c.CustDataCIFNo));
            if (awe.Contains(folder.CustDataCIFNo)) { return RedirectToAction("Error", "Home"); }

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
        //[AllowAnonymous]
        [Authorize(Roles = "Administrators,WebAppAdmins,WebAppPowerUsers,WebAppEditors,WebAppContributors")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) { return NotFound(); }

            var folderToEdit = await baseAsyncFolderRepo.GetByConditionAsync(filter: i => i.ID == id);

            ViewData["BoxID"] = folderToEdit.BoxID;

            return View(folderToEdit);
        }

        // POST: FolderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AllowAnonymous]
        [Authorize(Roles = "Administrators,WebAppAdmins,WebAppPowerUsers,WebAppEditors,WebAppContributors")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FolderName,BoxID,CustDataCIFNo")] Folder folder)
        {
            if (id != folder.ID)
            {
                return NotFound();
            }
            try
            {
                baseAsyncFolderRepo.Update(folder);
                await baseAsyncFolderRepo.SaveAsync();
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Box", action = "Index", id = folder.BoxID }));
            }
            catch (DbUpdateException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }


            return View(folder);
        }

        // GET: FolderController/Delete/5
        //[AllowAnonymous]
        [Authorize(Roles = "Administrators,WebAppAdmins,WebAppPowerUsers,WebAppEditors,WebAppContributors")]
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null) { return NotFound(); }

            var folderToDelete = await baseAsyncFolderRepo.GetByConditionAsync(fld => fld.ID == id, includeProperties: "Box,CustData");

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
        //[AllowAnonymous]
        [Authorize(Roles = "Administrators,WebAppAdmins,WebAppPowerUsers,WebAppEditors,WebAppContributors")]
        public async Task<IActionResult> Delete(int id, Folder folder)
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
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Box", action = "Index", id = ViewData["FolderBoxID"] ?? 1 }));
            }
            catch (DbUpdateException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                return RedirectToAction("Delete", new { ID = id, saveChangesError = true });
            }
        }

        [AcceptVerbs("GET", "POST")]
        [Authorize(Roles = "Administrators,WebAppAdmins,WebAppPowerUsers,WebAppEditors,WebAppContributors")]
        public async Task<IActionResult> IsCIFNoUsed(string custDataCIFNo)
        {
            var cif = await baseAsyncFolderRepo.GetByConditionAsync(f => f.CustDataCIFNo == custDataCIFNo);

            if (cif == null) { return Json(true); }
            else { return Json($"CIFNo {custDataCIFNo} is already registered in box {cif.BoxID}"); }

        }



        [AcceptVerbs("GET", "POST")]
        [Authorize(Roles = "Administrators,WebAppAdmins,WebAppPowerUsers,WebAppEditors,WebAppContributors")]
        public async Task<IActionResult> CIFExists(string custDataCIFNo)
        {
     
            var cif = await baseAsyncCustDataRepo.GetByConditionAsync(cd => cd.CIFNo == custDataCIFNo);

                if (cif!=null)
            {
                return Json(true);
            }

            else 
            {
                 return Json($" CIF doesn't exist. Please check out the spelling and if the problem persists contact operations ");
            }
                                   
        }

    }
}
