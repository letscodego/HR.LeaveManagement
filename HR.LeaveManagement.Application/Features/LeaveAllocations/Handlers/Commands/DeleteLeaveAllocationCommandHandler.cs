using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands
{
    public class DeleteLeaveAllocationCommandHandler : IRequestHandler<DeleteLeaveAllocationCommand>
    {
        public DeleteLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository)
        {
            LeaveAllocationRepository = leaveAllocationRepository;
        }

        public ILeaveAllocationRepository LeaveAllocationRepository { get; }

        public async Task<Unit> Handle(DeleteLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var leaveAllocation = await LeaveAllocationRepository.Get(request.Id);
            await LeaveAllocationRepository.Delete(leaveAllocation);
            return Unit.Value;
        }
    }
}
