set dir=%~dp0
for %%a in (%dir:~0,-1%) do set name=%%~na

cd G:\불얼모드
G:

if exist "%dir%%name%\bin\Debug\net4.8" (
	copy /Y %dir%%name%\bin\Debug\net4.8\%name%.dll %name%\%name%\%name%.dll
) else if exist "%dir%%name%\bin\Debug" (
	copy /Y %dir%%name%\bin\Debug\%name%.dll %name%\%name%\%name%.dll
) else (
	copy /Y %dir%\bin\Debug\%name%.dll %name%\%name%\%name%.dll
)

del %name%.zip
cd %name%

zip -r G:\불얼모드\%name%.zip %name%

copy /Y G:\불얼모드\%name%.zip G:\UnityModManager\모드\%name%.zip

if not exist "C:\Program Files (x86)\Steam\steamapps\common\A Dance of Fire and Ice\Mods\%name%" (
	mkdir "C:\Program Files (x86)\Steam\steamapps\common\A Dance of Fire and Ice\Mods\%name%"
)
xcopy G:\불얼모드\%name%\%name%\*.* "C:\Program Files (x86)\Steam\steamapps\common\A Dance of Fire and Ice\Mods\%name%\" /K /Y

setlocal enabledelayedexpansion
set string=
for /f "delims=" %%G in (G:\불얼모드\%name%\%name%\Info.json) do (
    set line=%%G
    set string=!string!!line: =!
)
set string=%string:"=%
set "string=%string:~1,-1%"
set "string=%string::==%"
set "string[%string:,=" & set "string[%"

set version=%string[Version:.=-%

if not exist G:\UnityModManager\ADOFAI\%name% (
	mkdir G:\UnityModManager\ADOFAI\%name%
)
copy /Y G:\UnityModManager\모드\%name%.zip G:\UnityModManager\ADOFAI\%name%\%name%-%version%.zip
pause