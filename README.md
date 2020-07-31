# MIMHelper PowerShell Module

MIMHelper is an utility module containing cmdlets that are designed to help development with Microsoft Identity Manager.

## Module requirements

This module depends on following modules:
- [Lithnet PowerShell Module](https://github.com/lithnet/miis-powershell) 
- [ActiveDirectory PowerShell Module](https://docs.microsoft.com/en-us/powershell/module/addsadministration/?view=win10-ps)

### Other requirements
- [.NET Core](https://dotnet.microsoft.com/download) 

## Installation/Usage instructions

1. Clone the git repository
```sh
git clone https://github.com/jacker92/MIMHelper.git
```
2. Install dependencies & build project
```sh
dotnet restore
dotnet build
```
3. Import dll to PowerShell
```sh
Import-Module "C:\build-path\MIMHelper.dll"
```

## Cmdlets

#### Get-EmptyOrganizationUnits
Gets the empty organization units from AD. Only organization units that are completely empty (they cannot contain other OUs or other AD objects: they are leafs in tree structure) are returned from the cmdlet.

Organization units are returned as Microsoft.ActiveDirectory.Management.ADOrganizationalUnit. 

Usage example:
```pwsh
Get-EmptyOrganizationUnits -BaseOU "ou=Users,dc=domain,dc=local"
```

#### Get-ExpiredADUsers
Gets the expired AD users based on the password expiration date. With parameter "DaysAfterPwdExpired" you can further filter the users.

Cmdlet returns all expired users as Microsoft.ActiveDirectory.Management.ADUser.

Usage example:
```pwsh
Get-ExpiredADUsers -DaysAfterPwdExpired 10
```

#### Get-MetaverseObjects
Helper cmdlet to retrieve all metaverse objects in one large object. Useful when need to get an full overview of metaverse size or some operation needs to be executed to all objects in metaverse.

Usage example:
```pwsh
Get-MetaverseObjects
```
