﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryIT.Areas.Page.Controllers
{
    [Authorize]
    [Area("Page")]
    [Route("page/konsolidasi")]
    public class KonsolidasiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
