using Microsoft.AspNetCore.Mvc;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Payment;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.API.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        [Route("vnpay/create-payment")]
        public IActionResult CreatePaymentUrl(PaymentInformationDto model)
        {
            var url = _paymentService.CreatePaymentUrl(model, HttpContext);

            var responseDto = new ResponseDto
            {
                StatusCode = StatusCodes.Status200OK,
                Result = new { PaymentUrl = url },  
                Message = "Payment url created successfully"
            };

            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet]
        [Route("vnpay/get-payment")]
        public IActionResult PaymentCallback()
        {
            var response = _paymentService.PaymentExecute(Request.Query);

            var responseDto = new ResponseDto
            {
                StatusCode = StatusCodes.Status200OK,
                Result = response,
                Message = "Get created payment infomation successfully"
            };
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}
