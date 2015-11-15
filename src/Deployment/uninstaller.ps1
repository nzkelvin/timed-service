# This scirpt file should be run under the administrator priviledge 
#$ServiceName = "Timed Service"

$service = Get-WmiObject -Class Win32_Service -Filter "Name='Timed Service'"
if ($service -ne $null)
{
	$service | stop-service -Force
	start-sleep -s 10
	$service.Delete()
}
#write-host "Press any key to continue ..."
#$null = $host.ui.RawUI.ReadKey("NoEcho,IncludeKeyDown") # This doesn't not work in PS ISE