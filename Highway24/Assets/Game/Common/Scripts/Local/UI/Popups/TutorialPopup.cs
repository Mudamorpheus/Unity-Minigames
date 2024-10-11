using UnityEngine;

using Zenject;

using Game.Common.Scripts.Global.Managers;
using Game.Common.Scripts.Local.UI.Huds;
using Game.Common.Scripts.Local.UI.Misc;

namespace Game.Common.Scripts.Local.UI.Popups
{
    public class TutorialPopup : Popup
    {
        //================================
        //===INSPECTOR
        //================================

        [SerializeField] private GameObject[] uiTutorialStages;

        //================================
        //===FIELDS
        //================================

        private Hud uiHud;
        private PlayerManager playerManager;
        private int uiTutorialStage = 0;

        //===========================
        //===FACTORY
        //===========================

        public class Factory : PlaceholderFactory<TutorialPopup>
        {
            private static TutorialPopup factoryPrefab;

            public static void BindPrefab(TutorialPopup factoryPrefab)
            {
                Factory.factoryPrefab = factoryPrefab;
            }

            public TutorialPopup Create(Hud uiHud, PlayerManager playerManager)
            {
                var popup = Instantiate(factoryPrefab, uiHud.Canvas.transform);
                popup.Initialize(uiHud, playerManager);
                popup.Open();
                return popup;
            }
        }

        //================================
        //===UI
        //================================

        public void Initialize(Hud uiHud, PlayerManager playerManager)
        {
            this.uiHud = uiHud;
            this.playerManager = playerManager;
        }

        public void Switch(int stage)
        {
            for(int i = 0; i < uiTutorialStages.Length; i++)
            {
                var view = uiTutorialStages[i];
                view.gameObject.SetActive(i == stage);
            }
        }

        public void Back()
        {
            //Zero
            if(uiTutorialStage == 0)
            {
                Close();
            }

            //Back
            uiTutorialStage--;
            Switch(uiTutorialStage);
        }

        public void Next()
        {
            //Max
            if (uiTutorialStage == uiTutorialStages.Length-1)
            {
                Close();
            }

            //Back
            uiTutorialStage++;
            Switch(uiTutorialStage);
        }

        public override void Open()
        {
            base.Open();

            uiHud.SwitchButtons(false);
        }
        public override void Close()
        {
            base.Close();

            uiHud.SwitchButtons(true);
        }
    }
}
