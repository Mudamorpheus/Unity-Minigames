using UnityEngine;
using UnityEngine.SceneManagement;

using Zenject;

using Game.Common.Scripts.Data;
using Game.Common.Scripts.Systems;

namespace Game.Common.Scripts.DI.Cores.Initializers
{
    public class BootInitializer : BaseInitializer
    {
        #region Vars

        [Inject] private PlayerData playerData;

        #endregion

        //===========================================

        #region BaseInitializer

        protected override void Start()
        {
            base.Start();

            //Player
            playerData.Init();
            playerData.Load();
            playerData.Save();

            //Bg
            MusicSystem.Instance.Init("bg");

            //Audio
			MusicSystem.Instance.SwitchMusic(playerData.Music);
            SoundSystem.Instance.SwitchSound(playerData.Sound);            

            //Scene
            SceneManager.LoadScene(StaticData.SceneMenuName);
        }

        protected override void Update()
        {
            base.Update();
        }

        #endregion
    }
}