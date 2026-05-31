param(
    [Parameter(Position = 0)]
    [ValidateSet('tools', 'dictionaries')]
    [string]$Target
)

function download-dictionaries {
    Set-Location $dictionariesPath 
    curl.exe -L -O "https://github.com/LibreOffice/dictionaries/blob/master/en/en_GB.aff"
    curl.exe -L -O "https://github.com/LibreOffice/dictionaries/blob/master/en/en_GB.dic"
    curl.exe -L -O "https://github.com/LibreOffice/dictionaries/blob/master/en/en_US.aff"
    curl.exe -L -O "https://github.com/LibreOffice/dictionaries/blob/master/en/en_US.dic"
    curl.exe -L -O "https://github.com/LibreOffice/dictionaries/blob/master/hu_HU/hu_HU.aff"
    curl.exe -L -O "https://github.com/LibreOffice/dictionaries/blob/master/hu_HU/hu_HU.dic"
    curl.exe -L -O "https://github.com/LibreOffice/dictionaries/blob/master/hr_HR/hr_HR.aff"
    curl.exe -L -O "https://github.com/LibreOffice/dictionaries/blob/master/hr_HR/hr_HR.dic"
    curl.exe -L -O "https://github.com/LibreOffice/dictionaries/blob/master/it_IT/it_IT.aff"
    curl.exe -L -O "https://github.com/LibreOffice/dictionaries/blob/master/it_IT/it_IT.dic"
    curl.exe -L -O "https://github.com/LibreOffice/dictionaries/blob/master/nl_NL/nl_NL.aff"
    curl.exe -L -O "https://github.com/LibreOffice/dictionaries/blob/master/nl_NL/nl_NL.dic"
    curl.exe -L -O "https://github.com/LibreOffice/dictionaries/blob/master/de/de_DE_frami.aff"
    curl.exe -L -O "https://github.com/LibreOffice/dictionaries/blob/master/de/de_DE_frami.dic"
    curl.exe -L -O "https://github.com/LibreOffice/dictionaries/blob/master/es/es_ES.aff"
    curl.exe -L -O "https://github.com/LibreOffice/dictionaries/blob/master/es/es_ES.dic"
    Set-Location $startdir
}

function download-tools {
    Set-Location $toolsPath
    curl.exe -L -O "https://github.com/erweixin/RaTeX/releases/download/v0.1.10/ratex-cli-v0.1.10-x86_64-pc-windows-msvc.zip"
    curl.exe -L -O "https://github.com/erweixin/RaTeX/releases/download/v0.1.10/ratex-cli-v0.1.10-aarch64-unknown-linux-musl.tar.gz"
    curl.exe -L -O "https://github.com/1jehuang/mermaid-rs-renderer/releases/download/v0.2.2/mmdr-x86_64-pc-windows-msvc.zip"
    curl.exe -L -O "https://github.com/1jehuang/mermaid-rs-renderer/releases/download/v0.2.2/mmdr-x86_64-unknown-linux-gnu.tar.gz"
    
    # mmdr
    Expand-Archive  .\mmdr-x86_64-pc-windows-msvc.zip -Force -DestinationPath .
    Remove-Item .\mmdr-x86_64-pc-windows-msvc.zip
    tar -xzf .\mmdr-x86_64-unknown-linux-gnu.tar.gz -C .
    Remove-Item .\mmdr-x86_64-unknown-linux-gnu.tar.gz
    
    # RaTeX
    Expand-Archive .\ratex-cli-v0.1.10-x86_64-pc-windows-msvc.zip -Force -DestinationPath .
    Move-Item -Force .\ratex-cli-v0.1.10-x86_64-pc-windows-msvc\render-svg.exe .\ratex-svg.exe
    Remove-Item .\ratex-cli-v0.1.10-x86_64-pc-windows-msvc.zip
    Remove-Item .\ratex-cli-v0.1.10-x86_64-pc-windows-msvc -Recurse -Force

    tar -xzf .\ratex-cli-v0.1.10-aarch64-unknown-linux-musl.tar.gz -C .
    Move-Item -Force .\ratex-cli-v0.1.10-aarch64-unknown-linux-musl\render-svg .\ratex-svg
    Remove-Item .\ratex-cli-v0.1.10-aarch64-unknown-linux-musl.tar.gz
    Remove-Item .\ratex-cli-v0.1.10-aarch64-unknown-linux-musl -Recurse -Force
    
    Set-Location $startdir
}

$startdir = Get-Location
$dictionariesPath = ".\Assets\dictionaries"
$toolsPath = ".\Assets\tools"

if (-not (Test-Path $dictionariesPath)) {
    New-Item -ItemType Directory -Path $dictionariesPath | Out-Null
}

if (-not (Test-Path $toolsPath)) {
    New-Item -ItemType Directory -Path $toolsPath | Out-Null
}

# Main execution logic
if ([string]::IsNullOrEmpty($Target)) {
    # No arguments - run both functions
    download-dictionaries
    download-tools
}
elseif ($Target -eq 'tools') {
    download-tools
}
elseif ($Target -eq 'dictionaries') {
    download-dictionaries
}
