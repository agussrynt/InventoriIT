using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using InventoryIT.Areas.Identity;
using InventoryIT.Areas.Master.Interface;
using InventoryIT.Areas.Master.Models;
using InventoryIT.Areas.Master.Service;
using InventoryIT.Data;
using InventoryIT.Models;
using System.Drawing;
using System.Linq;

namespace InventoryIT.Areas.Master.Controllers
{
	[Authorize(Roles = "Admin")]
	[Area("Master")]
	[Route("master/project-revenue")]
	public class ProjectRevenue : Controller
	{
		private readonly PlanCorpDbContext _context;
		private readonly IProjectService _projectService;

		public ProjectRevenue(PlanCorpDbContext context, IProjectService projectService)
		{
			_context = context;
			_projectService = projectService;
		}

		[AllowAnonymous]
		[HttpPost]
		[Route("get-project-list")]
		public JsonResult GetDataProjects()
		{
			try
			{
				var list = _projectService.GetAllProject();

				return Json(new
				{
					Success = true,
					Data = list.Result.Data
				});
			}
			catch (Exception ex)
			{
				// Info
				Console.Write(ex);

				return Json(new
				{
					Success = false,
					Message = ex.InnerException
				});
			}
		}

		[AllowAnonymous]
		[HttpPost]
		[Route("get-project-detail")]
		public JsonResult GetDataProjectsByID(int idProject)
		{
			try
			{
				var list = _projectService.GetProjectsByID(idProject);

				return Json(new
				{
					Success = true,
					Data = list.Result.Data
				});
			}
			catch (Exception ex)
			{
				// Info
				Console.Write(ex);

				return Json(new
				{
					Success = false,
					Message = ex.InnerException
				});
			}
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		[Route("getDetail")]
		public IActionResult Display()
		{
			return View();
		}

		[HttpGet]
		[Route("get-segmenDD-ajax")]
		public JsonResult GetSegmenDD()
		{
			try
			{
				var list = _projectService.GetSegmenProject();

				return Json(new
				{
					Success = true,
					Data = list
				});
			}
			catch (Exception ex)
			{
				// Info
				Console.Write(ex);

				return Json(new
				{
					Success = false,
					Message = ex.InnerException
				});
			}
		}

		[HttpGet]
		[Route("get-assetDD-ajax")]
		public JsonResult GetAssetDD()
		{
			try
			{
				var list = _projectService.GetAssetProject();

				return Json(new
				{
					Success = true,
					Data = list
				});
			}
			catch (Exception ex)
			{
				// Info
				Console.Write(ex);

				return Json(new
				{
					Success = false,
					Message = ex.InnerException
				});
			}
		}

		[HttpGet]
		[Route("get-CostCenterFD-ajax")]
		public JsonResult GetCostCenterFD(int IDCC)
		{
			try
			{
				var list = _projectService.GetAssetProject().Where(x => x.ID == IDCC);

				return Json(new
				{
					Success = true,
					Data = list
				});
			}
			catch (Exception ex)
			{
				// Info
				Console.Write(ex);

				return Json(new
				{
					Success = false,
					Message = ex.InnerException
				});
			}
		}

		[HttpGet]
		[Route("get-customerDD-ajax")]
		public JsonResult GetCustomerDD()
		{
			try
			{
				var list = _projectService.GetCustomerProject();

				return Json(new
				{
					Success = true,
					Data = list
				});
			}
			catch (Exception ex)
			{
				// Info
				Console.Write(ex);

				return Json(new
				{
					Success = false,
					Message = ex.InnerException
				});
			}
		}

		[HttpGet]
		[Route("get-Regional-ajax")]
		public JsonResult GetRegional(int IDCust)
		{
			try
			{
				var list = _projectService.GetCustomerProject().Where(x => x.ID == IDCust);

				return Json(new
				{
					Success = true,
					Data = list
				});
			}
			catch (Exception ex)
			{
				// Info
				Console.Write(ex);

				return Json(new
				{
					Success = false,
					Message = ex.InnerException
				});
			}
		}

		[HttpGet]
		[Route("get-pekerjaanDD-ajax")]
		public JsonResult GetPekerjaanDD()
		{
			try
			{
				var list = _projectService.GetPekerjaanProject();

				return Json(new
				{
					Success = true,
					Data = list
				});
			}
			catch (Exception ex)
			{
				// Info
				Console.Write(ex);

				return Json(new
				{
					Success = false,
					Message = ex.InnerException
				});
			}
		}

		[HttpGet]
		[Route("get-contractTypeDD-ajax")]
		public JsonResult GetContractTypeDD()
		{
			try
			{
				var list = _projectService.GetContractTypeProject();

				return Json(new
				{
					Success = true,
					Data = list
				});
			}
			catch (Exception ex)
			{
				// Info
				Console.Write(ex);

				return Json(new
				{
					Success = false,
					Message = ex.InnerException
				});
			}
		}

		[HttpGet]
		[Route("get-SBTDD-ajax")]
		public JsonResult GetSBTDD()
		{
			try
			{
				var list = _projectService.GetSBTProject();

				return Json(new
				{
					Success = true,
					Data = list
				});
			}
			catch (Exception ex)
			{
				// Info
				Console.Write(ex);

				return Json(new
				{
					Success = false,
					Message = ex.InnerException
				});
			}
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[Route("create-project-ajax")]
		public JsonResult CreateProject(Projects param)
		{
			try
			{
				param.CreatedBy = HttpContext.Session.GetString("username");
				var r = _projectService.SaveOrUpdate(param);
				return Json(new
				{
					Success = r.Success,
					Message = r.Message
				});
			}
			catch (Exception ex)
			{
				return Json(new
				{
					Success = false,
					Message = ex.Message
				});
			}
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[Route("update-project-ajax")]
		public JsonResult UpdateProject(Projects param)
		{
			try
			{
				var r = _projectService.SaveOrUpdate(param);
				return Json(new
				{
					Success = r.Success,
					Message = r.Message
				});
			}
			catch (Exception ex)
			{
				return Json(new
				{
					Success = false,
					Message = ex.Message
				});
			}
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[Route("delete-project-ajax")]
		public JsonResult DeleteDataProject(int idProject)
		{
			try
			{
				var r = _projectService.DeleteProject(idProject);
				return Json(new
				{
					Success = r.Success,
					Message = r.Message
				});
			}
			catch (Exception ex)
			{
				return Json(new
				{
					Success = false,
					Message = ex.Message
				});
			}
		}
	}
}