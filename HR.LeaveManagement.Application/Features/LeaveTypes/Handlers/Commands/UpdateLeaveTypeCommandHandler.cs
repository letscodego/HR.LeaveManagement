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
        public ILeaveTypeRepository LeaveTypeRepository { get; }
        public IMapper Mapper { get; }

        public UpdateLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository, IMapper mapper)
        {
            LeaveTypeRepository = leaveTypeRepository;
            Mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateLeaveTypeDtoValidator();
            var validationResult = await validator.ValidateAsync(request.LeaveTypeDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult);

            var leaveType = await LeaveTypeRepository.Get(request.LeaveTypeDto.Id);
            Mapper.Map(request.LeaveTypeDto, leaveType);

            await LeaveTypeRepository.Update(leaveType);
            return Unit.Value;
        }
    }
}
