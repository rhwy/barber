using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Args;
using System.IO.Pipes;
using System.Text.RegularExpressions;
using System.IO;
using NLog;
using ArtOfNet.NamedPipesHelper;
using System.Timers;
using System.Diagnostics;

namespace ArtOfNet.ACR
{
    public abstract class CommandLineApplication
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        protected string[] _args;
        public string[] ApplicationArgumentsRaw
        {
            get
            {
                return _args;
            }
        }

        private string _executable;

        public string Executable
        {
            get
            {
                return _executable;
            }
        }

        public ApplicationContext ApplicationContext {get;private set;}

        /// <summary>
        /// Contruct without parameters : try to get arguments from
        /// command line invocation
        /// </summary>
        protected CommandLineApplication()
        {
            _args = Environment.GetCommandLineArgs().Skip(1).ToArray();
            _executable = Environment.GetCommandLineArgs().Take(1).Single();
        }

        /// <summary>
        /// Construct with a list of arguments
        /// </summary>
        /// <param name="args"></param>
        public CommandLineApplication(string[] args)
        {
            _args = args;
            _executable = Environment.GetCommandLineArgs().Take(1).Single();
        }

        public void Run()
        {
            _logger.Info("Start Runing application");
            _logger.Info("Initialize Components"); 
            InitializeComponents();
            _logger.Info("Define Start mode");
            DefineStartMode();
            _logger.Info("Application start event"); 
            InvokerHelper.InvokeSpecialMethod(this, "ApplicationStart");
            _logger.Info("Start runing internal processes"); 
            Process();
            _logger.Info("Application end event"); 
            InvokerHelper.InvokeSpecialMethod(this, "ApplicationEnd");
        }

        private void InitializeComponents()
        {
            ControllerFactory.Initialize();
            _logger.Info("Found {0} controllers : {1}",
                ControllerFactory.Controllers.Count,
                string.Join(",", ControllerFactory.Controllers.Keys)); 
            
        }

        private void DefineStartMode()
        {
            IModelBindingDefinition<ServerModeArgumentModel> definition =
                Args.Configuration.Configure<ServerModeArgumentModel>();
            
            ServerModeArgumentModel model = null;
           

            try
            {
                model = definition.CreateAndBind(_args);
                ApplicationContext = new ApplicationContext(_args);
                ApplicationContext.StartMode = model;
                
                
            }
            catch
            {
                ApplicationContext = new ApplicationContext(_args);
                ApplicationContext.StartMode = new ServerModeArgumentModel(){ ServerMode = ServerMode.StandAlone,StartOptions = string.Empty};
            }
            _logger.Info("Start Mode : {0}",ApplicationContext.StartMode.ServerMode);

            if (ApplicationContext.StartMode.StartOptions != null && ApplicationContext.StartMode.StartOptions.Contains("wait"))
            {
                //Timer t = new Timer(5000);
                ////t.Elapsed += new ElapsedEventHandler(t_Elapsed);
                //bool waitForTimer = true;
                //t.Elapsed += (s,e) => {waitForTimer = false; };
                //while (waitForTimer)
                //{

                //}
                _logger.Info("Please attach a debugger to continue...");
                while (!Debugger.IsAttached)
                {
                    
                }
                _logger.Info("Debugger attached, let's continue");
            }
            if (_args[0] == model.ServerMode.ToString())
            {
                int start = 1;
                if (model != null && !string.IsNullOrEmpty(model.StartOptions) && _args.Length >= 3)
                {
                    start = 3;

                }
                List<string> newargs = new List<string>();
                for (int i = start; i < _args.Length; i++)
                {
                    newargs.Add(_args[i]);
                }
                _args = newargs.ToArray();
                ApplicationContext.Args = _args;
            }
            
        }

        private void Process()
        {
            ControllerResult result = null;

            switch (ApplicationContext.StartMode.ServerMode)
            {
                case ServerMode.NamedPipes:
                    result = ProcessNamedPipes();
                    break;
                case ServerMode.Rest:
                    break;
                case ServerMode.StandAlone:
                    result = ProcessStandardMode();
                    break;
                default:
                    break;
            }

            if (result is StringResult)
            {
                StringResult stringResult = (StringResult)result;
                Console.WriteLine(stringResult.Model);
                if (stringResult.Paused)
                {
                    Console.Write("Press Enter to continue...");
                    Console.Read();
                }
            }
            else if (result is FileResult)
            {
                FileResult fileresult = (FileResult)result;
                File.WriteAllText(fileresult.Path,fileresult.Content);
            }
        }

