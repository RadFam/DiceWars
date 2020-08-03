using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using GameControls;


namespace UIControls
{
    public class HumanElementScript : MonoBehaviour, IPointerClickHandler
    {
        public int myNum;
        public int myColor;

        public Image backGrndImg;
        public Image valueImg;
        // Start is called before the first frame update
        void Start()
        {

        }

        public void SetInitials(int newNum, int newColor)
        {
            myNum = newNum;
            myColor = newColor;

            backGrndImg.sprite = CommonControl.instance.allGraphics.diceSprite_class_d6[newColor];
            int value = Random.Range(0, 6);
            if (newColor >= 1 && newColor <= 5)
            {
                valueImg.sprite = CommonControl.instance.allGraphics.valueSprite_class_d6_white[value];
            }
            else
            {
                valueImg.sprite = CommonControl.instance.allGraphics.valueSprite_class_d6_black[value];
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            MenuTwoUIScript mtUIS = FindObjectOfType<MenuTwoUIScript>();
            mtUIS.OnHumanClicked(myNum);
        }
    }
}


