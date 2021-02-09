using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using skcyDMSCataloguing.DAL;
using skcyDMSCataloguing.DAL.Repositories;
using skcyDMSCataloguing.Models;
using skcyDMSCataloguing.Services;
using skcyDMSCataloguing.ViewModels;

namespace skcyDMSCataloguing.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IBaseAsyncRepo<CustData> baseAsyncCustDataRepo;
        private readonly IBaseAsyncRepo<PrjHelix1> baseAsyncPrjHelix1Repo;
        private readonly IBaseAsyncRepo<PrjVelocity1> baseAsyncPrjVelocity1Repo;
        private readonly IBaseAsyncRepo<PrjVelocity2> baseAsyncPrjVelocity2Repo;

        public CustomerController(
                                   IBaseAsyncRepo<CustData> baseAsyncCustDataRepo,
                                   IBaseAsyncRepo<PrjHelix1> baseAsyncPrjHelix1Repo,
                                   IBaseAsyncRepo<PrjVelocity1> baseAsyncPrjVelocity1Repo,
                                   IBaseAsyncRepo<PrjVelocity2> baseAsyncPrjVelocity2Repo
                                 )
        {
            this.baseAsyncCustDataRepo = baseAsyncCustDataRepo;
            this.baseAsyncPrjHelix1Repo = baseAsyncPrjHelix1Repo;
            this.baseAsyncPrjVelocity1Repo = baseAsyncPrjVelocity1Repo;
            this.baseAsyncPrjVelocity2Repo = baseAsyncPrjVelocity2Repo;
        }
      
        [HttpGet]
        public async Task<IActionResult> GetCIF(string CIFNo)
        {
            var viewmodel = new CifManagedByViewModel();           

            if (CIFNo == null)
            {                
                return NotFound();
            }
             
            viewmodel.CustData = await baseAsyncCustDataRepo.GetByConditionAsync
                  (filter: cst => cst.CIFNo == CIFNo );

            if (viewmodel.CustData == null)
            {
                TempData["NotFound"] = CIFNo + " doesn't exists " ;
                return View("~/Views/Error/NotFound.cshtml"); 
            }

            
            var h1 = viewmodel.PrjHelix1 = await baseAsyncPrjHelix1Repo.GetByConditionAsync
                  (filter: cst => cst.CustDataCIFNo == CIFNo);
            var v1 = viewmodel.PrjVelocity1 = await baseAsyncPrjVelocity1Repo.GetByConditionAsync
                  (filter: cst => cst.CustDataCIFNo == CIFNo);
            var v2 = viewmodel.PrjVelocity2 = await baseAsyncPrjVelocity2Repo.GetByConditionAsync
                  (filter: cst => cst.CustDataCIFNo == CIFNo);


            if (h1 == null)
            {
                viewmodel.IsHelix1 = false;
            }
            else { viewmodel.IsHelix1 = true; }

            if (v1 == null)
            {
                viewmodel.IsVelocity1 = false;
            }
            else { viewmodel.IsVelocity1 = true; }
            if (v2 == null)
            {
                viewmodel.IsVelocity2 = false;
            }
            else { viewmodel.IsVelocity2 = true; }

            return View(viewmodel);
        }
    }
}

//https://localhost:44329/Customer/GetCIF?CIFNo=CCC0C5
//https://localhost:44329/Customer/GetCIF?CIFNo=CCE1A1