        private static readonly object _sync = new object();

        void Server_OnReceivedMessage(object sender, ReceivedMessageEventArgs e)
        {
            
            int CrlfPosition = e.Message.LastIndexOf("\r\n");
            e.Message = e.Message.Remove(CrlfPosition);
            if (e.Message.Contains("EXEC"))
            {
                lock (_sync)
                {
                    _waitForCommand = false;
                }
                
                _logger.Info("READY TO EXEC", e.Message);
            }
            else
            {
                _clientArgs.Add(e.Message);
                _logger.Info("RCV : {0}", e.Message);
            }
        }

        private bool _waitForCommand = true;

        private List<string> _clientArgs = new List<string>();

        private ControllerResult ProcessNamedPipes()
        {
            _logger.Info("Entering NamedPipes Mode");

            NamedPipeServer Server = new NamedPipeServer("InProc");
            Server.OnReceivedMessage += new EventHandler<ReceivedMessageEventArgs>(Server_OnReceivedMessage);
            Server.Start();
            
            bool waitForCommand = true;
            while (waitForCommand)
            {
                waitForCommand = _waitForCommand;
            }
            ApplicationContext.Args = _clientArgs.ToArray();
            ControllerResult result = ProcessStandardMode();
            if (result is StringResult)
            {
                Server.Write(((StringResult)result).Model);
            }
            return result;
        }

        
        private ControllerResult ProcessStandardMode()
        {
            IModelBindingDefinition<ControllerSelectionArgumentModel> definition = 
                Args.Configuration.Configure<ControllerSelectionArgumentModel>();
            ControllerSelectionArgumentModel model;

            try
            {
                model = definition.CreateAndBind(ApplicationContext.Args);
                //model = definition.CreateAndBind(_args);
                if (ApplicationContext.Args.First().ToLower().Contains(model.ControllerName))
                {
                    ApplicationContext.Args = ApplicationContext.Args.Skip(1).ToArray();
                }
            }
            catch
            {
                model = new ControllerSelectionArgumentModel();
                model.ControllerName = "Render";
            }
            
            //IRtcController controller;
            ControllerResult result;
            
            switch (model.ControllerName.ToLowerInvariant())
            {
                case "help":
                    HelpController controllerHelp = new HelpController();
                    controllerHelp.ArgumentModel = Args.Configuration.Configure<HelpArgumentModel>().CreateAndBind(ApplicationContext.Args);
                    result = controllerHelp.Execute();
                    break;
                case "install":
                    InstallController controllerInstall = new InstallController();
                    //IModelBindingDefinition<InstallArgumentModel> controllerDefinition =
                    //    Args.Configuration.Configure<InstallArgumentModel>();
                    controllerInstall.ArgumentModel = Args.Configuration.Configure<InstallArgumentModel>().CreateAndBind(ApplicationContext.Args);
                    result = controllerInstall.Execute();
                    break;
                default:
                    RenderController controllerRender = new RenderController();
                    controllerRender.Context = ApplicationContext;

                    IModelBindingDefinition<RenderArgumentModel> definitionRender = Args.Configuration.Configure<RenderArgumentModel>();
                    //IModelBindingDefinition<RenderArgumentModel> controllerDefinition =
                    //    Args.Configuration.Configure<RenderArgumentModel>();
                    string[] controllerArgs = (string[])ApplicationContext.Args.Clone();
                    controllerRender.ArgumentModel = definitionRender.CreateAndBind(controllerArgs);
                    _logger.Info("Start Rendreding with arguments:");
                    _logger.Info("Template Content : " + controllerRender.ArgumentModel.TemplateContent);
                    _logger.Info("Model Content : " + controllerRender.ArgumentModel.ModelContent);
                    _logger.Info("Result path : " + controllerRender.ArgumentModel.ResultFile);
                    result = controllerRender.Execute();
                    break;
            }

            
            return result;
        }



    }
}
