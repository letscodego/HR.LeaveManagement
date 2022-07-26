using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Queries
{
    public class GetLeaveRequestListRequestHandler : IRequestHandler<GetLeaveRequestListRequest, List<LeaveRequestListDto>>
    {
        public GetLeaveRequestListRequestHandler(ILeaveRequestRepository leaveRequestRepository, IMapper mapper )
        {
            LeaveRequestRepository = leaveRequestRepository;
            Mapper = mapper;
        }

        public ILeaveRequestRepository LeaveRequestRepository { get; }
        public IMapper Mapper { get; }

        public async Task<List<LeaveRequestListDto>> Handle(GetLeaveRequestListRequest request, CancellationToken cancellationToken)
        {
            var leaveRequests = await LeaveRequestRepository.GetLeaveRequestsWithDetails();
            return Mapper.Map<List<LeaveRequestListDto>>(leaveRequests);
        }
    }
}
