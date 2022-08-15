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
        public CreateLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository, 
            IMapper mapper,
            ILeaveTypeRepository leaveTypeRepository,
            IEmailSender emailSender,
            IHttpContextAccessor httpContextAccessor,
            ILeaveAllocationRepository leaveAllocationRepository)
        {
            LeaveRequestRepository = leaveRequestRepository;
            Mapper = mapper;
            LeaveTypeRepository = leaveTypeRepository;
            EmailSender = emailSender;
            HttpContextAccessor = httpContextAccessor;
            LeaveAllocationRepository = leaveAllocationRepository;
        }

        public ILeaveTypeRepository LeaveTypeRepository { get; }
        public ILeaveRequestRepository LeaveRequestRepository { get; }
        public IMapper Mapper { get; }
        public IEmailSender EmailSender { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
        public ILeaveAllocationRepository LeaveAllocationRepository { get; }

        public async Task<BaseCommandResponse> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            var validator = new CreateLeaveRequestDtoValidator(LeaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request.LeaveRequestDto, cancellationToken);
            var userId = HttpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c=> c.Type =="uid")?
                .Value;
            var allocation = await LeaveAllocationRepository.GetUserAllocations(userId, request.LeaveRequestDto.LeaveTypeId);
            int daysRequested = (int)(request.LeaveRequestDto.EndDate - request.LeaveRequestDto.StartDate).TotalDays;

            if(daysRequested > allocation.NumberOfDays)
            {
                validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(
                    nameof(request.LeaveRequestDto.EndDate), "You do not have enough days for this request"));
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
            leaveRequest = await LeaveRequestRepository.Add(leaveRequest);
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
