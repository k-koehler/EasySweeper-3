; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{C76843D6-97DD-4B4D-8C8B-039B5127B7FA}
AppName=Easy Winterface
AppVersion=1.3
;AppVerName=Easy Winterface 1.3
AppPublisher=Kevin Koehler
DefaultDirName={pf}\Dungeoneering
DisableProgramGroupPage=yes
OutputBaseFilename=setup
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "C:\Users\Chelsea\Source\Repos\EasySweeper-3\EasySweeper 3\EasyWinterface\bin\Release\EasyWinterface.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Chelsea\Source\Repos\EasySweeper-3\EasySweeper 3\EasyWinterface\Secrets.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Chelsea\Source\Repos\EasySweeper-3\EasySweeper 3\EasyWinterface\Glyphs\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs 
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{commonprograms}\Easy Winterface"; Filename: "{app}\EasyWinterface.exe"
Name: "{commondesktop}\Easy Winterface"; Filename: "{app}\EasyWinterface.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\EasyWinterface.exe"; Description: "{cm:LaunchProgram,Easy Winterface}"; Flags: nowait postinstall skipifsilent

