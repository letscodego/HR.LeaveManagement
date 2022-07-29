using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    internal class DeleteLeaveRequestCommandHandler : IRequestHandler<DeleteLeaveRequestCommand>
    {
        public DeleteLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository)
        {
            LeaveRequestRepository = leaveRequestRepository;
        }

        public ILeaveRequestRepository LeaveRequestRepository { get; }

        public async Task<Unit> Handle(DeleteLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await LeaveRequestRepository.Get(request.Id);
            await LeaveRequestRepository.Delete(leaveRequest);
            return Unit.Value;
        }
    }
}
