﻿using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static ArnoldVinkCode.AVCertificate;
using static ArnoldVinkCode.AVFirewall;
using static ArnoldVinkCode.AVFunctions;
using static ArnoldVinkCode.AVTaskScheduler;
using static Launcher.AppVariables;

namespace Launcher
{
    public partial class App
    {
        //Application startup code
        private async System.Threading.Tasks.Task AppStartup()
        {
            try
            {
                //Set application details
                if (!Details.AppSetDetails())
                {
                    //Show launcher message
                    List<string> messageAnswers = new List<string>();
                    messageAnswers.Add("Ok");
                    string messageResult = await new AVMessageBox().Popup(null, "Application Launcher", "No executable found to launch, please check your installation.", messageAnswers);
                    return;
                }

                //Check process administrator permission
                bool administratorPermission = ProcessCheckAdminPermission();

                //Check current task status
                TaskStatus taskStatus = TaskCheck(Launcher_TaskName, Launcher_LauncherExePath);

                Debug.WriteLine("Target application: " + Launcher_TargetName + " (" + Launcher_TargetExePath + ")");
                Debug.WriteLine("Admin permission: " + administratorPermission);
                Debug.WriteLine("Task status: " + taskStatus);

                //Check administrator permission
                if (administratorPermission)
                {
                    //Check task status
                    if (taskStatus == TaskStatus.Unknown || taskStatus == TaskStatus.TaskNotFound || taskStatus == TaskStatus.ExeNotFound || taskStatus == TaskStatus.PathChanged)
                    {
                        //Create service task
                        TaskCreate(Launcher_TaskName, Launcher_Description, Launcher_Author, Launcher_LauncherExePath, Launcher_LauncherRootPath);
                    }

                    //Install certificate
                    byte[] certificateBytes = AVEmbedded.EmbeddedResourceToBytes(null, "Launcher.Certificate.ArnoldVinkCertificate.cer");
                    InstallCertificate(certificateBytes);

                    //Allow application in firewall
                    Firewall_ExecutableAllow(Launcher_TargetName, Launcher_TargetExePath, true);

                    //Registry enable uiaccess
                    bool secureUIAEnabled = Registry.SecureUIAPathsCheck();
                    if (!secureUIAEnabled)
                    {
                        Registry.SecureUIAPathsAllow();
                        await System.Threading.Tasks.Task.Delay(500);
                    }

                    //Run application executable
                    if (!AVProcess.Launch_ShellExecute(Launcher_TargetExeName, "", "", true))
                    {
                        //Show launcher message
                        List<string> messageAnswers = new List<string>();
                        messageAnswers.Add("Ok");
                        await new AVMessageBox().Popup(null, "Application Launcher", "Failed launching executable, please check your installation.", messageAnswers);
                    }

                    //Registry disable uiaccess
                    if (!secureUIAEnabled)
                    {
                        await System.Threading.Tasks.Task.Delay(5000);
                        Registry.SecureUIAPathsBlock();
                    }
                }
                else
                {
                    //Check task status
                    if (taskStatus == TaskStatus.Unknown || taskStatus == TaskStatus.TaskNotFound || taskStatus == TaskStatus.ExeNotFound || taskStatus == TaskStatus.PathChanged)
                    {
                        //Show launcher message
                        List<string> messageAnswers = new List<string>();
                        messageAnswers.Add("Continue");
                        messageAnswers.Add("Cancel");

                        string messageResult = await new AVMessageBox().Popup(null, Launcher_Description, "It seems like this is the first time you are using the launcher or the application path has changed so you will have to accept the upcoming administrator prompt, after that you will be able to run this launcher without the administrator prompt.", messageAnswers);
                        if (messageResult == "Continue")
                        {
                            //Run application executable
                            if (!AVProcess.Launch_ShellExecute(Launcher_LauncherExePath, "", "", true))
                            {
                                //Show launcher message
                                List<string> messageAnswersAdmin = new List<string>();
                                messageAnswersAdmin.Add("Ok");
                                await new AVMessageBox().Popup(null, "Application Launcher", "Failed restarting as administrator, please check your installation.", messageAnswersAdmin);
                                return;
                            }
                        }
                    }
                    else
                    {
                        //Run service task
                        await TaskRun(Launcher_TaskName, string.Empty, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Launcher error: " + ex.Message);

                //Show launcher message
                List<string> messageAnswers = new List<string>();
                messageAnswers.Add("Ok");
                await new AVMessageBox().Popup(null, "Application Launcher", "Launcher error: " + ex.Message, messageAnswers);
            }
            finally
            {
                //Shutdown application
                Environment.Exit(0);
            }
        }
    }
}