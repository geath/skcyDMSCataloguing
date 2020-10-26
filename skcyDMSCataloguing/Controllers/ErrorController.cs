using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using skcyDMSCataloguing.Services;

namespace skcyDMSCataloguing.Controllers
{
    public class ErrorController : Controller
    {
        public ILogger<ErrorController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ErrorController(ILogger<ErrorController> logger,
                                IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [Route("Error/{statusCode}")]
        public IActionResult StatusCodeErrorHandler(int statusCode)
        {
            var errorCodeValue = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {

                case 404:
                    if (TempData["ResourceId"] != null)
                    {
                       
                        _logger.LogWarning(skcyDMSCataloguingEvents.GetItemNotFound,$"ErrorID: {skcyDMSCataloguingEvents.GetItemNotFound}. A 404 error occured by the user {_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value} . resourceid = {TempData["ResourceId"]} " +
                                            $" resourcename : {TempData["ResourceName"]}");
                        ViewBag.ErrorMessage = "Unreachable Resource. If the error persists contact App Admin ";
                        return View("NotFound");
                    }

                    ViewBag.ErrorMessage = "Unreachable Resource or Page. If the error persists contact App Admin ";
                    _logger.LogWarning(skcyDMSCataloguingEvents.PageNotFound,$"ErrorID:{skcyDMSCataloguingEvents.PageNotFound}. A 404 Error Occured by the user {_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value} . Path = {errorCodeValue.OriginalPath}" +
                                         $" Query String : {errorCodeValue.OriginalQueryString}");
                    break;
                case 452:
                    ViewBag.ErrorMessage = "A Resource was requested but no value is assigned to it. " +
                                                "If the error persists contact App Admin ";
                    _logger.LogWarning(skcyDMSCataloguingEvents.GetItemIsNull,$"ErrorID: {skcyDMSCataloguingEvents.GetItemIsNull}. A 452 Error Occured by the user {_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value}. The Resource : {TempData["ResourceName"]} was requested" +
                                            $" with null value");
                    return View("NotFound");
            }
            return View("NotFound");
        }

        [Route("Error")]
        [AllowAnonymous]
        public IActionResult ErrorHandler()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            _logger.LogError($"The path {exceptionDetails.Path} trew an exception {exceptionDetails.Error} ");

            return View("Error");

        }
    }
}
