REM COPY AND RENAME FILE AS "postbuild.user.bat" AND CHANGE LOCAL PATH VALUES.  PLEASE DO NOT ADD "postbuild.user.bat" TO SOURCE CONTROL.

if %computername%==M-CND6457J51 goto :PEBIGLER
if %computername%==M-CNU422B9PB goto :SASONAR
if %computername%==DARTHALIEN goto :SOTEY
if %computername%==ITS-TFSPRDBLD02 goto :done

::========================================================================================================================================
:PEBIGLER
:SASONAR
echo copy all dlls to DART Developer's local machine...
xcopy "C:\DART\BBEC-V4\MAIN\Catalog\UM.CustomFx.GratefulPatient.Catalog\UM.CustomFx.GratefulPatient.UiModel\htmlforms\*.*" "C:\BB.4.0.131.0\bbappfx\vroot\browser\htmlforms\" /e /y /r
xcopy "C:\DART\BBEC-V4\MAIN\Catalog\UM.CustomFx.GratefulPatient.Catalog\UM.CustomFx.GratefulPatient.Catalog\bin\debug\*.*" "C:\BB.4.0.131.0\bbappfx\vroot\bin\custom\" /e /y /r
goto :END
::========================================================================================================================================

::========================================================================================================================================
:SOTEY

echo copy all dlls to the local machine...
xcopy "%~dp0htmlforms\*.*" "D:\BBEC\Instances\BB40\bbappfx\vroot\browser\htmlforms\" /e /y /r
xcopy "%~dp0bin\debug\*.*" "D:\BBEC\Instances\BB40\bbappfx\vroot\bin\custom\" /e /y /r
goto :END
::========================================================================================================================================



xcopy "%~dp0htmlforms\*.*" "C:\BB.4.0.131.0\bbappfx\vroot\browser\htmlforms\" /e /y /r
xcopy "%~dp0bin\debug\*.*" "C:\BB.4.0.131.0\bbappfx\vroot\bin\custom\" /e /y /r

:done
:END
