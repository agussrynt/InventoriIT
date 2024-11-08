﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlanCorp.Areas.Page.Controllers
{
	[Authorize]
	[Area("Page")]
	[Route("page/rev-hpp-ga")]
	public class RevHPPGAController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
