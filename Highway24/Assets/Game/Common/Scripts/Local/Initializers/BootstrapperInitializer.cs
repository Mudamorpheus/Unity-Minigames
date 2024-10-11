using UnityEngine;
using UnityEngine.SceneManagement;

using Zenject;

using Game.Common.Scripts.Global.Managers;
using Game.Common.Scripts.Global.Systems;

namespace Game.Common.Scripts.Local.Initializers 
{
	
	public class BootstrapperInitializer : MonoBehaviour
	{
        //================================
        //===INSPECTOR
        //================================

        [SerializeField] private string                 bootstrapperScene;
        [SerializeField] private ShopManager.ShopData   shopData;
        [SerializeField] private BoardManager.BoardData boardData;

        //================================
        //===INJECTS
        //================================

        [Inject] private PlayerManager playerManager;
        [Inject] private ShopManager   shopManager;
        [Inject] private BoardManager  boardManager;

        //================================
        //===MONOBEHAVIOUR
        //================================

        private void Start()
        {
            //Shop
            shopManager.Initialize(shopData);
            boardManager.Initialize(boardData);

            //Player
            string user = SystemInfo.deviceName + " Highway24";
            playerManager.Initialize(user, shopManager.Data, boardManager.Data);
            playerManager.LoadProfile();
            playerManager.SaveProfile();

            //Music
            MusicSystem.Instance.Initialize("bg");
            MusicSystem.Instance.SwitchMusic(playerManager.Music);
            SoundSystem.Instance.SwitchSound(playerManager.Sound);

            //Scene
            SceneManager.LoadScene(bootstrapperScene);
        }
    }
	
}
