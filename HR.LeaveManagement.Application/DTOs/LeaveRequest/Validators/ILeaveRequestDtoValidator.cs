using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;

namespace HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators
{
    public class ILeaveRequestDtoValidator : AbstractValidator<ILeaveRequestDto>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        public ILeaveRequestDtoValidator(ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveTypeRepository = leaveTypeRepository;

            RuleFor(x => x.StartDate).NotEmpty()
                .LessThan(p => p.EndDate).WithMessage("{PropertyName} must be befor {ComparisonValue}");

            RuleFor(x => x.EndDate).NotEmpty()
                .GreaterThan(p => p.StartDate).WithMessage("{PropertyName} must be after {ComparisonValue}");

            RuleFor(x => x.LeaveTypeId).NotEmpty()
                .GreaterThan(0)
                .MustAsync(async (id, tokeh) =>
                {
                    bool leaveTypeExsists = await _leaveTypeRepository.Exists(id);
                    return leaveTypeExsists;
                })
                .WithMessage("{PropertyName} does not exist");
        }
    }
}
