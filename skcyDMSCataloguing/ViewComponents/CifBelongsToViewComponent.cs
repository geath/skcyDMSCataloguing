using Microsoft.AspNetCore.Mvc;
using skcyDMSCataloguing.Models;
using skcyDMSCataloguing.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace skcyDMSCataloguing.ViewComponents
{
    public class CifBelongsToViewComponent : ViewComponent
    {
        private readonly IBaseAsyncRepo<CustData> _custData;
        private readonly IBaseAsyncRepo<PrjVelocity1> _veloc1;
        private readonly IBaseAsyncRepo<PrjVelocity2> _veloc2;
        private readonly IBaseAsyncRepo<PrjHelix1> _helix1;


        public CifBelongsToViewComponent(IBaseAsyncRepo<CustData> custData,
                                         IBaseAsyncRepo<PrjVelocity1> veloc1,
                                         IBaseAsyncRepo<PrjVelocity2> veloc2,
                                         IBaseAsyncRepo<PrjHelix1> helix1
                                         )
        {
            _custData = custData;
            _veloc1 = veloc1;
            _veloc2 = veloc2;
            _helix1 = helix1;
        }



    }
}
