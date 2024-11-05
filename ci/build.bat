set UNITY_CMD="C:\Program Files\Unity\Hub\Editor\2022.3.16f1\Editor\Unity.exe"
set UNITY_DIR="GMTK2024"

%UNITY_CMD% ^
  -projectPath %UNITY_DIR% ^
  -quit ^
  -batchmode ^
  -nographics ^
  -executeMethod WebGLBuilder.BuildGame ^
  -logFile