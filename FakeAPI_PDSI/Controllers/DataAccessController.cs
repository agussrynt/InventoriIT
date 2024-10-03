using FakeAPI_PDSI.Model;
using FakeAPI_PDSI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FakeAPI_PDSI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataAccessController : ControllerBase
    {
        private readonly IDataAccessService _service;

        public DataAccessController(IDataAccessService service)
        {
            _service = service;
        }

        [HttpGet]
        public  async Task<ResponseModel> Get()
        {
            ResponseModel response = new ResponseModel();

            response = await _service.Gets();

            return response;
        }
    }
}
