using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace SmartNS.Editor {
    public class SmartNsConversionWindow : EditorWindow {
        [MenuItem("[PROJECT]/[AppPrepare helper]/===>>> Namespace Converter <<<===")]
        static void Init() {
            // Get existing open window or if none, make a new one:
            SmartNsConversionWindow window =
                (SmartNsConversionWindow) EditorWindow.GetWindow(typeof(SmartNsConversionWindow));
            window.titleContent = new GUIContent("Namespace Converter");
            window.Show();
        }

        private string _baseDirectory = "";
        private string _heapDirectory = "";
        private bool _isProcessing = false;
        private List<string> _assetsToProcess;
        private int _progressCount;


        private string _scriptRootSettingsValue;
        private string _prefixSettingsValue;
        private string _universalNamespaceSettingsValue;
        private bool _useSpacesSettingsValue;
        private int _numberOfSpacesSettingsValue;
        private string _directoryDenyListSettingsValue;
        private bool _enableDebugLogging;

        private HashSet<string> _ignoredDirectories;


        private static string GetClickedDirFullPath() {
            if (Selection.assetGUIDs.Length > 0) {
                var clickedAssetGuid = Selection.assetGUIDs[0];
                var clickedPath = AssetDatabase.GUIDToAssetPath(clickedAssetGuid);
                var clickedPathFull = Path.Combine(Directory.GetCurrentDirectory(), clickedPath);

                FileAttributes attr = File.GetAttributes(clickedPathFull);

                if (attr.HasFlag(FileAttributes.Directory)) {
                    // This is a directory. Return it.
                    return clickedPath;
                } else {
                    // Strip off the file name.
                    var lastForwardSlashIndex = clickedPath.LastIndexOf('/');
                    var lastBackSlashIndex = clickedPath.LastIndexOf('\\');

                    if (lastForwardSlashIndex >= 0) {
                        return clickedPath.Substring(0, lastForwardSlashIndex);
                    } else if (lastBackSlashIndex >= 0) {
                        return clickedPath.Substring(0, lastBackSlashIndex);
                    }
                }
            }

            return null;
        }

        void OnGUI() {
            if (string.IsNullOrWhiteSpace(_baseDirectory)) {
                _baseDirectory = GetClickedDirFullPath();
            }

            if (string.IsNullOrWhiteSpace(_baseDirectory)) {
                _baseDirectory = Directory.Exists("Assets/Game") ? "Assets/Game" : "Assets";
            }

            if (!string.IsNullOrEmpty(PrepareAppResources.HeapName)) {
                _baseDirectory = PrepareAppResources.HeapName;
                _heapDirectory = PrepareAppResources.HeapName;
            }

            var yPos = 20;
            var baseDirectoryLabel = new GUIContent($"Base Directory: {(string.IsNullOrEmpty(PrepareAppResources.HeapName) ? _baseDirectory : PrepareAppResources.HeapName)}",
                "SmartNS will search all scripts in, or below, this directory. Use this to limit the search to a subdirectory.");

            if (GUI.Button(new Rect(3, yPos, position.width - 6, 20), baseDirectoryLabel)) {
                var fullPath = EditorUtility.OpenFolderPanel("Choose root folder", _baseDirectory, "");
                _baseDirectory = fullPath.Replace(Application.dataPath, "Assets").Trim();
                if (string.IsNullOrWhiteSpace(_baseDirectory)) {
                    _baseDirectory = "Assets";
                }
            }

            yPos += 30;
            
            if (!_isProcessing) {
                var submitButtonLight = new GUIContent("Light [Only Rename file tree]", "Begin rename files");
                var submitButtonHard = new GUIContent("Hardcore [Rename and shuffle files]", "Begin shuffling files");
                var submitButtonContent = new GUIContent("Namespace Conversion", "Begin processing scripts");
                var submitButtonStyle = new GUIStyle(GUI.skin.button);
                submitButtonStyle.normal.textColor = new Color(1, 1, 1);
                
                if (GUI.Button(new Rect(position.width / 2 - 10 - 250, yPos, 250, 30), submitButtonLight,
                        submitButtonStyle)) {
                    if (EditorUtility.DisplayDialog("Внимание!",
                            $"Файлы из каталога {_baseDirectory} и из вложенных каталогов будут переименованы случайным образом и перемещены в новый каталог С СОХРАНЕНИЕМ СТУКТУРЫ КАТАЛОГОВ. Прежде чем начать, СЛЕДУЕТ СОЗДАТЬ РЕЗЕРВНУЮ КОПИЮ ПРОЕКТА на случай, если что-то пойдет не так.",
                            "Ok",
                            "Cancel")) {
                        PrepareAppResources.LightShuffle(_baseDirectory);
                    }
                }
                
                if (GUI.Button(new Rect(position.width / 2 + 10, yPos, 250, 30), submitButtonHard,
                        submitButtonStyle)) {
                    if (EditorUtility.DisplayDialog("Внимание!",
                    $"Файлы из каталога {_baseDirectory} и из вложенных каталогов будут переименованы случайным образом и перемещены в новый каталог БЕЗ СОХРАНЕНИЯ СТРУКТУРЫ КАТАЛОГОВ. Прежде чем начать, СЛЕДУЕТ СОЗДАТЬ РЕЗЕРВНУЮ КОПИЮ ПРОЕКТА на случай, если что-то пойдет не так.",
                    "ОК",
                    "Отмена")) {
                        PrepareAppResources.HardcoreShuffle(_baseDirectory);
                    }
                }
                
                yPos += 50;
                if (GUI.Button(new Rect(position.width / 2 - 350 / 2, yPos, 350, 30), submitButtonContent,
                        submitButtonStyle)) {
                    var assetBasePath = string.IsNullOrEmpty(_heapDirectory) ? _baseDirectory : _heapDirectory;
                    if (!assetBasePath.EndsWith("/")) {
                        assetBasePath += "/";
                    }


                    _assetsToProcess = GetAssetsToProcess(assetBasePath);

                    if (EditorUtility.DisplayDialog("Внимание!",
                    $"Будет обработано {_assetsToProcess.Count} скриптов, найденных в каталоге «{assetBasePath}» и во вложенных каталогах, и их пространства имен будут обновлены на основе текущих настроек SmartNS. Вы уверены, что хотите это сделать?",
                            string.Format("OK", _assetsToProcess.Count),
                            "Отмена")) {
                        var smartNSSettings = SmartNSSettings.GetSerializedSettings();
                        _scriptRootSettingsValue = smartNSSettings.FindProperty("m_ScriptRoot").stringValue;
                        _prefixSettingsValue = smartNSSettings.FindProperty("m_NamespacePrefix").stringValue;
                        _universalNamespaceSettingsValue =
                            smartNSSettings.FindProperty("m_UniversalNamespace").stringValue;
                        _useSpacesSettingsValue = smartNSSettings.FindProperty("m_IndentUsingSpaces").boolValue;
                        _numberOfSpacesSettingsValue = smartNSSettings.FindProperty("m_NumberOfSpaces").intValue;
                        _directoryDenyListSettingsValue =
                            smartNSSettings.FindProperty("m_DirectoryIgnoreList").stringValue;
                        _enableDebugLogging = smartNSSettings.FindProperty("m_EnableDebugLogging").boolValue;

                        // Cache this once now, for performance reasons.
                        _ignoredDirectories = global::SmartNS.Editor.SmartNS.GetIgnoredDirectories();


                        _progressCount = 0;
                        _isProcessing = true;
                    }
                }

                

            }


            if (_isProcessing) {
                var cancelButtonContent = new GUIContent("Cancel", "Cancel script conversion");
                var cancelButtonStyle = new GUIStyle(GUI.skin.button);
                cancelButtonStyle.normal.textColor = new Color(.5f, 0, 0);
                if (GUI.Button(new Rect(position.width / 2 - 50 / 2, yPos, 50, 30), cancelButtonContent,
                        cancelButtonStyle)) {
                    _isProcessing = false;
                    _progressCount = 0;
                    AssetDatabase.Refresh();
                    Log("Cancelled");
                }

                yPos += 40;

                if (_progressCount < _assetsToProcess.Count) {
                    EditorGUI.ProgressBar(new Rect(3, yPos, position.width - 6, 20),
                        (float) _progressCount / (float) _assetsToProcess.Count,
                        string.Format("Processing {0} ({1}/{2})", _assetsToProcess[_progressCount], _progressCount,
                            _assetsToProcess.Count));
                    Log("Processing " + _assetsToProcess[_progressCount]);

                    global::SmartNS.Editor.SmartNS.UpdateAssetNamespace(_assetsToProcess[_progressCount],
                        _scriptRootSettingsValue,
                        _prefixSettingsValue,
                        _universalNamespaceSettingsValue,
                        _useSpacesSettingsValue,
                        _numberOfSpacesSettingsValue,
                        _directoryDenyListSettingsValue,
                        _enableDebugLogging,
                        directoryIgnoreList: _ignoredDirectories);

                    _progressCount++;
                } else {
                    // We done. 
                    global::SmartNS.Editor.SmartNS.UpdateUsings();
                    
                    _isProcessing = false;
                    _ignoredDirectories = null;
                    _progressCount = 0;
                    AssetDatabase.Refresh();
                    Debug.Log("===>>> Namespace Conversion complete.");
                }
            }
        }
        
        
        
        private List<string> GetAssetsToProcess(string assetBasePath) {
            var ignoredDirectories = global::SmartNS.Editor.SmartNS.GetIgnoredDirectories();

            Func<string, bool> isInIgnoredDirectory = (assetPath) => {
                var indexOfAsset = Application.dataPath.LastIndexOf("Assets");
                var fullFilePath = Application.dataPath.Substring(0, indexOfAsset) + assetPath;
                var fileInfo = new FileInfo(fullFilePath);
                return ignoredDirectories.Contains(fileInfo.Directory.FullName);
            };

            return AssetDatabase.GetAllAssetPaths()
                .Where(s => s.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)
                            // We ALWAYS require that the scripts be within Assets, regardless of anything else. We don't want to clobber Packages, for example.
                            && s.StartsWith("Assets", StringComparison.OrdinalIgnoreCase)
                            && s.StartsWith(assetBasePath, StringComparison.OrdinalIgnoreCase)
                            && !isInIgnoredDirectory(s)).ToList();
        }

        void Update() {
            if (_isProcessing) {
                // Without this, we don't get updates every frame, and the whole window just creeps along.
                Repaint();
            }
        }

        private void Log(string message) {
            Debug.Log(string.Format("[SmartNS] {0}", message));
        }
    }
}
