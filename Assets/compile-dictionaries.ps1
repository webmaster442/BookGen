$dictionariesPath = ".\dictionaries"
if (-not (Test-Path $dictionariesPath)) {
    New-Item -ItemType Directory -Path $dictionariesPath | Out-Null
}

if (Test-Path "dictionaries.zip") {
    Write-Host "dictionaries.zip already exists. Exiting script."
    exit
}

cd $dictionariesPath 
curl -L -O "https://github.com/LibreOffice/dictionaries/blob/master/en/en_GB.aff"
curl -L -O "https://github.com/LibreOffice/dictionaries/blob/master/en/en_GB.dic"
curl -L -O "https://github.com/LibreOffice/dictionaries/blob/master/en/en_US.aff"
curl -L -O "https://github.com/LibreOffice/dictionaries/blob/master/en/en_US.dic"
curl -L -O "https://github.com/LibreOffice/dictionaries/blob/master/hu_HU/hu_HU.aff"
curl -L -O "https://github.com/LibreOffice/dictionaries/blob/master/hu_HU/hu_HU.dic"
curl -L -O "https://github.com/LibreOffice/dictionaries/blob/master/hr_HR/hr_HR.aff"
curl -L -O "https://github.com/LibreOffice/dictionaries/blob/master/hr_HR/hr_HR.dic"
curl -L -O "https://github.com/LibreOffice/dictionaries/blob/master/it_IT/it_IT.aff"
curl -L -O "https://github.com/LibreOffice/dictionaries/blob/master/it_IT/it_IT.dic"
curl -L -O "https://github.com/LibreOffice/dictionaries/blob/master/nl_NL/nl_NL.aff"
curl -L -O "https://github.com/LibreOffice/dictionaries/blob/master/nl_NL/nl_NL.dic"
curl -L -O "https://github.com/LibreOffice/dictionaries/blob/master/de/de_DE_frami.aff"
curl -L -O "https://github.com/LibreOffice/dictionaries/blob/master/de/de_DE_frami.dic"
curl -L -O "https://github.com/LibreOffice/dictionaries/blob/master/es/es_ES.aff"
curl -L -O "https://github.com/LibreOffice/dictionaries/blob/master/es/es_ES.dic"
cd ..

Compress-Archive -Path "$dictionariesPath\*" -DestinationPath "dictionaries.zip" -CompressionLevel Optimal
