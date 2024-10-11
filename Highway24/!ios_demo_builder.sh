#!/bin/zsh
PROJECT_DIR="${PWD##*/}"

PROJECT_TYPE=$1

ALL_PROJECTS_PATH=$2

BUILDS_ROOT_PATH=$3

UNITY_PATH=$4

CONFIG_FILE="build_config.json"

if  [ $PROJECT_TYPE = "test" ]; then
    "$UNITY_PATH" -quit -batchmode -logFile "$BUILDS_ROOT_PATH/$PROJECT_DIR/logs.txt" -projectPath "$ALL_PROJECTS_PATH/$PROJECT_DIR" -buildpath "$BUILDS_ROOT_PATH/$PROJECT_DIR" -config "$ALL_PROJECTS_PATH/$PROJECT_DIR/$CONFIG_FILE" -executeMethod Builder.BuildApps.iOSTestBuild
else
    "$UNITY_PATH" -quit -batchmode -logFile "$BUILDS_ROOT_PATH/$PROJECT_DIR/logs.txt" -projectPath "$ALL_PROJECTS_PATH/$PROJECT_DIR" -buildpath "$BUILDS_ROOT_PATH/$PROJECT_DIR" -config "$ALL_PROJECTS_PATH/$PROJECT_DIR/$CONFIG_FILE" -executeMethod Builder.BuildApps.iOSProductionBuild
fi