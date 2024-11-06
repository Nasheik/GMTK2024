@echo off
setlocal enabledelayedexpansion

:: Unity build command
set UNITY_CMD="C:\Program Files\Unity\Hub\Editor\2022.3.16f1\Editor\Unity.exe"
set UNITY_DIR="GMTK2024"

%UNITY_CMD% ^
  -projectPath %UNITY_DIR% ^
  -quit ^
  -batchmode ^
  -nographics ^
  -executeMethod WebGLBuilder.BuildGame ^
  -logFile


:: Store exit code
set UNITY_EXIT_CODE=%ERRORLEVEL%

:: Check exit code
if %UNITY_EXIT_CODE% EQU 0 (
    echo Run succeeded, no failures occurred
) else if %UNITY_EXIT_CODE% EQU 2 (
    echo Run succeeded, some tests failed
) else if %UNITY_EXIT_CODE% EQU 3 (
    echo Run failure (other failure)
) else (
    echo Unexpected exit code %UNITY_EXIT_CODE%
)

:: List contents of build directory
dir /B "%BUILD_PATH%"

:: Check if build folder is empty
dir /B "%BUILD_PATH%\*" >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo Build folder is empty!
    exit /b 1
)


endlocal