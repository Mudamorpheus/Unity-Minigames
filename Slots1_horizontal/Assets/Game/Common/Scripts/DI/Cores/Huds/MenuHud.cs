using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using TMPro;
using DG.Tweening;

using Game.Common.Scripts.UI.Buttons;
using Game.Common.Scripts.UI.Items;
using Game.Common.Scripts.UI.Frames;
using Game.Common.Scripts.Systems;
using Game.Common.Scripts.Data;

namespace Game.Common.Scripts.DI.Cores.Huds
{
    public class MenuHud : BaseHud
    {
        #region Fields

        //=======
        [SerializeField] private GameObject[]    uiViews;
        [SerializeField] private Button[]        uiButtons;
        [SerializeField] private GameObject      uiBankView;        
        //
        [SerializeField] private TMP_Text[]      uiBalanceTexts;
        [SerializeField] private TMP_Text[]      uiLevelTexts;
        [SerializeField] private Image[]         uiLevelProgress;

        //=======
        [SerializeField] private RewardFrame     uiRewardFrame;
        [SerializeField] private Button          uiRewardButton;

        //=======
        [SerializeField] private GameObject      uiSlotFrame;
        [SerializeField] private int             uiSlotFrameColCount;
        [SerializeField] private int             uiSlotFrameRowCount;
        [SerializeField] private int             uiSlotFrameSymbols;
        [SerializeField] private int[]           uiSlotFrameStreaks;
        //
        [SerializeField] private GameObject[]    uiSlotFrameColumns;        
        //
        [SerializeField] private GameObject[]    uiSlotFrameRowsWins;
        [SerializeField] private Image[]         uiSlotFrameRowsNumbersLeft;
        [SerializeField] private Image[]         uiSlotFrameRowsNumbersRight;
        //        
        [SerializeField] private ButtonCounter   uiSlotFrameBetCounter;
        [SerializeField] private ButtonCounter   uiSlotFrameLinesCounter;
        [SerializeField] private TMP_Text        uiSlotFrameTotalBetText;
        [SerializeField] private TMP_Text        uiSlotFrameWinsText;
        
        //=======
        private List<int>      uiSlotFrameResults;
        private int            uiSlotFrameWin;
        private float          uiSlotFrameSpinLoopY;
        private float          uiSlotFrameSpinSlotY;
        //
        private List<Sequence> uiSlotFrameWinSequences;
        private List<Image>    uiSlotFrameWinSlots;        

        #endregion

        //===========================================

        #region MonoBehaviour

        protected override void Clear()
        {
            base.Clear();

            foreach (var button in uiButtons)
            {
                if (button) { button.onClick.RemoveAllListeners(); }
            }
        }

        #endregion

        //===========================================

        #region BaseHud

        public override void Init()
        {
            base.Init();

            //Init
            uiSlotFrameResults = new List<int>();

            //Values
            UpdateCountersValues();
            UpdateRewardAttention();
            SwitchSlotFrame();

            //Taps
            foreach (var button in uiButtons)
            {
                if (button) { button.onClick.AddListener(Tap); }
            }

            //Lists
            uiSlotFrameWinSequences = new List<Sequence>();
            uiSlotFrameWinSlots     = new List<Image>();
    }

        public override void UpdateValues()
        {
            base.UpdateValues();

            UpdateBalanceValues();
            UpdateLevelValues();
            UpdateCountersValues();
        }

        public void UpdateBalanceValues()
        {
            foreach (var item in uiBalanceTexts)
            {
                item.text = playerData.Balance.ToString();
            }
        }
        public void UpdateLevelValues()
        {
            foreach (var item in uiLevelTexts)
            {
                item.text = "Level: " + playerData.Level.ToString();
            }
            foreach (var item in uiLevelProgress)
            {
                float spent   = PlayerData.Spent;
                float prev    = (playerData.Level) * StaticData.LevelRequired;
                float require = (playerData.Level + 1) * StaticData.LevelRequired;
                item.DOFillAmount((spent-prev)/(require-prev), StaticData.FillDuration);
            }
        }
        public void UpdateCountersValues()
        {
            uiSlotFrameBetCounter.SetValue(playerData.Bet);
            uiSlotFrameLinesCounter.SetValue(playerData.Lines);
        }

        public void UpdateSlotsValues()
        {
            uiSlotFrameTotalBetText.text = (playerData.Bet * playerData.Lines).ToString();
            uiSlotFrameWinsText.text = uiSlotFrameWin.ToString();
        }

