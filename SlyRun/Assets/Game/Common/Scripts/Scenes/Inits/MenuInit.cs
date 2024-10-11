using UnityEngine;

using Zenject;

using Game.Common.Scripts.Scenes.Huds;

namespace Game.Common.Scripts.Scenes.Inits
{
    public class MenuInit : BaseInit
    {
        //======INJECTS

        [Inject] private MenuHud uiHud;

        //======MONOBEHAVIOUR

        public void Awake()
        {
            uiHud.Init();
        }
    }
}