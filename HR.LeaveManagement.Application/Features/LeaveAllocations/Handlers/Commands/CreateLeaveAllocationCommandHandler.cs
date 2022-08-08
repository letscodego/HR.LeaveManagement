using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveType.Validators;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using MediatR;
using HR.LeaveManagement.Application.Responses;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands
{
    public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, BaseCommandResponse>
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
        public async Task<BaseCommandResponse> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            var validator = new CreateLeaveAllocationDtoValidator(LeaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request.LeaveAllocationDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Creation failed!";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return response;
            }
                

            var leaveAllocation = Mapper.Map<LeaveAllocation>(request.LeaveAllocationDto);
            leaveAllocation = await LeaveAllocationRepository.Add(leaveAllocation);

            response.Success = true;
            response.Message = "Creation Successful!";
            response.Id = leaveAllocation.Id;

            return response;
        }
    }
}
