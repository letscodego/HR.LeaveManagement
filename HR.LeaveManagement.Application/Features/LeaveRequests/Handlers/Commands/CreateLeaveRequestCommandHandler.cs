using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, int>
    {
        public CreateLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository, 
            IMapper mapper,
            ILeaveTypeRepository LeaveTypeRepository)
        {
            LeaveRequestRepository = leaveRequestRepository;
            Mapper = mapper;
            LeaveTypeRepository = LeaveTypeRepository;
        }

        public ILeaveTypeRepository LeaveTypeRepository { get; }
        public ILeaveRequestRepository LeaveRequestRepository { get; }
        public IMapper Mapper { get; }
        public async Task<int> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveRequestDtoValidator(LeaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request.LeaveRequestDto);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult);

            var leaveRequest = Mapper.Map<LeaveRequest>(request.LeaveRequestDto);
            leaveRequest = await LeaveRequestRepository.Add(leaveRequest);
            return leaveRequest.Id;
        }
    }
}
