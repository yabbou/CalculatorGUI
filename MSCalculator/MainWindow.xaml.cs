using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MSCalculatorModel;

namespace MSCalculator
{
    public partial class MainWindow : Window
    {
        #region vars
        private Model model = new Model();

        private List<Button> memoryFunctions;
        private List<Button> mathFunctions;
        private List<Button> clearFunctions;

        private List<Button> numbers;
        private List<Button> operators;

        private Button prevOperationButton;
        private string prevOpString;
        private readonly string equalsOpString;

        private Button prevButton;

        private Button history;
        private List<string> historyListLocal;


        public MainWindow()
        {
            InitializeComponent();
            InitCalculatorButtons();

            InitText();
            equalsOpString = bEquals.Content.ToString();
        }

        private void InitCalculatorButtons()
        {
            memoryFunctions = new List<Button> { bMemoryClear, bMemoryAdd, bMemorySubtract, bMemoryReturn, bMemoryState };
            mathFunctions = new List<Button> { bPercent, bSqrt, bExp, bToDenominator, bNegate };
            clearFunctions = new List<Button> { bClearAll, bClear, bDelete };

            numbers = new List<Button> { b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, bDecimal };
            operators = new List<Button> { bDivide, bTimes, bMinus, bPlus, bEquals };

            history = bHistory;
            prevOperationButton = bEquals;
            historyListLocal = new List<string>();

            tDisplayNumber.IsEnabled = false;
            tCurrentEquation.IsEnabled = false; //dry
        }
        internal void InitText()
        {
            prevOpString = equalsOpString;
            tDisplayNumber.Text = "0";
        }

        #endregion

        #region click operations
        private void BNumber_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if (b == null)
            {
                return;
            }

            string numberPressed = b.Content.ToString();
            CheckToOverWriteAugendString();

            model.AddDigitsToAugendString(numberPressed);
            SetPrevButton(b);
            DisplayNumberFormatted(model.GetAugendString());
        }

        private void CheckToOverWriteAugendString()
        {
            if (operators.Contains(prevButton) || memoryFunctions.Contains(prevButton) || mathFunctions.Contains(prevButton))
            {
                Model.ResetAugendString();
                Model.ResetAddend();
            }
        }

        private void DisplayNumberFormatted(string num)
        {
            bool isErrorMessage = char.IsLetter(num[0]);

            if (isErrorMessage) { tDisplayNumber.Text = num; }//decimal exceptoin here
            else
            {
                tDisplayNumber.Text = num.Length > 0 ? string.Format("{0:n0}", double.Parse(num)) : ""; //formqatting that includesdecimals AND commas
            }
        }

        private void BMemory_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if (b == null)
            {
                return;
            }

            double current = model.GetAddend();
            MemoryFunc mFunc = (MemoryFunc)memoryFunctions.IndexOf(b);

            model.ProcessMemoryFunction(mFunc, current);

            if (mFunc == MemoryFunc.MR)
            {
                DisplayNumberFormatted(model.GetAddendString());
            }

