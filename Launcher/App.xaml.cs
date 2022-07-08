using ArnoldVinkCode;
using Launcher.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using static Launcher.AppVariables;

namespace Launcher
{
    public partial class App : Application
    {
        //Application Startup
        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                //Resolve missing assembly dll files
                AppDomain.CurrentDomain.AssemblyResolve += AppAssemblyResolve;

                //Set application details
                if (!AppSetDetails())
                {
                    //Show launcher message
                    List<string> messageAnswers = new List<string>();
                    messageAnswers.Add("Ok");

                    string messageResult = await new AVMessageBox().Popup(null, "Application Launcher", "No executable found to launch, please check your installation.", messageAnswers);
                    return;
                }

                //Check admin permission
                bool administratorPermission = Administrator.AdminCheck();

                //Check current task status
                TaskStatus taskStatus = Tasks.TaskCheck();

                Debug.WriteLine("Launching application: " + Launcher_AppName);
                Debug.WriteLine("Admin permission: " + administratorPermission);
                Debug.WriteLine("Task status: " + taskStatus);

                //Check administrator permission
                if (administratorPermission)
                {
                    //Check task status
                    if (taskStatus == TaskStatus.NotFound || taskStatus == TaskStatus.Unknown || taskStatus == TaskStatus.PathChanged)
                    {
                        //Create service task
                        Tasks.TaskCreate();
                    }

                    //Install certificate
                    byte[] certificateBytes = await EmbeddedResources.EmbeddedResourceToBytes("Launcher.Certificate.ArnoldVinkCertificate.cer");
                    Certificate.InstallCertificate(certificateBytes);

                    //Registry enable uiaccess
                    bool secureUIAEnabled = Registry.SecureUIAPathsCheck();
                    if (!secureUIAEnabled)
                    {
                        Registry.SecureUIAPathsAllow();
                        await System.Threading.Tasks.Task.Delay(500);
                    }

                    //Run application executable
                    if (!Processes.ProcessStartAdmin(Launcher_ExecutableFile))
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
                    if (taskStatus == TaskStatus.NotFound || taskStatus == TaskStatus.Unknown || taskStatus == TaskStatus.PathChanged)
                    {
                        //Show launcher message
                        List<string> messageAnswers = new List<string>();
                        messageAnswers.Add("Continue");
                        messageAnswers.Add("Cancel");

                        string messageResult = await new AVMessageBox().Popup(null, Launcher_Description, "It seems like this is the first time you are using the launcher or the application path has changed so you will have to accept the upcoming administrator prompt, after that you will be able to run this launcher without the administrator prompt.", messageAnswers);
                        if (messageResult == "Continue")
                        {
                            //Run application executable
                            if (!Processes.ProcessStartAdmin(Launcher_FilePath))
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
                        Tasks.TaskRun();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Launcher error: " + ex.Message);
            }
            finally
            {
                //Shutdown application
                Environment.Exit(0);
            }
        }
    }
}