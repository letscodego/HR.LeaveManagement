using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveType.Validators;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands
{
    public class UpdateLeaveAllocationCommandHandler : IRequestHandler<UpdateLeaveAllocationCommand, Unit>
    {
        public UpdateLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository, 
            IMapper mapper,
            ILeaveTypeRepository leaveTypeRepository)

        {
            LeaveAllocationRepository = leaveAllocationRepository;
            Mapper = mapper;
            LeaveTypeRepository = leaveTypeRepository;
        }
        public ILeaveTypeRepository LeaveTypeRepository { get; }
        public ILeaveAllocationRepository LeaveAllocationRepository { get; }
        public IMapper Mapper { get; }

        public async Task<Unit> Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateLeaveAllocationDtoValidator(LeaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request.UpdateLeaveAllocationDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult);

            var leaveAllocation = await LeaveAllocationRepository.Get(request.UpdateLeaveAllocationDto.Id);
            Mapper.Map(request.UpdateLeaveAllocationDto, leaveAllocation);

            await LeaveAllocationRepository.Update(leaveAllocation);
            return Unit.Value;
        }
    }
}
