# This scirpt file should be run under the administrator priviledge 
$ServiceFile = $pwd.path + "\TimedService.exe"
write-host $ServiceFile
new-service -Name "Timed Service" -BinaryPathName $ServiceFile -StartupType "Automatic"
start-service -Name "Timed Service"
#write-host "Press any key to continue ..."
#$null = $host.ui.RawUI.ReadKey("NoEcho,IncludeKeyDown") # This doesn't not work in PS ISE