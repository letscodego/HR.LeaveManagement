using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, BaseCommandResponse>
    {
        public CreateLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository, 
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
        public async Task<BaseCommandResponse> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            var validator = new CreateLeaveRequestDtoValidator(LeaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request.LeaveRequestDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Creation failed!";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return response;
            }

            var leaveRequest = Mapper.Map<LeaveRequest>(request.LeaveRequestDto);
            leaveRequest = await LeaveRequestRepository.Add(leaveRequest);
            response.Success = true;
            response.Message = "Creation Successful!";
            response.Id = leaveRequest.Id;
            return response;
        }
    }
}
