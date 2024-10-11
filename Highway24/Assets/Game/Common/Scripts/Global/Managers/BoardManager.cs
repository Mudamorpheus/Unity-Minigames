using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using static Game.Common.Scripts.Global.Managers.ShopManager;

namespace Game.Common.Scripts.Global.Managers 
{
	public class BoardManager
	{
        //================================
        //===STRUCTS
        //================================

        [Serializable]
        public struct BoardChallenge
        {
            //Type
            public enum Requirement
            {
                Levels,
                Coins,
                Highscore
            }

            //Data
            public string      challenge_id;
            public string      challenge_desc;
            public int         challenge_reward;
            public Requirement challenge_requirement;
            public int         challenge_requirement_value; 

            //Constructors
            public BoardChallenge(string id, string desc, int reward, Requirement req, int reqvalue)
            {
                challenge_id                = id;
                challenge_desc              = desc;
                challenge_reward            = reward;
                challenge_requirement       = req;
                challenge_requirement_value = reqvalue;
            }
        }

        [Serializable]
        public struct BoardData
        {
            //Data
            public List<BoardChallenge> board_challenges_data;
            public GameObject board_challenges_prefab;

            //Constructors
            public BoardData(List<BoardChallenge> challenges, GameObject prefab)
            {
                board_challenges_data = challenges;
                board_challenges_prefab = prefab;
            }
        }

        //================================
        //===FIELDS
        //================================

        private BoardData boardData;

        //================================
        //===PROPERTIES
        //================================

        public BoardData            Data       { get { return boardData; } }
        public List<BoardChallenge> Challenges { get { return boardData.board_challenges_data; } }

        //================================
        //===SHOP
        //================================

        public void Initialize(BoardData data)
        {
            boardData = data;
        }
    }
	
}
