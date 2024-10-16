﻿using Builder;
#if UNITY_IOS
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
#endif

namespace L10nPostProcessor {
    public class LocalizePostProcessor {
    #if UNITY_IOS
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string buildPath) {
            if (buildTarget == BuildTarget.iOS) {
                L10nLoader.LoadL10nFile();
                var project = new PBXProject();
                var projectPath = PBXProject.GetPBXProjectPath(buildPath);
                project.ReadFromFile(projectPath);

                if (project != null) {
                    var targetId = "";
                #if UNITY_2019_3_OR_NEWER
                    targetId = project.GetUnityFrameworkTargetGuid();
                #else
                        targetId = project.TargetGuidByName("Unity-iPhone");
                #endif
                    project.AddFrameworkToProject(targetId, "AppTrackingTransparency.framework", true);
                    project.AddFrameworkToProject(targetId, "AdSupport.framework", false);
                    project.AddFrameworkToProject(targetId, "StoreKit.framework", false);
                    project.WriteToFile(PBXProject.GetPBXProjectPath(buildPath));
                }

                /*
                 * PList
                 */
                var plist = new PlistDocument();
                plist.ReadFromString(File.ReadAllText(buildPath + "/Info.plist"));
                if (plist != null) {
                    // Get root
                    var rootDict = plist.root;
                    var mainTargetId = "";
                #if UNITY_2019_3_OR_NEWER
                    mainTargetId = project.GetUnityMainTargetGuid();
                #else
                    mainTargetId = project.TargetGuidByName("Unity-iPhone");
                #endif

                #if USE_GEO
                        //Add geolocation request localization
                        var geolocationVersion = BuildApps.iOSBuildConfig.GeolocationLocalizationVersion;
                        var geolocationDictionary = L10nLoader.geoLocationDict[geolocationVersion];
                        
                        rootDict.SetString("NSLocationAlwaysUsageDescription", geolocationDictionary["EN"]);
                        rootDict.SetString("NSLocationWhenInUseUsageDescription", geolocationDictionary["EN"]);
                        File.WriteAllText(buildPath + "/Info.plist", plist.WriteToString());

                        foreach (var (systemLanguage, localizedMessageString) in geolocationDictionary) {
                            AddDescriptionLocalizedString(
                                "NSLocationAlwaysUsageDescription", 
                                localizedMessageString,
                                systemLanguage,
                                buildPath,
                                project, 
                                mainTargetId);
                            
                            AddDescriptionLocalizedString(
                                "NSLocationWhenInUseUsageDescription", 
                                localizedMessageString,
                                systemLanguage,
                                buildPath,
                                project, 
                                mainTargetId);
                        }
                #else
                    var basePath = $"{buildPath}/Libraries/Common/Plugins/iOS/";
                    const string fileName = "LocationGeocoder.mm";
                    var fileGuid = project.FindFileGuidByRealPath($"Libraries/Common/Plugins/iOS/{fileName}");
                    project.RemoveFile(fileGuid);
                    if (File.Exists($"{basePath}{fileName}")) {
                        var fileList = Directory.GetFiles(basePath, fileName);
                        foreach (var file in fileList) {
                            File.Delete(file);
                        }    
                    }
                #endif
                    //Add tracking localization
                    var appTrackingVersion = BuildApps.iOSBuildConfig.AppTrackingLocalizationVersion;
                    var appTrackingDictionary = L10nLoader.appTrackingDict[appTrackingVersion];
                    
                    rootDict.SetString("NSUserTrackingUsageDescription", appTrackingDictionary["EN"]);
                    File.WriteAllText(buildPath + "/Info.plist", plist.WriteToString());
                    
                    foreach (var (systemLanguage, localizedMessageString) in appTrackingDictionary) {
                        AddDescriptionLocalizedString(
                            "NSUserTrackingUsageDescription",
                            localizedMessageString,
                            systemLanguage,
                            buildPath,
                            project,
                            mainTargetId);
                    }
                }
                project.WriteToFile(PBXProject.GetPBXProjectPath(buildPath));
            }
        }

        private static void AddDescriptionLocalizedString(string localizationKey, string locationUsageDescription,
                                                          string localeCode, string buildPath, PBXProject project,
                                                          string targetGuid) {
            const string resourcesDirectoryName = "Localizations";
            var resourcesDirectoryPath = Path.Combine(buildPath, resourcesDirectoryName);
            var localeSpecificDirectoryName = localeCode + ".lproj";
            var localeSpecificDirectoryPathToCreate = Path.Combine(resourcesDirectoryPath, localeSpecificDirectoryName);
            var localeSpecificDirectoryPath = Path.Combine(resourcesDirectoryName, localeSpecificDirectoryName);
            var infoPlistStringsFilePath = Path.Combine(localeSpecificDirectoryPathToCreate, "InfoPlist.strings");

            // Create intermediate directories as needed.
            if (!Directory.Exists(resourcesDirectoryPath)) {
                Directory.CreateDirectory(resourcesDirectoryPath);
            }

            if (!Directory.Exists(localeSpecificDirectoryPathToCreate)) {
                Directory.CreateDirectory(localeSpecificDirectoryPathToCreate);
            }

            var localizedDescriptionLine1 = $"\"{localizationKey}\" = \"" + locationUsageDescription + "\";\n";
            // File already exists, update it in case the value changed between builds.
            if (File.Exists(infoPlistStringsFilePath)) {
                var output = new List<string>();
                var lines = File.ReadAllLines(infoPlistStringsFilePath);
                var keyUpdated = false;
                foreach (var line in lines) {
                    if (line.Contains(localizationKey)) {
                        output.Add(localizedDescriptionLine1);
                        keyUpdated = true;
                    } else {
                        output.Add(line);
                    }
                }

                if (!keyUpdated) {
                    output.Add(localizedDescriptionLine1);
                }

                File.WriteAllText(infoPlistStringsFilePath, string.Join("\n", output.ToArray()) + "\n");
            }
            // File doesn't exist, create one.
            else {
                File.WriteAllText(infoPlistStringsFilePath,
                    "/* Localized versions of Info.plist keys */\n" + localizedDescriptionLine1);
            }

            var guid = project.AddFolderReference(localeSpecificDirectoryPath,
                Path.Combine(resourcesDirectoryName, localeSpecificDirectoryName), PBXSourceTree.Source);
            project.AddFileToBuild(targetGuid, guid);
        }
    #endif
    }
}
