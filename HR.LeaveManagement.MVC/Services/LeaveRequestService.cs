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

        public async Task ApproveLeaveRequest(int id, bool approved)
        {
            AddBearerToken();
            try
            {
                var request = new ChangeLeaveRequestApprovalDto { Approved = approved, Id = id };
                await Client.ChangeapprovalAsync(id, request);
            }
            catch (ApiException ex)
            {
                throw;
                //return ConvertApiExceptions<int>(ex);
            }
        }

        public async Task<Response<int>> CreateLeaveRequest(CreateLeaveRequestVM leaveRequest)
        {
            try
            {
                var response = new Response<int>();
                var createLeaveRequest = Mapper.Map<CreateLeaveRequestDto>(leaveRequest);
                AddBearerToken();
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

        public async Task DeleteLeaveRequest(int id)
        {
            AddBearerToken();
            await Client.LeaveRequestsDELETEAsync(id);
        }

        public async Task<AdminLeaveRequestViewVM> GetAdminLeaveRequestList()
        {
            AddBearerToken();
            var leaveRequests = await Client.LeaveRequestsAllAsync(isLoggedInUser: false);

            var model = new AdminLeaveRequestViewVM
            {
                TotalRequests = leaveRequests.Count,
                ApprovedRequests = leaveRequests.Count(q => q.Approved == true),
                PendingRequests = leaveRequests.Count(q => q.Approved == null),
                RejectedRequests = leaveRequests.Count(q => q.Approved == false),
                LeaveRequests = Mapper.Map<List<LeaveRequestVM>>(leaveRequests)
            };
            return model;
        }

        public async Task<LeaveRequestVM> GetLeaveRequest(int id)
        {
            AddBearerToken();
            var leaveRequest = await Client.LeaveRequestsGETAsync(id);
            return Mapper.Map<LeaveRequestVM>(leaveRequest);
        }

        public async Task<List<LeaveRequestVM>> GetLeaveRequests()
        {
            AddBearerToken();
            var leaveRequests = await Client.LeaveRequestsAllAsync(false);
            return Mapper.Map<List<LeaveRequestVM>>(leaveRequests);
        }

        public async Task<EmployeeLeaveRequestViewVM> GetUserLeaveRequest()
        {
            AddBearerToken();
            var leaveRequests = await Client.LeaveRequestsAllAsync(isLoggedInUser: true);
            var allocations = await Client.LeaveAllocationsAllAsync(isLoggedInUser: true);
            var model = new EmployeeLeaveRequestViewVM
            {
                LeaveAllocations = Mapper.Map<List<LeaveAllocationVM>>(allocations),
                LeaveRequests = Mapper.Map<List<LeaveRequestVM>>(leaveRequests)
            };

            return model;
        }

        public async Task<Response<int>> UpdateLeaveRequest(int id, LeaveRequestVM leaveRequest)
        {
            try
            {
                var leaveRequestDto = Mapper.Map<UpdateLeaveRequestDto>(leaveRequest);
                AddBearerToken();
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
