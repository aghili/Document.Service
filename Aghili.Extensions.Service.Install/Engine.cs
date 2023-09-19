using Aghili.Extensions.Service.Install.ApplicationTypes;
using Aghili.Extensions.Service.Install.Exceptions;
using Aghili.Extensions.Service.Install.Register;
using Aghili.Extensions.Service.Install.Utilities;
using System.Reflection;
using System.Security.Principal;
using System.ServiceProcess;

namespace Aghili.Extensions.Service.Install;

public class Engine
{
    public EventHandler<ServiceResultEventArg> OnServiceResult;

    public static ServiceResult Result = new ServiceResult
    {
        Result = true
    };

    private static readonly Dictionary<EnCommand, string> Commands = new Dictionary<EnCommand, string>
    {
        {
            EnCommand.info,
            " Information about console commands."
        },
        {
            EnCommand.status,
            "Status of service state such as installation and running states."
        },
        {
            EnCommand.install,
            "Add service to this computer, if service was installed did not return error."
        },
        {
            EnCommand.uninstall,
            "Remove service from this computer, if service was uninstalled did not return error."
        },
        {
            EnCommand.firewalladd,
            "Add service file access rule to firewall."
        },
        {
            EnCommand.firewallremove,
            "Remove service file access rule from firewall."
        },
        {
            EnCommand.start,
            "Start service if exist in this computer."
        },
        {
            EnCommand.stop,
            "Stop service if exist in this computer."
        },
        {
            EnCommand.s,
            "Short command for Status."
        },
        {
            EnCommand.i,
            "Short command for install."
        },
        {
            EnCommand.u,
            "Short command for unistall."
        },
        {
            EnCommand.fa,
            "Short command for FirewallAdd."
        },
        {
            EnCommand.fr,
            "Short command for FirewallRemove."
        }
    };

    private static readonly Dictionary<EnArgument, string> Arguments = new Dictionary<EnArgument, string>
    {
        {
            EnArgument.silent,
            "Run command in silent mode."
        },
        {
            EnArgument.out_json,
            "[Default]output result as json string."
        }
    };

    private static readonly Dictionary<EnCommand, List<EnArgument>> CommandArguments = new Dictionary<EnCommand, List<EnArgument>>
    {
        {
            EnCommand.install,
            new List<EnArgument>()
        },
        {
            EnCommand.uninstall,
            new List<EnArgument>()
        },
        {
            EnCommand.info,
            new List<EnArgument>()
        },
        {
            EnCommand.status,
            new List<EnArgument>()
        },
        {
            EnCommand.firewalladd,
            new List<EnArgument>()
        },
        {
            EnCommand.firewallremove,
            new List<EnArgument>()
        },
        {
            EnCommand.start,
            new List<EnArgument>()
        },
        {
            EnCommand.stop,
            new List<EnArgument>()
        },
        {
            EnCommand.output,
            new List<EnArgument>
            {
                EnArgument.silent,
                EnArgument.out_json
            }
        }
    };

    private const int ACTION_DONE = 1;

    private const int ACTION_ERROR = -100;

    private const int ACTION_IS_NOT_CALL = 0;

    private const int ACTION_TERMINATE_PROGRAM = -1;

    private ServiceController sc => ((IEnumerable<ServiceController>)ServiceController.GetServices()).FirstOrDefault((Func<ServiceController, bool>)((ServiceController s) => s.ServiceName.Equals(_application.Name, StringComparison.InvariantCultureIgnoreCase)));

    private static IEngineType _application { get; set; }

    public string ServiceName => _application.Name;

    public Engine(string applicationName)
    {
        _application = new EngineTypeService(applicationName);
    }

    public Engine()
    {
        _application = new EngineTypeApplication();
    }

