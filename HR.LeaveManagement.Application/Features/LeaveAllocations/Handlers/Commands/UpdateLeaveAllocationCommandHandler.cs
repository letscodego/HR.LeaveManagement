using AutoMapper;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands
{
    public class UpdateLeaveAllocationCommandHandler : IRequestHandler<UpdateLeaveAllocationCommand, Unit>
    {
        public UpdateLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository, IMapper mapper)
        {
            LeaveAllocationRepository = leaveAllocationRepository;
            Mapper = mapper;
        }

        public ILeaveAllocationRepository LeaveAllocationRepository { get; }
        public IMapper Mapper { get; }

        public async Task<Unit> Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var leaveAllocation = await LeaveAllocationRepository.Get(request.UpdateLeaveAllocationDto.Id);
            Mapper.Map(request.UpdateLeaveAllocationDto, leaveAllocation);

            await LeaveAllocationRepository.Update(leaveAllocation);
            return Unit.Value;
        }
    }
}
