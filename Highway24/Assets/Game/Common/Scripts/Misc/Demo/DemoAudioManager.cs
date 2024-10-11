using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class DemoAudioManager : MonoBehaviour {
    public static DemoAudioManager instance;
    public Sprite onSound, onMusic, offSound, offMusic;
    [HideInInspector] public Button soundButton;
    [HideInInspector] public Button musicButton;
    [HideInInspector] public Slider musicSlider;
    [HideInInspector] public Slider soundSlider;
    private Image soundImage, musicImage;
    public event Action ChangeSoundStateCallback;
    private DemoData data;
    public AudioClip[] clips;
    public AudioSource SFX;


    public void PlayClipByIndex(int clipIndex) {
        if (!DemoData.Instance.DemoSound) return;
        if (clipIndex > clips.Length - 1 || clips[clipIndex] == null) {
            Debug.LogError(
                $"DemoAudioManager.PlayClipByIndex(): index [{clipIndex}] more than clips array lenght or clip with index [{clipIndex}] is null");
            return;
        }

        SFX.PlayOneShot(clips[clipIndex]);
    }

    public void PlayClipByName(string clipName) {
        if (!DemoData.Instance.DemoSound) return;
        var clip = clips.FirstOrDefault(x => x.name == clipName);
        {
            if (clip) {
                SFX.PlayOneShot(clip);
            } else {
                Debug.LogError($"DemoAudioManager.PlayClipByIndex(): Clip with name [{clipName}] doesn't exist");
            }
        }
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }

        data = DemoData.Instance;
    }

    public void UpdateSoundButton() {
        soundImage = soundButton.GetComponent<Image>();
        soundButton.onClick.AddListener(ChangeSoundState);
    }

    public void UpdateMusicButton() {
        musicImage = musicButton.GetComponent<Image>();
        musicButton.onClick.AddListener(ChangeMusicState);
    }

    public void UpdateMusicSlider() {
        musicSlider.onValueChanged.AddListener(delegate { CheckAllSoundsState(); });
    }

    public void UpdateSoundSlider() {
        soundSlider.onValueChanged.AddListener(delegate { CheckAllSoundsState(); });
    }


    private void ChangeSoundState() {
        data.DemoSound = !data.DemoSound;
        CheckAllSoundsState();
    }

    private void ChangeMusicState() {
        data.DemoMusic = !data.DemoMusic;
        CheckAllSoundsState();
    }

    public void CheckAllSoundsState() {
        if (musicSlider != null) {
            DemoData.Instance.DemoMusicVolume = musicSlider.value;
        }

        if (soundSlider != null) {
            DemoData.Instance.DemoSoundVolume = soundSlider.value;
        }
        if (soundImage != null) soundImage.sprite = data.DemoSound ? onSound : offSound;
        if (musicImage != null) musicImage.sprite = data.DemoMusic ? onMusic : offMusic;
        ChangeSoundStateCallback?.Invoke();
    }
}