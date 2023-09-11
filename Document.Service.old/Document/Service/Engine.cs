// Decompiled with JetBrains decompiler
// Type: Document.Service.Engine
// Assembly: Document.Service, Version=0.12.1.0, Culture=neutral, PublicKeyToken=9c8f15b9f8ae24cc
// MVID: 397D359F-A322-4ECB-BA44-B758DFC28E5C
// Assembly location: D:\Mahak\Tools\MahakTray\Document.Service.dll

using Document.Service.ApplicationTypes;
using Document.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Principal;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;

namespace Document.Service
{
  public class Engine
  {
    public EventHandler<ServiceResultEventArg> OnServiceResult;
    public static ServiceResult Result = new ServiceResult()
    {
      Result = true
    };
    private static readonly Dictionary<EnCommand, string> Commands = new Dictionary<EnCommand, string>()
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
    private static readonly Dictionary<EnArgument, string> Arguments = new Dictionary<EnArgument, string>()
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
    private static readonly Dictionary<EnCommand, List<EnArgument>> CommandArguments = new Dictionary<EnCommand, List<EnArgument>>()
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
        new List<EnArgument>()
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

    private ServiceController sc => ((IEnumerable<ServiceController>) ServiceController.GetServices()).FirstOrDefault<ServiceController>((Func<ServiceController, bool>) (s => s.ServiceName.Equals(Engine._application.Name, StringComparison.InvariantCultureIgnoreCase)));

    private static IEngineType _application { get; set; }

    public Engine(ServiceBase application) => Engine._application = (IEngineType) new EngineTypeService(application);

    public Engine() => Engine._application = (IEngineType) new EngineTypeApplication();

    public void Run(string[] args)
    {
      List<ActionArgument> actionArgument = this.ExtractActionArgument(args);
      IQueryable<MethodInfo> source = ((IEnumerable<MethodInfo>) this.GetType().GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).AsQueryable<MethodInfo>();
      Expression<Func<MethodInfo, bool>> predicate = (Expression<Func<MethodInfo, bool>>) (i => i.Name.Contains("ServiceStep"));
      foreach (MethodBase methodBase in (IEnumerable<MethodInfo>) source.Where<MethodInfo>(predicate))
      {
        if (methodBase.Invoke((object) this, new object[1]
        {
          (object) actionArgument
        }) is int num)
        {
          switch (num)
          {
            case -100:
              try
              {
                this.ServiceStepOutput(new List<ActionArgument>());
                return;
              }
              catch
              {
                Console.WriteLine(this.ServiceStatus().ToStringJson());
                return;
              }
            case -1:
              return;
            default:
              continue;
          }
        }
      }
    }

    private List<ActionArgument> ExtractActionArgument(string[] args)
    {
      List<ActionArgument> actionArgument1 = new List<ActionArgument>();
      List<string> list = ((IEnumerable<string>) args).ToList<string>();
      Stack<string> args1 = new Stack<string>();
      list.Reverse();
      foreach (string str in list)
        args1.Push(str);
      ActionArgument actionArgument2;
      while (args1.Count > 0 && (actionArgument2 = new ActionArgument(ref args1)) != null && actionArgument2.Command != EnCommand.none)
        actionArgument1.Add(actionArgument2);
      return actionArgument1;
    }

    private void CheckRequirement()
    {
      if (!this.IsRunAsAdministrator())
        throw new AdministratorAccessNeedException("Run program with administrator user access!");
    }

    private bool IsRunAsAdministrator() => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

    private EnArgument getOutputType(List<EnArgument> arguments)
    {
      if (arguments.Contains(EnArgument.silent))
        return EnArgument.silent;
      if (arguments.Contains(EnArgument.out_xml))
        return EnArgument.out_xml;
      return arguments.Contains(EnArgument.out_object) ? EnArgument.out_object : EnArgument.out_json;
    }

