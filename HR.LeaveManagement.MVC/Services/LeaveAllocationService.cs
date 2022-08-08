using AutoMapper;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Services
{
    public class LeaveAllocationService : BaseHttpService, ILeaveAllocationService
    {
        public LeaveAllocationService(IMapper mapper, IClient httpclient, ILocalStorageService localStorageService)
            : base(httpclient, localStorageService)
        {
            Mapper = mapper;
            Httpclient = httpclient;
            LocalStorageService = localStorageService;
        }

        public IMapper Mapper { get; }
        public IClient Httpclient { get; }
        public ILocalStorageService LocalStorageService { get; }

        public async Task<Response<int>> CreateLeaveAllocation(LeaveAllocationVM leaveAllocation)
        {
            try
            {
                var response = new Response<int>();
                var createLeaveAllocation = Mapper.Map<CreateLeaveAllocationDto>(leaveAllocation);
                var apiResponse = await Client.LeaveAllocationsPOSTAsync(createLeaveAllocation);
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

        public async Task<Response<int>> DeleteLeaveAllocation(int id)
        {
            try
            {
                await Client.LeaveAllocationsDELETEAsync(id);
                return new Response<int>() { Success = true };
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<int>(ex);
            }
        }

        public async Task<LeaveAllocationVM> GetLeaveAllocation(int id)
        {
            var leaveAllocation = await Client.LeaveAllocationsGETAsync(id);
            return Mapper.Map<LeaveAllocationVM>(leaveAllocation);
        }

        public async Task<List<LeaveAllocationVM>> GetLeaveAllocations()
        {
            var leaveAllocations = await Client.LeaveAllocationsAllAsync();
            return Mapper.Map<List<LeaveAllocationVM>>(leaveAllocations);
        }

        public async Task<Response<int>> UpdateLeaveAllocation(LeaveAllocationVM leaveAllocation)
        {
            try
            {
                var leaveAllocationDto = Mapper.Map<UpdateLeaveAllocationDto>(leaveAllocation);
                await Client.LeaveAllocationsPUTAsync(leaveAllocationDto);

                return new Response<int>() { Success = true };
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<int>(ex);
            }
        }
    }
}
