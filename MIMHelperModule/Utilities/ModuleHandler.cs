using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace MIMHelper.Utilities
{
    public class ModuleHandler
    {
        private static readonly string _getADModuleCmd = "Get-Module -ListAvailable -Name ActiveDirectory";
        private static readonly string _getLithnetMiisAutomationModuleCmd = "Get-Module -ListAvailable -Name LithnetMiisAutomation";
        private static readonly string _importADModuleCmd = "Import-Module -Name ActiveDirectory";
        private static readonly string _importLithnetMiisAutomationModuleCmd = "Import-Module -Name LithnetMiisAutomation";

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

        public static void CheckLithnetMiisAutomationModuleOrThrow()
        {
            using (var ps = PowerShell.Create())
            {
                var hasADModule = ps.AddScript(_getLithnetMiisAutomationModuleCmd).Invoke();

                if (hasADModule.Count == 0)
                {
                    throw new CommandNotFoundException($"Module LithnetMiisAutomation cannot be found with command: {_getLithnetMiisAutomationModuleCmd}.");
                }
            }
        }

        public static PowerShell GetPowerShellWithModules()
        {
            return PowerShell.Create().AddScript(_importADModuleCmd).AddScript(_importLithnetMiisAutomationModuleCmd);
        }

    }
}