        private Sequence attentionSequence;
        public void UpdateRewardAttention()
        {
            var level = PlayerData.LevelsData.Find(x => x.State == PlayerData.Fields.LevelState.Completed);

            attentionSequence.Kill();
            uiRewardButton.transform.localScale = Vector3.one;
            if (level != null)
            {
                attentionSequence = DOTween.Sequence();
                attentionSequence.Append(uiRewardButton.transform.DOScale(1.3f, StaticData.AlertDuration));
                attentionSequence.Append(uiRewardButton.transform.DOScale(1f, StaticData.AlertDuration));
                attentionSequence.SetLoops(-1);
            }
        }

        public void SaveBetCounter()
        {
            playerData.Bet = uiSlotFrameBetCounter.Value;
            
        }

        public void SaveLinesCounter()
        {
            playerData.Lines = uiSlotFrameLinesCounter.Value;
        }

        #endregion

        //===========================================

        #region MenuHud

        public void HideViews()
        {
            foreach (var view in uiViews)
            {
                view.SetActive(false);
            }
        }

        public void SwitchView(GameObject view)
        {
            HideViews();
            view.SetActive(true);
            view.transform.position = sceneCanvas.transform.position;
        }

        public void SwitchBank()
        {
            SwitchView(uiBankView);
        }

        public void SwitchButtons(bool state)
        {
            foreach(var button in uiButtons)
            {
                if(button != null)
                {
                    button.enabled = state;
                    var image = button.GetComponent<Image>();

                    if (state)
                    {
                        image.DOColor(new Color(1, 1, 1, 1f), StaticData.ColorDuration);
                    }
                    else
                    {
                        image.DOColor(new Color(1, 1, 1, 0.5f), StaticData.ColorDuration);
                    }
                }                              
            }
        }

        #endregion

        //===========================================

        #region Actions

        public override void BuyBalance(int balance)
        {
            base.BuyBalance(balance);

            UpdateBalanceValues();
        }

        public bool BetSpend(int value)
        {
            if(playerData.Balance - value >= 0)
            {
                playerData.Balance -= value;
                playerData.Spent   += value;

                LvlUp();

                UpdateBalanceValues();
                UpdateLevelValues();

                return true;
            }
            else
            {
                uiBankruptPopupFactory.Create(this, sceneEngine);

                return false;
            }
        }

        public void BetReward(int value)
        {
            //uiSlotFrameWins++;
            //UpdateSlotsValues();

            playerData.Balance += value;
            UpdateBalanceValues();
        }

        public void LvlUp()
        {
            int require = (playerData.Level+1) * StaticData.LevelRequired;
            if (playerData.Spent > require)
            {
                uiRewardFrame.Complete(playerData.Level);
                playerData.Complete(playerData.Level);                
                playerData.Level++;

                UpdateRewardAttention();
                SoundSystem.Instance.PlaySound("new lvl");
            }
            UpdateLevelValues();
        }

        public void Tap()
        {
            SoundSystem.Instance.PlaySound("tap");
        }

        #endregion

        //===========================================

        #region Spin

        public void SwitchSlotFrame()
        {
            //Index
            int count = uiSlotFrameLinesCounter.Value;

            //Show
            for(int i = 0; i < uiSlotFrameRowsWins.Length; i++)
            {
                if(i < count)
                {                    
                    uiSlotFrameRowsNumbersLeft[i].color  = new Color(1f, 1f, 1f, 1f);
                    uiSlotFrameRowsNumbersRight[i].color = new Color(1f, 1f, 1f, 1f);
                }
                else
                {
                    uiSlotFrameRowsNumbersLeft[i].color  = new Color(1f, 1f, 1f, 0.5f);
                    uiSlotFrameRowsNumbersRight[i].color = new Color(1f, 1f, 1f, 0.5f);
                }
            }
        }

        public void Spin()
        {
            //Win slots
            foreach (var sequence in uiSlotFrameWinSequences)
            {
                sequence.Complete();
                sequence.Kill();
            }
            uiSlotFrameWinSequences.Clear();
            foreach (var border in uiSlotFrameWinSlots)
            {
                border.color = new Color(1f, 1f, 1f, 0f);
                border.enabled = false;
            }
            uiSlotFrameWinSlots.Clear();

            //Counter
            uiSlotFrameWin = 0;
            UpdateSlotsValues();

            //Size            
            uiSlotFrameSpinSlotY = uiSlotFrameColumns[0].transform.GetChild(0).GetComponent<RectTransform>().rect.height;
            uiSlotFrameSpinLoopY = 12 * uiSlotFrameSpinSlotY;

            //Buy
            int price = uiSlotFrameBetCounter.Value * uiSlotFrameLinesCounter.Value;
            bool enough = BetSpend(price);
            if (!enough) { return; }

            //Clear
            uiSlotFrameResults.Clear();

            //Buttons
            SwitchButtons(false);

            //Spin
            for(int i = 0; i < uiSlotFrameColumns.Length; i++)
            {
                var column = uiSlotFrameColumns[i];

                Sequence sequence = DOTween.Sequence();  
                for(int k = 0; k < 5+i; k++)
                {
                    bool last = ((i == uiSlotFrameColumns.Length-1) && (k == 5+i-1));

                    sequence.Append(column.transform.
                        DOLocalMoveY(uiSlotFrameSpinLoopY, StaticData.SpinDuration).
                        SetEase(Ease.Linear));
                    sequence.Append(column.transform.
                        DOLocalMoveY(0, 0f).
                        SetEase(Ease.Linear));
                    sequence.OnComplete(delegate { RandomizeColumn(column, last); });
                }                
            }

            //Sound
            SoundSystem.Instance.PlaySound("spin");
        }

