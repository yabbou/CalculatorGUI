using System;

namespace MSCalculatorModel
{
    public class MemoryFunctions
    {
        private static double memoryStore = 0;

        public void ProcessMemoryFunction(MemoryFunc mFunc, double augend)
        {
            switch (mFunc)
            {
                case MemoryFunc.MC:
                    ClearMemory();
                    break;
                case MemoryFunc.M_ADD:
                    AddToMemory(augend);
                    break;
                case MemoryFunc.M_SUBTRACT:
                    SubtractFromMemory(augend);
                    break;
                case MemoryFunc.MR:
                    ReturnFromMemory();
                    break;
                case MemoryFunc.MS:
                    SaveToMemory(augend);
                    break;
                default:
                    ThrowMemoryException();
                    break;
            }
        }

        private void ClearMemory()
        {
            memoryStore = 0;
        }

        private void AddToMemory(double augend)
        {
            memoryStore += augend;
        }

        private void SubtractFromMemory(double augend)
        {
            memoryStore -= augend;
        }

        private void ReturnFromMemory()
        {
            Model.SetAugendString(memoryStore.ToString()); //make super-method return string, with "" added to display if actally void
        }

        private void SaveToMemory(double augend)
        {
            memoryStore = augend;
        }

        private void ThrowMemoryException()
        {
            throw new Exception("Memory function not supported");
        }
    }

}
