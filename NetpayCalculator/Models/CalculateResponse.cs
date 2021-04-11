namespace NetpayCalculator.Models
{
    public class CalculateResponse
    {
        public decimal Gross { get; set; }
        public decimal Paye { get; set; }
        public decimal Nis { get; set; }
        public decimal NetPay { get; set; }
    }
}
