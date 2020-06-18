REM COPY AND RENAME FILE AS "postbuild.user.bat" AND CHANGE LOCAL PATH VALUES.  PLEASE DO NOT ADD "postbuild.user.bat" TO SOURCE CONTROL.

if %computername%==DARTHALIEN goto :SOTEY
if %computername%==M-CNU30190S6 goto :PEBIGLER
if %computername%==M-CNU422B9PB goto :SASONAR
if %computername%==ITS-TFSPRDBLD02 goto :done

::========================================================================================================================================
:PEBIGLER
:SASONAR
echo copy all dlls to DART Developer's local machine...
xcopy  "%~dp0bin\debug\*.*" "C:\BB.4.0.131.0\bbappfx\vroot\bin\custom\" /e /y /r
goto :END
::========================================================================================================================================

::========================================================================================================================================
:SOTEY

echo copy all dlls to the local machine...
xcopy "%~dp0bin\debug\*.*" "D:\BBEC\Instances\BB40\bbappfx\vroot\bin\custom\" /e /y /r
goto :END
::========================================================================================================================================


xcopy "%~dp0bin\debug\*.*" "C:\BB.4.0.131.0\bbappfx\vroot\bin\custom\" /e /y /r

:done
:END
