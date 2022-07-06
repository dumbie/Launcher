using Launcher.Classes;
using Microsoft.Win32.TaskScheduler;
using System;
using System.Diagnostics;
using static Launcher.AppVariables;

namespace Launcher
{
    public class Tasks
    {
        //Run service task
        public static void TaskRun()
        {
            try
            {
                Debug.WriteLine("Running the service task: " + Launcher_TaskName);
                using (TaskService taskService = new TaskService())
                {
                    using (Task task = taskService.GetTask(Launcher_TaskName))
                    {
                        task.Run();
                    }
                }
            }
            catch { }
        }

        //Check service task
        public static TaskStatus TaskCheck()
        {
            try
            {
                Debug.WriteLine("Checking the service task: " + Launcher_TaskName);
                using (TaskService taskService = new TaskService())
                {
                    using (Task task = taskService.GetTask(Launcher_TaskName))
                    {
                        if (task == null)
                        {
                            Debug.WriteLine("The task does not exist.");
                            return TaskStatus.NotFound;
                        }
                        else
                        {
                            //Check if the application path has changed
                            if (!task.Definition.Actions.ToString().Contains(Launcher_FilePath))
                            {
                                Debug.WriteLine("Application path has changed.");
                                return TaskStatus.PathChanged;
                            }
                            else
                            {
                                Debug.WriteLine("The task should be working.");
                                return TaskStatus.Exists;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to check service task: " + ex.Message);
                return TaskStatus.Unknown;
            }
        }

        //Create service task
        public static bool TaskCreate()
        {
            try
            {
                Debug.WriteLine("Creating the service task: " + Launcher_TaskName);
                using (TaskService taskService = new TaskService())
                {
                    using (TaskDefinition taskDefinition = taskService.NewTask())
                    {
                        taskDefinition.RegistrationInfo.Description = Launcher_Description;
                        taskDefinition.RegistrationInfo.Author = Launcher_Author;
                        taskDefinition.Settings.RunOnlyIfIdle = false;
                        taskDefinition.Settings.AllowDemandStart = true;
                        taskDefinition.Settings.DisallowStartIfOnBatteries = false;
                        taskDefinition.Settings.StopIfGoingOnBatteries = false;
                        taskDefinition.Settings.ExecutionTimeLimit = new TimeSpan();
                        taskDefinition.Settings.IdleSettings.StopOnIdleEnd = false;
                        taskDefinition.Settings.AllowHardTerminate = false;
                        taskDefinition.Principal.RunLevel = TaskRunLevel.Highest;
                        taskDefinition.Actions.Add(new ExecAction(Launcher_FilePath, null, Launcher_WorkingPath));
                        taskService.RootFolder.RegisterTaskDefinition(Launcher_TaskName, taskDefinition);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to create service task: " + ex.Message);
                return false;
            }
        }

        //Remove service run task
        public static void TaskRemove(string taskName)
        {
            try
            {
                using (TaskService taskService = new TaskService())
                {
                    taskService.RootFolder.DeleteTask(taskName);
                    Debug.WriteLine("Removed service task: " + taskName);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to remove service task: " + ex.Message);
            }
        }
    }
}