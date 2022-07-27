using AutoMapper;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Persistence.Contracts;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, int>
    {
        public CreateLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository, IMapper mapper)
        {
            LeaveRequestRepository = leaveRequestRepository;
            Mapper = mapper;
        }

        public ILeaveRequestRepository LeaveRequestRepository { get; }
        public IMapper Mapper { get; }
        public async Task<int> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = Mapper.Map<LeaveRequest>(request.LeaveRequestDto);
            leaveRequest = await LeaveRequestRepository.Add(leaveRequest);
            return leaveRequest.Id;
        }
    }
}
