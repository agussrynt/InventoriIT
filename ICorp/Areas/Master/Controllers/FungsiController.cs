using InventoryIT.Areas.Master.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryIT.Areas.Master.Controllers
{
    [Area("Master")]
    [Route("master/fungsi")]
    public class FungsiController : Controller
    {
        private readonly IFungsiService _fungsiService;

        public FungsiController(IFungsiService fungsiService)
        {
            _fungsiService = fungsiService;
        }

        [HttpGet]
        [Route("get-list-ajax")]
        public JsonResult GetListAjax()
        {
            try
            {
                var list = _fungsiService.Gets();

                return Json(new
                {
                    Success = true,
                    Data = list,
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = ex.Message,
                });
            }
        }

        [HttpGet]
        [Route("get-item-ajax")]
        public JsonResult GetItemAjax(string id)
        {
            try
            {
                var list = _fungsiService.Get(id);

                return Json(new
                {
                    Success = true,
                    Data = list,
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = ex.Message,
                });
            }
        }
    }
}
