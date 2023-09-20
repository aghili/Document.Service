using Aghili.Extensions.Service.Install.Exceptions;
using NetFwTypeLib;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Aghili.Extensions.Service.Install.Register.FirewallServices.NetFwMgr;

internal class FirewallServiceRegister : IFirewallServiceRegister
{
    private static INetFwMgr? _fwMgr = null;

    public FirewallServiceRegister()
    {
    }

    private static INetFwMgr FwMgr
    {
        get
        {
            return _fwMgr ??= new Func<INetFwMgr>(() =>
            {
                Type typeFromProgID = Type.GetTypeFromProgID("HNetCfg.FwMgr", throwOnError: false);
                _fwMgr = null;
                if (typeFromProgID != null)
                {
                    try
                    {
                        _fwMgr = (INetFwMgr)Activator.CreateInstance(typeFromProgID);
                    }
                    catch (ArgumentException)
                    {
                    }
                    catch (NotSupportedException)
                    {
                    }
                    catch (TargetInvocationException)
                    {
                    }
                    catch (MissingMethodException)
                    {
                    }
                    catch (MethodAccessException)
                    {
                    }
                    catch (MemberAccessException)
                    {
                    }
                    catch (InvalidComObjectException)
                    {
                    }
                    catch (COMException)
                    {
                    }
                    catch (TypeLoadException)
                    {
                    }
                }
                return _fwMgr;
            }).Invoke();
        }
    }

    public static bool IsReady
    {
        get { return FwMgr != null && GetFirewallInstallStatus() && GetAppAuthorizationsAllowedStatus(); }
    }

    private static bool GetFirewallInstallStatus()
    {
        return FwMgr != null && FwMgr.LocalPolicy != null && FwMgr.LocalPolicy.CurrentProfile != null;
    }
    private static bool GetFirewallEnableStatus()
    {
        return GetFirewallInstallStatus() && FwMgr.LocalPolicy.CurrentProfile.FirewallEnabled;
    }
    private static bool GetAppAuthorizationsAllowedStatus()
    {
        return GetFirewallInstallStatus() && !FwMgr.LocalPolicy.CurrentProfile.ExceptionsNotAllowed;
    }

    public bool IsFirewallInstalled => GetFirewallInstallStatus();

    public bool IsFirewallEnabled => GetFirewallEnableStatus();

    public bool AppAuthorizationsAllowed => GetAppAuthorizationsAllowedStatus();

    public void GrantAuthorization(string applicationFullPath, string appName)
    {
        CheckPathCharacters(applicationFullPath);
        if (!IsFirewallInstalled)
        {
            throw new ExceptionFirewallHelper("Cannot grant authorization: Firewall is not installed.");
        }

        if (!AppAuthorizationsAllowed)
        {
            throw new ExceptionFirewallHelper("Application exemptions are not allowed.");
        }

        if (HasAuthorization(applicationFullPath))
        {
            return;
        }

        Type typeFromProgID = Type.GetTypeFromProgID("HNetCfg.FwAuthorizedApplication", throwOnError: false);
        INetFwAuthorizedApplication netFwAuthorizedApplication = null;
        if (typeFromProgID != null)
        {
            try
            {
                netFwAuthorizedApplication = (INetFwAuthorizedApplication)Activator.CreateInstance(typeFromProgID);
            }
            catch (ArgumentException)
            {
            }
            catch (NotSupportedException)
            {
            }
            catch (TargetInvocationException)
            {
            }
            catch (MissingMethodException)
            {
            }
            catch (MethodAccessException)
            {
            }
            catch (MemberAccessException)
            {
            }
            catch (InvalidComObjectException)
            {
            }
            catch (COMException)
            {
            }
            catch (TypeLoadException)
            {
            }
        }

        if (netFwAuthorizedApplication == null)
        {
            throw new ExceptionFirewallHelper("Could not grant authorization: can't create INetFwAuthorizedApplication instance.");
        }

        netFwAuthorizedApplication.Name = appName;
        netFwAuthorizedApplication.ProcessImageFileName = applicationFullPath;
        FwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Add(netFwAuthorizedApplication);
    }

    public void RemoveAuthorization(string applicationFullPath)
    {
        CheckPathCharacters(applicationFullPath);
        if (!IsFirewallInstalled)
        {
            throw new ExceptionFirewallHelper("Cannot remove authorization: Firewall is not installed.");
        }

        if (HasAuthorization(applicationFullPath))
        {
            FwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Remove(applicationFullPath);
        }
    }

    public bool HasAuthorization(string applicationFullPath)
    {
        CheckPathCharacters(applicationFullPath);
        if (!IsFirewallInstalled)
        {
            throw new ExceptionFirewallHelper("Cannot get authorization status : Firewall is not installed.");
        }

        foreach (string authorizedAppPath in GetAuthorizedAppPaths())
        {
            if (authorizedAppPath.ToLower() == applicationFullPath.ToLower())
            {
                return true;
            }
        }

        return false;
    }

    private static void CheckPathCharacters(string applicationFullPath)
    {
        if (applicationFullPath == null)
        {
            throw new ArgumentNullException("applicationFullPath");
        }

        if (applicationFullPath.Trim().Length == 0)
        {
            throw new ArgumentException("applicationFullPath must not be blank");
        }

        if (applicationFullPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
        {
            throw new ArgumentException("applicationFullPath must not contain invalid path characters");
        }

        if (Path.GetFileName(applicationFullPath)!.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            throw new ArgumentException("applicationFullPath must not contain invalid FileName characters");
        }

        if (!Path.IsPathRooted(applicationFullPath))
        {
            throw new ArgumentException("applicationFullPath is not an absolute path");
        }

        if (!File.Exists(applicationFullPath))
        {
            throw new FileNotFoundException("File does not exist.", applicationFullPath);
        }
    }

    public ICollection GetAuthorizedAppPaths()
    {
        if (!IsFirewallInstalled)
        {
            throw new ExceptionFirewallHelper("Cannot remove authorization: Firewall is not installed.");
        }

        ArrayList arrayList = new ArrayList();
        foreach (INetFwAuthorizedApplication authorizedApplication in FwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications)
        {
            arrayList.Add(authorizedApplication.ProcessImageFileName);
        }

        return arrayList;
    }
}