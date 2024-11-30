﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PlanCorp.Areas.Identity;
using PlanCorp.Areas.Master.Interface;
using PlanCorp.Areas.Master.Models;
using PlanCorp.Areas.Master.Service;
using PlanCorp.Data;
using PlanCorp.Models;
using System.Linq;

namespace PlanCorp.Areas.Master.Controllers
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

        public IActionResult Index()
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