        public void RandomizeColumn(GameObject column, bool last)
        {
            //Random
            int rnd = Random.Range(4, uiSlotFrameSymbols-1);
            uiSlotFrameResults.Add(rnd);

            //Shift            
            float shift = uiSlotFrameSpinSlotY * rnd;
            
            //Move
            if(last) 
            {
                column.transform.DOLocalMoveY(shift, 1f).OnComplete(delegate { CalculateColumns(); });
            }
            else
            {
                column.transform.DOLocalMoveY(shift, 1f);
            }
        }

        public void CalculateColumns()
        {
            //End            
            SwitchButtons(true);
            for (int i = 0; i < uiSlotFrameColumns.Length; i++)
            {
                var column = uiSlotFrameColumns[i];
                column.transform.DOComplete();
            }

            //Win rows            
            for (int i = 0; i < playerData.Lines; i++)
            {
                //Gather sreaks
                var slots = new List<SlotItem>();
                var row = uiSlotFrameRowsWins[i];
                if(row.activeSelf)
                {
                    //Rnd columns                                        
                    for (int k = 0; k < uiSlotFrameResults.Count; k++)
                    {
                        int rnd = uiSlotFrameResults[k];
                        var col = uiSlotFrameColumns[k];
                        var len = col.transform.childCount;
                        var symb = col.transform.GetChild(rnd+(3-i));
                        var slot = symb.GetComponent<SlotItem>();

                        slots.Add(slot);
                    }
                }

                //Reward
                CalculateStreaks(slots);
            }
        }

        public void CalculateStreaks(List<SlotItem> slots)
        {
            //Victory
            bool win = false;

            //Calculate
            HashSet<int> winIds   = new HashSet<int>();
            HashSet<int> checkIds = new HashSet<int>();               
            int maxStreak         = 0;
            int curStreak         = 0;
            int prevValue         = -1;
            int curValue          = -1;

            //Loop
            int reward = 0;
            for (int i = 0; i < slots.Count; i++)
            {                
                //Compare
                prevValue = curValue;
                curValue = slots[i].Id;

                if (curValue == prevValue)
                {
                    curStreak++;
                    checkIds.Add(i-1);
                    checkIds.Add(i);

                    //Streak
                    if (curStreak >= maxStreak) 
                    {
                        foreach (var value in checkIds) { winIds.Add(value); }
                        maxStreak = curStreak; 
                    }
                }
                else
                {                    
                    curStreak = 0;
                    checkIds.Clear();

                    //Reward            
                    if (maxStreak >= 1)
                    {
                        reward += uiSlotFrameStreaks[maxStreak] * uiSlotFrameBetCounter.Value;
                        maxStreak = 0;
                    }
                }                                           
            }

            //Reward            
            if (maxStreak >= 1)
            {
                reward += uiSlotFrameStreaks[maxStreak] * uiSlotFrameBetCounter.Value;
                maxStreak = 0;
            }

            //Win
            if (reward > 0)
            {
                //Counter
                uiSlotFrameWin += reward;
                UpdateSlotsValues();

                //Borders
                if(winIds != null)
                {
                    foreach(var id in winIds)
                    {
                        var slot = slots[id];
                        var border = slot.WinBorder;
                        border.enabled = true;

                        var sequence = DOTween.Sequence();
                        sequence.Append(border.DOFade(1f, StaticData.WinBorderDuration));
                        sequence.Append(border.DOFade(0f, StaticData.WinBorderDuration));
                        sequence.SetLoops(-1);

                        uiSlotFrameWinSequences.Add(sequence);
                        uiSlotFrameWinSlots.Add(border);

                        BetReward(reward);
                    }
                }                

                //Sound
                SoundSystem.Instance.PlaySound("win");
            }
        }

        #endregion
    }
}