using System.Linq;

using UnityEngine;

using Zenject;

using Game.Common.Scripts.Global.Managers;

namespace Game.Common.Scripts.Global.Systems
{
    public class MusicSystem : MonoBehaviour
    {
        //================================
        //===SINGLETON
        //================================

        public static MusicSystem Instance;

        //================================
        //===INSPECTOR
        //================================

        [SerializeField] public AudioSource audioSource;
        [SerializeField] public AudioClip[] audioClips;

        //================================
        //===INJECTS
        //================================

        [Inject] private PlayerManager playerManager;

        //================================
        //===FIELDS
        //================================

        private string audioMusicOst;
        private bool audioMusicActive = true;

        //================================
        //===MONOBEHAVIOUR
        //================================

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        //================================
        //===AUDIO
        //================================

        public void Initialize(string ost)
        {
            audioMusicOst = ost;
        }

        public void PlayMusic(string name)
        {
            if (!audioMusicActive) return;

            var clip = audioClips.FirstOrDefault(x => x.name == name);
            if (clip)
            {
                audioSource.clip = clip;
                audioSource.loop = true;
                audioSource.Play();
            }
            else
            {
                Debug.LogError($"AudioSystem.PlayMusic(): Clip with name [{name}] doesn't exist.");
            }
        }

        public void SwitchMusic(bool state)
        {
            audioMusicActive = state;
            audioSource.enabled = state;
            
            if(audioMusicActive)
            {
                PlayMusic(audioMusicOst);
            }
        }
    }
}