            SetPrevButton(b);
        }

        private void BMathFunc_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if (b == null)
            {
                return;
            }

            MathFunc mFunc = (MathFunc)mathFunctions.IndexOf(b);
            string addend = model.ProcessMathFunction(mFunc);
            string addendString = CheckToFormatAddendString(mFunc, addend);

            Model.SetAugendString(addend);
            model.SetMathFuncString(addendString);

            SetPrevButton(b);

            DisplayNumberFormatted(addend);
            BuildCurrentEquation(addendString);
        }

        private string CheckToFormatAddendString(MathFunc mFunc, string addend)
        {
            if (mFunc.Equals(MathFunc.EXP) || mFunc.Equals(MathFunc.SQRT) ||
                (mFunc.Equals(MathFunc.NEGATE) && !model.GetAugendString().Equals("0")))
            {
                return model.ProcessMathFunctionFormatted(mFunc);
            }
            else if (mFunc.Equals(MathFunc.NEGATE) && model.GetAugendString().Equals("0"))
            {
                return "";
            }
            return addend;
        }

        private void BClear_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if (b == null)
            {
                return;
            }

            string numString = tDisplayNumber.Text.ToString();
            ClearFunc cFunc = (ClearFunc)clearFunctions.IndexOf(b);

            if (numString.Length > 0) { model.ProcessClearFunction(cFunc, numString); }
            if (cFunc.Equals(ClearFunc.C)) { ResetCurrentEquation(); ResetPrevOperatorButtonColour(); }

            DisplayNumberFormatted(model.GetAugendString());
        }

        private void BOperator_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if (b == null)
            {
                return;
            }

            Operator op = (Operator)operators.IndexOf(b);
            ResetPrevOperatorButtonColour();

            string prevAugendString = GetAugendStringWithMathFunc();
            var prevAddendString = model.GetAddendString();

            if (op == Operator.EQUALS)
            {
                Operator prevOp = (Operator)operators.IndexOf(prevOperationButton);
                var resultString = model.ProcessOperator(prevOp);
                DisplayNumberFormatted(resultString);

                Operator prev = (Operator)operators.IndexOf(prevButton);
                var optionalAugendAndOp = prev == Operator.EQUALS ? prevAddendString + prevOpString : "";
                BuildCurrentEquation(optionalAugendAndOp + prevAugendString + equalsOpString + resultString);

                Model.AddEquationToHistory(tCurrentEquation.Text);
                ResetCurrentEquation();
            }
            else
            {
                b.Background = Brushes.BlanchedAlmond;
                prevOperationButton = b;
                prevOpString = b.Content.ToString();

                if (IsNotEqualsButton())
                {
                    model.AddAugendToAddend();
                    BuildCurrentEquation(prevAugendString + prevOpString);
                }
                else if (IsSwitchingOperator())
                {
                    var current = tCurrentEquation.Text;
                    tCurrentEquation.Text = current.Equals("") ? prevAddendString + prevOpString :
                        current.Substring(0, current.Length - 1) + prevOpString;
                }
            }

            SetPrevButton(b);
        }

        private string GetAugendStringWithMathFunc()
        {
            if (mathFunctions.Contains(prevButton)) return ""; //fix
            else return model.GetAugendString();
        }

        private bool IsNotEqualsButton()
        {
            return !operators.Contains(prevButton);
        }
        private bool IsSwitchingOperator()
        {
            return prevButton != prevOperationButton;
        }


        private void ResetPrevOperatorButtonColour()
        {
            prevOperationButton.Background = Brushes.LightGray;
        }

        private void BuildCurrentEquation(string numOrOpString)
        {
            var equation = tCurrentEquation.Text; //implement with list .add .remove ...

            tCurrentEquation.Text = equation.Equals("0") ?
             numOrOpString : equation + numOrOpString;

            if (mathFunctions.Contains(prevButton))
            {
                equation = tCurrentEquation.Text;
                var mFuncString = model.GetMathFuncString();
                var mFuncLength = mFuncString.Length;
                var mFuncWithinEquation = equation.Substring(equation.Length - mFuncLength);

                if (mFuncString.Equals(mFuncWithinEquation)) { tCurrentEquation.Text = mFuncWithinEquation; }
            }

        }
        private void ResetCurrentEquation()
        {
            tCurrentEquation.Text = "";
        }

        private void BHistory_Click(object sender, RoutedEventArgs e)
        {
            DisplayHistoryPanel();
        }
        private void DisplayHistoryPanel()
        {
            StringBuilder builder = new StringBuilder();
            historyListLocal.AddRange(model.GetHistory().Except(historyListLocal));

            foreach (var equation in historyListLocal)
            {
                builder.AppendLine(equation + "\n");
            }

            MessageBox.Show("History:\n\n" + builder);
        }

        private void SetPrevButton(Button b)
        {
            prevButton = b;
        }

        #endregion
    }
}