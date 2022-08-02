using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveType.Validators;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;
using HR.LeaveManagement.Application.Contracts.Infrastructure;
using HR.LeaveManagement.Application.Models;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands
{
    public class UpdateLeaveAllocationCommandHandler : IRequestHandler<UpdateLeaveAllocationCommand, Unit>
    {
        public UpdateLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository, 
            IMapper mapper,
            ILeaveTypeRepository leaveTypeRepository,
            IEmailSender emailSender)

        {
            LeaveAllocationRepository = leaveAllocationRepository;
            Mapper = mapper;
            LeaveTypeRepository = leaveTypeRepository;
            EmailSender = emailSender;
        }
        public ILeaveTypeRepository LeaveTypeRepository { get; }
        public ILeaveAllocationRepository LeaveAllocationRepository { get; }
        public IMapper Mapper { get; }
        public IEmailSender EmailSender { get; }

        public async Task<Unit> Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateLeaveAllocationDtoValidator(LeaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request.UpdateLeaveAllocationDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult);

            var leaveAllocation = await LeaveAllocationRepository.Get(request.UpdateLeaveAllocationDto.Id);
            Mapper.Map(request.UpdateLeaveAllocationDto, leaveAllocation);

            await LeaveAllocationRepository.Update(leaveAllocation);
            try
            {
                await EmailSender.SendEmailAsync(new Email()
                {
                    To = "a@a.com",
                    Subject = "Leave Allocation Updated",
                    Body = $"Your leave request...{leaveAllocation.DateCreated}"
                });
            }
            catch (Exception ex)
            {
            }
            
            return Unit.Value;
        }
    }
}
