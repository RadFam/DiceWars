using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameControls;

namespace UIControls
{
    public class UIBattleAIFieldScript : MonoBehaviour
    {
        public Text score_A;
        public Text score_B;
        public List<UIDiceScript> dices_A_1;
        public List<UIDiceScript> dices_B_1;

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
            //UIAnimationStage();
        }

        public void OnDisable()
        {
            ClearImages(0);
            ClearImages(1);

            score_A.text = "0";
            score_B.text = "0";

            sum_A = 0;
            sum_B = 0;

            gameObject.SetActive(false);
        }

        void ClearImages(int side)
        {
            if (side == 0) // side A
            {
                foreach (UIDiceScript uids in dices_A_1)
                {
                    uids.ClearData();
                }
            }

            if (side == 1) // side B
            {
                foreach (UIDiceScript uids in dices_B_1)
                {
                    uids.ClearData();
                }
            }
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
                    dices_A_1[dNum].SetData(aType, playerNum, playerRes);
                }
                if (type == 2)
                {
                    playerRes = CommonControl.instance.battleStack.playerA_res_3[dNum];
                    aType = ControlScript.ArmyTypes.Dice_d20;
                    dices_A_1[dNum].SetData(aType, playerNum, playerRes);
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
                    dices_B_1[dNum].SetData(aType, playerNum, playerRes);
                }
                if (type == 2)
                {
                    playerRes = CommonControl.instance.battleStack.playerB_res_3[dNum];
                    aType = ControlScript.ArmyTypes.Dice_d20;
                    dices_B_1[dNum].SetData(aType, playerNum, playerRes);
                }
            }
        }

        void UIAnimationStage()
        {
            StartCoroutine(Dices_AB_AI());
        }

        public IEnumerator Dices_AB_AI()
        {
            ClearImages(0);
            for (int i = 0; i < CommonControl.instance.battleStack.playerA_res_1.Count; ++i)
            {
                sum_A += CommonControl.instance.battleStack.playerA_res_1[i];
                score_A.text = sum_A.ToString();
                DrawDice(i, 0, 0);
                yield return new WaitForSeconds(.1f);
            }
            //ClearImages(0);
            for (int i = 0; i < CommonControl.instance.battleStack.playerA_res_2.Count; ++i)
            {
                sum_A += CommonControl.instance.battleStack.playerA_res_2[i];
                score_A.text = sum_A.ToString();
                DrawDice(i, 0, 1);
                yield return new WaitForSeconds(.1f);
            }
            //ClearImages(0);
            for (int i = 0; i < CommonControl.instance.battleStack.playerA_res_3.Count; ++i)
            {
                sum_A += CommonControl.instance.battleStack.playerA_res_3[i];
                score_A.text = sum_A.ToString();
                DrawDice(i, 0, 2);
                yield return new WaitForSeconds(.1f);
            }

            ClearImages(1);
            for (int i = 0; i < CommonControl.instance.battleStack.playerB_res_1.Count; ++i)
            {
                sum_B += CommonControl.instance.battleStack.playerB_res_1[i];
                score_B.text = sum_B.ToString();
                DrawDice(i, 1, 0);
                yield return new WaitForSeconds(.1f);
            }
            //ClearImages(1);
            for (int i = 0; i < CommonControl.instance.battleStack.playerB_res_2.Count; ++i)
            {
                sum_B += CommonControl.instance.battleStack.playerB_res_2[i];
                score_B.text = sum_B.ToString();
                DrawDice(i, 1, 1);
                yield return new WaitForSeconds(.1f);
            }
            //ClearImages(1);
            for (int i = 0; i < CommonControl.instance.battleStack.playerB_res_3.Count; ++i)
            {
                sum_B += CommonControl.instance.battleStack.playerB_res_3[i];
                score_B.text = sum_B.ToString();
                DrawDice(i, 1, 2);
                yield return new WaitForSeconds(.1f);
            }

            yield return new WaitForSeconds(0.4f);

            OnDisable();
        }
    }
}
