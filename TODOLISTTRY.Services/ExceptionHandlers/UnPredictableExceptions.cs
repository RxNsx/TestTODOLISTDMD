using System;

namespace TODOLISTTRY.Services.ExceptionHandlers
{
    public class UnPredictableExceptions : Exception
    {
        public UnPredictableExceptions(string methodName) : base(string.Format($"{methodName} - unpredictable exception"))
        {

        }
    }
}
