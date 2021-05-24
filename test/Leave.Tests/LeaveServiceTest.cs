using NSubstitute;
using Xunit;

namespace Leave.Tests
{
    public class LeaveServiceTest
    {
        [Fact]
        public void requests_of_performers_will_be_manually_processed_after_26th_day()
        {
            //given
            _database.FindByEmployeeId(One).Returns(new Employee(One, "PERFORMER", 10));
            
            //when
            var result = _leaveService.RequestPaidDaysOff(30, One);

            //then
            Assert.Equal(Result.Manual, result);
            _escalationManager.Received(1).NotifyNewPendingRequest(One);
            _emailSender.DidNotReceiveWithAnyArgs().Send(default);
            _messageBus.DidNotReceiveWithAnyArgs().SendEvent(default);
            _database.DidNotReceiveWithAnyArgs().Save(default);
        }

        [Fact]
        public void performers_cannot_get_more_than_45_days()
        {
            //given
            _database.FindByEmployeeId(One).Returns(new Employee(One, "PERFORMER", 10));
            
            //when
            var result = _leaveService.RequestPaidDaysOff(50, One);

            //then
            Assert.Equal(Result.Denied, result);
            _emailSender.Received(1).Send("next time");
            _escalationManager.DidNotReceiveWithAnyArgs().NotifyNewPendingRequest(default);
            _messageBus.DidNotReceiveWithAnyArgs().SendEvent(default);
            _database.DidNotReceiveWithAnyArgs().Save(default);
        }
        
        [Fact]
        public void slackers_do_not_get_any_leave()
        {
            //given
            _database.FindByEmployeeId(One).Returns(new Employee(One, "SLACKER", 10));
            
            //when
            var result = _leaveService.RequestPaidDaysOff(1, One);
            
            //then
            Assert.Equal(Result.Denied, result);
        }

        [Fact]
        public void slackers_get_a_nice_email()
        {
            //given
            _database.FindByEmployeeId(One).Returns(new Employee(One, "SLACKER", 10));

            //when
            _leaveService.RequestPaidDaysOff(1, One);

            //then
            _emailSender.Received(1).Send("next time");
        }

        [Fact]
        public void regular_employee_doesnt_get_more_than_26_days()
        {
            //given
            _database.FindByEmployeeId(One).Returns(new Employee(One, "REGULAR", 10));
            
            //when
            var result = _leaveService.RequestPaidDaysOff(20, One);
            
            //then
            Assert.Equal(Result.Denied, result);
            _emailSender.Received(1).Send("next time");
            _escalationManager.DidNotReceiveWithAnyArgs().NotifyNewPendingRequest(default);
            _messageBus.DidNotReceiveWithAnyArgs().SendEvent(default);
            _database.DidNotReceiveWithAnyArgs().Save(default);
        }

        [Fact]
        public void regular_employee_gets_26_days()
        {
            //given
            var regular = new Employee(One, "REGULAR", 10);
            _database.FindByEmployeeId(One).Returns(regular);
            
            //when
            var result = _leaveService.RequestPaidDaysOff(5, One);
            
            //then
            Assert.Equal(Result.Approved, result);
            _messageBus.Received(1).SendEvent("request approved");
            _escalationManager.DidNotReceiveWithAnyArgs().NotifyNewPendingRequest(default);
            _emailSender.DidNotReceiveWithAnyArgs().Send(default);
            _database.Received(1).Save(regular);
        }

        private const long One = 1;
        private readonly ILeaveDatabase _database;
        private readonly IMessageBus _messageBus;
        private readonly IEmailSender _emailSender;
        private readonly IEscalationManager _escalationManager;
        private readonly LeaveService _leaveService;

        public LeaveServiceTest()
        {
            _database = Substitute.For<ILeaveDatabase>();
            _messageBus = Substitute.For<IMessageBus>();
            _emailSender = Substitute.For<IEmailSender>();
            _escalationManager = Substitute.For<IEscalationManager>();
            _leaveService = new LeaveService(_database, _messageBus, _emailSender, _escalationManager);
        }
    }
}