using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Queries;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Queries
{
    public class GetLeaveRequestListRequestHandler : IRequestHandler<GetLeaveAllocationListRequest, List<LeaveAllocationDto>>
    {
        public GetLeaveRequestListRequestHandler(ILeaveAllocationRepository leaveAllocationRepository, IMapper mapper )
        {
            LeaveAllocationRepository = leaveAllocationRepository;
            Mapper = mapper;
        }

        public ILeaveAllocationRepository LeaveAllocationRepository { get; }
        public IMapper Mapper { get; }

        public async Task<List<LeaveAllocationDto>> Handle(GetLeaveAllocationListRequest request, CancellationToken cancellationToken)
        {
            var leaveAllocations = await LeaveAllocationRepository.GetLeaveAllocationsWithDetails();
            return Mapper.Map<List<LeaveAllocationDto>>(leaveAllocations);
        }
    }
}
