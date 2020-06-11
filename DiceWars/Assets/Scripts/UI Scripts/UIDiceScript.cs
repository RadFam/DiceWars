using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameControls;

namespace UIControls
{
    public class UIDiceScript : MonoBehaviour
    {

        public Image bkgrIm;
        public Image valvIm;

        public void SetData(ControlScript.ArmyTypes aType, int playerNum, int value)
        {
            if (aType == ControlScript.ArmyTypes.Dice_d6)
            {
                bkgrIm.sprite = CommonControl.instance.allGraphics.diceSprite_class_d6[playerNum];
                if (playerNum >= 1 && playerNum <= 5)
                {
                    valvIm.sprite = CommonControl.instance.allGraphics.valueSprite_class_d6_white[value - 1];
                }
                else
                {
                    valvIm.sprite = CommonControl.instance.allGraphics.valueSprite_class_d6_black[value - 1];
                }
            }
            if (aType == ControlScript.ArmyTypes.Dice_d12)
            {

            }
            if (aType == ControlScript.ArmyTypes.Dice_d20)
            {

            }
        }

        public void ClearData()
        {
            bkgrIm.sprite = null;
            valvIm.sprite = null;
        }
    }
}
