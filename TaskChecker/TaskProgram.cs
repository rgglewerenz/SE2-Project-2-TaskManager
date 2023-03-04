using DataAcess;
using DatabaseInterop;
using DatabaseInterop.Models;
using DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseEnums;
using Microsoft.Extensions.Configuration;
using DatabaseBAL;

namespace TaskChecker
{
    public class TaskProgram
    {
        private readonly TaskDA taskDA;
        private readonly UsersDA usersDA;
        private TimeSpan CheckDelay = TimeSpan.FromMinutes(1);
        private DateTime StartCheck = DateTime.Now;
        private Queue<TaskRecurrenceModal> taskQueue;
        public TaskProgram(IConfiguration _config)
        {

            usersDA = new UsersDA(new UnitOfWork(_config), _config);
            taskDA = new TaskDA(new UnitOfWork(_config));
        }

        public async Task Run()
        {
            var recurranceOptions = taskDA.GetTaskRecurrenceModals();
     
            var timer = new PeriodicTimer(CheckDelay);
            CheckTasks(recurranceOptions);
            while (await timer.WaitForNextTickAsync())
            {
                StartCheck = DateTime.Now;
                recurranceOptions = taskDA.GetTaskRecurrenceModals();
                TaskRecurrenceModal next = new TaskRecurrenceModal();
                CheckTasks(recurranceOptions);
                
            }
        }

        void IfDaily(TaskRecurrenceModal options)
        {
            if (options.FirstOccurrance == null)
                return;

            DateTime StartDate = options.FirstOccurrance ?? StartCheck;

            if (RoundUp(StartCheck.Date + StartDate.TimeOfDay, CheckDelay) == RoundUp(StartCheck.AddMinutes(60), CheckDelay))
                Task.Run(async () => await SendEmail(options.TaskID));
        }

        void IfWeekly(TaskRecurrenceModal options)
        {
            if (options.RecurringDays == null || options.FirstOccurrance == null)
            {
                return;
            }

            DateTime startDate = options.FirstOccurrance ?? StartCheck;

            foreach (char item in options.RecurringDays)
            {
                DayOfWeek char_day = GetDayFromChar(item);
                if (StartCheck.DayOfWeek == char_day)
                {
                    if (RoundUp(StartCheck.Date + startDate.TimeOfDay, CheckDelay) == RoundUp(StartCheck.AddMinutes(60), CheckDelay))
                    {
                        Task.Run(async () => await SendEmail(options.TaskID));
                    }
                }
            }


        }

        void IfBiWeekly(TaskRecurrenceModal options)
        {
            if (options.RecurringDays == null || options.FirstOccurrance == null)
                return;

            DateTime startDate = options.FirstOccurrance ?? StartCheck;


            DateTime WeeklyDate = startDate;

            while (true)
            {
                //If next week's cycle
                if (RoundUp(WeeklyDate, TimeSpan.FromDays(7)) > RoundUp(StartCheck, TimeSpan.FromDays(7)))
                    return;
                //If this week's cycle
                if (RoundUp(WeeklyDate, TimeSpan.FromDays(7)) == RoundUp(StartCheck, TimeSpan.FromDays(7)))
                    break;
                WeeklyDate.AddDays(14);
            }

            foreach (char item in options.RecurringDays)
            {
                DayOfWeek char_day = GetDayFromChar(item);
                if (DateTime.Now.DayOfWeek == char_day)
                    if (RoundUp(StartCheck.Date + startDate.TimeOfDay, CheckDelay) == RoundUp(StartCheck.AddMinutes(60), CheckDelay))
                        Task.Run(async () => await SendEmail(options.TaskID));
            }
        }

        void IfMonthly(TaskRecurrenceModal options)
        {
            if (options.RecurringDays == null || options.FirstOccurrance == null)
                return;

            DateTime startDate = options.FirstOccurrance ?? StartCheck;

            foreach (char item in options.RecurringDays)
            {
                int char_day = ((int)item) - 32;
                Calendar calendar = CultureInfo.InvariantCulture.Calendar;
                if (calendar.GetDayOfMonth(DateTime.Now) == char_day)
                    if (RoundUp(StartCheck.Date + startDate.TimeOfDay, CheckDelay) == RoundUp(StartCheck.AddMinutes(60), CheckDelay))
                        Task.Run(async () => await SendEmail(options.TaskID));
            }

        }

        void IfNoRecurring(TaskRecurrenceModal options)
        {
            var startDate = options.FirstOccurrance ?? StartCheck;
            if (RoundUp(startDate, CheckDelay) == RoundUp(StartCheck, CheckDelay))
                Task.Run(async () => await SendEmail(options.TaskID));
        }

        DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }

        async Task SendEmail(int taskID)
        {
            var _emailer = usersDA.GetEmailClient();
            try
            {
                var user = taskDA.GetUserFromTaskID(taskID);

                await _emailer.SendTaskReminderEmail(user.Email, taskDA.GetTaskModalByID(taskID));

            }
            catch(Exception ex)
            {
                await _emailer.SendErrorEmail("rgglewerenz@gmail.com", ex);
            }
        }

        DayOfWeek GetDayFromChar(char item)
        {
            item = char.ToLower(item);
            if (item == 'm')
                return DayOfWeek.Monday;
            if (item == 't')
                return DayOfWeek.Tuesday;
            if (item == 'w')
                return DayOfWeek.Wednesday;
            if (item == 'r')
                return DayOfWeek.Thursday;
            if (item == 'f')
                return DayOfWeek.Friday;
            if (item == 's')
                return DayOfWeek.Saturday;
            if (item == 'u')
                return DayOfWeek.Sunday;
            throw new Exception($"Unknown Char {item}");
        }

        void CheckTasks(List<TaskRecurrenceModal> recurranceOptions)
        {
            foreach (var item in recurranceOptions)
            {
                switch (item.RecurringType)
                {
                    case RecurrentTypes.Daily:
                        IfDaily(item);
                        break;
                    case RecurrentTypes.Weekly:
                        IfWeekly(item);
                        break;
                    case RecurrentTypes.BiWeekly:
                        IfBiWeekly(item);
                        break;
                    case RecurrentTypes.Monthly:
                        IfMonthly(item);
                        break;
                    case RecurrentTypes.Never:
                        IfNoRecurring(item);
                        break;
                    default:
                        throw new Exception("Should not get to this point in the switch statment");
                }
            }
        }

    }
}
