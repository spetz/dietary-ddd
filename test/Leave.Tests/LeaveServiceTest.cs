using NSubstitute;
using Xunit;

namespace Leave.Tests
{
    public class LeaveServiceTest
    {
        [Fact]
        public void requests_of_performers_will_be_manually_processed_after_26th_day()
        {

        }

        [Fact]
        public void performers_cannot_get_more_than_45_days()
        {

        }

        [Fact]
        public void slackers_do_not_get_any_leave()
        {

        }

        [Fact]
        public void regular_employee_doesnt_get_more_than_26_days()
        {

        }

        [Fact]
        public void regular_employee_gets_26_days()
        {

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