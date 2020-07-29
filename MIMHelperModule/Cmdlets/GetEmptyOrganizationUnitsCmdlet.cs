using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace MIMHelper.Cmdlets
{
    /// <summary>
    /// This Cmdlet will get all empty organization units from AD.
    /// <example>
    /// <code>Get-EmptyOrganizationUnits</code>
    /// </example>
    /// <example>
    /// <code>Get-EmptyOrganizationUnits -BaseOU "OU=Sales,OU=UserAccounts,DC=FABRIKAM,DC=COM"</code>
    /// </example>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "EmptyOrganizationUnits")]
    [OutputType(typeof(Microsoft.ActiveDirectory.Management.ADOrganizationalUnit))]
    public class GetEmptyOrganizationUnitsCmdlet : Cmdlet
    {
        /// <summary>
        /// BaseOU that search will be executed. 
        /// </summary>
        [Parameter(Position = 0)]
        public string BaseOU { get; set; }

        /// <summary>
        /// Search filter.
        /// </summary>
        [Parameter(Position = 1)]
        public string Filter { get; set; }

        protected override void BeginProcessing()
        {
            ModuleHandler.CheckADModuleOrThrow();
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }

        protected override void ProcessRecord()
        {
            using (var ps = ModuleHandler.GetPowerShellWithADModule())
            {
                var allOrganizationUnits = GetAllOrganizationUnits(ps);
                ps.Commands.Clear();

                var results = GetAllEmptyOrganizationUnitsWithNoChildren(ps, allOrganizationUnits);

                WriteDebug($"Found {results.Count()} empty organization units.");

                results.ToList().ForEach(y => WriteObject(y));
            }
        }

        private IEnumerable<PSObject> GetAllEmptyOrganizationUnitsWithNoChildren(PowerShell ps, Collection<PSObject> allOrganizationUnits)
        {
            return allOrganizationUnits.ToList().Where(ou =>
            {
                ps.AddCommand("Get-ADObject").AddParameter("SearchBase", ou)
                                             .AddParameter("SearchScope", "OneLevel")
                                             .AddParameter("Filter", "*");

                var currentResult = ps.Invoke();

                if (currentResult.Count == 0)
                {
                    WriteDebug("Found empty organization unit: " + ou.Members["DistinguishedName"].Value);
                    return true;
                }

                return false;
            });
        }

        private Collection<PSObject> GetAllOrganizationUnits(PowerShell ps)
        {
            ps.AddCommand("Get-ADOrganizationalUnit");

            if (!string.IsNullOrWhiteSpace(BaseOU))
            {
                WriteDebug("Executing search with BaseOU: " + BaseOU);
                ps.AddParameter("SearchBase", BaseOU);
            }
            else
            {
                WriteDebug("Executing forest-wide search." + BaseOU);
            }

            if (!string.IsNullOrWhiteSpace(Filter))
            {
                WriteDebug("Using filter: " + Filter);
                ps.AddParameter("Filter", Filter);
            }
            else
            {
                ps.AddParameter("Filter", "*");
            }

            return ps.Invoke();
        }

        protected override void StopProcessing()
        {
            base.StopProcessing();
        }
    }
}
