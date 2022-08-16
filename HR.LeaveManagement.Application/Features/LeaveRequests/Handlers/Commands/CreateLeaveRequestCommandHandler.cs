using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Domain;
using MediatR;
using HR.LeaveManagement.Application.Contracts.Infrastructure;
using HR.LeaveManagement.Application.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, BaseCommandResponse>
    {
        public CreateLeaveRequestCommandHandler(IUnitOfWork unitOfWork, 
            IMapper mapper,
            IEmailSender emailSender,
            IHttpContextAccessor httpContextAccessor)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
            EmailSender = emailSender;
            HttpContextAccessor = httpContextAccessor;
        }

        public IUnitOfWork UnitOfWork { get; }
        public IMapper Mapper { get; }
        public IEmailSender EmailSender { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }

        public async Task<BaseCommandResponse> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            var validator = new CreateLeaveRequestDtoValidator(UnitOfWork.LeaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request.LeaveRequestDto, cancellationToken);
            var userId = HttpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c=> c.Type =="uid")?
                .Value;
            var allocation = await UnitOfWork.LeaveAllocationRepository.GetUserAllocations(userId, request.LeaveRequestDto.LeaveTypeId);
            if (allocation is null)
            {
                validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(nameof(request.LeaveRequestDto.LeaveTypeId),
                    "You do not have any allocations for this leave type."));
            }
            else
            {
                int daysRequested = (int)(request.LeaveRequestDto.EndDate - request.LeaveRequestDto.StartDate).TotalDays;
                if (daysRequested > allocation.NumberOfDays)
                {
                    validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(
                        nameof(request.LeaveRequestDto.EndDate), "You do not have enough days for this request"));
                }
            }

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Request failed!";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return response;
            }

            var leaveRequest = Mapper.Map<LeaveRequest>(request.LeaveRequestDto);
            leaveRequest.RequestingEmployeeId = userId;
            leaveRequest = await UnitOfWork.LeaveRequestRepository.Add(leaveRequest);

            await UnitOfWork.Save();
            response.Success = true;
            response.Message = "Request created successful!";
            response.Id = leaveRequest.Id;

            try
            {
                var emailAddress = HttpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

                var email = new Email()
                {
                    To = emailAddress,
                    Subject = "Leave Request Submitted",
                    Body = $"Your leave request {request.LeaveRequestDto.StartDate:D} to {request.LeaveRequestDto.EndDate:D} " +
                        $"has been submitted successfully."
                };

                await EmailSender.SendEmailAsync(email);
            }
            catch (Exception ex)
            {
            }

            return response;
        }
    }
}
