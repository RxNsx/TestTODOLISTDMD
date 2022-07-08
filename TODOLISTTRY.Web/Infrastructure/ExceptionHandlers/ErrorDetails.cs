using Newtonsoft.Json;

namespace TODOLISTTRY.Web.Infrastructure.ExceptionHandlers
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string ExMessage { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
