using System;

namespace MSCalculatorModel
{
    public class ClearFunctions
    {
        public void ProcessClearFunction(ClearFunc cFunc, string numString)
        {
            switch (cFunc)
            {
                case ClearFunc.CE:
                    ClearCurrent();
                    break;
                case ClearFunc.C:
                    ClearAll();
                    break;
                case ClearFunc.DELETE:
                    DeleteLastNumber(numString);
                    break;
                default:
                    ThrowClearException();
                    break;
            }
        }

        private void ClearCurrent()
        {
            Model.ResetAugendString();
        }

        private void ClearAll()
        {
            Model.ResetState();
        }

        private void DeleteLastNumber(string numString)
        {
            string current;
            if ((numString[0].Equals("-") && numString.Length == 2) ||
                (numString.Length <= 1))
            {
                current = "0";
            }
            else
            {
                current = numString.Substring(0, numString.Length - 1);
            }
            Model.SetAugendString(current);
        }

        private void ThrowClearException()
        {
            throw new Exception("Unknown clear command.");
        }

    }
}
