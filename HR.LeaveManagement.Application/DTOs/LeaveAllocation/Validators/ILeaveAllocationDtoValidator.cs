using FluentValidation;
using HR.LeaveManagement.Application.DTOs.LeaveAllocation;
using HR.LeaveManagement.Application.Persistence.Contracts;

namespace HR.LeaveManagement.Application.DTOs.LeaveType.Validators
{
    public class ILeaveAllocationDtoValidator : AbstractValidator<ILeaveAllocationDto>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        public ILeaveAllocationDtoValidator(ILeaveAllocationRepository leaveAllocationRepository)
        {
            _leaveAllocationRepository = leaveAllocationRepository;

            RuleFor(x => x.Period)
                .GreaterThanOrEqualTo(DateTime.Now.Year).WithMessage("{PropertyName} must be greater than {ComparisonValue}");
            
            RuleFor(x => x.NumberOfDays)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be at least 1.")
                .LessThan(100).WithMessage("{PropertyName} must be less than 100.");

            RuleFor(x => x.LeaveTypeId)
                .GreaterThan(0)
                .MustAsync(async (id, tokeh) =>
                {
                    bool leaveAllocationExsists = await _leaveAllocationRepository.Exists(id);
                    return !leaveAllocationExsists;

                })
                .WithMessage("{PropertyName} does not exist.");
        }
    }
}
