using System.Linq;

using UnityEngine;

using Zenject;

using Game.Common.Scripts.Global.Managers;

namespace Game.Common.Scripts.Global.Systems 
{	
	public class SoundSystem : MonoBehaviour
	{
        //================================
        //===SINGLETON
        //================================

        public static SoundSystem Instance;        

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

        private bool audioSoundActive = true;              

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

        public void PlaySound(string name)
        {
            if (!audioSoundActive) return;

            var clip = audioClips.FirstOrDefault(x => x.name == name);
            if (clip)
            {
                audioSource.PlayOneShot(clip);
            }
            else
            {
                Debug.LogError($"AudioSystem.PlaySound(): Clip with name [{name}] doesn't exist.");
            }
        }

        public void SwitchSound(bool state)
        {
            audioSoundActive = state;
            audioSource.enabled = state;
        }
    }
}
