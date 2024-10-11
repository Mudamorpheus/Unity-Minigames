using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Common.Editor.Scripts {
    public class TexturePreprocessor : AssetPostprocessor {
        private static TexturePreprocessorSettings _texturesSettings = null;

        void OnPreprocessTexture() {
            if(_texturesSettings == null) 
                _texturesSettings = Resources.Load<TexturePreprocessorSettings>("TexturePreprocessor/Settings");

            var importer = assetImporter as TextureImporter;

            if ( importer == null ) {
                return;
            }

            if ( importer.assetPath.Contains("Assets/Game") ) {
                TextureSettings textureSettings = _texturesSettings?.texturesSettings.FirstOrDefault
                (e => importer.assetPath == e.textureAssetPath);

                var iosSettings = importer.GetPlatformTextureSettings("IPhone");
                importer.SetPlatformTextureSettings(DefineSettings(iosSettings, textureSettings));

                var androidSettings = importer.GetPlatformTextureSettings("Android");
                importer.SetPlatformTextureSettings(DefineSettings(androidSettings, textureSettings));
            }
        }

        static TextureImporterPlatformSettings DefineSettings(TextureImporterPlatformSettings settings, TextureSettings texturesSettings) {
            settings.overridden = true;

            if(texturesSettings == null || texturesSettings.maxTextureSize == MaxTextureSize.DEFAULT) settings.maxTextureSize = 1024;
            else settings.maxTextureSize = texturesSettings.maxTextureIntSize;

            settings.format = TextureImporterFormat.ASTC_6x6;
            return settings;
        }
    }
}