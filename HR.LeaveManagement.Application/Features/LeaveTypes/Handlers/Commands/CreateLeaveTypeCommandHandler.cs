using AutoMapper;
using FluentValidation;
using HR.LeaveManagement.Application.DTOs.LeaveType.Validators;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Domain;
using MediatR;


namespace HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands
{
    public class CreateLeaveTypeCommandHandler : IRequestHandler<CreateLeaveTypeCommand, int>
    {
        public ILeaveTypeRepository LeaveTypeRepository { get; }
        public IMapper Mapper { get; }

        public CreateLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository, IMapper mapper)
        {
            LeaveTypeRepository = leaveTypeRepository;
            Mapper = mapper;
        }

        public async Task<int> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveTypeDtoValidator();
            var validationResult = await validator.ValidateAsync(request.LeaveTypeDto);

            if (!validationResult.IsValid)
                throw new Exception();

            var leaveType = Mapper.Map<LeaveType>(request.LeaveTypeDto);
            leaveType= await LeaveTypeRepository.Add(leaveType);
            return leaveType.Id;
        }
    }
}
