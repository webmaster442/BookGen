$dictionariesPath = ".\dictionaries"
if (-not (Test-Path $dictionariesPath)) {
    New-Item -ItemType Directory -Path $dictionariesPath | Out-Null
}

cd $dictionariesPath 
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
cd ..

