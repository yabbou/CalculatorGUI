using System;

namespace MSCalculatorModel
{
    public class MathFunctions
    {
        public string ProcessMathFunction(MathFunc func, double x, double y)
        {
            switch (func)
            {
                case MathFunc.PERCENT:
                    return CalculatePercent(x, y);
                case MathFunc.SQRT:
                    return CalculateRoot(x);
                case MathFunc.EXP:
                    int exp = 2;
                    return CalculateExponent(x, exp);
                case MathFunc.TO_DENOM:
                    return CalculateToDenominator(x);
                case MathFunc.NEGATE:
                    return Negate(x);
                default:
                    return ThrowMathException();
            }
        }
        private string CalculatePercent(double x, double y)
        {
            return Convert.ToString(x / y);
        }
        private string CalculateRoot(double x)
        {
            return (x > 0) ? Math.Sqrt(x).ToString() :
                ErrorMessage();
        }

        public string ErrorMessage()
        {
            return "Invalid input";
        }

        private string CalculateExponent(double x, int exp)
        {
            return Math.Pow(x, exp).ToString();
        }
        private string CalculateToDenominator(double x)
        {
            return Convert.ToString(1 / x);
        }
        private string Negate(double x)
        {
            return Convert.ToString(-x);
        }
        private string ThrowMathException()
        {
            string msg = "Unknown math function";
            throw new Exception(msg);
            return msg;
        }

        /* formatted - sqrt, exp, negate */
        internal string ProcessMathFunctionFormatted(MathFunc func, string x)
        {
            switch (func)
            {
                case MathFunc.SQRT:
                    return CalculateRootFormatted(x);
                case MathFunc.EXP:
                    int exp = 2;
                    return CalculateExponentFormatted(x, exp);
                case MathFunc.NEGATE:
                    return NegateFormatted(x);
                default:
                    return ThrowMathException();
            }
        }

        public string CalculateRootFormatted(string x)
        {
            return $"sqrt({x})";
        }

        public string CalculateExponentFormatted(string x, int exp)
        {
            return $"({x})^{exp}";
        }

        public string NegateFormatted(string x)
        {
            return $"negate({x})";
        }
    }
}
