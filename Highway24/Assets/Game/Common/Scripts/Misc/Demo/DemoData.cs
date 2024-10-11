using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoData {
    private static DemoData _instance;

    public static DemoData Instance {
        get {
            _instance ??= new DemoData();
            return _instance;
        }
    }

    public int TopScore {
        get => PlayerPrefs.GetInt("demoTopScore", 0); 
        set => PlayerPrefs.SetInt("demoTopScore", value);
    }

    public int SubIndex {
        get => PlayerPrefs.GetInt($"Index", 0);
        set => PlayerPrefs.SetInt($"Index", value);
    }

    public bool DemoSound {
        get => bool.Parse(PlayerPrefs.GetString("demoSound", "true"));
        set => PlayerPrefs.SetString("demoSound", value.ToString());
    }

    public bool DemoMusic {
        get => bool.Parse(PlayerPrefs.GetString("demoMusic", "true"));
        set => PlayerPrefs.SetString("demoMusic", value.ToString());
    }
    public float DemoMusicVolume
    {
        get => PlayerPrefs.GetFloat("demoMusicVolume", 1);
        set => PlayerPrefs.SetFloat("demoMusicVolume", value);
    }
    public float DemoSoundVolume
    {
        get => PlayerPrefs.GetFloat("demoSoundVolume", 1);
        set => PlayerPrefs.SetFloat("demoSoundVolume", value);
    }
}