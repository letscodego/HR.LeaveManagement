using AutoMapper;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands
{
    public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, int>
    {
        public CreateLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository, IMapper mapper)
        {
            LeaveAllocationRepository = leaveAllocationRepository;
            Mapper = mapper;
        }

        public ILeaveAllocationRepository LeaveAllocationRepository { get; }
        public IMapper Mapper { get; }
        public async Task<int> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var leaveAllocation = Mapper.Map<LeaveAllocation>(request.LeaveAllocationDto);
            leaveAllocation = await LeaveAllocationRepository.Add(leaveAllocation);
            return leaveAllocation.Id; ;
        }
    }
}
