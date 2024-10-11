using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using Random = System.Random;

namespace SmartNS.Editor {

    public class PrepareAppResources : MonoBehaviour {

        private static readonly List<DirectoryInfo> DirsTree = new();
        private static List<string> _shuffledDict = new();
        private static int _cursor = 0;
        private const string RootDir = "Assets";
        private static string _sourceDir = string.Empty;
        public static string HeapName = string.Empty;
        private static T DeserializeFromFile<T>(string filePath) {
            var textAsset = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(textAsset);
        }

        private static List<string> LoadDict() {
            const string filePath = "External_configs/words.json";
            if (!File.Exists(filePath)) return new List<string>();
            var dict = DeserializeFromFile<List<string>>(filePath);
            return Shuffle(dict) ?? new List<string>();
        }

        private static List<string> Shuffle(List<string> list) {
            var random = new Random();
            var n = list.Count;
            while (n > 1) {
                n--;
                var k = random.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }

            return list;
        }

        private static void LoadDirs(string path) {
            var di = new DirectoryInfo(path);
            DirsTree.Add(di);
            var subDirs = di.GetDirectories();
            foreach (var subDir in subDirs) {
                LoadDirs(subDir.FullName);
            }
        }

        private static string GetNextWord() {
            if (!(_shuffledDict?.Count > _cursor)) return string.Empty;
            var currentDict =
                System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_shuffledDict[_cursor]);
            _cursor++;
            return currentDict;
        }

        private static string GetRandomGuid() {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
        }
        
        public static void LightShuffle(string path) {
            _sourceDir = path;
            if (!string.IsNullOrEmpty(_sourceDir)) {
                ShuffledDirsTree(false);
            }
            
        }

        public static void HardcoreShuffle(string path) {
            _sourceDir = path;
            if (!string.IsNullOrEmpty(_sourceDir)) {
                ShuffledDirsTree(true);
            }
        }

