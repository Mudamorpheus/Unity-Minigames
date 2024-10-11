REM Директория со всеми проектами, например: set ALL_PROJECTS_PATH=d:/WORK/GAMES 
set ALL_PROJECTS_PATH=y:/MINT/demo_games
REM Директория для готовых билдов, например: set BUILDS_ROOT_PATH=d:/WORK/Android_builds
set BUILDS_ROOT_PATH=y:/MINT/Android_builds/demo_games
REM Путь до Unity, поменять на актуальный.
set UNITY_PATH=C:/Program Files/Unity/Hub/Editor/2022.3.11f1/Editor/Unity.exe
REM Тип пакета: aab или apk
set PACKAGE_TYPE=apk

!android_demo_builder %PACKAGE_TYPE% %ALL_PROJECTS_PATH% %BUILDS_ROOT_PATH% "%UNITY_PATH%"