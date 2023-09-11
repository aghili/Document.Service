using Aghili.Extensions.Service.Install.Exceptions;
using NetFwTypeLib;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Aghili.Extensions.Service.Install.Utilities;

public class FirewallHelper
{
    private static FirewallHelper instance;

    private INetFwMgr fwMgr;

    public static FirewallHelper Instance
    {
        get
        {
            lock (typeof(FirewallHelper))
            {
                if (instance == null)
                {
                    instance = new FirewallHelper();
                }

                return instance;
            }
        }
    }

    public bool IsFirewallInstalled
    {
        get
        {
            if (fwMgr != null && fwMgr.LocalPolicy != null && fwMgr.LocalPolicy.CurrentProfile != null)
            {
                return true;
            }

            return false;
        }
    }

    public bool IsFirewallEnabled
    {
        get
        {
            if (IsFirewallInstalled && fwMgr.LocalPolicy.CurrentProfile.FirewallEnabled)
            {
                return true;
            }

            return false;
        }
    }

    public bool AppAuthorizationsAllowed
    {
        get
        {
            if (IsFirewallInstalled && !fwMgr.LocalPolicy.CurrentProfile.ExceptionsNotAllowed)
            {
                return true;
            }

            return false;
        }
    }

    private FirewallHelper()
    {
        Type typeFromProgID = Type.GetTypeFromProgID("HNetCfg.FwMgr", throwOnError: false);
        fwMgr = null;
        if (typeFromProgID != null)
        {
            try
            {
                fwMgr = (INetFwMgr)Activator.CreateInstance(typeFromProgID);
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
    }

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
        fwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Add(netFwAuthorizedApplication);
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
            fwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Remove(applicationFullPath);
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
        foreach (INetFwAuthorizedApplication authorizedApplication in fwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications)
        {
            arrayList.Add(authorizedApplication.ProcessImageFileName);
        }

        return arrayList;
    }
}