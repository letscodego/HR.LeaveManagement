using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;
using HR.LeaveManagement.Application.Contracts.Identity;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Queries
{
    public class GetLeaveRequestDetailRequestHandler : IRequestHandler<GetLeaveRequestDetailRequest, LeaveRequestDto>
    {
        public GetLeaveRequestDetailRequestHandler(ILeaveRequestRepository leaveRequestRepository, IMapper mapper,
            IUserService userService)
        {
            LeaveRequestRepository = leaveRequestRepository;
            Mapper = mapper;
            UserService = userService;
        }

        public ILeaveRequestRepository LeaveRequestRepository { get; }
        public IMapper Mapper { get; }
        public IUserService UserService { get; }

        public async Task<LeaveRequestDto> Handle(GetLeaveRequestDetailRequest request, CancellationToken cancellationToken)
        {
            var leaveRequest = Mapper.Map<LeaveRequestDto>(await LeaveRequestRepository.GetLeaveRequestWithDetails(request.Id));
            leaveRequest.Employee = await UserService.GetEmployee(leaveRequest.RequestingEmployeeId);
            return Mapper.Map<LeaveRequestDto>(leaveRequest);
        }
    }
}
