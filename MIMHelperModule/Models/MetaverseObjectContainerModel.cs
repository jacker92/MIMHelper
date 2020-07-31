using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;

namespace MIMHelper.Models
{
    public class MetaverseObjectContainerModel
    {
        public long TotalMVObjects { get; set; } = 0;

        public long TotalMAs { get; set; } = 0;

        public Dictionary<string, Collection<PSObject>> Objects { get; set; }
    }
}
