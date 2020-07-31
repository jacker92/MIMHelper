using MIMHelper;
using MIMHelper.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;

namespace MIMHelper.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "ExpiredADUsers")]
    [OutputType(typeof(Microsoft.ActiveDirectory.Management.ADUser))]
    public class GetExpiredADUsersCmdlet : Cmdlet
    {
        [Parameter(Position = 0)]
        public int DaysAfterPwdExpired { get; set; } = 0;

        [Parameter(Position = 1)]
        public string BaseOU { get; set; }

        [Parameter(Position = 2)]
        public string OrganizationName { get; set; }

        protected override void BeginProcessing()
        {
            WriteDebug("Begin processing.");

            ModuleHandler.CheckADModuleOrThrow();
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }

        protected override void ProcessRecord()
        {
            WriteDebug("Processing record.");

            using (var ps = ModuleHandler.GetPowerShellWithModules())
            {
                var allUsers = GetAllUsers(ps);

                var filteredUsers = GetFilteredUsers(allUsers);

                filteredUsers.ToList().ForEach(y => WriteObject(y));
            }
        }

        private IEnumerable<PSObject> GetFilteredUsers(Collection<PSObject> allUsers)
        {
            var filteredUsers = allUsers.ToList().Where(x =>
            {
                var timeComputed = x.Properties["msDS-UserPasswordExpiryTimeComputed"]?.Value.ToString();

                WriteDebug($"TimeComputed: {timeComputed}");

                if (!string.IsNullOrWhiteSpace(timeComputed))
                {
                    var filterDate = DateTime.Now.AddDays(-DaysAfterPwdExpired).ToFileTime();
                    return long.Parse(timeComputed) < filterDate;
                }

                return false;
            }).ToList();

            WriteDebug($"Found {filteredUsers.Count} expired users.");

            return filteredUsers;
        }

        private Collection<PSObject> GetAllUsers(PowerShell ps)
        {
            ps.AddCommand("Get-ADUser").AddParameter("filter", "*").AddParameter("Properties", new string[] { "*", "msDS-UserPasswordExpiryTimeComputed" });

            if (!string.IsNullOrWhiteSpace(BaseOU))
            {
                ps.AddParameter("SearchBase", BaseOU);
            }

            WriteDebug("Invoking Get-ADUser filter * Properties *, msDS-UserPasswordExpiryTimeComputed");

            var allUsers = ps.Invoke();

            WriteDebug($"Got {allUsers.Count} users from first command.");
            return allUsers;
        }

        protected override void StopProcessing()
        {
            base.StopProcessing();
        }
    }
}
