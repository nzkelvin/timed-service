using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using TimedService.Config;
using TimedService.Logger;
using Ninject;

namespace TimedService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            System.Diagnostics.Debugger.Break();

            var ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<IConfigurationHelper>().To<ConfigurationHelper>();
            if (Environment.UserInteractive)
            {
                ninjectKernel.Bind<ILoggerFactory>().ToConstant<ConsoleLoggerFactory>(new ConsoleLoggerFactory());
            }
            else
            {
                ninjectKernel.Bind<ILoggerFactory>().ToConstant<BasicLoggerFactory>(new BasicLoggerFactory(ninjectKernel.Get<IConfigurationHelper>()));
                //ninjectKernel.Bind<ILoggerFactory>().ToMethod(context => new BasicLoggerFactory()).InSingletonScope(); // Alternative to ToConstant
            }
            var timedService = ninjectKernel.Get<TimedService>();

            if (Environment.UserInteractive)
            {
                timedService.RunFromConsole();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
                { 
                    timedService
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
