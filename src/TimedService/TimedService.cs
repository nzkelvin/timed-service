using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TimedService.Config;
using TimedService.Logger;

namespace TimedService
{
    public partial class TimedService : ServiceBase
    {
        private static volatile Dictionary<string, Timer> _timers = null;
        public static volatile Object _timersLock = new Object();
        public static volatile List<string> _runningCommands = new List<string>();

        public Dictionary<string, Timer> Timers {
            get
            {
                if (_timers == null)
                {
                    lock (_timersLock)
                    {
                        if (_timers == null)
                            _timers = new Dictionary<string, Timer>();
                    }
                }
                return _timers;
            }
        }
        
        public IConfigurationHelper ConfigurationHelper { get; set; }
        private ILoggerFactory _loggerFactory;
        private ILogger _logger;

        public TimedService(IConfigurationHelper configHelper, ILoggerFactory loggerFactory)
        {
            InitializeComponent();
            ConfigurationHelper = configHelper;
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.ProduceLogger("");

        }

        protected override void OnStart(string[] args)
        {
            Debugger.Break();
            _logger.Log("Starting timed services");

            var commands = ConfigurationHelper.GetSection<TimedServicesConfig>("TimedServices").TimedServiceCommandConfigs;
            foreach (var cmd in commands)
            {
                if (cmd.RunAt.HasValue)
                {
                    Timers.Add(cmd.Name, new Timer(OnTimedEventProxy, cmd, CalculateRunAtTimeSpan(cmd.RunAt.Value), new TimeSpan(1, 0, 0, 0)));
                }
                else if (cmd.RunEvery.HasValue)
                {
                    var runEveryTimeSpan = CalculateRunEveryTimeSpan(cmd.RunEvery.Value);
                    Timers.Add(cmd.Name, new Timer(OnTimedEventProxy, cmd, runEveryTimeSpan, runEveryTimeSpan)); //start in X time span and interval in X time span too.
                }
            }
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            foreach(var timer in Timers)
            {
                timer.Value.Dispose();
            }

            _logger.Log("Ending timed services");
        }

        public void RunFromConsole()
        {
            Console.WriteLine("Starting services for manual run");

            var commandConfigs = ConfigurationHelper.GetSection<TimedServicesConfig>("TimedServices").TimedServiceCommandConfigs;
            foreach(var config in commandConfigs)
            {
                Console.WriteLine("Name: {0}", config.Name);
                Console.WriteLine("Type: {0}", config.Type);
                if (config.RunAt != null)
                {
                    Console.WriteLine("Set to run at: {0}", config.RunAt.Value);
                }

                if (config.RunEvery != null)
                {
                    TimeSpan runTimeSpan = config.RunEvery.Value - DateTime.Today;
                    Console.WriteLine("Set to run every: {0}", runTimeSpan);
                }

                if (config.Parameters != null && config.Parameters.Length > 0)
                {
                    foreach(var parameter in config.Parameters)
                    {
                        Console.WriteLine("Parameter {0} value {1}", parameter.Name, parameter.Value);
                    }   
                }

                Console.Write("Run timed service {0}? [Y/N]", config.Name);
                var key = Console.ReadKey();
                Console.WriteLine();
                if (string.Equals(key.KeyChar.ToString(CultureInfo.InvariantCulture), "Y", StringComparison.OrdinalIgnoreCase))
                {
                    OnTimedEventProxy(config);
                }
                Console.WriteLine(String.Empty.PadRight(Console.WindowWidth, '='));
            }
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        public void OnTimedEventProxy(object param)
        {
            try
            {
                var commandConfig = param as TimedServiceCommandConfig;
                var commandName = commandConfig.Name;

                var eventLogger = _loggerFactory.ProduceLogger(commandName);

                if (_runningCommands.Contains(commandName))
                {
                    eventLogger.LogException(new Exception("Process is still running from last timed event, skipping"));
                }

                // Build and run timed command
                var commandParams = commandConfig.Parameters == null
                                    ? new Dictionary<string, string>()
                                    : commandConfig.Parameters.ToDictionary(p => p.Name, p => p.Value ?? p.ValueBody);

                try
                {
                    eventLogger.Log("Starting timed event: {0}", commandName);
                    _runningCommands.Add(commandName);

                    var command = GetTimedCommandInstance(commandConfig);
                    command.OnTimedService(commandName, commandParams, eventLogger);
                }
                catch (Exception ex)
                {
                    eventLogger.LogException(ex);
                }
                finally
                {
                    _runningCommands.Remove(commandName);
                    eventLogger.Log("Ending timed event: {0}", commandName);
                    eventLogger.ClearLoggedMessages();
                }
            }
            catch (Exception e)
            {
                _logger.LogException(e, "Exception during reading command configuration object.");
            }
        }

        /// <summary>
        /// CalculateRunAtTimeSpan for the initial due time
        /// </summary>
        /// <returns></returns>
        public TimeSpan CalculateRunAtTimeSpan(DateTime runAtTime)
        {
            var now = DateTime.Now;

            // Ensure runAtTime's date is today
            runAtTime = runAtTime.Add(now.Date - runAtTime.Date);

            runAtTime = DateTime.Compare(now, runAtTime) > 0 // if now > runAtTime then runattime should be next day
                            ? runAtTime.AddDays(1)
                            : runAtTime;

            return runAtTime - now;

        }

        /// <summary>
        /// CalculateRunEveryTimeSpan for the time interval between invocations
        /// </summary>
        /// <returns></returns>
        public TimeSpan CalculateRunEveryTimeSpan(DateTime runEveryTime)
        {
            return new TimeSpan(hours:runEveryTime.Hour, minutes: runEveryTime.Minute, seconds: runEveryTime.Second);
            //return runEveryTime - DateTime.Today; // Another way to do it but this relies on the deserializer to give runEveryTime today's date as the default. 
        }

        public ITimedServiceCommand GetTimedCommandInstance(TimedServiceCommandConfig commandConfig)
        {
            var commandInstance = Activator.CreateInstance(Type.GetType(commandConfig.Type)) as ITimedServiceCommand;
            if (commandConfig == null)
                throw new Exception(string.Format("Unable to create ITimedServiceCommand for type: {0}, please confirm type.", commandConfig.Name));

            return commandInstance;
        }
    }
}
