# MIMHelper PowerShell Module

MIMHelper is an utility module containing cmdlets that are designed to help development with Microsoft Identity Manager.

## Module requirements

This module uses following modules:
- [Lithnet PowerShell Module](https://github.com/lithnet/miis-powershell) 
- [ActiveDirectory PowerShell Module](https://docs.microsoft.com/en-us/powershell/module/addsadministration/?view=win10-ps)

### Other requirements
- [.NET Core](https://dotnet.microsoft.com/download) 

## Installation/Usage instructions

1. Clone the git repository
```sh
git clone https://github.com/jacker92/MIMHelper.git
```
2. Build project
```sh
dotnet build
```
3. Import dll to PowerShell
```sh
Import-Module "C:\build-path\MIMHelper.dll"
```

## Cmdlets

**Get-EmptyOrganizationUnits**

**Get-ExpiredADUsersInfo**

**Get-MetaverseObjects**
