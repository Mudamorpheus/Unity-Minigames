using UnityEngine;

using Zenject;

using Game.Common.Scripts.Scenes.Huds;

namespace Game.Common.Scripts.Scenes.Popups
{
    public class TutorialPopup : BasePopup
    {        
        //======FACTORY

        public class Factory : PlaceholderFactory<TutorialPopup>
        {
            private static TutorialPopup factoryPrefab;

            public static void BindPrefab(TutorialPopup factoryPrefab)
            {
                Factory.factoryPrefab = factoryPrefab;
            }

            public TutorialPopup Create(BaseHud hud)
            {
                var popup = Instantiate(factoryPrefab, hud.Canvas.transform);
                popup.sceneHud = hud;

                popup.Open();

                return popup;
            }
        }

        //======INSPECTOR

        [SerializeField] private GameObject[] popupTabs;

        //======FIELDS

        private int popupTabIndex = 0;

        //======MONOBEHAVIOUR

        public void Awake()
        {
            if(popupTabs.Length > 0)
            {
                SwitchTab(0);
            }            
        }

        //======POPUP

        public void ShowTab(int index)
        {            
            popupTabs[index].gameObject.SetActive(true);
        }

        public void HideTabs()
        {
            foreach(var tab in popupTabs)
            {
                tab.gameObject.SetActive(false);
            }
        }

        public void SwitchTab(int index)
        {
            HideTabs();
            ShowTab(index);
        }

        public void ShowNextTab()
        {
            popupTabIndex++;
            if(popupTabIndex >= popupTabs.Length)
            {
                return;
            }

            SwitchTab(popupTabIndex);
        }

        public void ShowPrevTab()
        {
            popupTabIndex--;
            if (popupTabIndex < 0)
            {
                return;
            }

            SwitchTab(popupTabIndex);
        }
    }
}
