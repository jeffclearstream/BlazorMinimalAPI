using BlazorMinimalApis.Data;
using BlazorMinimalApis.Endpoints.Pages.Contacts;
using BlazorMinimalApis.Lib.Routing;

namespace BlazorMinimalApis.Endpoints.Pages
{
    public class DatasetHandler : PageHandler
    {
        public IResult Edit(int id)
        {
            return Page<DatasetDetails>(id);
        }

        public IResult Login()
        {
            return Page<Login>();
        }
    }
}
