using UnityEngine;

using Zenject;

using Game.Common.Scripts.Scenes.Huds;
using Game.Common.Scripts.Scenes.Engines;
using TMPro;

namespace Game.Common.Scripts.Scenes.Popups
{
    public class DefeatPopup : BasePopup
    {
        //======INSPECTOR

        [SerializeField] private TMP_Text uiScoreCurrent;
        [SerializeField] private TMP_Text uiScoreBest;

        //======FACTORY

        public class Factory : PlaceholderFactory<DefeatPopup>
        {
            private static DefeatPopup factoryPrefab;

            public static void BindPrefab(DefeatPopup factoryPrefab)
            {
                Factory.factoryPrefab = factoryPrefab;
            }

            public DefeatPopup Create(BaseHud hud, BaseEngine engine)
            {
                var popup = Instantiate(factoryPrefab, hud.Canvas.transform);
                popup.sceneHud = hud;
                popup.sceneEngine = engine;

                popup.Init();
                popup.Open();                

                return popup;
            }
        }

        //======DEFEAT

        public void Init()
        {
            //Current
            if(sceneEngine as GameEngine)
            {
                uiScoreCurrent.text = ((GameEngine)sceneEngine).Score.ToString();
            }

            //Best
            uiScoreBest.text = sceneHud.Account.Data.Score.ToString();
        }
    }
}