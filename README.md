# Windows Firewall and service Helper Class Library
[![](https://img.shields.io/github/license/aghili/Document.Service.svg?style=flat-square)](https://github.com/aghili/Document.Service/blob/master/LICENSE)
[![](https://img.shields.io/github/commit-activity/y/aghili/Document.Service.svg?style=flat-square)](https://github.com/aghili/Document.Service/commits/master)
[![](https://img.shields.io/github/issues/aghili/Document.Service.svg?style=flat-square)](https://github.com/aghili/Document.Service/issues)

A class library to manage the Windows Firewall as well as adding your program to the Windows Firewall Exception list.

This project supports Net6 and Net7, therefore, is compatible with Net6 and any version of dotNet equal or greater than it

Even though it is possible to reference this library under Linux or Mac; it's obviously not going to work.

## How to get
[![](https://img.shields.io/nuget/dt/Aghili.Extensions.Service.Install.svg?style=flat-square)](https://www.nuget.org/packages/Aghili.Extensions.Service.Install)
[![](https://img.shields.io/nuget/v/Aghili.Extensions.Service.Install?style=flat-square)](https://www.nuget.org/packages/Aghili.Extensions.Service.Install)

This library is available as a NuGet package at [nuget.org](https://www.nuget.org/packages/Aghili.Extensions.Service.Install/).

## Help me fund my own Death Star

[![](https://img.shields.io/badge/crypto-CoinPayments-8a00a3.svg?style=flat-square)](https://www.coinpayments.net/index.php?cmd=_donate&reset=1&merchant=xxxx&item_name=Donate&currency=USD&amountf=20.00000000&allow_amount=1&want_shipping=0&allow_extra=1)
[![](https://img.shields.io/badge/shetab-ZarinPal-8a00a3.svg?style=flat-square)](https://zarinp.al/@aghili)
[![](https://img.shields.io/badge/usd-Paypal-8a00a3.svg?style=flat-square)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=aghili@gmail.com&lc=US&item_name=Donate&no_note=0&cn=&curency_code=USD&bn=PP-DonationsBF:btn_donateCC_LG.gif:NonHosted)

**--OR--**

You can always donate your time by contributing to the project or by introducing it to others.

## How to use
Create new instance of `Engine` class and send string argument of Application name to Class creator for use in service registering title.

## Results
*  Run the program without sending Commands, all the features of the package will be displayed in the console output.
```
Extention for manage windows service by himself.

Commands:

        info     Information about console commands.
        status  Status of service state such as installation and running states.
        install Add service to this computer, if service was installed did not return error.
        uninstall       Remove service from this computer, if service was uninstalled did not return error.
        firewalladd     Add service file access rule to firewall.
        firewallremove  Remove service file access rule from firewall.
        start   Start service if exist in this computer.
        stop    Stop service if exist in this computer.
        s       Short command for Status.
        i       Short command for install.
        u       Short command for unistall.
        fa      Short command for FirewallAdd.
        fr      Short command for FirewallRemove.

Arguments:

        silent  Run command in silent mode.
        out_json        [Default]output result as json string.
```
*  If the program is called with one of the arguments, the output will be displayed as below and the output type can be selected with arguments
```json
{
    "ServiceRunStatus":"0",
    "ServiceIsInstalled":False,
    "FirewallIsInstall":False,
    "FirewallIsEnable":False,
    "FirewallRuleAdded":False,
    "Message":"",
    "Result":True
}
```

## Examples
Check the 'Document.Service.Sample' projects as a brief example of what can be done using this class library.

### Console example
*  Create new console application and for Program block add below code:
```C#
internal class Program
{
    private static void Main(string[] args)
    {
        new Document.Service.Engine("TestApp").Run(args);
        Console.ReadLine();
    }
}
```


## License
MIT License

Copyright (c) 2010-2023 mostafa aghili

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.