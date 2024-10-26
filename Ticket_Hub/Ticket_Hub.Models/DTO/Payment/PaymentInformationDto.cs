namespace Ticket_Hub.Models.DTO.Payment
{
    public class PaymentInformationDto
    {
        public string CardId { get; set; }
        public string OrderType { get; set; }
        public string OrderDescription { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
    }
}
