echo copy the html and js files to the deploy folder on the webserver...

if %computername%==M-CNU422B9PB goto :SASONAR
if %computername%==M-CNU30190S6 goto :PEBIGLER
if %computername%==DARTHALIEN goto :SOTEY
if %computername%==ITS-TFSPRDBLD02 goto :done


::========================================================================================================================================
:PEBIGLER
:SASONAR

echo copy all dlls to the DART Developer's local machine...
xcopy "C:\DART\BBEC-V4\MAIN\Catalog\UM.CustomFx.GratefulPatient.Catalog\UM.CustomFx.GratefulPatient.UiModel\htmlforms\*.*" "C:\BB.4.0.131.0\bbappfx\vroot\browser\htmlforms\" /e /y /r
xcopy "C:\DART\BBEC-V4\MAIN\Catalog\UM.CustomFx.GratefulPatient.Catalog\UM.CustomFx.GratefulPatient.UIModel\bin\debug\*.*" "C:\BB.4.0.131.0\bbappfx\vroot\bin\custom\" /e /y /r
goto :END
::========================================================================================================================================


::========================================================================================================================================
:SOTEY

echo copy all dlls to the local machine...
xcopy "%~dp0htmlforms\*.*" "D:\BBEC\Instances\BB40\bbappfx\vroot\browser\htmlforms\" /e /y /r
xcopy "%~dp0bin\debug\*.*" "D:\BBEC\Instances\BB40\bbappfx\vroot\bin\custom\" /e /y /r
::xcopy "%~dp0bin\debug\*.dll" "C:\BBEC\Instances\BB40\bbappfx\vroot\bin\custom" /e /y /r
::xcopy "%~dp0bin\debug\*.pdb" "C:\BBEC\Instances\BB40\bbappfx\vroot\bin\custom" /e /y /r
goto :END
::========================================================================================================================================

xcopy "%~dp0htmlforms\*.*" "C:\BB.4.0.131.0\bbappfx\vroot\browser\htmlforms\" /e /y /r
xcopy "%~dp0bin\debug\*.*" "C:\BB.4.0.131.0\bbappfx\vroot\bin\custom\" /e /y /r

:done
:END