using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket_Hub.Models.DTO.Payment;

namespace Ticket_Hub.Services.IServices;

public interface IPaymentService
{
    string CreatePaymentUrl(PaymentInformationDto model, HttpContext context);
    PaymentResponseDto PaymentExecute(IQueryCollection collections);
}
