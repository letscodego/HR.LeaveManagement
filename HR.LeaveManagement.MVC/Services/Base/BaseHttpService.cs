using HR.LeaveManagement.MVC.Contracts;
using System.Net.Http.Headers;

namespace HR.LeaveManagement.MVC.Services.Base
{
    public class BaseHttpService
    {
        public BaseHttpService(IClient client, ILocalStorageService localStorageService)
        {
            Client = client;
            LocalStorageService = localStorageService;
        }

        public IClient Client { get; }
        public ILocalStorageService LocalStorageService { get; }

        protected Response<Guid> ConvertApiExceptions<Guid>(ApiException apiException)
        {
            if (apiException.StatusCode == 400)
            {
                return new Response<Guid>() { Message = "Validation errors have occured.", ValidationErrors = apiException.Response, Success = false };
            }
            else if (apiException.StatusCode == 404)
            {
                return new Response<Guid>() { Message = "The request item could not be found.", Success = false };
            }
            else
            {
                return new Response<Guid>() { Message = "Something went wrong, please try again.", Success = false };
            }
        }

        protected void AddBearerToken()
        {
            if (LocalStorageService.Exists("token"))
                Client.HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", LocalStorageService.GetStorageValue<string>("token"));
        }
    }
}
