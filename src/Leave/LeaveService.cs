using System;

namespace Leave
{
    public class LeaveService
    {
        private readonly ILeaveDatabase _database;
        private readonly IMessageBus _messageBus;
        private readonly IEmailSender _emailSender;
        private readonly IEscalationManager _escalationManager;

        public LeaveService(ILeaveDatabase database, IMessageBus messageBus, IEmailSender emailSender,
            IEscalationManager escalationManager)
        {
            _database = database;
            _messageBus = messageBus;
            _emailSender = emailSender;
            _escalationManager = escalationManager;
        }

        public Result RequestPaidDaysOff(int days, long employeeId)
        {
            if (days < 0)
            {
                throw new ArgumentException();
            }

            Result result;
            var employeeData = _database.FindByEmployeeId(employeeId);
            var employeeStatus = (string) employeeData[0];
            var daysSoFar = (int) employeeData[1];

            if (daysSoFar + days > 26)
            {
                if (employeeStatus.Equals("PERFORMER") && daysSoFar + days < 45)
                {
                    result = Result.Manual;
                    _escalationManager.NotifyNewPendingRequest(employeeId);
                }
                else
                {
                    result = Result.Denied;
                    _emailSender.Send("next time");
                }
            }
            else
            {
                if (employeeStatus.Equals("SLACKER"))
                {
                    result = Result.Denied;
                    _emailSender.Send("next time");
                }
                else
                {
                    employeeData[1] = daysSoFar + days;
                    result = Result.Approved;
                    _database.Save(employeeData);
                    _messageBus.SendEvent("request approved");
                }
            }

            return result;
        }
    }

    public interface ILeaveDatabase
    {
        object[] FindByEmployeeId(long employeeId);
        void Save(object[] employeeData);
    }

    public class LeaveDatabase : ILeaveDatabase
    {
        public object[] FindByEmployeeId(long employeeId)
        {
            return new object[0];
        }

        public void Save(object[] employeeData)
        {

        }
    }

    public interface IMessageBus
    {
        void SendEvent(string msg);
    }

    public class MessageBus : IMessageBus
    {
        public void SendEvent(string msg)
        {
        }
    }

    public interface IEmailSender
    {
        void Send(string msg);
    }

    public class EmailSender : IEmailSender
    {
        public void Send(string msg)
        {
        }
    }

    public interface IEscalationManager
    {
        void NotifyNewPendingRequest(long employeeId);
    }

    public class EscalationManager : IEscalationManager
    {
        public void NotifyNewPendingRequest(long employeeId)
        {
        }
    }

    public class Configuration
    {
        public int GetMaxDaysForPerformers()
        {
            return 45;
        }

        public int GetMaxDays()
        {
            return 26;
        }
    }
}