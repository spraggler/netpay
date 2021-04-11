using System;
using System.Runtime.InteropServices;
using NetpayCalculator.Models;

namespace NetpayCalculator.Services
{
    public class CalculationService : ICalculationService
    {
        private decimal PayeBandOnePercentage = .125M;
        private decimal PayeBandTwoPercentage = .285M;
        // private double payeBandOneCeiling = 50000;
        public CalculateResponse CalculateNetpay(string frequency, decimal gross)
        {
            var nis = CalculateNis(frequency, gross);
            var paye = CalculatePaye(frequency, gross);

            return new CalculateResponse
            {
                Gross = gross,
                Paye = paye,
                Nis = nis,
                NetPay = gross - (nis + paye)
            };
        }

        private decimal CalculatePaye(string frequency, decimal gross)
        {
            // These values should ideally be stored and loaded from a database.
            // Will refactor to also correctly calculate regardless to the number of tax bands

            var result = 0.0M;

            if (!string.IsNullOrEmpty(frequency))
            {
                var allowance = 0M;
                var bandOneCeiling = 0M;

                switch (frequency)
                {
                    case "Monthly":
                        allowance = 2083.33M;
                        bandOneCeiling = 4166.66M;
                        break;
                    case "Weekly":
                        allowance = 480.77M;
                        bandOneCeiling = 961.54M;
                        break;
                    default:
                        allowance = 2083.33M;
                        bandOneCeiling = 4166.66M;
                        break;
                }

                var taxableGross = gross - allowance;
                if (taxableGross < 0)
                {
                    result = 0M;
                }

                if (taxableGross < bandOneCeiling && taxableGross > 0)
                {
                    result = taxableGross * PayeBandOnePercentage;
                }

                if (taxableGross > bandOneCeiling)
                {
                    var bandOneTax = allowance * PayeBandOnePercentage;
                    var bandTwoTax = (taxableGross - allowance) * PayeBandTwoPercentage;
                    result = bandOneTax + bandTwoTax;
                }
            }
            return decimal.Round(result,2);
        }

        private decimal CalculateNis(string frequency, decimal gross)
        {
            var result = 0M;

            if (!string.IsNullOrEmpty(frequency))
            {
                var allowance = 0M;
                var nisCeiling = 0M;
                var nisFloor = 0M;
                var nisEmployeeRate = .111M;

                switch (frequency)
                {
                    case "Monthly":
                        nisCeiling = 4880M;
                        nisFloor = 91M;
                        break;
                    case "Weekly":
                        nisCeiling = 1126M;
                        nisFloor = 21M;
                        break;
                    default:
                        nisCeiling = 4880M;
                        nisFloor = 90M;
                        break;
                }

                var taxableGross = gross;
                if (taxableGross < nisFloor)
                {
                    result = 0M;
                }

                if (taxableGross > nisCeiling)
                {
                    result = nisCeiling * nisEmployeeRate;
                }

                if (taxableGross >= nisFloor && taxableGross <= nisCeiling)
                {
                    result = taxableGross * nisEmployeeRate;
                }
            }

            return decimal.Round(result, 2);
        }
    }
}
