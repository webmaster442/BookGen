#include "version.iss"

#define MyAppName "BookGen"
#define MyAppPublisher "webmaster442"
#define MyAppURL "https://github.com/webmaster442/BookGen"
#define LauncherBootstrapper "BookGen.Launcher.exe"
#define CliBootstrapper "BookGen.exe"
#define CliIconName "BookGen shell"
#define LauncherIconName "BookGen launcher"

[Setup]
AppId={{EA96EDA6-EF13-4AD9-BFA6-1800AF466EA4}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName=c:\{#MyAppName}
DefaultGroupName={#MyAppName}
LicenseFile=licence.rtf
OutputDir=..\bin
OutputBaseFilename=published.exe
SolidCompression=yes
InternalCompressLevel=max
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog commandline
UsePreviousPrivileges=false

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "..\bin\Release\*"; DestDir: "{app}"; Flags: ignoreversion createallsubdirs recursesubdirs

[Icons]
Name: "{group}\{#LauncherBootstrapper}"; Filename: "{app}\{#LauncherBootstrapper}"
Name: "{group}\{#CliBootstrapper}"; Filename: "{app}\{#CliBootstrapper}"
Name: "{group}\{cm:ProgramOnTheWeb,{#MyAppName}}"; Filename: "{#MyAppURL}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#CliIconName}"; Filename: "{app}\{#LauncherBootstrapper}"; Tasks: desktopicon
Name: "{commondesktop}\{#LauncherIconName}"; Filename: "{app}\{#CliBootstrapper}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#LauncherBootstrapper}"; Flags: nowait postinstall skipifsilent; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"
Filename: "https://www.microsoft.com/store/productId/9N0DX20HK701"; Flags: runasoriginaluser shellexec nowait postinstall skipifsilent; Check: CheckTerminalInstall 
Filename: "https://www.microsoft.com/store/productId/9MZ1SNWT0N5D"; Flags: runasoriginaluser shellexec nowait postinstall skipifsilent; Check: CheckPsCoreInstall

[Code]
function StrSplit(Text: String; Separator: String): TArrayOfString;
var
  i, p: Integer;
  Dest: TArrayOfString; 
begin
  i := 0;
  repeat
    SetArrayLength(Dest, i+1);
    p := Pos(Separator,Text);
    if p > 0 then begin
      Dest[i] := Copy(Text, 1, p-1);
      Text := Copy(Text, p + Length(Separator), Length(Text));
      i := i + 1;
    end else begin
      Dest[i] := Text;
      Text := '';
    end;
  until Length(Text)=0;
  Result := Dest
end;

function IsInstalled(programName: String) : Boolean;
var 
  pathArray: array of String;
  i: integer;
  directory: string;
  installed: Boolean;
begin
  installed := false;
  pathArray := StrSplit(ExpandConstant('{%PATH}'), ';');
  for i:=0 to Length(pathArray)-1 do
  begin
    directory := pathArray[i];
    if FileExists(directory+'\'+programName) then begin
      installed := true;
    end;
  end; 
  Result := installed;
end;

function CheckTerminalInstall() : Boolean;
begin
  Result := not IsInstalled('wt.exe');
end;

function CheckPsCoreInstall() : Boolean;
begin
  Result := not IsInstalled('pwsh.exe'); 
end;

function GetUninstallString(): String;
var
  sUnInstPath: String;
  sUnInstallString: String;
begin
  sUnInstPath := ExpandConstant('Software\Microsoft\Windows\CurrentVersion\Uninstall\{#emit SetupSetting("AppId")}_is1');
  sUnInstallString := '';
  if not RegQueryStringValue(HKLM, sUnInstPath, 'UninstallString', sUnInstallString) then
    RegQueryStringValue(HKCU, sUnInstPath, 'UninstallString', sUnInstallString);
  Result := sUnInstallString;
end;

function UnInstallOldVersion(): Integer;
var
  sUnInstallString: String;
  iResultCode: Integer;
begin
  // Return Values:
  // 1 - uninstall string is empty
  // 2 - error executing the UnInstallString
  // 3 - successfully executed the UnInstallString
  // default return value
  Result := 0;
  // get the uninstall string of the old app
  sUnInstallString := GetUninstallString();
  if sUnInstallString <> '' then begin
    sUnInstallString := RemoveQuotes(sUnInstallString);
    if Exec(sUnInstallString, '/SILENT /NORESTART /SUPPRESSMSGBOXES','', SW_HIDE, ewWaitUntilTerminated, iResultCode) then
      Result := 3
    else
      Result := 2;
  end else
    Result := 1;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if (CurStep=ssInstall) then
  begin
    if (GetUninstallString() <> '') then UnInstallOldVersion();
  end;
end;                 
