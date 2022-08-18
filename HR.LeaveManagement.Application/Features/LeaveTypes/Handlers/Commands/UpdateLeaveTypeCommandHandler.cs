using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveType.Validators;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands
{
    public class UpdateLeaveTypeCommandHandler : IRequestHandler<UpdateLeaveTypeCommand, Unit>
    {
        public IUnitOfWork UnitOfWork { get; }
        public IMapper Mapper { get; }

        public UpdateLeaveTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateLeaveTypeDtoValidator();
            var validationResult = await validator.ValidateAsync(request.LeaveTypeDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult);

            var leaveType = await UnitOfWork.LeaveTypeRepository.Get(request.LeaveTypeDto.Id);
            
            if (leaveType == null)
                throw new NotFoundException(nameof(leaveType), request.LeaveTypeDto.Id);

            Mapper.Map(request.LeaveTypeDto, leaveType);

            await UnitOfWork.LeaveTypeRepository.Update(leaveType);
            await UnitOfWork.Save();

            return Unit.Value;
        }
    }
}
