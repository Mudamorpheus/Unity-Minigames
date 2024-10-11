using System.Collections.Generic;
using UnityEngine;
using static Game.Common.Scripts.Data.PlayerData.Fields;

namespace Game.Common.Scripts.Data
{
    public class PlayerData
    {
        #region Subclasses

        [System.Serializable]
        public class Fields
        {
            [System.Serializable]
            public enum LevelState
            {
                Default   = 0,
                Completed = 1,
                Claimed   = 2,
            }

            [System.Serializable]
            public class LevelData
            {
                public int        Level;
                public LevelState State;
                public LevelData(int level, LevelState state)
                {
                    Level = level;
                    State = state;
                }
            }

            public int                    Balance;
            public int                    Spent;
            //
            public int                    Level;
            public int                    Tier;
            public bool                   Music;
            public bool                   Sound;
            //
            public int                    Bet;
            public int                    Lines;
            //            
            public List<LevelData>        LevelsData;
        }

        #endregion

        //===========================================

        #region Vars

        private Fields dataFields;
        private string dataSipher;

        #endregion

        //===========================================

        #region Properties

        public int             Balance     { get { return dataFields.Balance; } set { dataFields.Balance = value; Save(); } }
        public int             Spent       { get { return dataFields.Spent;   } set { dataFields.Spent   = value; Save(); } }
        //
        public int             Level       { get { return dataFields.Level;   } set { dataFields.Level   = value; Save(); } }
        public int             Tier        { get { return dataFields.Tier;    } set { dataFields.Tier    = value; Save(); } }
        //
        public bool            Music       { get { return dataFields.Music;   } set { dataFields.Music   = value; Save(); } }
        public bool            Sound       { get { return dataFields.Sound;   } set { dataFields.Sound   = value; Save(); } }
        //
        public int             Bet         { get { return dataFields.Bet;     } set { dataFields.Bet     = value; Save(); } }
        public int             Lines       { get { return dataFields.Lines;   } set { dataFields.Lines   = value; Save(); } }
        //
        public List<LevelData> LevelsData  { get { return dataFields.LevelsData; } }

        #endregion

        //===========================================

        #region PlayerData

        public void Init()
        {
            dataSipher = "]gLHs5u72[(hl[)'s-leE1Splx]EZ.{wl|_mEXdJSO!$_6VAM>";

            dataFields             = new Fields();
            //
            dataFields.Balance     = 1000;
            dataFields.Level       = 0;
            dataFields.Music       = true;
            dataFields.Sound       = true;
            //
            dataFields.Bet         = 50;
            dataFields.Lines       = 1;
            //
            dataFields.LevelsData  = new List<LevelData>();
        }

        public void Save()
        {
            string key = dataSipher;
            string json = JsonUtility.ToJson(dataFields);
            PlayerPrefs.SetString(key, json);
        }

        public void Load()
        {
            string key = dataSipher;
            string json = PlayerPrefs.GetString(key);
            if (json != "")
            {
                dataFields = JsonUtility.FromJson<Fields>(json);
            }
        }

        #endregion

        //===========================================

        #region Actions

        public void AddBalance(int balance)
        {
            Balance += balance;
        }

        public void Complete(int level)
        {
            var prev = LevelsData.Find(x => x.Level == level);
            if (prev != null)
            {
                LevelsData.Remove(prev);
            }
            var data = new LevelData(level, LevelState.Completed);
            LevelsData.Add(data);
            Save();
        }

        public void Claim(int level)
        {
            var prev = LevelsData.Find(x => x.Level == level);
            if(prev != null)
            {
                LevelsData.Remove(prev);
            }
            var data = new LevelData(level, LevelState.Claimed);
            LevelsData.Add(data);
            Save();
        }

        #endregion
    }
}