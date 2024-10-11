using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum componentType {
    Sound,
    Music,
    SoundButton,
    MusicButton,
    MusicSlider,
    SoundSlider
}

public class DemoAudioController : MonoBehaviour {
    private DemoAudioManager manager;
    private AudioSource[] srcs;
    private bool isMusic;
    public componentType type;

    private void Start() {
        manager = DemoAudioManager.instance;
        CheckControllerState();
    }

    private void CheckControllerState() {
        switch (type) {
            case componentType.SoundButton: {
                manager.soundButton = GetComponent<Button>();
                manager.UpdateSoundButton();
                manager.CheckAllSoundsState();
                break;
            }
            case componentType.MusicButton: {
                manager.musicButton = GetComponent<Button>();
                manager.UpdateMusicButton();
                manager.CheckAllSoundsState();
                break;
            }
            case componentType.MusicSlider:
            {
                manager.musicSlider = GetComponent<Slider>();
                manager.musicSlider.value = DemoData.Instance.DemoMusicVolume;
                manager.UpdateMusicSlider();
                break;
            }
            case componentType.SoundSlider:
            {
                manager.soundSlider = GetComponent<Slider>();
                manager.soundSlider.value = DemoData.Instance.DemoSoundVolume;
                manager.UpdateSoundSlider();
                break;
            }
            
            case componentType.Sound: {
                srcs = GetComponents<AudioSource>();
                foreach (var src in srcs) {
                    src.mute = !DemoData.Instance.DemoSound;
                    src.volume = DemoData.Instance.DemoSoundVolume;
                }

                manager.ChangeSoundStateCallback += CheckSoundState;
                break;
            }
            case componentType.Music: {
                srcs = GetComponents<AudioSource>();
                foreach (var src in srcs) {
                    src.mute = !DemoData.Instance.DemoMusic;
                    src.volume = DemoData.Instance.DemoMusicVolume;
                }

                manager.ChangeSoundStateCallback += CheckSoundState;
                break;
            }
        }
    }

    private void OnDestroy() {
        if (type == componentType.MusicButton || type == componentType.SoundButton) return;
       if (manager == null) manager = DemoAudioManager.instance;
       manager.ChangeSoundStateCallback -= CheckSoundState;
    }

    private void CheckSoundState() {
        if (type == componentType.Music) {
            foreach (var src in srcs) {
                src.mute = !DemoData.Instance.DemoMusic;
                src.volume = DemoData.Instance.DemoMusicVolume;
            }
        }


        if (type == componentType.Sound) {
            foreach (var src in srcs) {
                src.mute = !DemoData.Instance.DemoSound;
                src.volume = DemoData.Instance.DemoSoundVolume;
            }
        }

    }
}