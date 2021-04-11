using System;
using Microsoft.AspNetCore.Mvc;
using NetpayCalculator.Models;
using NetpayCalculator.Services;

namespace NetpayCalculator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculateNetPayController : ControllerBase
    {
        private readonly ICalculationService _service;

        public CalculateNetPayController(ICalculationService service)
        {
            _service = service;
        }

        [HttpPost]
        public ActionResult<CalculateResponse> Post([FromBody]CalculateRequest request)
        {
            var response = _service.CalculateNetpay(request.Frequency, request.Gross);
            return response;
        }
    }
}
