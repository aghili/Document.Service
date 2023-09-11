// Decompiled with JetBrains decompiler
// Type: Document.Service.Utilities.FirewallHelper
// Assembly: Document.Service, Version=0.12.1.0, Culture=neutral, PublicKeyToken=9c8f15b9f8ae24cc
// MVID: 397D359F-A322-4ECB-BA44-B758DFC28E5C
// Assembly location: D:\Mahak\Tools\MahakTray\Document.Service.dll

using NetFwTypeLib;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Document.Service.Utilities
{
  public class FirewallHelper
  {
    private static FirewallHelper instance;
    private INetFwMgr fwMgr;

    public static FirewallHelper Instance
    {
      get
      {
        lock (typeof (FirewallHelper))
        {
          if (FirewallHelper.instance == null)
            FirewallHelper.instance = new FirewallHelper();
          return FirewallHelper.instance;
        }
      }
    }

    private FirewallHelper()
    {
      Type typeFromProgId = Type.GetTypeFromProgID("HNetCfg.FwMgr", false);
      this.fwMgr = (INetFwMgr) null;
      if (!(typeFromProgId != (Type) null))
        return;
      try
      {
        this.fwMgr = (INetFwMgr) Activator.CreateInstance(typeFromProgId);
      }
      catch (ArgumentException ex)
      {
      }
      catch (NotSupportedException ex)
      {
      }
      catch (TargetInvocationException ex)
      {
      }
      catch (MissingMethodException ex)
      {
      }
      catch (MethodAccessException ex)
      {
      }
      catch (MemberAccessException ex)
      {
      }
      catch (InvalidComObjectException ex)
      {
      }
      catch (COMException ex)
      {
      }
      catch (TypeLoadException ex)
      {
      }
    }

    public bool IsFirewallInstalled => this.fwMgr != null && this.fwMgr.LocalPolicy != null && this.fwMgr.LocalPolicy.CurrentProfile != null;

    public bool IsFirewallEnabled => this.IsFirewallInstalled && this.fwMgr.LocalPolicy.CurrentProfile.FirewallEnabled;

    public bool AppAuthorizationsAllowed => this.IsFirewallInstalled && !this.fwMgr.LocalPolicy.CurrentProfile.ExceptionsNotAllowed;

    public void GrantAuthorization(string applicationFullPath, string appName)
    {
      FirewallHelper.CheckPathCharacters(applicationFullPath);
      if (!this.IsFirewallInstalled)
        throw new FirewallHelperException("Cannot grant authorization: Firewall is not installed.");
      if (!this.AppAuthorizationsAllowed)
        throw new FirewallHelperException("Application exemptions are not allowed.");
      if (this.HasAuthorization(applicationFullPath))
        return;
      Type typeFromProgId = Type.GetTypeFromProgID("HNetCfg.FwAuthorizedApplication", false);
      // ISSUE: variable of a compiler-generated type
      INetFwAuthorizedApplication app = (INetFwAuthorizedApplication) null;
      if (typeFromProgId != (Type) null)
      {
        try
        {
          app = (INetFwAuthorizedApplication) Activator.CreateInstance(typeFromProgId);
        }
        catch (ArgumentException ex)
        {
        }
        catch (NotSupportedException ex)
        {
        }
        catch (TargetInvocationException ex)
        {
        }
        catch (MissingMethodException ex)
        {
        }
        catch (MethodAccessException ex)
        {
        }
        catch (MemberAccessException ex)
        {
        }
        catch (InvalidComObjectException ex)
        {
        }
        catch (COMException ex)
        {
        }
        catch (TypeLoadException ex)
        {
        }
      }
      if (app == null)
        throw new FirewallHelperException("Could not grant authorization: can't create INetFwAuthorizedApplication instance.");
      app.Name = appName;
      app.ProcessImageFileName = applicationFullPath;
      // ISSUE: reference to a compiler-generated method
      this.fwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Add(app);
    }

    public void RemoveAuthorization(string applicationFullPath)
    {
      FirewallHelper.CheckPathCharacters(applicationFullPath);
      if (!this.IsFirewallInstalled)
        throw new FirewallHelperException("Cannot remove authorization: Firewall is not installed.");
      if (!this.HasAuthorization(applicationFullPath))
        return;
      // ISSUE: reference to a compiler-generated method
      this.fwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Remove(applicationFullPath);
    }

    public bool HasAuthorization(string applicationFullPath)
    {
      FirewallHelper.CheckPathCharacters(applicationFullPath);
      if (!this.IsFirewallInstalled)
        throw new FirewallHelperException("Cannot get authorization status : Firewall is not installed.");
      foreach (string authorizedAppPath in (IEnumerable) this.GetAuthorizedAppPaths())
      {
        if (authorizedAppPath.ToLower() == applicationFullPath.ToLower())
          return true;
      }
      return false;
    }

    private static void CheckPathCharacters(string applicationFullPath)
    {
      if (applicationFullPath == null)
        throw new ArgumentNullException(nameof (applicationFullPath));
      if (applicationFullPath.Trim().Length == 0)
        throw new ArgumentException("applicationFullPath must not be blank");
      if (applicationFullPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
        throw new ArgumentException("applicationFullPath must not contain invalid path characters");
      if (Path.GetFileName(applicationFullPath).IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        throw new ArgumentException("applicationFullPath must not contain invalid FileName characters");
      if (!Path.IsPathRooted(applicationFullPath))
        throw new ArgumentException("applicationFullPath is not an absolute path");
      if (!File.Exists(applicationFullPath))
        throw new FileNotFoundException("File does not exist.", applicationFullPath);
    }

    public ICollection GetAuthorizedAppPaths()
    {
      if (!this.IsFirewallInstalled)
        throw new FirewallHelperException("Cannot remove authorization: Firewall is not installed.");
      ArrayList authorizedAppPaths = new ArrayList();
      foreach (INetFwAuthorizedApplication authorizedApplication in this.fwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications)
        authorizedAppPaths.Add((object) authorizedApplication.ProcessImageFileName);
      return (ICollection) authorizedAppPaths;
    }
  }
}
