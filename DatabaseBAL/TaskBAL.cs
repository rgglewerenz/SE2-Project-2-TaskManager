using DataAcess;
using DatabaseInterop;
using DatabaseInterop.Models;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseBAL
{
    public class TaskBAL
    {
        private readonly TaskDA _tasksDA = new TaskDA(new UnitOfWork());


        public List<TaskModal> GetTasks(int userID)
        {
            return _tasksDA.GetTasks(userID);
        }

        public List<TaskTransferModal> GetTasksTransferModals(int userID)
        {
            return _tasksDA.GetTaskTransferModals(userID);
        }

        public void AddTask(TaskTransferModal task,  int userID) {
            _tasksDA.AddTask(task,  userID);
        }

        public void UpdateTask(TaskTransferModal taskTransferModal, int userID)
        {
            _tasksDA.UpdateTask(taskTransferModal, userID);
        }
    }
}
