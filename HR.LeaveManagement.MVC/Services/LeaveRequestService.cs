using AutoMapper;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Services
{
    public class LeaveRequestService : BaseHttpService, ILeaveRequestService
    {
        public LeaveRequestService(IMapper mapper, IClient httpclient, ILocalStorageService localStorageService) 
            : base(httpclient, localStorageService)
        {
            Mapper = mapper;
            Httpclient = httpclient;
            LocalStorageService = localStorageService;
        }

        public IMapper Mapper { get; }
        public IClient Httpclient { get; }
        public ILocalStorageService LocalStorageService { get; }
        public async Task<Response<int>> CreateLeaveRequest(LeaveRequestVM leaveRequest)
        {
            try
            {
                var response = new Response<int>();
                var createLeaveRequest = Mapper.Map<CreateLeaveRequestDto>(leaveRequest);
                var apiResponse = await Client.LeaveRequestsPOSTAsync(createLeaveRequest);
                if (apiResponse.Success)
                {
                    response.Success = true;
                    response.Data = apiResponse.Id;
                }
                else
                {
                    response.Success = false;
                    foreach (var item in apiResponse.Errors)
                    {
                        response.ValidationErrors += item + Environment.NewLine;
                    }
                }

                return response;
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<int>(ex);
            }
        }

        public async Task<Response<int>> DeleteLeaveRequest(int id)
        {
            try
            {
                await Client.LeaveRequestsDELETEAsync(id);
                return new Response<int>() { Success = true };
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<int>(ex);
            }
        }

        public async Task<LeaveRequestVM> GetLeaveRequest(int id)
        {
            var leaveRequest = await Client.LeaveRequestsGETAsync(id);
            return Mapper.Map<LeaveRequestVM>(leaveRequest);
        }

        public async Task<List<LeaveRequestVM>> GetLeaveRequests()
        {
            var leaveRequests = await Client.LeaveRequestsAllAsync();
            return Mapper.Map<List<LeaveRequestVM>>(leaveRequests);
        }

        public async Task<Response<int>> UpdateLeaveRequest(int id, LeaveRequestVM leaveRequest)
        {
            try
            {
                var leaveRequestDto = Mapper.Map<UpdateLeaveRequestDto>(leaveRequest);
                await Client.LeaveRequestsPUTAsync(id, leaveRequestDto);

                return new Response<int>() { Success = true };
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<int>(ex);
            }
        }
    }
}
