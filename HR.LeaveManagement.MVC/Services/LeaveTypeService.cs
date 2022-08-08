using AutoMapper;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Services
{
    public class LeaveTypeService : BaseHttpService, ILeaveTypeService
    {
        public LeaveTypeService(IMapper mapper, IClient httpclient, ILocalStorageService localStorageService)
            : base(httpclient, localStorageService)
        {
            Mapper = mapper;
            Httpclient = httpclient;
            LocalStorageService = localStorageService;
        }

        public IMapper Mapper { get; }
        public IClient Httpclient { get; }
        public ILocalStorageService LocalStorageService { get; }

        public async Task<Response<int>> CreateLeaveType(LeaveTypeVM leaveType)
        {
            try
            {
                var response = new Response<int>();
                var createLeaveType = Mapper.Map<CreateLeaveTypeDto>(leaveType);
                var apiResponse= await Client.LeaveTypesPOSTAsync(createLeaveType);
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

        public async Task<Response<int>> DeleteLeaveType(int id)
        {
            try
            {
                await Client.LeaveTypesDELETEAsync(id);
                return new Response<int>() { Success = true };
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<int>(ex);
            }
        }

        public async Task<LeaveTypeVM> GetLeaveType(int id)
        {
            var leaveType = await Client.LeaveTypesGETAsync(id);
            return Mapper.Map<LeaveTypeVM>(leaveType);
        }

        public async Task<List<LeaveTypeVM>> GetLeaveTypes()
        {
            var leaveTypes = await Client.LeaveTypesAllAsync();
            return Mapper.Map<List<LeaveTypeVM>>(leaveTypes);
        }

        public async Task<Response<int>> UpdateLeaveType(LeaveTypeVM leaveType)
        {
            try
            {
                var leaveTypeDto = Mapper.Map<LeaveTypeDto>(leaveType);
                await Client.LeaveTypesPUTAsync(leaveTypeDto);

                return new Response<int>() { Success = true };
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<int>(ex);
            }
        }
    }
}