    private void ShowInfo()
    {
      Console.WriteLine("Service manager ver " + this.GetAssemblyVersion() + "\nExtention for manage windows service by himself.\n\nCommands:\n");
      foreach (KeyValuePair<EnCommand, string> command in Engine.Commands)
        Console.WriteLine(string.Format("\t{0}\t{1}", (object) command.Key, (object) command.Value));
      Console.WriteLine("\nArguments:\n");
      foreach (KeyValuePair<EnArgument, string> keyValuePair in Engine.Arguments)
        Console.WriteLine(string.Format("\t{0}\t{1}", (object) keyValuePair.Key, (object) keyValuePair.Value));
    }

    private string GetAssemblyVersion() => typeof (Engine).Assembly.GetName().Version.ToString();

    private List<EnArgument> Extractarguments(string[] args)
    {
      List<EnArgument> enArgumentList = new List<EnArgument>();
      foreach (string str in args)
      {
        try
        {
          EnArgument result = EnArgument.silent;
          if (Enum.TryParse<EnArgument>(this.RemovePrefixs(str), out result))
            enArgumentList.Add(result);
        }
        catch
        {
        }
      }
      return enArgumentList;
    }

    private string RemovePrefixs(string item) => item.Trim('/', '-', ' ');

    private List<EnCommand> ExtractCommands(string[] args)
    {
      List<EnCommand> commands = new List<EnCommand>();
      foreach (string str in args)
      {
        try
        {
          EnCommand result = EnCommand.info;
          if (Enum.TryParse<EnCommand>(this.RemovePrefixs(str), out result))
            commands.Add(result);
        }
        catch
        {
        }
      }
      return commands;
    }

    public void UnistallAndInstallServiceASync() => new Thread((ThreadStart) (() => this.UnistallAndInstallService())).Start();

    public void ServiceStopAsync() => new Thread((ThreadStart) (() => this.ServiceStop())).Start();

    public void ServiceStartAsync() => new Thread((ThreadStart) (() => this.ServiceStart())).Start();

    public ServiceResult UnistallAndInstallService()
    {
      ServiceResult result = new ServiceResult()
      {
        ServiceIsInstalled = false,
        ServiceRunStatus = ServiceControllerStatus.Stopped
      };
      try
      {
        try
        {
          this.ServiceUninstall();
        }
        catch
        {
        }
        this.ServiceInstall();
        result.ServiceIsInstalled = this.sc != null;
        result.ServiceRunStatus = this.sc == null ? ServiceControllerStatus.Stopped : this.sc.Status;
      }
      catch (Exception ex)
      {
        result.Message = ex.Message;
      }
      try
      {
        EventHandler<ServiceResultEventArg> onServiceResult = this.OnServiceResult;
        if (onServiceResult != null)
          onServiceResult((object) this, new ServiceResultEventArg(result));
      }
      catch
      {
      }
      return result;
    }

    private ServiceResult ServiceInstall()
    {
      if (this.sc == null)
        ManagedInstallerClass.InstallHelper(new string[2]
        {
          Application.ExecutablePath ?? "",
          "/LogToConsole=false"
        });
      return this.ServiceStatus();
    }

    public ServiceResult ServiceStatus()
    {
      try
      {
        Engine.Result.ServiceIsInstalled = this.sc != null;
        Engine.Result.ServiceRunStatus = this.sc == null ? ServiceControllerStatus.Stopped : this.sc.Status;
        Engine.Result.FirewallIsEnable = FirewallHelper.Instance.IsFirewallEnabled;
        Engine.Result.FirewallIsInstall = FirewallHelper.Instance.IsFirewallInstalled;
        Engine.Result.AppAuthorizationsAllowed = FirewallHelper.Instance.AppAuthorizationsAllowed;
        try
        {
          Engine.Result.FirewallRuleAdded = FirewallHelper.Instance.HasAuthorization(Application.ExecutablePath);
        }
        catch (Exception ex)
        {
          Engine.Result.Message = ex.Message;
          Engine.Result.Result = false;
          Engine.Result.FirewallRuleAdded = false;
        }
      }
      catch (Exception ex)
      {
        Engine.Result.Message = ex.Message;
        Engine.Result.Result = false;
      }
      return Engine.Result;
    }

