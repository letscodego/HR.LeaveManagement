using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
    {
        public UpdateLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository, 
            IMapper mapper,
            ILeaveTypeRepository leaveTypeRepository)
        {
            LeaveRequestRepository = leaveRequestRepository;
            Mapper = mapper;
            LeaveTypeRepository = leaveTypeRepository;
        }
        public ILeaveTypeRepository LeaveTypeRepository { get; }
        public ILeaveRequestRepository LeaveRequestRepository { get; }
        public IMapper Mapper { get; }

        public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateLeaveRequestDtoValidator(LeaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request.LeaveRequestDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult);

            var leaveRequest = await LeaveRequestRepository.Get(request.Id);
            
            if(request.LeaveRequestDto != null)
            {
                Mapper.Map(request.LeaveRequestDto, leaveRequest);
                await LeaveRequestRepository.Update(leaveRequest);
            }
            else if (request.ChangeLeaveRequestApprovalDto != null)
            {
                await LeaveRequestRepository.ChangeApprovalStatus(leaveRequest, request.ChangeLeaveRequestApprovalDto.Approved);
            }
            return Unit.Value;
        }
    }
}
