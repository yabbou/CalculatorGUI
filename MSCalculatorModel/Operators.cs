using System;

namespace MSCalculatorModel
{
    public class Operators
    {
        private double augend;
        private double addend;

        public string ProcessOperator(Operator op, double augend, double addend)
        {
            this.augend = augend;
            this.addend = addend;

            switch (op)
            {
                case Operator.DIVIDE:
                    return Divide();
                case Operator.TIMES:
                    return Multiply();
                case Operator.MINUS:
                    return Subtract();
                case Operator.PLUS:
                    return Add();
                default:
                    return ThrowOperatorException();
            }
        }

        private string Divide()
        {
            return (addend == 0) ? ErrorMessage() : Convert.ToString(augend / addend);
        }

        private string Multiply()
        {
            return Convert.ToString(augend * addend);
        }

        private string Subtract()
        {
            return Convert.ToString(augend - addend);
        }

        private string Add()
        {
            return Convert.ToString(augend + addend);
        }

        private string ThrowOperatorException()
        {
            var msg = "Operator not supported.";
            throw new Exception(msg);
            return msg;
        }

        public string ErrorMessage()
        {
            return "Not a number";
        }
    }

}
