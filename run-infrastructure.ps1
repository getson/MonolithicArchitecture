#region Check elevated mode
# Get the ID and security principal of the current user account
$myWindowsID = [System.Security.Principal.WindowsIdentity]::GetCurrent();
$myWindowsPrincipal = new-object System.Security.Principal.WindowsPrincipal($myWindowsID);
 
# Get the security principal for the Administrator role
$adminRole = [System.Security.Principal.WindowsBuiltInRole]::Administrator;
 
# Check to see if we are currently running "as Administrator"
if ($myWindowsPrincipal.IsInRole($adminRole)) {
    # We are running "as Administrator" - so change the title to indicate this
    $Host.UI.RawUI.WindowTitle = $myInvocation.MyCommand.Definition + "(Elevated)";
    Clear-Host
}
else {
    # We are not running "as Administrator" - so relaunch as administrator    
    # Create a new process object that starts PowerShell
    $newProcess = new-object System.Diagnostics.ProcessStartInfo "PowerShell";

    # Specify the current script path and name as a parameter   
    $newProcess.Arguments = -join ("& '", $script:MyInvocation.MyCommand.Path, "'")
    
    # Indicate that the process should be elevated    
    $newProcess.Verb = "runas";

    $currentLocation = Get-Location
    # Start the new process    
    [System.Diagnostics.Process]::Start($newProcess);

   
    # Exit from the current, unelevated, process
    exit
}
#endregion
$scriptPath =Split-Path $script:MyInvocation.MyCommand.Path
Set-Location $scriptPath 

$dataPath = "$scriptPath\data" 

if (!(Test-Path -Path $dataPath)) {
    New-Item -ItemType Directory -Path $dataPath

    $acl = Get-Acl $dataPath
    $usersid = New-Object System.Security.Principal.Ntaccount ("Everyone")
    $accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule("Everyone", "Write", "Allow")
    $acl.SetAccessRule($accessRule)
    $acl.SetOwner($usersid)

    $acl | Set-Acl $dataPath
}

$env:DATA_PATH=$dataPath; docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml up postgres

[console]::beep(900, 400) 
[console]::beep(1000, 400) 
[console]::beep(800, 400) 
[console]::beep(400, 400) 
[console]::beep(600, 1600)
[console]::ReadKey()