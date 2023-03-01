using DatabaseInterop;
using DatabaseInterop.Models;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess
{
    public class TaskDA : BaseDA
    {
        public TaskDA(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
        }

        public bool AddTask(TaskModal taskModal, TaskRecurrenceModal taskRecurrenceModal, int userID)
        {
            try
            {
                UnitOfWork.StartTransaction();
                //Checks if user exists
                var user = (from Users in UnitOfWork.UserRepository.GetQuery()
                            where Users.UserID == userID
                            select Users.UserID);

                if (user.Count() == 0)
                {
                    throw new Exception($"Unable to find a user with id {userID}");
                }

                UnitOfWork.TaskRepository.Insert(taskModal);
                UnitOfWork.Save();


                UnitOfWork.UserTaskMapRepository.Insert(new UserTaskMap()
                {
                    UserID = userID,
                    TaskID = taskModal.TaskID,
                });
				UnitOfWork.Save();

				taskRecurrenceModal.TaskID = taskModal.TaskID;

                UnitOfWork.TaskRecurranceRepository.Insert(taskRecurrenceModal);

                UnitOfWork.Save();
                UnitOfWork.CommitTransaction();
                return true;
            }
            catch(Exception ex)
            {
                UnitOfWork.RollbackTransaction();
                return false;
            }
            
        }

        public bool AddTask(TaskTransferModal task, int userID)
        {
            if (task.recurrenceOptions != null)
                return AddTask(new TaskModal()
                        {
                            Title = task.Title,
                            Description = task.Description,
                        },
                        new TaskRecurrenceModal()
                        {
                            FirstOccurrance = task.recurrenceOptions.FirstOccurrance,
                            RecurringDays = task.recurrenceOptions.RecurringDays,
                            RecurringType = task.recurrenceOptions.RecurringType,
                        },
                        userID);
            else
                throw new Exception("The recurrence Modal can not be null");
        }

        public List<TaskModal> GetTasks(int userID)
        {
            //Checks if user exists
            var user = (from Users in UnitOfWork.UserRepository.GetQuery()
                        where Users.UserID == userID
                        select Users.UserID);

            if (user.Count() == 0)
            {
                throw new Exception($"Unable to find a user with id {userID}");
            }


            var taskIDs = (from UserTaskMapping in UnitOfWork.UserTaskMapRepository.GetQuery()
                           where UserTaskMapping.UserID == userID
                           select UserTaskMapping.TaskID);


            var tasks = (from Tasks in UnitOfWork.TaskRepository.GetQuery()
                         where taskIDs.Contains(Tasks.TaskID)
                         select Tasks);


            return tasks.ToList();
        }

        public List<TaskTransferModal> GetTaskTransferModals(int userID)
        {
            //Checks if user exists
            var user = (from Users in UnitOfWork.UserRepository.GetQuery()
                        where Users.UserID == userID
                        select Users.UserID);

            if (user.Count() == 0)
            {
                throw new Exception($"Unable to find a user with id {userID}");
            }


            var taskIDs = (from UserTaskMapping in UnitOfWork.UserTaskMapRepository.GetQuery()
                           where UserTaskMapping.UserID == userID
                           select UserTaskMapping.TaskID);


            var tasks = (from Tasks in UnitOfWork.TaskRepository.GetQuery()
                         where taskIDs.Contains(Tasks.TaskID)
                         select Tasks);


            var tasksRecurrence = (from TasksRecurrence in UnitOfWork.TaskRecurranceRepository.GetQuery()
                                   where taskIDs.Contains(TasksRecurrence.TaskID)
                                   select TasksRecurrence);

            if(taskIDs.Count() != tasksRecurrence.Count()) {
                var taskCount = taskIDs.Count();
                var RecurranceCount = tasksRecurrence.Count();
                throw new Exception("A task does not a recurrence map");
            }

            return  tasks.Join(tasksRecurrence, x => x.TaskID, x => x.TaskID, (task, reccurrence) => new TaskTransferModal()
            {
                TaskID = task.TaskID,
                Description = task.Description,
                Title= task.Title,
                recurrenceOptions = new TaskRecurrenceTransferModal()
                {
                    FirstOccurrance = reccurrence.FirstOccurrance,
                    RecurringDays = reccurrence.RecurringDays,
                    RecurringType= reccurrence.RecurringType,
                    TaskID = task.TaskID
                }
            }).ToList();
            

        }

        public bool UpdateTask(TaskModal taskModal, TaskRecurrenceModal taskRecurrenceModal, int userID)
        {
            try
            {
                UnitOfWork.StartTransaction();
                //Checks if user exists
                var user = (from Users in UnitOfWork.UserRepository.GetQuery()
                            where Users.UserID == userID
                            select Users.UserID);

                if (user.Count() == 0)
                {
                    throw new Exception($"Unable to find a user with id {userID}");
                }

                var oldModal = (from TaskRepo in UnitOfWork.TaskRepository.GetQuery()
                                where TaskRepo.TaskID == taskModal.TaskID
                                select TaskRepo).First();

                if(oldModal != taskModal)
                {
                    oldModal.Title = taskModal.Title;
                    oldModal.Description = taskModal.Description;
                    UnitOfWork.TaskRepository.Update(oldModal);
                    UnitOfWork.Save();
                }


                taskRecurrenceModal.TaskID = taskModal.TaskID;

                var oldRecurrance = (from TaskRepo in UnitOfWork.TaskRecurranceRepository.GetQuery()
                                     where TaskRepo.TaskID == taskModal.TaskID
                                     select TaskRepo).First();

                if (taskRecurrenceModal != oldRecurrance)
                {
                    oldRecurrance.FirstOccurrance = taskRecurrenceModal.FirstOccurrance;
                    oldRecurrance.RecurringDays = taskRecurrenceModal.RecurringDays;
                    oldRecurrance.RecurringType = taskRecurrenceModal.RecurringType;
                    UnitOfWork.TaskRecurranceRepository.Update(oldRecurrance);
                    UnitOfWork.Save();
                } 

                UnitOfWork.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                UnitOfWork.RollbackTransaction();
                return false;
            }

        }

        public bool UpdateTask(TaskTransferModal task, int userID)
        {
            if (task.recurrenceOptions != null)
                return UpdateTask(new TaskModal()
                {
                    TaskID = task.recurrenceOptions.TaskID,
                    Title = task.Title,
                    Description = task.Description,
                },
                        new TaskRecurrenceModal()
                        {
                            TaskID = task.recurrenceOptions.TaskID,
                            FirstOccurrance = task.recurrenceOptions.FirstOccurrance,
                            RecurringDays = task.recurrenceOptions.RecurringDays,
                            RecurringType = task.recurrenceOptions.RecurringType
                        },
                        userID);
            else
                throw new Exception("The recurrence Modal can not be null");
        }

        public bool DeleteTask(int taskID)
        {
            try
            {
                UnitOfWork.StartTransaction();

                UnitOfWork.TaskRecurranceRepository.Delete(UnitOfWork.TaskRecurranceRepository.Get(x => x.TaskID == taskID).First());
                UnitOfWork.Save();

                UnitOfWork.UserTaskMapRepository.Delete(UnitOfWork.UserTaskMapRepository.Get(x => x.TaskID == taskID).First());
                UnitOfWork.Save();

                UnitOfWork.TaskRepository.Delete(UnitOfWork.TaskRepository.Get(x => x.TaskID == taskID).First());

                UnitOfWork.Save();
                UnitOfWork.CommitTransaction();
                return true;
            }
            catch(Exception ex)
            {
                UnitOfWork.RollbackTransaction();
                return false;
            }

        }

        public List<TaskRecurrenceModal> GetTaskRecurrenceModals()
        {
            return (from Tasks in UnitOfWork.TaskRecurranceRepository.GetQuery()
                    select Tasks).ToList();
        }

        public UserModal GetUserFromTaskID(int taskID)
        {
            var userID = (from Task in UnitOfWork.UserTaskMapRepository.GetQuery()
                         where Task.TaskID == taskID
                         select Task.UserID).First();

            return (from User in UnitOfWork.UserRepository.GetQuery()
                    where User.UserID == userID
                    select User).First();
        }

        public TaskModal GetTaskModalByID(int taskID)
        {
            return (from Task in UnitOfWork.TaskRepository.GetQuery()
                    where Task.TaskID == taskID
                    select Task).First();
        }
    }
}
