using Frontend.Singletons;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{

    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public partial class DataChangeController : ControllerBase
    {

        private readonly HttpClient _httpClient;
        private readonly EventsSingleton _eventsSingleton;
        public DataChangeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _eventsSingleton = EventsSingleton.Instance;
        }


        [HttpGet("GetVlanUpdated")]
        public bool GetVlanUpdated()
        {
            try
            {
                _eventsSingleton.InvokeVlanChangeHandler();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        [HttpGet("GetLldpPortUpdated")]
        public bool GetLldpPortUpdated()
        {
            try
            {
                _eventsSingleton.InvokeLldpPortHandler();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
