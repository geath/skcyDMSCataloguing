﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using skcyDMSCataloguing.DAL;
using skcyDMSCataloguing.DAL.Repositories;
using skcyDMSCataloguing.Models;
using skcyDMSCataloguing.Services;
using skcyDMSCataloguing.ViewModels;


namespace skcyDMSCataloguing.Controllers
{
    public class BoxController : Controller
    {

        #region Class Properties & Constructor 
        private readonly IBaseAsyncRepo<Box> baseAsyncBoxRepo;
        private readonly IBaseAsyncRepo<BoxCreator> baseAsyncBoxCreatorRepo;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<Box> logger;
        private readonly IGetObjectType getObjectType;

        
        public BoxController(IBaseAsyncRepo<Box>  baseAsyncBoxRepo, 
                             IBaseAsyncRepo<BoxCreator> baseAsyncBoxCreatorRepo,
                             IHttpContextAccessor httpContextAccessor,
                             ILogger<Box> logger,
                             IGetObjectType getObjectType)
        {
            this.baseAsyncBoxRepo = baseAsyncBoxRepo;
            this.baseAsyncBoxCreatorRepo = baseAsyncBoxCreatorRepo;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
            this.getObjectType = getObjectType;
        }

        #endregion


        #region IndexMethod
        // GET: BookController
        //[AllowAnonymous]
        // [Authorize(Roles = "Administrators,WebAppAdmins,WebAppPowerUsers,WebAppEditors,WebAppContributors,WebAppViewers")]
        public async Task<ActionResult> Index(int? id, int? folderid,
                                              string searchBox, string searchCreator, string sortOrder,
                                              DateTime? searchDateFrom, DateTime? searchDateTo,
                                              int? pageNumber)
        {
            
            #region Shared Parameters 
            var viewmodel = new BoxFolderDocIndexData();

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CreatorSortParm"] = sortOrder == "Creator" ? "creator_desc" : "Creator";
      
            ViewData["searchBox"] = searchBox??"";
            ViewData["searchCreator"] = searchCreator??"";
            ViewData["searchDateFrom"] = (searchDateFrom ==null) ?"" : searchDateFrom.Value.ToString("yyyy-MM-dd");
            ViewData["searchDateTo"] =  (searchDateTo==null) ? "" : searchDateTo.Value.ToString("yyyy-MM-dd");

            ViewData["BoxID"] = (id==null)?0:id.Value;            

            var model = await baseAsyncBoxRepo.GetAllAsync(includeproperty: source => source
                                         .Include(bc => bc.BoxCreator)
                                         .Include(fl => fl.Folders)
                                           .ThenInclude(ct => ct.CustData)
                                            .ThenInclude(hl => hl.PrjHelixes1)
                                            .Include(fl => fl.Folders)
                                           .ThenInclude(ct => ct.CustData)
                                            .ThenInclude(hl => hl.PrjVelocities1)
                                            .Include(fl => fl.Folders)
                                           .ThenInclude(ct => ct.CustData)
                                            .ThenInclude(hl => hl.PrjVelocities2)                                            
                                            );
            
            
            
            viewmodel.PageSize = 5;
            #endregion

            #region SearchFunctionality

            #region Search By Box OR Creator whithout Date restriction
            if ((searchDateFrom == null && searchDateTo == null) && 
                    (
                        (!String.IsNullOrEmpty(searchBox) && String.IsNullOrEmpty(searchCreator)) ||
                        (String.IsNullOrEmpty(searchBox) && !String.IsNullOrEmpty(searchCreator))
                    )
                )
            {
                var pvm = model;

                if (String.IsNullOrEmpty(searchCreator)) {
                     pvm = model.Where(b => b.BoxDescription.Contains(searchBox));               
                }
                if (String.IsNullOrEmpty(searchBox))
                {
                     pvm = model.Where(b => b.BoxCreator.CreatorName.Contains(searchCreator));
                 }

                viewmodel.Boxes = pvm;
                viewmodel.CountedBoxes = viewmodel.Boxes.Count();
                viewmodel.CurrentPage = pageNumber ?? 1;
                viewmodel.TotalPages = (int)Math.Ceiling(viewmodel.CountedBoxes / (double)viewmodel.PageSize);

                viewmodel.Boxes = pvm.Skip((viewmodel.CurrentPage - 1) * viewmodel.PageSize).Take(viewmodel.PageSize);

                if (id != null)
                {
                    ViewData["BoxDescription"] = model.Where(b => b.ID == id)
                                    .Select(d => d.BoxDescription).FirstOrDefault().ToString();

                    var alt = pvm.Where(b => b.ID == id).Single().Folders;
                    if (alt.Any())
                    {
                        viewmodel.Folders = pvm
                                     .Where(b => b.ID == id)
                                     .Single().Folders;
                    }
                }

                return View(viewmodel);
            }
            #endregion

            #region Search By Box AND Creator whithout Date restriction
            if (
                    (searchDateFrom == null && searchDateTo == null) &&
                    (
                        !String.IsNullOrEmpty(searchBox) && !String.IsNullOrEmpty(searchCreator))
                    )
            {
                var pvm = model;
                
                pvm = model.Where(b => b.BoxDescription.Contains(searchBox) && b.BoxCreator.CreatorName.Contains(searchCreator));                

                viewmodel.Boxes = pvm;
                viewmodel.CountedBoxes = viewmodel.Boxes.Count();
                viewmodel.CurrentPage = pageNumber ?? 1;
                viewmodel.TotalPages = (int)Math.Ceiling(viewmodel.CountedBoxes / (double)viewmodel.PageSize);
                viewmodel.Boxes = pvm.Skip((viewmodel.CurrentPage - 1) * viewmodel.PageSize).Take(viewmodel.PageSize);

                if (id != null)
                {
                    ViewData["BoxDescription"] = model.Where(b => b.ID == id)
                                    .Select(d => d.BoxDescription).FirstOrDefault().ToString();

                    var alt = pvm.Where(b => b.ID == id).Single().Folders;
                    if (alt.Any())
                    {
                        viewmodel.Folders = pvm
                                     .Where(b => b.ID == id)
                                     .Single().Folders;
                    }
                }

                return View(viewmodel);
            }
            #endregion

            #region Search By Creator in specific Date
            if ( !String.IsNullOrEmpty(searchCreator) &&
                (searchDateFrom!=null && searchDateTo==null) ||
                (searchDateFrom == null && searchDateTo != null) //maybe not needed
                ) 
            {
                var pvm = model;
         
                pvm = model.Where(b => b.DateBoxCreated.Date.Equals(searchDateFrom) 
                                    && b.BoxCreator.CreatorName.Contains(searchCreator));
              
                viewmodel.Boxes = pvm;
                viewmodel.CountedBoxes = viewmodel.Boxes.Count();
                viewmodel.CurrentPage = pageNumber ?? 1;
          
                viewmodel.TotalPages = (int)Math.Ceiling(viewmodel.CountedBoxes / (double)viewmodel.PageSize);
                viewmodel.Boxes = pvm.Skip((viewmodel.CurrentPage - 1) * viewmodel.PageSize).Take(viewmodel.PageSize);

                if (id != null)
                {
                    ViewData["BoxDescription"] = model.Where(b => b.ID == id)
                                    .Select(d => d.BoxDescription).FirstOrDefault().ToString();

                    var alt = pvm.Where(b => b.ID == id).Single().Folders;
                    if (alt.Any())
                    {
                        viewmodel.Folders = pvm
                                     .Where(b => b.ID == id)
                                     .Single().Folders;
                    }
                }

                return View(viewmodel);
            }
            #endregion

            #region Search in specific Date
            if (    String.IsNullOrEmpty(searchCreator) && 
                   (searchDateFrom != null && searchDateTo == null)           
               )
            {
                var pvm = model;
                            
                pvm = model.Where(b => b.DateBoxCreated.Date.Equals(searchDateFrom))
                                    .OrderBy(b=>b.DateBoxCreated);                

                viewmodel.Boxes = pvm;
                viewmodel.CountedBoxes = viewmodel.Boxes.Count();
                viewmodel.CurrentPage = pageNumber ?? 1;
                
                viewmodel.TotalPages = (int)Math.Ceiling(viewmodel.CountedBoxes / (double)viewmodel.PageSize);
                viewmodel.Boxes = pvm.Skip((viewmodel.CurrentPage - 1) * viewmodel.PageSize).Take(viewmodel.PageSize);

                if (id != null)
                {
                    ViewData["BoxDescription"] = model.Where(b => b.ID == id)
                                    .Select(d => d.BoxDescription).FirstOrDefault().ToString();

                    var alt= pvm.Where(b => b.ID == id).Single().Folders;
                    if (alt.Any())
                    {
                        viewmodel.Folders = pvm
                                     .Where(b => b.ID == id)
                                     .Single().Folders;
                    }                 
                }

                return View(viewmodel);              
            }
            #endregion

            #region Search By Creator in a range of Dates
            if (
                 !String.IsNullOrEmpty(searchCreator) &&
                 (searchDateFrom != null && searchDateTo!=null)
               )
            {
                var pvm = model;

                pvm = model.Where(b => (b.DateBoxCreated.Date >=searchDateFrom 
                                            && b.DateBoxCreated.Date <= searchDateTo)
                                        && b.BoxCreator.CreatorName.Contains(searchCreator))
                                        .OrderBy(b=>b.DateBoxCreated);

                viewmodel.Boxes = pvm;
                viewmodel.CountedBoxes = viewmodel.Boxes.Count();
                viewmodel.CurrentPage = pageNumber ?? 1;
                //pagesize
                viewmodel.TotalPages = (int)Math.Ceiling(viewmodel.CountedBoxes / (double)viewmodel.PageSize);
                viewmodel.Boxes = pvm.Skip((viewmodel.CurrentPage - 1) * viewmodel.PageSize).Take(viewmodel.PageSize);

                if (id != null)
                {
                    ViewData["BoxDescription"] = model.Where(b => b.ID == id)
                                    .Select(d => d.BoxDescription).FirstOrDefault().ToString();

                    var alt = pvm.Where(b => b.ID == id).Single().Folders;
                    if (alt.Any())
                    {
                        viewmodel.Folders = pvm
                                     .Where(b => b.ID == id)
                                     .Single().Folders;
                    }
                }

                return View(viewmodel);
            }
            #endregion

            #region Search By a range of Dates
            if (
                 String.IsNullOrEmpty(searchCreator) &&
                 (searchDateFrom != null && searchDateTo != null)
               )
            {
                var pvm = model;

                pvm = model.Where(b => (b.DateBoxCreated.Date >= searchDateFrom
                                            && b.DateBoxCreated.Date <= searchDateTo))
                                        .OrderBy(b => b.DateBoxCreated);
 
                viewmodel.Boxes = pvm;

                viewmodel.CountedBoxes = viewmodel.Boxes.Count();
                viewmodel.CurrentPage = pageNumber ?? 1;
                //pagesize
                viewmodel.TotalPages = (int)Math.Ceiling(viewmodel.CountedBoxes / (double)viewmodel.PageSize);
                viewmodel.Boxes = pvm.Skip((viewmodel.CurrentPage - 1) * viewmodel.PageSize).Take(viewmodel.PageSize);

                if (id != null)
                {
                    ViewData["BoxDescription"] = model.Where(b => b.ID == id).Select(d => d.BoxDescription).FirstOrDefault().ToString();
                    viewmodel.Folders = pvm
                                        .Where(b => b.ID == id)
                                        .Single().Folders;
                }

                    return View(viewmodel);
            }
            #endregion

            #endregion

            #region SortFunctionality
            if (!String.IsNullOrEmpty(sortOrder)) 
            {                               
                switch (sortOrder)
                {
                    case "name_desc":
                        viewmodel.Boxes = await baseAsyncBoxRepo.GetAllAsync(includeproperty: source => source
                                           .OrderByDescending(b => b.BoxDescription)
                                           .Include(bc => bc.BoxCreator)
                                           .Include(fl => fl.Folders)
                                             .ThenInclude(ct => ct.CustData)
                                              .ThenInclude(hl => hl.PrjHelixes1)
                                              .Include(fl => fl.Folders)
                                             .ThenInclude(ct => ct.CustData)
                                              .ThenInclude(hl => hl.PrjVelocities1)
                                              .Include(fl => fl.Folders)
                                             .ThenInclude(ct => ct.CustData)
                                              .ThenInclude(hl => hl.PrjVelocities2)
                                              );
                        break;
                    case "Date":
                        viewmodel.Boxes = await baseAsyncBoxRepo.GetAllAsync(includeproperty: source => source
                                                .OrderBy(b => b.DateBoxCreated)
                                                .Include(bc => bc.BoxCreator)
                                                .Include(fl => fl.Folders)
                                                  .ThenInclude(ct => ct.CustData)
                                                   .ThenInclude(hl => hl.PrjHelixes1)
                                                   .Include(fl => fl.Folders)
                                                  .ThenInclude(ct => ct.CustData)
                                                   .ThenInclude(hl => hl.PrjVelocities1)
                                                   .Include(fl => fl.Folders)
                                                  .ThenInclude(ct => ct.CustData)
                                                   .ThenInclude(hl => hl.PrjVelocities2)
                                                   );
                        break;
                    case "date_desc":
                        viewmodel.Boxes = await baseAsyncBoxRepo.GetAllAsync(includeproperty: source => source
                                                .OrderByDescending(b => b.DateBoxCreated)
                                                .Include(bc => bc.BoxCreator)
                                                .Include(fl => fl.Folders)
                                                  .ThenInclude(ct => ct.CustData)
                                                   .ThenInclude(hl => hl.PrjHelixes1)
                                                   .Include(fl => fl.Folders)
                                                  .ThenInclude(ct => ct.CustData)
                                                   .ThenInclude(hl => hl.PrjVelocities1)
                                                   .Include(fl => fl.Folders)
                                                  .ThenInclude(ct => ct.CustData)
                                                   .ThenInclude(hl => hl.PrjVelocities2)
                                                   );
                        break;
                    case "Creator":
                        viewmodel.Boxes = await baseAsyncBoxRepo.GetAllAsync(includeproperty: source => source
                                               .OrderBy(b => b.BoxCreator)
                                               .Include(bc => bc.BoxCreator)
                                               .Include(fl => fl.Folders)
                                                 .ThenInclude(ct => ct.CustData)
                                                  .ThenInclude(hl => hl.PrjHelixes1)
                                                  .Include(fl => fl.Folders)
                                                 .ThenInclude(ct => ct.CustData)
                                                  .ThenInclude(hl => hl.PrjVelocities1)
                                                  .Include(fl => fl.Folders)
                                                 .ThenInclude(ct => ct.CustData)
                                                  .ThenInclude(hl => hl.PrjVelocities2)
                                                  );
                        break;
                    case "creator_desc":
                        viewmodel.Boxes = await baseAsyncBoxRepo.GetAllAsync(includeproperty: source => source
                                           .OrderByDescending(b => b.BoxCreator)
                                           .Include(bc => bc.BoxCreator)
                                           .Include(fl => fl.Folders)
                                             .ThenInclude(ct => ct.CustData)
                                              .ThenInclude(hl => hl.PrjHelixes1)
                                              .Include(fl => fl.Folders)
                                             .ThenInclude(ct => ct.CustData)
                                              .ThenInclude(hl => hl.PrjVelocities1)
                                              .Include(fl => fl.Folders)
                                             .ThenInclude(ct => ct.CustData)
                                              .ThenInclude(hl => hl.PrjVelocities2)
                                              );
                        break;
                    default:
                        viewmodel.Boxes = await baseAsyncBoxRepo.GetAllAsync(includeproperty: source => source
                                         .Include(bc => bc.BoxCreator)
                                         .Include(fl => fl.Folders)
                                           .ThenInclude(ct => ct.CustData)
                                            .ThenInclude(hl => hl.PrjHelixes1)
                                            .Include(fl => fl.Folders)
                                           .ThenInclude(ct => ct.CustData)
                                            .ThenInclude(hl => hl.PrjVelocities1)
                                            .Include(fl => fl.Folders)
                                           .ThenInclude(ct => ct.CustData)
                                            .ThenInclude(hl => hl.PrjVelocities2)
                                            );
                        break;
                }
                return View(viewmodel);
            }
            #endregion

            if (id != null)
            {
                ViewData["BoxID"] = id.Value;
                ViewData["BoxDescription"] = model.Where(b => b.ID == id).Select(d => d.BoxDescription).FirstOrDefault().ToString();

                viewmodel.Boxes = model;

                var alt = model.Where(b => b.ID == id).Single().Folders;
                if (alt.Any())
                {
                    viewmodel.Folders = model
                                    .Where(b => b.ID == id)
                                    .Single().Folders;
                }
            }

            if (folderid !=null)
            {
                ViewData["FolderID"] = folderid.Value;

            }

            var pvmo = model.OrderBy(d=>d.DateBoxCreated);

            viewmodel.Boxes = pvmo;
            viewmodel.CountedBoxes = viewmodel.Boxes.Count();
            viewmodel.CurrentPage = pageNumber ?? 1;
            viewmodel.PageSize = 5;
            viewmodel.TotalPages = (int)Math.Ceiling(viewmodel.CountedBoxes / (double)viewmodel.PageSize);

            viewmodel.Boxes = pvmo.Skip((viewmodel.CurrentPage - 1) * viewmodel.PageSize).Take(viewmodel.PageSize);
          

            return View(viewmodel);
        }
        #endregion

