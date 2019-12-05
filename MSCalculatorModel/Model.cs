
using System.Collections.Generic;

namespace MSCalculatorModel
{
    public enum MemoryFunc { MC = 0, M_ADD = 1, M_SUBTRACT = 2, MR = 3, MS = 4 }
    public enum MathFunc { PERCENT = 0, SQRT = 1, EXP = 2, TO_DENOM = 3, NEGATE = 4 }
    public enum ClearFunc { CE = 0, C = 1, DELETE = 2 }
    public enum Operator { DIVIDE = 0, TIMES = 1, MINUS = 2, PLUS = 3, EQUALS = 4 }

    public class Model
    {
        #region local vars

        MemoryFunctions memoryFunctions; //make them static methods
        MathFunctions mathFunctions;
        Operators operators;
        ClearFunctions clearFunctions;
        static List<string> historyList; //listbox?

        private static string AugendString { get; set; }
        private static double Addend { get; set; }
        private string MathFuncString { get; set; }

        public Model()
        {
            memoryFunctions = new MemoryFunctions();
            mathFunctions = new MathFunctions();
            operators = new Operators();
            clearFunctions = new ClearFunctions();
            historyList = new List<string>();

            AugendString = "0";
            MathFuncString = "";
        }

        #endregion

        #region delegating methods 

        public void ProcessMemoryFunction(MemoryFunc mFunc, double augend)
        {
            memoryFunctions.ProcessMemoryFunction(mFunc, augend);
        }
        public string ProcessMathFunction(MathFunc mFunc)
        {
            var addendString = mathFunctions.ProcessMathFunction(mFunc, GetAugend(), Addend);

            Addend = addendString.Equals(mathFunctions.ErrorMessage()) ? //find format to return sci-notatoin
                0 : double.Parse(addendString);

            return addendString;
        }
        public string ProcessMathFunctionFormatted(MathFunc mFunc)
        {
            return mathFunctions.ProcessMathFunctionFormatted(mFunc, GetAugendString()); //should not be getaugendstring, but mfunc PLUS augend string inside
        }

        public void ProcessClearFunction(ClearFunc cFunc, string numString)
        {
            clearFunctions.ProcessClearFunction(cFunc, numString);
        }

        public string ProcessOperator(Operator op)
        {
            var addendString = (op == Operator.EQUALS) ? AugendString : //wahts happening here?
                operators.ProcessOperator(op, Addend, GetAugend());

            Addend = addendString.Equals(operators.ErrorMessage()) ?
                0 : double.Parse(addendString);

            return addendString;
        }

        public static void AddEquationToHistory(string addendString)
        {
            historyList.Add(addendString);
        }

        #endregion

        #region getters & setters

        /* current number */

        public double GetAugend()
        {
            return double.Parse(AugendString);
        }

        public string GetAugendString()
        {
            return AugendString;
        }

        public static void SetAugendString(string numString)
        {
            AugendString = numString;
        }

        public void AddDigitsToAugendString(string digits)
        {
            var current = AugendString;
            AugendString = current.Equals("0") ?//set flag to not allow second digit
                digits : current + digits;
        }

        public void AddAugendToAddend()
        {
            Addend += GetAugend();
        }

        /* addend */

        public double GetAddend() { return Addend; }
        public string GetAddendString()
        {
            return Addend.ToString();
        }

        public static void SetAddend(double addend)
        {
            Addend = addend;
        }

        /* mathFunc */
       
        public string GetMathFuncString() { return MathFuncString; }

        public void SetMathFuncString(string mFuncString)
        {
            MathFuncString = mFuncString;
        }

        /* reset */
        public static void ResetState()
        {
            ResetAugendString();
            ResetAddend();
        }

        public static void ResetAugendString()
        {
            AugendString = "0";
        }

        public static void ResetAddend()
        {
            Addend=0;
        }

        /* history */
        public List<string> GetHistory()
        {
            return historyList;
        }

        #endregion

    }
}