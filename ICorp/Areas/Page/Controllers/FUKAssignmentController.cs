using PlanCorp.Areas.Master.Interface;
using PlanCorp.Areas.Page.Interfaces;
using PlanCorp.Areas.Page.Models;
using PlanCorp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlanCorp.Areas.Page.Controllers
{
    [Authorize]
    [Area("Page")]
    [Route("page/fuk-assignment")]
    public class FUKAssignmentController : Controller
    {
        private readonly IFUKAssignmentService _fUKAssignmentService;
        private readonly IMailService _mail;

        public FUKAssignmentController(IFUKAssignmentService fUKAssignmentService, IMailService mail)
        {
            _fUKAssignmentService = fUKAssignmentService;
            _mail = mail;
        }

        [Route("")]
        public IActionResult List()
        {
            return View();
        }

        [Route("get-list")]
        public JsonResult GetListFUKAjax(int ParameterId)
        {
            try
            {
                var list = _fUKAssignmentService.GetList(ParameterId);
                return Json(new
                {
                    Success = true,
                    Data = list
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
        [Route("set-ajax")]
        public async Task<JsonResult> AssignmentFUKAjax([FromBody] FUKAssignment fUKAssignment)
        {
            string Message = string.Empty;
            try
            {
                fUKAssignment.CreatedBy = User.Identity.Name;
                var _params = _fUKAssignmentService.SetAssignment(fUKAssignment);
                if (_params.Success)
                {
                    Message = "Successfully set assignment!";

                    List<MailAccModel> acc = _mail.GetEmailAcc(fUKAssignment.PIC);
                    if (acc.Count > 0)
                    {
                        MailModel mail = new MailModel();
                        mail.body = "Dear " + acc.First().Nama + " Anda mendapatkan assigment FUK";
                        mail.recipient = acc.First().Email;
                        mail.subject = "FUK Assigment";
                        MailResultModel r = await _mail.SendMail(mail);
                    }
                }
                return Json(new
                {
                    Success = _params.Success,
                    Message = Message
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

        [HttpGet]
        [Route("/detail")]
        public IActionResult Detail()
        {
            return View();
        }
    }
}
