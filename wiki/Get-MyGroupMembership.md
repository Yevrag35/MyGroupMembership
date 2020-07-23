# Get-MyGroupMembership

## SYNOPSIS
Gets a list of the currently logged-in user's local and domain group membership.

## SYNTAX

### All Parameter Sets
```
Get-MyGroupMembership [[-Type] <GroupType[]>] [<CommonParameters>]
```

## DESCRIPTION
Parses the output from 'whoami.exe' to retrieve a PowerShell object list of groups that the currently logged-in user in a member of.

## EXAMPLES

### EXAMPLE 1
Retrieve only groups of the 'WellKnown' type.
```powershell
C:\PS> Get-MyGroupMembership -Type WellKnownGroup
```

## PARAMETERS

### Type
Indicates to only return the specified type(s) of 'groups' from whoami.exe.

```yaml
Type: GroupType[]
Parameter Sets: Set 1
Aliases: t

Required: false
Position: 0
Default Value: 
Accepted Values: Alias
                 Group
                 Label
                 WellKnownGroup
Pipeline Input: False
```

### \<CommonParameters\>
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None


## OUTPUTS

### MG.Membership.MyGroup
An object with "GroupName", "Type", "SID", and "Attributes".

## RELATED LINKS

[Online Version:](https://github.com/Yevrag35/MyGroupMembership/wiki/Get-MyGroupMembership)