    public void Run(string[] args)
    {
        List<ActionArgument> list = ExtractActionArgument(args);
        foreach (MethodInfo item in from i in GetType().GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).AsQueryable()
                                    where i.Name.Contains("ServiceStep")
                                    select i)
        {
            object obj = item.Invoke(this, new object[1] { list });
            if (!(obj is int))
            {
                continue;
            }

            switch ((int)obj)
            {
                case -1:
                    return;
                case -100:
                    try
                    {
                        ServiceStepOutput(new List<ActionArgument>());
                    }
                    catch
                    {
                        Console.WriteLine(ServiceStatus().ToStringJson());
                    }

                    return;
            }
        }
    }

    private List<ActionArgument> ExtractActionArgument(string[] args)
    {
        List<ActionArgument> list = new List<ActionArgument>();
        List<string> list2 = args.ToList();
        Stack<string> args2 = new Stack<string>();
        list2.Reverse();
        foreach (string item in list2)
        {
            args2.Push(item);
        }

        ActionArgument actionArgument = null;
        while (args2.Count > 0 && (actionArgument = new ActionArgument(ref args2)) != null && actionArgument.Command != EnCommand.none)
        {
            list.Add(actionArgument);
        }

        return list;
    }

    private void CheckRequirement()
    {
        if (!IsRunAsAdministrator())
        {
            throw new AdministratorAccessNeedException("Run program with administrator user access!");
        }
    }

    private bool IsRunAsAdministrator()
    {
        return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
    }

    private EnArgument getOutputType(List<EnArgument> arguments)
    {
        if (arguments.Contains(EnArgument.silent))
        {
            return EnArgument.silent;
        }

        if (arguments.Contains(EnArgument.out_xml))
        {
            return EnArgument.out_xml;
        }

        if (arguments.Contains(EnArgument.out_object))
        {
            return EnArgument.out_object;
        }

        return EnArgument.out_json;
    }

    private void ShowInfo()
    {
        Console.WriteLine("Service manager ver " + GetAssemblyVersion() + "\nExtention for manage windows service by himself.\n\nCommands:\n");
        foreach (KeyValuePair<EnCommand, string> command in Commands)
        {
            Console.WriteLine($"\t{command.Key}\t{command.Value}");
        }

        Console.WriteLine("\nArguments:\n");
        foreach (KeyValuePair<EnArgument, string> argument in Arguments)
        {
            Console.WriteLine($"\t{argument.Key}\t{argument.Value}");
        }
    }

    private string GetAssemblyVersion()
    {
        return typeof(Engine).Assembly.GetName().Version.ToString();
    }

    private List<EnArgument> Extractarguments(string[] args)
    {
        List<EnArgument> list = new List<EnArgument>();
        foreach (string item in args)
        {
            try
            {
                EnArgument result = EnArgument.silent;
                if (Enum.TryParse<EnArgument>(RemovePrefixs(item), out result))
                {
                    list.Add(result);
                }
            }
            catch
            {
            }
        }

        return list;
    }

    private string RemovePrefixs(string item)
    {
        return item.Trim('/', '-', ' ');
    }

    private List<EnCommand> ExtractCommands(string[] args)
    {
        List<EnCommand> list = new List<EnCommand>();
        foreach (string item in args)
        {
            try
            {
                EnCommand result = EnCommand.info;
                if (Enum.TryParse<EnCommand>(RemovePrefixs(item), out result))
                {
                    list.Add(result);
                }
            }
            catch
            {
            }
        }

        return list;
    }

    public void UninstallAndInstallServiceASync()
    {
        new Thread((ThreadStart)delegate
        {
            UninstallAndInstallService();
        }).Start();
    }

    public void ServiceStopAsync()
    {
        new Thread((ThreadStart)delegate
        {
            ServiceStop();
        }).Start();
    }

    public void ServiceStartAsync()
    {
        new Thread((ThreadStart)delegate
        {
            ServiceStart();
        }).Start();
    }

    public ServiceResult UninstallAndInstallService()
    {
        ServiceResult serviceResult = new ServiceResult
        {
            ServiceIsInstalled = false,
            ServiceRunStatus = (ServiceControllerStatus)1
        };
        try
        {
            try
            {
                ServiceUninstall();
            }
            catch
            {
            }

            ServiceInstall();
            serviceResult.ServiceIsInstalled = sc != null;
            serviceResult.ServiceRunStatus = (ServiceControllerStatus)((sc == null) ? 1 : ((int)sc.Status));
        }
        catch (Exception ex)
        {
            serviceResult.Message = ex.Message;
        }

        try
        {
            EventHandler<ServiceResultEventArg> onServiceResult = OnServiceResult;
            if (onServiceResult != null)
            {
                onServiceResult(this, new ServiceResultEventArg(serviceResult));
                return serviceResult;
            }

            return serviceResult;
        }
        catch
        {
            return serviceResult;
        }
    }

    private ServiceResult ServiceInstall()
    {
        new WindowsService(new Register.WindowsServices.WindowsServiceInformation(ApplicationInfo.ProcessPath, ServiceName)).Install();

        return ServiceStatus();
    }

    public ServiceResult ServiceStatus()
    {
        try
        {
            var Service = new FirewallService();
            Result.ServiceIsInstalled = sc != null;
            Result.ServiceRunStatus = (ServiceControllerStatus)((sc == null) ? 1 : ((int)sc.Status));
            Result.FirewallIsEnable = Service.IsFirewallEnabled;
            Result.FirewallIsInstall = Service.IsFirewallInstalled;
            Result.AppAuthorizationsAllowed = Service.AppAuthorizationsAllowed;
            try
            {
                Result.FirewallRuleAdded = Service.HasAuthorization(ApplicationInfo.ProcessPath);
            }
            catch (Exception ex)
            {
                Result.Message = ex.Message;
                Result.Result = false;
                Result.FirewallRuleAdded = false;
            }
        }
        catch (Exception ex2)
        {
            Result.Message = ex2.Message;
            Result.Result = false;
        }

        return Result;
    }

    private ServiceResult ServiceUninstall()
    {
        if (sc != null)
        {
            if ((int)sc.Status != 1)
            {
                sc.Stop();
                sc.WaitForStatus((ServiceControllerStatus)1);
            }

            new WindowsService(new Register.WindowsServices.WindowsServiceInformation(ApplicationInfo.ProcessPath, ServiceName)).Uninstall();

        }

        return ServiceStatus();
    }

    public ServiceResult ServiceStop()
    {
        Result.ServiceIsInstalled = sc != null;
        if (Result.ServiceIsInstalled)
        {
            if ((int)sc.Status != 4)
            {
                return Result;
            }

            sc.Stop();
            sc.WaitForStatus((ServiceControllerStatus)1, TimeSpan.FromSeconds(10.0));
            Result.ServiceRunStatus = sc.Status;
        }

        try
        {
            Result = ServiceStatus();
            OnServiceResult?.Invoke(this, new ServiceResultEventArg(Result));
        }
        catch
        {
        }

        return Result;
    }

    public ServiceResult ServiceStart()
    {
        Result.ServiceIsInstalled = sc != null;
        if (Result.ServiceIsInstalled)
        {
            if ((int)sc.Status != 1)
            {
                return Result;
            }

            sc.Start();
            sc.WaitForStatus((ServiceControllerStatus)4, TimeSpan.FromSeconds(10.0));
            Result.ServiceRunStatus = sc.Status;
        }

        try
        {
            Result = ServiceStatus();
            OnServiceResult?.Invoke(this, new ServiceResultEventArg(Result));
        }
        catch
        {
        }

        return Result;
    }

    public ServiceResult FirewallAdd()
    {
        var Service = new FirewallService();
        if (!Service.HasAuthorization(ApplicationInfo.ProcessPath))
            try
            {
                Service.GrantAuthorization(ApplicationInfo.ProcessPath, _application.Name);
            }
            catch (ExceptionFirewallHelper)
            {

            }
        try
        {
            Result = ServiceStatus();
            OnServiceResult?.Invoke(this, new ServiceResultEventArg(Result));
        }
        catch
        {
        }

        return Result;
    }

    public ServiceResult FirewallRemove()
    {
        var Service = new FirewallService();
        if (Service.HasAuthorization(ApplicationInfo.ProcessPath))
        {
            Service.RemoveAuthorization(ApplicationInfo.ProcessPath);
        }

        try
        {
            Result = ServiceStatus();
            OnServiceResult?.Invoke(this, new ServiceResultEventArg(Result));
        }
        catch
        {
        }

        return Result;
    }

    protected int ServiceStepInfo(List<ActionArgument> Actions)
    {
        if (Actions.Count == 0 || Actions.Any((ActionArgument i) => i.Command == EnCommand.info))
        {
            ShowInfo();
            return 1;
        }

        return 0;
    }

    protected int ServiceStepInstall(List<ActionArgument> Actions)
    {
        try
        {
            if (Actions.Any((ActionArgument i) => i.Command == EnCommand.install) || Actions.Any((ActionArgument i) => i.Command == EnCommand.i))
            {
                CheckRequirement();
                ServiceInstall();
                return 1;
            }

            return 0;
        }
        catch (Exception ex)
        {
            Result.Result = false;
            Result.Message = ex.Message;
            return -100;
        }
    }

    protected int ServiceStepUninstall(List<ActionArgument> Actions)
    {
        try
        {
            if (Actions.Any((ActionArgument i) => i.Command == EnCommand.uninstall) || Actions.Any((ActionArgument i) => i.Command == EnCommand.u))
            {
                CheckRequirement();
                ServiceUninstall();
                return 1;
            }

            return 0;
        }
        catch (Exception ex)
        {
            Result.Result = false;
            Result.Message = ex.Message;
            return -100;
        }
    }

    protected int ServiceStepStart(List<ActionArgument> Actions)
    {
        try
        {
            if (Actions.Any((ActionArgument i) => i.Command == EnCommand.start))
            {
                CheckRequirement();
                ServiceStart();
                return 1;
            }

            return 0;
        }
        catch (Exception ex)
        {
            Result.Result = false;
            Result.Message = ex.Message;
            return -100;
        }
    }

    protected int ServiceStepStop(List<ActionArgument> Actions)
    {
        try
        {
            if (Actions.Any((ActionArgument i) => i.Command == EnCommand.stop))
            {
                CheckRequirement();
                ServiceStop();
                return 1;
            }

            return 0;
        }
        catch (Exception ex)
        {
            Result.Result = false;
            Result.Message = ex.Message;
            return -100;
        }
    }

    protected int ServiceStepFirewallAdd(List<ActionArgument> Actions)
    {
        try
        {
            if (Actions.Any((ActionArgument i) => i.Command == EnCommand.firewalladd || i.Command == EnCommand.fa))
            {
                CheckRequirement();
                FirewallAdd();
                return 1;
            }

            return 0;
        }
        catch (Exception ex)
        {
            Result.Result = false;
            Result.Message = ex.Message;
            return -100;
        }
    }

    protected int ServiceStepFirewallRemove(List<ActionArgument> Actions)
    {
        try
        {
            if (Actions.Any((ActionArgument i) => i.Command == EnCommand.firewallremove || i.Command == EnCommand.fr))
            {
                CheckRequirement();
                FirewallRemove();
                return 1;
            }

            return 0;
        }
        catch (Exception ex)
        {
            Result.Result = false;
            Result.Message = ex.Message;
            return -100;
        }
    }

    protected int ServiceStepStatus(List<ActionArgument> Actions)
    {
        try
        {
            if (Actions.Any((ActionArgument i) => i.Command == EnCommand.s || i.Command == EnCommand.status))
            {
                ServiceStatus();
                return 1;
            }

            return 0;
        }
        catch (Exception ex)
        {
            Result.Result = false;
            Result.Message = ex.Message;
            return -100;
        }
    }

    protected int ServiceStepOutput(List<ActionArgument> Actions)
    {
        try
        {
            EnArgument enArgument = EnArgument.out_json;
            if (Actions.Any((ActionArgument i) => i.Command == EnCommand.output))
            {
                enArgument = Actions.Find((ActionArgument i) => i.Command == EnCommand.output)!.Arguments.Keys.First();
            }

            if (enArgument != EnArgument.silent)
            {
                ServiceResult result = Result;
                switch (enArgument)
                {
                    case EnArgument.out_xml:
                        Console.WriteLine(result.ToStringXml());
                        break;
                    case EnArgument.out_object:
                        Console.WriteLine(result.ToSerialize());
                        break;
                    default:
                        Console.WriteLine(result.ToStringJson());
                        break;
                    case EnArgument.silent:
                        break;
                }

                return 1;
            }

            return 0;
        }
        catch (Exception ex)
        {
            Result.Result = false;
            Result.Message = ex.Message;
            return -100;
        }
    }
}