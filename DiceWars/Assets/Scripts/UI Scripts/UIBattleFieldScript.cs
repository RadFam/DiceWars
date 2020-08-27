using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameControls;

namespace UIControls
{
    public class UIBattleFieldScript : MonoBehaviour
    {
        public Text score_A;
        public Text score_B;
        public Image battleImage;
        public List<UIDiceScript> dices_A_1;
        public List<UIDiceScript> dices_A_2;
        public List<UIDiceScript> dices_A_3;
        public List<UIDiceScript> dices_B_1;
        public List<UIDiceScript> dices_B_2;
        public List<UIDiceScript> dices_B_3;

        private int sum_A;
        private int sum_B;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void OnEnable()
        {
            sum_A = 0;
            sum_B = 0;
            AnimationStage();
        }

        void AnimationStage()
        {
            StartCoroutine(Dices_AB_Player());
            // После того, как вывелись кубики, решаем, кто победил
            //ChangeWinner();
            //StartCoroutine(WinnerAnimate());
            //OnDisable();
        }

        public void OnDisable()
        {
            foreach (UIDiceScript uids in dices_A_1)
            {
                uids.ClearData();
            }
            foreach (UIDiceScript uids in dices_A_2)
            {
                uids.ClearData();
            }
            foreach (UIDiceScript uids in dices_A_3)
            {
                uids.ClearData();
            }
            foreach (UIDiceScript uids in dices_B_1)
            {
                uids.ClearData();
            }
            foreach (UIDiceScript uids in dices_B_2)
            {
                uids.ClearData();
            }
            foreach (UIDiceScript uids in dices_B_3)
            {
                uids.ClearData();
            }
            battleImage.sprite = CommonControl.instance.allGraphics.battleSwords[0];
            battleImage.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            score_A.text = "00";
            score_B.text = "00";

            sum_A = 0;
            sum_B = 0;

            ControlSequenceOfActions csoa = FindObjectOfType<ControlSequenceOfActions>();
            //csoa.GoAhead();
            gameObject.SetActive(false);
        }

        void DrawDice(int dNum, int side_AB, int type) // type == 0/1/2
        {
            int playerNum = 0;
            int playerRes = 0;

            if (side_AB == 0) // side A
            {
                playerNum = CommonControl.instance.battleStack.playerA_num;
                ControlScript.ArmyTypes aType = ControlScript.ArmyTypes.Dice_d6;

                if (type == 0)
                {
                    playerRes = CommonControl.instance.battleStack.playerA_res_1[dNum];
                    aType = ControlScript.ArmyTypes.Dice_d6;
                    dices_A_1[dNum].SetData(aType, playerNum, playerRes);
                }
                if (type == 1)
                {
                    playerRes = CommonControl.instance.battleStack.playerA_res_2[dNum];
                    aType = ControlScript.ArmyTypes.Dice_d12;
                    dices_A_2[dNum].SetData(aType, playerNum, playerRes);
                }
                if (type == 2)
                {
                    playerRes = CommonControl.instance.battleStack.playerA_res_3[dNum];
                    aType = ControlScript.ArmyTypes.Dice_d20;
                    dices_A_3[dNum].SetData(aType, playerNum, playerRes);
                }
            }
            else // == 1 side B
            {
                playerNum = CommonControl.instance.battleStack.playerB_num;
                ControlScript.ArmyTypes aType = ControlScript.ArmyTypes.Dice_d6;

                if (type == 0)
                {
                    playerRes = CommonControl.instance.battleStack.playerB_res_1[dNum];
                    aType = ControlScript.ArmyTypes.Dice_d6;
                    dices_B_1[dNum].SetData(aType, playerNum, playerRes);
                }
                if (type == 1)
                {
                    playerRes = CommonControl.instance.battleStack.playerB_res_2[dNum];
                    aType = ControlScript.ArmyTypes.Dice_d12;
                    dices_B_2[dNum].SetData(aType, playerNum, playerRes);
                }
                if (type == 2)
                {
                    playerRes = CommonControl.instance.battleStack.playerB_res_3[dNum];
                    aType = ControlScript.ArmyTypes.Dice_d20;
                    dices_B_3[dNum].SetData(aType, playerNum, playerRes);
                }
            }
        }

        void ChangeWinner()
        {
            if (sum_A > sum_B)
            {
                battleImage.sprite = CommonControl.instance.allGraphics.battleSwords[1]; // winblue sprite
            }
            else if (sum_A < sum_B)
            {
                battleImage.sprite = CommonControl.instance.allGraphics.battleSwords[2]; // winred sprite
            }
        }

        IEnumerator Dices_AB_Player()
        {             
            for (int i = 0; i < CommonControl.instance.battleStack.playerA_res_1.Count; ++i)
            {
                sum_A += CommonControl.instance.battleStack.playerA_res_1[i];
                score_A.text = sum_A.ToString();
                DrawDice(i, 0, 0);
                yield return new WaitForSeconds(.1f);
            }
            for (int i = 0; i < CommonControl.instance.battleStack.playerA_res_2.Count; ++i)
            {
                sum_A += CommonControl.instance.battleStack.playerA_res_2[i];
                score_A.text = sum_A.ToString();
                DrawDice(i, 0, 1);
                yield return new WaitForSeconds(.1f);
            }
            for (int i = 0; i < CommonControl.instance.battleStack.playerA_res_3.Count; ++i)
            {
                sum_A += CommonControl.instance.battleStack.playerA_res_3[i];
                score_A.text = sum_A.ToString();
                DrawDice(i, 0, 2);
                yield return new WaitForSeconds(.1f);
            }

            for (int i = 0; i < CommonControl.instance.battleStack.playerB_res_1.Count; ++i)
            {
                sum_B += CommonControl.instance.battleStack.playerB_res_1[i];
                score_B.text = sum_B.ToString();
                DrawDice(i, 1, 0);
                yield return new WaitForSeconds(.1f);
            }
            for (int i = 0; i < CommonControl.instance.battleStack.playerB_res_2.Count; ++i)
            {
                sum_B += CommonControl.instance.battleStack.playerB_res_2[i];
                score_B.text = sum_B.ToString();
                DrawDice(i, 1, 1);
                yield return new WaitForSeconds(.1f);
            }
            for (int i = 0; i < CommonControl.instance.battleStack.playerB_res_3.Count; ++i)
            {
                sum_B += CommonControl.instance.battleStack.playerB_res_3[i];
                score_B.text = sum_B.ToString();
                DrawDice(i, 1, 2);
                yield return new WaitForSeconds(.1f);
            }

            StartCoroutine(WinnerAnimate());
        }

        IEnumerator WinnerAnimate()
        {
            for (int i = 0; i < 10; ++i)
            {
                Vector3 scale = battleImage.transform.localScale;
                battleImage.transform.localScale = new Vector3(scale.x + 0.1f, scale.y + 0.1f, scale.z);              
                yield return new WaitForSeconds(.01f);
            }

            ChangeWinner();
            yield return new WaitForSeconds(0.1f);

            for (int i = 0; i < 20; ++i)
            {
                Vector3 scale = battleImage.transform.localScale;
                battleImage.transform.localScale = new Vector3(scale.x - 0.05f, scale.y - 0.05f, scale.z);
                yield return new WaitForSeconds(.01f);
            }
            

            yield return new WaitForSeconds(1.0f);

            ControlScript CS = FindObjectOfType<ControlScript>();
            CS.UndarkForHuman();

            OnDisable();
        }
    }

}