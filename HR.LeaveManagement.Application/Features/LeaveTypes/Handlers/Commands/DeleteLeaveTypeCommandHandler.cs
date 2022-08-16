using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands
{
    public class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand>
    {
        public IUnitOfWork UnitOfWork { get; }

        public DeleteLeaveTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var leaveType = await UnitOfWork.LeaveTypeRepository.Get(request.Id);

            if(leaveType == null) 
                throw new NotFoundException(nameof(LeaveType), request.Id);

            await UnitOfWork.LeaveTypeRepository.Delete(leaveType);
            await UnitOfWork.Save();

            return Unit.Value;
        }
    }
}
