$file = $args[0]
$hash = $args[1]

if ($($args.Count) -lt 2) {
	Write-host Usage: [file uri] [expected hash]
	exit
}

Write-host Stopping all running BookGen instances...
if((get-process "BookGen" -ea SilentlyContinue) -ne $Null){ 
	Stop-process -name BookGen -Force
}

Write-host Downloading update package...
Invoke-WebRequest -Uri $1 -OutFile $env:temp\update.zip

Write-host Verifying integrity...
if ($hash -ne (Get-FileHash -Algorithm SHA256 $env:temp\update.zip).Hash) {
	Write-host "Package integrity check failed"
	exit
}

Write-host Extracting update...
Expand-Archive -Path $env:temp\update.zip -DestinationPath (pwd).Path -Force 

Write-host Removing update package...
Remove-Item -Path $env:temp\update.zip -Force

Write-host DONE!
Write-host Press a key to exit
Read-Host
