@echo off
setlocal enabledelayedexpansion


set CODE_COVERAGE_PACKAGE=com.unity.testtools.codecoverage
set PACKAGE_MANIFEST_PATH=GMTK2024\Packages\manifest.json
set CI_PROJECT_NAME="TestCI"
:: Unity test command
set UNITY_CMD="C:\Program Files\Unity\Hub\Editor\2022.3.16f1\Editor\Unity.exe"
set UNITY_DIR="GMTK2024"
set TEST_PLATFORM=StandaloneWindows64
set TESTING_TYPE=JUNIT

echo Testing for %TEST_PLATFORM%, Unit Type: %TESTING_TYPE%

%UNITY_CMD% ^
  -projectPath %UNITY_DIR% ^
  -runTests ^
  -testPlatform %TEST_PLATFORM% ^
  -testResults %UNITY_DIR%\%TEST_PLATFORM%-results.xml ^
  -logFile CON ^
  -batchmode ^
  -nographics ^
  -enableCodeCoverage ^
  -coverageResultsPath %UNITY_DIR%\%TEST_PLATFORM%-coverage ^
  -coverageOptions "generateAdditionalMetrics;generateHtmlReport;generateHtmlReportHistory;generateBadgeReport;" ^
  -debugCodeOptimization

:: Store exit code
set UNITY_EXIT_CODE=%ERRORLEVEL%

:: Check exit code and handle JUNIT conversion if needed
if %UNITY_EXIT_CODE% EQU 0 (
    echo Run succeeded, no failures occurred
) else if %UNITY_EXIT_CODE% EQU 2 (
    echo Run succeeded, some tests failed
    if "%TESTING_TYPE%"=="JUNIT" (
        echo Converting results to JUNit for analysis
        :: Note: You'll need to ensure saxon-b is available on Windows
        saxonb-xslt -s %UNITY_DIR%\%TEST_PLATFORM%-results.xml -xsl %CI_PROJECT_DIR%\ci\nunit-transforms\nunit3-junit.xslt > %UNITY_DIR%\%TEST_PLATFORM%-junit-results.xml
    )
) else if %UNITY_EXIT_CODE% EQU 3 (
    echo Run failure (other failure)
    if "%TESTING_TYPE%"=="JUNIT" (
        echo Not converting results to JUNit
    )
) else (
    echo Unexpected exit code %UNITY_EXIT_CODE%
    if "%TESTING_TYPE%"=="JUNIT" (
        echo Not converting results to JUNit
    )
)

:: Check for code coverage package and handle coverage reports
findstr /C:"%CODE_COVERAGE_PACKAGE%" "%PACKAGE_MANIFEST_PATH%" >nul
if %ERRORLEVEL% EQU 0 (
    type "%UNITY_DIR%\%TEST_PLATFORM%-coverage\Report\Summary.xml" | findstr "Linecoverage"
    move "%UNITY_DIR%\%TEST_PLATFORM%-coverage\%CI_PROJECT_NAME%-opencov\*Mode\TestCoverageResults_*.xml" "%UNITY_DIR%\%TEST_PLATFORM%-coverage\coverage.xml"
    rmdir /S /Q "%UNITY_DIR%\%TEST_PLATFORM%-coverage\%CI_PROJECT_NAME%-opencov"
) else (
    echo [33mCode Coverage package not found in %PACKAGE_MANIFEST_PATH%. Please install the package "Code Coverage" through Unity's Package Manager to enable coverage reports.[0m
)

:: Display test results
type "%UNITY_DIR%\%TEST_PLATFORM%-results.xml" | findstr "test-run" | findstr "Passed"

exit /b %UNITY_EXIT_CODE%
endlocal