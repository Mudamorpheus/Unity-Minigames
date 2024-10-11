using UnityEngine;

namespace Game.Common.Scripts.Scenes.Systems
{
    public class AccountSystem : MonoBehaviour
    {
        //======SUBCLASS

        public class AccountData
        {
            public string Name;
            public int    Score;
            public bool   Audio;

            public AccountData(string name, int score, bool audio)
            {
                Name = name;
                Score = score;
                Audio = audio;
            }
        }

        //======FIELDS

        private AccountData data;

        //======PROPERTIES

        public AccountData Data { get { return data; } }

        //======PLAYER

        public void Init()
        {
            data = new AccountData("user", 0, true);
        }

        public void Save()
        {
            string key = Data.Name;
            string json = JsonUtility.ToJson(Data);
            PlayerPrefs.SetString(key, json);
        }

        public void Load()
        {
            string key = Data.Name;
            string json = PlayerPrefs.GetString(key);

            if(json != "")
            {
                data = JsonUtility.FromJson<AccountData>(json);
            }            
        }       
    }
}