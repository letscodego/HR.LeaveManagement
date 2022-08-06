using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using Moq;

namespace HR.LeaveManagement.Application.UnitTests.Mocks
{
    public static class MockLeaveTypeRepository
    {
        public static Mock<ILeaveTypeRepository> GetLeaveTypeRepository()
        {
            var leaveTypes = new List<LeaveType>
            {
                new LeaveType {
                    Id = 1,
                    Name = "Test Vaction",
                    DefaultDays =10,
                    CreatedBy = String.Empty,
                    DateCreated = DateTime.Now,
                    LastModifiedBy = String.Empty,
                    LastModifiedDate = DateTime.Now,
                },
                new LeaveType {
                    Id = 2,
                    Name = "Test Sick",
                    DefaultDays =15,
                    CreatedBy = String.Empty,
                    DateCreated = DateTime.Now,
                    LastModifiedBy = String.Empty,
                    LastModifiedDate = DateTime.Now,
                },
                new LeaveType {
                    Id = 3,
                    Name = "Test Maternity",
                    DefaultDays =15,
                    CreatedBy = String.Empty,
                    DateCreated = DateTime.Now,
                    LastModifiedBy = String.Empty,
                    LastModifiedDate = DateTime.Now,
                }
            };

            var mockRepo = new Mock<ILeaveTypeRepository>();
            mockRepo.Setup(r => r.GetAll()).ReturnsAsync(leaveTypes);

            mockRepo.Setup(r => r.Add(It.IsAny<LeaveType>())).ReturnsAsync((LeaveType leaveType) =>
              {
                  leaveTypes.Add(leaveType);
                  return leaveType;
              });

            return mockRepo;
        }
    }
}
