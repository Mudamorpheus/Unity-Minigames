#!/bin/zsh

ALL_PROJECTS_PATH="/Users/denispiskunov/MINT/demo_games"

BUILDS_ROOT_PATH="/Users/denispiskunov/MINT/iOS_builds/demo/store_builds"

UNITY_PATH="/Applications/Unity/Hub/Editor/2022.3.13f1/Unity.app/Contents/MacOS/Unity"

PROJECT_TYPE="store"

zsh !ios_demo_builder.sh $PROJECT_TYPE $ALL_PROJECTS_PATH $BUILDS_ROOT_PATH $UNITY_PATH