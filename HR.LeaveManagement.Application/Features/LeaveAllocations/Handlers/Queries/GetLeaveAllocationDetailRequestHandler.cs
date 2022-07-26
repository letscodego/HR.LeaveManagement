using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Queries;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;


namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Queries
{
    public class GetLeaveAllocationDetailRequestHandler : IRequestHandler<GetLeaveAllocationDetailRequest, LeaveAllocationDto>
    {
        public GetLeaveAllocationDetailRequestHandler(ILeaveAllocationRepository leaveAllocationRepository, IMapper mapper)
        {
            LeaveAllocationRepository = leaveAllocationRepository;
            Mapper = mapper;
        }

        public ILeaveAllocationRepository LeaveAllocationRepository { get; }
        public IMapper Mapper { get; }

        public async Task<LeaveAllocationDto> Handle(GetLeaveAllocationDetailRequest request, CancellationToken cancellationToken)
        {
            var leaveAllocation = await LeaveAllocationRepository.GetLeaveAllocationWithDetails(request.Id);
            return Mapper.Map<LeaveAllocationDto>(leaveAllocation);
        }
    }
}