        #region DetailsMethod

        // GET: BookController/Details/5
        //[AllowAnonymous]
        [Authorize(Roles = "Administrators,WebAppAdmins,WebAppPowerUsers,WebAppEditors,WebAppContributors")]

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["ResourceName"] = getObjectType.GetObjectName<Box>();
                return RedirectToAction("StatusCodeErrorHandler", "Error", new { statusCode = 452 });
            }

            var query = await baseAsyncBoxRepo.GetByConditionAsync(filter: b => b.ID == id, includeProperties: "Folders");
            if (query == null)
            {
                TempData["ResourceName"] = getObjectType.GetObjectName<Box>();
                TempData["ResourceId"] = id;
                return RedirectToAction("StatusCodeErrorHandler", "Error", new { statusCode = 404 });
            }
            return View(query);
        }
        #endregion

        #region CreateMethod
        // GET: BookController/Create
        //[AllowAnonymous]
        [Authorize(Roles = "Administrators,WebAppAdmins,WebAppPowerUsers,WebAppEditors,WebAppContributors")]
        public async Task<ActionResult> Create()
        {
            Box box = new Box();
            var maxid = (from boc in await baseAsyncBoxRepo.GetAllAsync()
                         select boc.ID).DefaultIfEmpty(1).Max();
            string currentuser = User.Identity.Name.ToString();

            box.BoxDescription = "Box." + maxid.ToString();
            box.DateBoxCreated = DateTime.Now;
            box.BoxCreatedBy = currentuser.Substring(currentuser.LastIndexOf('\\') + 1);
            box.BoxCreatorID = 1;

            PopulateBoxCreatorDropDownList();
            return View(box);
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AllowAnonymous]
        [Authorize(Roles = "Administrators,WebAppAdmins,WebAppPowerUsers,WebAppEditors,WebAppContributors")]
        public async Task<ActionResult> Create([Bind("BoxDescription,DateBoxCreated,BoxCreatedBy,BoxCreatorID")] Box box)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    baseAsyncBoxRepo.Insert(box);
                    await baseAsyncBoxRepo.SaveAsync();
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
            PopulateBoxCreatorDropDownList(box.BoxCreatorID);
            return RedirectToAction("Index");
        }

        #endregion


        #region EditMethod
        // GET: BookController/Edit/5
        //[AllowAnonymous]
        [Authorize(Roles = "Administrators,WebAppAdmins,WebAppPowerUsers,WebAppEditors,WebAppContributors")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            TempData["BoxEditId"] = JsonConvert.SerializeObject(id).ToString();
            var boxToEdit = await baseAsyncBoxRepo.GetByConditionAsync(bx => bx.ID == id);

            if (boxToEdit == null)
            {
                return NotFound();
            }
            PopulateBoxCreatorDropDownList();
            return View(boxToEdit);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AllowAnonymous]
        [Authorize(Roles = "Administrators,WebAppAdmins,WebAppPowerUsers,WebAppEditors,WebAppContributors")]
        public async Task<ActionResult> Edit(int id, [Bind("ID,BoxDescription,DateBoxCreated,BoxCreatedBy,BoxCreatorID")] Box box)
        {
            if (id != box.ID)
            {
                return NotFound();
            }
            try
            {
                baseAsyncBoxRepo.Update(box);
                await baseAsyncBoxRepo.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }

            PopulateBoxCreatorDropDownList(box.BoxCreatorID);
            return View(box);
        }
        #endregion

        #region DeleteMethod
        // GET: BookController/Delete/5
        //[AllowAnonymous]
        [Authorize(Roles = "Administrators,WebAppAdmins,WebAppPowerUsers,WebAppEditors,WebAppContributors")]
        public async Task<ActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boxToDelete = await baseAsyncBoxRepo.GetByConditionAsync(bx => bx.ID == id);

            if (boxToDelete == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(boxToDelete);
        }
       

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AllowAnonymous]
        [Authorize(Roles = "Administrators,WebAppAdmins,WebAppPowerUsers,WebAppEditors,WebAppContributors")]
        public async Task<ActionResult> Delete(int id, Box box)
        {
            if (box == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {

               baseAsyncBoxRepo.Delete(box);
                await baseAsyncBoxRepo.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
        }

        #endregion

        private void  PopulateBoxCreatorDropDownList(object selectedBoxCreator = null)
        {
            var boxCreatorQuery = from d in  baseAsyncBoxCreatorRepo.GetAll()
                                      select d;
             


            ViewBag.BoxCreatorID = new SelectList(boxCreatorQuery.ToList(), "ID", "CreatorName", selectedBoxCreator);
            
        }
    }
}
