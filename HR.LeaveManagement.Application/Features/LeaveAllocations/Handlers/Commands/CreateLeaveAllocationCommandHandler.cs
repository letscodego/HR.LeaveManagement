using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveType.Validators;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands
{
    public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, int>
    {
        public CreateLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository,
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
        public async Task<int> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveAllocationDtoValidator(LeaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request.LeaveAllocationDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult);

            var leaveAllocation = Mapper.Map<LeaveAllocation>(request.LeaveAllocationDto);
            leaveAllocation = await LeaveAllocationRepository.Add(leaveAllocation);
            return leaveAllocation.Id; ;
        }
    }
}
