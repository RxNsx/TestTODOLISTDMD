using System;

namespace TODOLISTTRY.Services.ExceptionHandlers
{
    public class DoNotFoundException : Exception
    {
        public DoNotFoundException(string methodName) : base(string.Format($"{methodName} cannot find a Do object"))
        {

        }
    }
}
