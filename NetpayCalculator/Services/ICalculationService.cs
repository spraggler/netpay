using NetpayCalculator.Models;

namespace NetpayCalculator.Services
{
    public interface ICalculationService
    {
        CalculateResponse CalculateNetpay(string frequency, decimal gross);
    }
}
