using FluentValidation;
using HR.LeaveManagement.Application.DTOs.LeaveAllocation;
using HR.LeaveManagement.Application.Contracts.Persistence;

namespace HR.LeaveManagement.Application.DTOs.LeaveType.Validators
{
    public class CreateLeaveAllocationDtoValidator : AbstractValidator<CreateLeaveAllocationDto>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        public CreateLeaveAllocationDtoValidator(ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveTypeRepository = leaveTypeRepository;

            RuleFor(x => x.LeaveTypeId).NotEmpty()
                .GreaterThan(0)
                .MustAsync(async (id, tokeh) =>
                {
                    bool leaveTypeExsists = await _leaveTypeRepository.Exists(id);
                    return leaveTypeExsists;
                })
                .WithMessage("{PropertyName} does not exist.");
        }
    }
}
