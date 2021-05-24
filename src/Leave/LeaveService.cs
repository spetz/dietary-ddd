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

            var employee = _database.FindByEmployeeId(employeeId);
            var result = employee.RequestDaysOff(days);

            if (result == Result.Manual)
            {
                _escalationManager.NotifyNewPendingRequest(employeeId);
            }

            if (result == Result.Denied)
            {
                _emailSender.Send("next time");
            }

            if (result == Result.Approved)
            {
                _messageBus.SendEvent("request approved");
                _database.Save(employee);
            }

            return result;
        }
    }

    public class Employee
    {
        private long _employeeId;
        private string _employeeStatus;
        private int _daysSoFar;

        public Employee(long employeeId, string employeeStatus, int daysSoFar)
        {
            _employeeId = employeeId;
            _employeeStatus = employeeStatus;
            _daysSoFar = daysSoFar;
        }

        public Result RequestDaysOff(int days)
        {
            if (_daysSoFar + days > 26)
            {
                if (_employeeStatus.Equals("PERFORMER") && _daysSoFar + days < 45)
                {
                    return Result.Manual;
                }
                else
                {
                    return Result.Denied;
                }
            }
            else
            {
                if (_employeeStatus.Equals("SLACKER"))
                {
                    return Result.Denied;
                }
                else
                {
                    _daysSoFar = _daysSoFar + days;
                    return Result.Approved;
                }
            }
        }
    }

    public interface ILeaveDatabase
    {
        Employee FindByEmployeeId(long employeeId);
        void Save(Employee employeeData);
    }

    public class LeaveDatabase : ILeaveDatabase
    {
        public Employee FindByEmployeeId(long employeeId)
        {
            return null;
        }

        public void Save(Employee employeeData)
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