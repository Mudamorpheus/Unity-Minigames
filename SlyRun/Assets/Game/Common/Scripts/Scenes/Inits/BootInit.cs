using UnityEngine;
using UnityEngine.SceneManagement;

using Zenject;

using Game.Common.Scripts.Scenes.Systems;
using Game.Common.Scripts.Global.Systems;

namespace Game.Common.Scripts.Scenes.Inits
{
    public class BootInit : BaseInit
    {
        //======INJECTS

        [Inject] private AccountSystem accountSystem;

        //======MONOBEHAVIOUR

        public void Start()
        {
            //Player
            accountSystem.Init();
            accountSystem.Load();
            accountSystem.Save();

            //Music
            MusicSystem.Instance.Init("Bg");
            if(accountSystem.Data.Audio)
            {
                MusicSystem.Instance.PlayMusic("Bg");
            }

            //Audio
            SoundSystem.Instance.SwitchSound(accountSystem.Data.Audio);
            MusicSystem.Instance.SwitchMusic(accountSystem.Data.Audio);

            //Scene
            SceneManager.LoadScene("Menu");
        }
    }
}