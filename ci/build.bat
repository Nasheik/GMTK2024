set UNITY_CMD="C:\Program Files\Unity\Hub\Editor\2020.3.26f1\Editor\Unity.exe"
set UNITY_DIR="GMTK2024"

%UNITY_CMD% ^
  -projectPath %UNITY_DIR% ^
  -quit ^
  -batchmode ^
  -nographics ^
  -executeMethod WebGLBuilder.BuildGame ^
  -logFile