using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace MIMHelper.Utilities
{
    public class ModuleHandler
    {
        private static readonly string _getADModuleCmd = "Get-Module -ListAvailable -Name ActiveDirectory";
        public static void CheckADModuleOrThrow()
        {
            using (var ps = PowerShell.Create())
            {
                var hasADModule = ps.AddScript(_getADModuleCmd).Invoke();

                if (hasADModule.Count == 0)
                {
                    throw new CommandNotFoundException($"Module ActiveDirectory cannot be found with command: {_getADModuleCmd}.");
                }
            }
        }

        public static PowerShell GetPowerShellWithADModule()
        {
            return PowerShell.Create().AddScript("Import-Module -Name ActiveDirectory");
        }
    }
}
