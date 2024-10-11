using System;
using System.Collections.Generic;

using UnityEngine;

namespace Game.Common.Scripts.Global.Managers 
{	
	public class PlayerManager
	{
        //================================
        //===STRUCTS
        //================================

        //TODO - Swap data to separate structs

        [Serializable]
        public class PlayerChallenge
        {
            public enum State
            {
                Todo,
                Completed,
                Claim
            }

            public string challenge_id;
            public State challenge_state;
        }
        
        [Serializable]
        public class PlayerSkin
        {
            public enum State
            {
                Sold,
                Unselected,
                Selected,
            }

            public string skin_id;
            public State skin_state;
        }        

        [Serializable]
        public class PlayerData
        {
            //Data
            public int  player_coins;
            public int  player_best_score;
            public int  player_incidents;
            public long player_date_online;

            //Settings
            public bool player_sound;
            public bool player_music;

            //Shop
            public List<PlayerSkin> player_skins;

            //Challenges
            public List<PlayerChallenge> player_challenges;

            //Constructors
            public PlayerData(int coins, int score, int incidents, long online, bool sound, bool music, List<PlayerSkin> skins, List<PlayerChallenge> challenges)
            {
                player_coins                = coins;
                player_best_score           = score;
                player_incidents            = incidents;
                player_date_online          = online;
                player_sound                = sound;
                player_music                = music;
                player_skins                = skins;
                player_challenges           = challenges;
            }
        }

        //================================
        //===FIELDS
        //================================

        private string playerName;
        private PlayerData playerData;

        public PlayerManager()
        {
            playerData = new PlayerData(0, 0, 0, 0, true, true, new List<PlayerSkin>(), new List<PlayerChallenge>());
        }

        //================================
        //===PROPERTIES
        //================================

        public PlayerData            Data       { get { return playerData; } }
        public bool                  Music      { get { return playerData.player_music; } } 
        public bool                  Sound      { get { return playerData.player_sound; } }
        public List<PlayerSkin>      Skins      { get { return playerData.player_skins; } }
        public List<PlayerChallenge> Challenges { get { return playerData.player_challenges; } }

        //================================
        //===PLAYER
        //================================

        public void Initialize(string name, ShopManager.ShopData shop, BoardManager.BoardData board)
        {
            //Player
            playerName = name;

            //Date
            long unixNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            playerData.player_date_online = unixNow;

            //Shop
            foreach (var skin in shop.shop_skins_data)
            {
                PlayerSkin data = new PlayerSkin();
                data.skin_id = skin.skin_id;
                data.skin_state = skin.skin_own ? PlayerSkin.State.Selected : PlayerSkin.State.Sold;
                playerData.player_skins.Add(data);
            }

            //Challenge
            foreach (var challenge in board.board_challenges_data)
            {
                PlayerChallenge data = new PlayerChallenge();
                data.challenge_id = challenge.challenge_id;
                data.challenge_state = PlayerChallenge.State.Todo;
                playerData.player_challenges.Add(data);
            }
        }

        public void SaveProfile()
        {
            string key = playerName;
            string json = JsonUtility.ToJson(playerData);

            PlayerPrefs.SetString(key, json);
        }

        public bool LoadProfile()
        {
            string key = playerName;
            string json = PlayerPrefs.GetString(key);

            //Json
            if (json != "")
            {
                playerData = JsonUtility.FromJson<PlayerData>(json);

                CheckDailyEvents();

                return true;
            }

            return false;
        }

        public void CheckDailyEvents()
        {
            //Time
            long unixNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            int savedDays = (int)(playerData.player_date_online/86400);
            int nowDays = (int)(unixNow/86400);
            playerData.player_date_online = unixNow;

            //Next day
            if(nowDays > savedDays)
            {
                RefreshChallenges();
            }

            //SaveProfile();
        }

        //================================
        //===PARAMS
        //================================

        public void SwitchMusic(bool state)
        {
            playerData.player_music = state;
        }

        public void SwitchSound(bool state)
        {
            playerData.player_sound = state;
        }

        public void AddCoins(int coins)
        {
            playerData.player_coins += coins;
        }
        public void SetBestScore(int score)
        {
            playerData.player_best_score = score;
        }
        public void AddIncidents(int incidents)
        {
            playerData.player_incidents += incidents;
        }

        //================================
        //===SHOP
        //================================

        //SET

        public void BuySkin(string id)
        {
            PlayerSkin data = new PlayerSkin();
            data.skin_id = id;
            data.skin_state = PlayerSkin.State.Unselected;
            playerData.player_skins.Add(data);

            SaveProfile();
        }

        public void SelectSkin(string id)
        {            
            for(int i = 0; i < playerData.player_skins.Count; i++)
            {
                var skin = playerData.player_skins[i];

                if (skin.skin_id == id)
                {
                    skin.skin_state = PlayerSkin.State.Selected;
                }
                else if (skin.skin_state == PlayerSkin.State.Selected)
                {
                    skin.skin_state = PlayerSkin.State.Unselected;
                }               
            }

            SaveProfile();
        }

        //GET

        public PlayerSkin GetSkin(string id)
        {
            return playerData.player_skins.Find(x => x.skin_id == id);
        }

        public PlayerSkin GetSelectedSkin()
        {
            return playerData.player_skins.Find(x => x.skin_state == PlayerSkin.State.Selected);
        }

        //================================
        //===BOARD
        //================================

        //SET

        public void CompleteChallenge(string id)
        {
            var challenge = GetChallenge(id);
            if (challenge.challenge_state != PlayerChallenge.State.Completed)
            {
                challenge.challenge_state = PlayerChallenge.State.Completed;
                SaveProfile();
            }            
        }
        public void ClaimChallenge(string id)
        {
            var challenge = GetChallenge(id);
            if(challenge.challenge_state == PlayerChallenge.State.Todo)
            {
                challenge.challenge_state = PlayerChallenge.State.Claim;
                SaveProfile();
            }
        }

        public void RefreshChallenges()
        {
            for(int i = 0; i < playerData.player_challenges.Count; i++)
            {
                var challenge = playerData.player_challenges[i];
                challenge.challenge_state = PlayerChallenge.State.Todo;
            }            
        }

        //GET

        public PlayerChallenge GetChallenge(string id)
        {
            return playerData.player_challenges.Find(x => x.challenge_id == id);
        }
    }
}
