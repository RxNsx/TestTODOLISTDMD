using System;

namespace TODOLISTTRY.Services.ExceptionHandlers
{
    public class SetStatusException : Exception
    {
        public SetStatusException() :base(string.Format($"The tasks in sublists cannot be done or subtask can be added to the done terminal task"))
        {

        }
    }
}