        private static void ShuffledDirsTree(bool moveToHeap = false) {
            _shuffledDict = LoadDict();

            LoadDirs(_sourceDir);
            RenameFiles(_sourceDir);
            RenameDirs();

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            CompilationPipeline.RequestScriptCompilation();
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            DirsTree.Clear();
            LoadDirs(_sourceDir);

            HeapName = $"{RootDir}/{GetNextWord()}";
            if (moveToHeap)
                MoveToHeap(HeapName);
            else
                SimpleRenameRootDir(HeapName);

            DeleteEditorPlugins();

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            CompilationPipeline.RequestScriptCompilation();
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        private static void RenameFiles(string path) {
            var di = new DirectoryInfo(path);
            var filesInfo = di.GetFiles("*.*");
            foreach (var fi in filesInfo) {
                var dir = fi.DirectoryName;
                var fileName = Path.GetFileNameWithoutExtension(fi.FullName);
                var ext = Path.GetExtension(fi.FullName);

                //Не трогаем метафайлы, сцены и ассеты и т.д.
                if (new List<string> {
                        ".anim",
                        ".asset",
                        ".cginc",
                        ".controller",
                        ".dll",
                        ".fbx",
                        ".mat",
                        ".meta",
                        ".mp3",
                        ".physicMaterial",
                        ".prefab",
                        ".shader",
                        ".unity",
                        ".wav"
                    }.Contains(ext)) {
                    continue;
                }

                // Не трогаем скрипт, если рядом с ним есть ассет с тем же именем. В дальнейшем преименовывать их парно.
                if (ext.Equals(".cs", StringComparison.InvariantCultureIgnoreCase) &&
                    File.Exists($"{dir}/{fileName}.asset")) continue;

                //Не трогаем файлы в папке с ресурсами, плагинами...
                if (fi.FullName.Contains("Plugins", StringComparison.InvariantCultureIgnoreCase)
                    || fi.FullName.Contains("Resources", StringComparison.InvariantCultureIgnoreCase)
                    || fi.FullName.Contains("TextMesh Pro", StringComparison.InvariantCultureIgnoreCase)) {
                    Debug.Log($"===>>>FILES: Plugins&Resources: {fi.FullName}");
                    continue;
                }

                var newFileName = GetNextWord();
                // Debug.Log($"===>>> File name: {fileName}{ext} to {newFileName}{ext}");
                fi.MoveTo($"{dir}/{newFileName}{ext}");

                var metaFi = new FileInfo($"{dir}/{fileName}{ext}.meta");
                // Debug.Log($"===>>> MetaFile name: {metaFi.Name} to {newFileName}{ext}.meta");
                metaFi.MoveTo($"{dir}/{newFileName}{ext}.meta");
            }

            var subDirs = di.GetDirectories();
            foreach (var subDir in subDirs) {
                RenameFiles(subDir.FullName);
            }
        }

        private static void RenameDirs() {
            for (var i = DirsTree.Count - 1; i >= 1; i--) {
                //Не трогаем папку с ресурсами, плагинами...
                if (DirsTree[i].FullName.Contains("Plugins", StringComparison.InvariantCultureIgnoreCase)
                    || DirsTree[i].FullName.Contains("Resources", StringComparison.InvariantCultureIgnoreCase)
                    || DirsTree[i].FullName.Contains("Editor", StringComparison.InvariantCultureIgnoreCase)
                    || DirsTree[i].FullName.Contains("TextMesh Pro", StringComparison.InvariantCultureIgnoreCase)) {
                    continue;
                }

                var parentDir = DirsTree[i].Parent?.FullName;
                var oldName = DirsTree[i].Name;
                var newName = GetNextWord();
                if (File.Exists($"{parentDir}/{oldName}.meta")) {
                    // Debug.Log($"===>>>> {parentDir}/{oldName}.meta -->> {parentDir}/{newName}.meta");
                    File.Move($"{parentDir}/{oldName}.meta", $"{parentDir}/{newName}.meta");
                }

                // Debug.Log($"===>>>From: {DirsTree[i].FullName} To: {parentDir}/{newName}");
                try {
                    DirsTree[i].MoveTo($"{parentDir}/{newName}");
                } catch (Exception e) {
                    Debug.Log($"===>>>RenameDirs error: {e}");
                }
            }
        }

        private static void MoveToHeap(string heapName) {
            Directory.CreateDirectory(heapName);
            Directory.CreateDirectory(heapName);
            Directory.CreateDirectory(heapName);
            for (var i = DirsTree.Count - 1; i >= 0; i--) {
                if (DirsTree[i].FullName.Contains("Plugins", StringComparison.InvariantCultureIgnoreCase)) {
                    if (!Directory.Exists($"{heapName}/Plugins"))
                        Directory.CreateDirectory($"{heapName}/Plugins");

                    try {
                        if (DirsTree[i].Name.Equals("Plugins", StringComparison.InvariantCultureIgnoreCase)) {
                            DirsTree[i].MoveTo($"{heapName}/Plugins");
                        } else {
                            DirsTree[i].MoveTo($"{heapName}/Plugins/{DirsTree[i].Name}");
                        }
                    } catch (Exception e) {
                        Debug.Log($"===>>> Moving resources to heap error: {e}. File: {DirsTree[i].Name}");
                    }

                    var parentDir = DirsTree[i].Parent?.FullName;
                    if (File.Exists($"{parentDir}/{DirsTree[i].Name}.meta")) {
                        try {
                            File.Move($"{parentDir}/{DirsTree[i].Name}.meta", $"{heapName}/{DirsTree[i].Name}.meta");
                        } catch (Exception e) {
                            Debug.Log($"===>>> Moving meta to heap error: {e}. File: {DirsTree[i].Name}");
                        }
                    }

                    continue;
                }

                if (DirsTree[i].FullName.Contains("TextMesh Pro", StringComparison.InvariantCultureIgnoreCase)) {
                    if (!Directory.Exists($"{heapName}/TextMesh Pro"))
                        Directory.CreateDirectory($"{heapName}/TextMesh Pro");

                    try {
                        if (DirsTree[i].Name.Equals("TextMesh Pro", StringComparison.InvariantCultureIgnoreCase)) {
                            DirsTree[i].MoveTo($"{heapName}/TextMesh Pro");
                        } else {
                            DirsTree[i].MoveTo($"{heapName}/TextMesh Pro/{DirsTree[i].Name}");
                        }
                    } catch (Exception e) {
                        Debug.Log($"===>>> Moving resources to heap error: {e}. File: {DirsTree[i].Name}");
                    }

                    var parentDir = DirsTree[i].Parent?.FullName;
                    if (File.Exists($"{parentDir}/{DirsTree[i].Name}.meta")) {
                        try {
                            File.Move($"{parentDir}/{DirsTree[i].Name}.meta", $"{heapName}/{DirsTree[i].Name}.meta");
                        } catch (Exception e) {
                            Debug.Log($"===>>> Moving meta to heap error: {e}. File: {DirsTree[i].Name}");
                        }
                    }

                    continue;
                }

                if (DirsTree[i].FullName.Contains("Resources", StringComparison.InvariantCultureIgnoreCase)) {
                    if (!Directory.Exists($"{heapName}/Resources"))
                        Directory.CreateDirectory($"{heapName}/Resources");

                    try {
                        if (DirsTree[i].Name.Equals("Resources", StringComparison.InvariantCultureIgnoreCase)) {
                            DirsTree[i].MoveTo($"{heapName}/Resources");
                        } else {
                            DirsTree[i].MoveTo($"{heapName}/Resources/{DirsTree[i].Name}");
                        }
                    } catch (Exception e) {
                        Debug.Log($"===>>> Moving resources to heap error: {e}. File: {DirsTree[i].Name}");
                    }

                    var parentDir = DirsTree[i].Parent?.FullName;
                    if (File.Exists($"{parentDir}/{DirsTree[i].Name}.meta")) {
                        try {
                            File.Move($"{parentDir}/{DirsTree[i].Name}.meta", $"{heapName}/{DirsTree[i].Name}.meta");
                        } catch (Exception e) {
                            Debug.Log($"===>>> Moving meta to heap error: {e}. File: {DirsTree[i].Name}");
                        }
                    }

                    continue;
                }

                var filesInfo = DirsTree[i].GetFiles("*.*");
                foreach (var fi in filesInfo) {
                    var fileName = Path.GetFileNameWithoutExtension(fi.FullName);
                    var ext = Path.GetExtension(fi.FullName);
                    Debug.Log($"===>>> Try move to heap: {fileName}{ext}");
                    try {
                        fi.MoveTo($"{heapName}/{fileName}{ext}");
                    } catch (Exception ex) {
                        Debug.Log($"===>>> Moving to heap error: {ex}. File: {fileName}{ext}");
                    }
                }
            }

            if (Directory.GetFiles(_sourceDir).Length < 1) {
                Directory.Delete(_sourceDir, true);

                if (File.Exists($"{_sourceDir}.meta")) {
                    File.Delete($"{_sourceDir}.meta");
                }
            }
        }

        private static void SimpleRenameRootDir(string heapName) {
            var di = new DirectoryInfo(_sourceDir);
            try {
                di.MoveTo(heapName);
            } catch (Exception e) {
                Debug.Log($"===>>> Rename root error: {e}");
            }
        }

        private static void DeleteEditorPlugins() {
            var paths = new List<string> {
                "Assets/AlmostEngine",
                "Assets/HotReload",
                "Assets/Plugins/Editor/JetBrains.RiderFlow",
                "Assets/RiderFlow.UserData",
                "Packages/com.singularitygroup.hotreload"
            };
            foreach (var path in paths) {
                if (Directory.Exists(path)) {
                    Directory.Delete(path, true);
                }

                if (File.Exists($"{path}.meta")) {
                    File.Delete($"{path}.meta");
                }
            }

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }
    }

}
