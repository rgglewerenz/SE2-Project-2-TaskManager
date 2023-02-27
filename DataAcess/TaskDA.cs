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

        public void AddTask(TaskModal taskModal, TaskRecurrenceModal taskRecurrenceModal, int userID)
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
                UnitOfWork.CommitTransaction();


                UnitOfWork.UserTaskMapRepository.Insert(new UserTaskMap()
                {
                    UserID = userID,
                    TaskID = taskModal.TaskID,
                });
                UnitOfWork.CommitTransaction();

                taskRecurrenceModal.TaskID = taskModal.TaskID;

                UnitOfWork.TaskTransferRepository.Insert(taskRecurrenceModal);
                UnitOfWork.CommitTransaction();
                UnitOfWork.Save();
            }
            catch(Exception ex)
            {
                UnitOfWork.RollbackTransaction();
            }
            
        }

        public void AddTask(TaskTransferModal task, int userID)
        {
            if (task.recurrenceOptions != null)
                AddTask(new TaskModal()
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


            var tasksRecurrence = (from TasksRecurrence in UnitOfWork.TaskTransferRepository.GetQuery()
                                   where tasks.Select(x => x.TaskID == TasksRecurrence.TaskID).Count() == 1
                                   select TasksRecurrence);

            if(taskIDs.Count() != tasksRecurrence.Count()) {
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

        public void UpdateTask(TaskModal taskModal, TaskRecurrenceModal taskRecurrenceModal, int userID)
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

                UnitOfWork.TaskRepository.Update(taskModal);
                UnitOfWork.CommitTransaction();


                UnitOfWork.UserTaskMapRepository.Update(new UserTaskMap()
                {
                    UserID = userID,
                    TaskID = taskModal.TaskID,
                });
                UnitOfWork.CommitTransaction();

                taskRecurrenceModal.TaskID = taskModal.TaskID;

                UnitOfWork.TaskTransferRepository.Update(taskRecurrenceModal);
                UnitOfWork.CommitTransaction();
                UnitOfWork.Save();
            }
            catch (Exception ex)
            {
                UnitOfWork.RollbackTransaction();
            }

        }

        public void UpdateTask(TaskTransferModal task, int userID)
        {
            if (task.recurrenceOptions != null)
                UpdateTask(new TaskModal()
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
    }
}
