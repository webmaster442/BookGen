$file = $args[0]
$hash = $args[1]

if ($($args.Count) -lt 2) {
	Write-host Usage: [file uri] [expected hash]
	exit
}

clear
Write-host "  ____              _     _____              _    _           _       _            "
Write-host " |  _ \            | |   / ____|            | |  | |         | |     | |           "
Write-host " | |_) | ___   ___ | | _| |  __  ___ _ __   | |  | |_ __   __| | __ _| |_ ___ _ __ "
Write-host " |  _ < / _ \ / _ \| |/ / | |_ |/ _ \ '_ \  | |  | | '_ \ / _| |/ _| | __/ _ \ '__|"
Write-host " | |_) | (_) | (_) |   <| |__| |  __/ | | | | |__| | |_) | (_| | (_| | ||  __/ |   "
Write-host " |____/ \___/ \___/|_|\_\\_____|\___|_| |_|  \____/| .__/ \__,_|\__,_|\__\___|_|   "
Write-host "                                                   | |                             "
Write-host "                                                   |_|                             "


Write-host Stopping all running BookGen instances...
if((get-process "BookGen" -ea SilentlyContinue) -ne $Null){ 
	Stop-process -name BookGen -Force
}

Write-host Downloading update package...
Invoke-WebRequest -Uri $file -OutFile $env:temp\update.zip

Write-host Verifying integrity...
if ($hash -ne (Get-FileHash -Algorithm SHA256 $env:temp\update.zip).Hash) {
	Write-host "Package integrity check failed"
	Remove-Item -Path $env:temp\update.zip -Force
	exit
}

Write-host Extracting update...
Expand-Archive -Path $env:temp\update.zip -DestinationPath (pwd).Path -Force 

Write-host Removing update package...
Remove-Item -Path $env:temp\update.zip -Force

Write-host DONE!
Write-host Press a key to exit

Remove-Item -Path .\updater.ps1

Read-Host
