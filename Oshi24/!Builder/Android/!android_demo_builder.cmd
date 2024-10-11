cd ..
cd ..
set "_cd=%cd:\=" & set "_cd=%"

set PACKAGE_TYPE=%1
set PROJECT_DIR=%_cd%
set ALL_PROJECTS_PATH=%2
set BUILDS_ROOT_PATH=%3
set UNITY_PATH=%4

IF %PACKAGE_TYPE%==apk %UNITY_PATH% -quit -batchmode -logFile "%BUILDS_ROOT_PATH%/%PROJECT_DIR%/logs.txt" -projectPath "%ALL_PROJECTS_PATH%/%PROJECT_DIR%" -buildpath "%BUILDS_ROOT_PATH%/%PROJECT_DIR%" -bundletype apk -executeMethod Builder.BuildApps.AndroidBuildProdAPK
IF %PACKAGE_TYPE%==aab %UNITY_PATH% -quit -batchmode -logFile "%BUILDS_ROOT_PATH%/%PROJECT_DIR%/logs.txt" -projectPath "%ALL_PROJECTS_PATH%/%PROJECT_DIR%" -buildpath "%BUILDS_ROOT_PATH%/%PROJECT_DIR%" -bundletype aab -executeMethod Builder.BuildApps.AndroidBuildProdAAB