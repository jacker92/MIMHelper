using Lithnet.Miiserver.Client;
using MIMHelper.Models;
using MIMHelper.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace MIMHelper.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "MetaverseObjects")]
    [OutputType(typeof(MetaverseObjectContainerModel))]
    public class GetMetaverseObjectsCmdlet : Cmdlet
    {
        protected override void BeginProcessing()
        {
            ModuleHandler.CheckLithnetMiisAutomationModuleOrThrow();
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
                var objectTypes = GetObjectTypes(ps);
                var allObjects = GetAllObjects(ps, objectTypes);

                var totalCount = 0;

                allObjects.Values.ToList().ForEach(x => totalCount += x.Count());

                WriteObject(new MetaverseObjectContainerModel
                {
                    TotalMVObjects = totalCount,
                    TotalMAs = objectTypes.Keys.Count(),
                    Objects = allObjects
                });
            }
        }

        private Dictionary<string, Collection<PSObject>> GetAllObjects(PowerShell ps, IReadOnlyDictionary<string, DsmlObjectClass> objectTypes)
        {
            WriteDebug("Getting all objects.");
            var allObjects = new Dictionary<string, Collection<PSObject>>();
            foreach (var objectType in objectTypes)
            {
                WriteDebug($"Processing objectType: {objectType.Key}");
                var values = ps.AddCommand("Get-MVObject").AddParameter("ObjectType", objectType.Key).Invoke();

                allObjects.Add(objectType.Key, values);
                ps.Commands.Clear();
            }
            return allObjects;
        }

        private IReadOnlyDictionary<string, DsmlObjectClass> GetObjectTypes(PowerShell ps)
        {
            ps.AddScript("Get-MVSchema");

            var schema = ps.Invoke();

            WriteDebug("Get-MVSchema invoked.");
            WriteDebug($"Schema count: {schema.Count}");

            if (schema == null || schema.Count == 0)
            {
                throw new ItemNotFoundException("Could not find any objecttypes from metaverse.");
            }

            var objectClasses = (ReadOnlyDictionary<string, DsmlObjectClass>)schema[0].Properties["ObjectClasses"].Value;
            WriteDebug($"Found {objectClasses.Keys.Count} objecttypes.");

            foreach (var item in objectClasses.Keys)
            {
                WriteDebug($"Key: {item}");
            }

            return objectClasses;
        }

        protected override void StopProcessing()
        {
            base.StopProcessing();
        }
    }
}