    private ServiceResult ServiceUninstall()
    {
      if (this.sc != null)
      {
        if (this.sc.Status != ServiceControllerStatus.Stopped)
        {
          this.sc.Stop();
          this.sc.WaitForStatus(ServiceControllerStatus.Stopped);
        }
        ManagedInstallerClass.InstallHelper(new string[3]
        {
          "/u",
          Application.ExecutablePath,
          "/LogToConsole=false"
        });
      }
      return this.ServiceStatus();
    }

    public ServiceResult ServiceStop()
    {
      Engine.Result.ServiceIsInstalled = this.sc != null;
      if (Engine.Result.ServiceIsInstalled)
      {
        if (this.sc.Status != ServiceControllerStatus.Running)
          return Engine.Result;
        this.sc.Stop();
        this.sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10.0));
        Engine.Result.ServiceRunStatus = this.sc.Status;
      }
      try
      {
        Engine.Result = this.ServiceStatus();
        EventHandler<ServiceResultEventArg> onServiceResult = this.OnServiceResult;
        if (onServiceResult != null)
          onServiceResult((object) this, new ServiceResultEventArg(Engine.Result));
      }
      catch
      {
      }
      return Engine.Result;
    }

    public ServiceResult ServiceStart()
    {
      Engine.Result.ServiceIsInstalled = this.sc != null;
      if (Engine.Result.ServiceIsInstalled)
      {
        if (this.sc.Status != ServiceControllerStatus.Stopped)
          return Engine.Result;
        this.sc.Start();
        this.sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10.0));
        Engine.Result.ServiceRunStatus = this.sc.Status;
      }
      try
      {
        Engine.Result = this.ServiceStatus();
        EventHandler<ServiceResultEventArg> onServiceResult = this.OnServiceResult;
        if (onServiceResult != null)
          onServiceResult((object) this, new ServiceResultEventArg(Engine.Result));
      }
      catch
      {
      }
      return Engine.Result;
    }

    public ServiceResult FirewallAdd()
    {
      if (!FirewallHelper.Instance.HasAuthorization(Application.ExecutablePath))
        FirewallHelper.Instance.GrantAuthorization(Application.ExecutablePath, Engine._application.Name);
      try
      {
        Engine.Result = this.ServiceStatus();
        EventHandler<ServiceResultEventArg> onServiceResult = this.OnServiceResult;
        if (onServiceResult != null)
          onServiceResult((object) this, new ServiceResultEventArg(Engine.Result));
      }
      catch
      {
      }
      return Engine.Result;
    }

    public ServiceResult FirewallRemove()
    {
      if (FirewallHelper.Instance.HasAuthorization(Application.ExecutablePath))
        FirewallHelper.Instance.RemoveAuthorization(Application.ExecutablePath);
      try
      {
        Engine.Result = this.ServiceStatus();
        EventHandler<ServiceResultEventArg> onServiceResult = this.OnServiceResult;
        if (onServiceResult != null)
          onServiceResult((object) this, new ServiceResultEventArg(Engine.Result));
      }
      catch
      {
      }
      return Engine.Result;
    }

    public string ServiceName => Engine._application.Name;

    protected int ServiceStepInfo(List<ActionArgument> Actions)
    {
      if (Actions.Count != 0 && !Actions.Any<ActionArgument>((Func<ActionArgument, bool>) (i => i.Command == EnCommand.info)))
        return 0;
      this.ShowInfo();
      return 1;
    }

    protected int ServiceStepInstall(List<ActionArgument> Actions)
    {
      try
      {
        if (!Actions.Any<ActionArgument>((Func<ActionArgument, bool>) (i => i.Command == EnCommand.install)) && !Actions.Any<ActionArgument>((Func<ActionArgument, bool>) (i => i.Command == EnCommand.i)))
          return 0;
        this.CheckRequirement();
        this.ServiceInstall();
        return 1;
      }
      catch (Exception ex)
      {
        Engine.Result.Result = false;
        Engine.Result.Message = ex.Message;
        return -100;
      }
    }

    protected int ServiceStepUninstall(List<ActionArgument> Actions)
    {
      try
      {
        if (!Actions.Any<ActionArgument>((Func<ActionArgument, bool>) (i => i.Command == EnCommand.uninstall)) && !Actions.Any<ActionArgument>((Func<ActionArgument, bool>) (i => i.Command == EnCommand.u)))
          return 0;
        this.CheckRequirement();
        this.ServiceUninstall();
        return 1;
      }
      catch (Exception ex)
      {
        Engine.Result.Result = false;
        Engine.Result.Message = ex.Message;
        return -100;
      }
    }

    protected int ServiceStepStart(List<ActionArgument> Actions)
    {
      try
      {
        if (!Actions.Any<ActionArgument>((Func<ActionArgument, bool>) (i => i.Command == EnCommand.start)))
          return 0;
        this.CheckRequirement();
        this.ServiceStart();
        return 1;
      }
      catch (Exception ex)
      {
        Engine.Result.Result = false;
        Engine.Result.Message = ex.Message;
        return -100;
      }
    }

    protected int ServiceStepStop(List<ActionArgument> Actions)
    {
      try
      {
        if (!Actions.Any<ActionArgument>((Func<ActionArgument, bool>) (i => i.Command == EnCommand.stop)))
          return 0;
        this.CheckRequirement();
        this.ServiceStop();
        return 1;
      }
      catch (Exception ex)
      {
        Engine.Result.Result = false;
        Engine.Result.Message = ex.Message;
        return -100;
      }
    }

    protected int ServiceStepFirewallAdd(List<ActionArgument> Actions)
    {
      try
      {
        if (!Actions.Any<ActionArgument>((Func<ActionArgument, bool>) (i => i.Command == EnCommand.firewalladd || i.Command == EnCommand.fa)))
          return 0;
        this.CheckRequirement();
        this.FirewallAdd();
        return 1;
      }
      catch (Exception ex)
      {
        Engine.Result.Result = false;
        Engine.Result.Message = ex.Message;
        return -100;
      }
    }

    protected int ServiceStepFirewallRemove(List<ActionArgument> Actions)
    {
      try
      {
        if (!Actions.Any<ActionArgument>((Func<ActionArgument, bool>) (i => i.Command == EnCommand.firewallremove || i.Command == EnCommand.fr)))
          return 0;
        this.CheckRequirement();
        this.FirewallRemove();
        return 1;
      }
      catch (Exception ex)
      {
        Engine.Result.Result = false;
        Engine.Result.Message = ex.Message;
        return -100;
      }
    }

    protected int ServiceStepStatus(List<ActionArgument> Actions)
    {
      try
      {
        if (!Actions.Any<ActionArgument>((Func<ActionArgument, bool>) (i => i.Command == EnCommand.s || i.Command == EnCommand.status)))
          return 0;
        this.ServiceStatus();
        return 1;
      }
      catch (Exception ex)
      {
        Engine.Result.Result = false;
        Engine.Result.Message = ex.Message;
        return -100;
      }
    }

    protected int ServiceStepOutput(List<ActionArgument> Actions)
    {
      try
      {
        EnArgument enArgument = EnArgument.out_json;
        if (Actions.Any<ActionArgument>((Func<ActionArgument, bool>) (i => i.Command == EnCommand.output)))
          enArgument = Actions.Find((Predicate<ActionArgument>) (i => i.Command == EnCommand.output)).Arguments.Keys.First<EnArgument>();
        if (enArgument == EnArgument.silent)
          return 0;
        ServiceResult result = Engine.Result;
        switch (enArgument)
        {
          case EnArgument.out_xml:
            Console.WriteLine(result.ToStringXml());
            goto case EnArgument.silent;
          case EnArgument.out_object:
            Console.WriteLine(result.ToSerialize());
            goto case EnArgument.silent;
          case EnArgument.silent:
            return 1;
          default:
            Console.WriteLine(result.ToStringJson());
            goto case EnArgument.silent;
        }
      }
      catch (Exception ex)
      {
        Engine.Result.Result = false;
        Engine.Result.Message = ex.Message;
        return -100;
      }
    }
  }
}
