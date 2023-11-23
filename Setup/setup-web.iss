#include "version.iss"
#include "commons.iss"

[Setup]
AppId={{DE96DAA6-EF13-4FB9-BFA7-2912AF474AE5}
OutputDir=..\bin
OutputBaseFilename=published

[Files]
Source: "..\bin\Release\*"; DestDir: "{app}"; Flags: ignoreversion createallsubdirs recursesubdirs