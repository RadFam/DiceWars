using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameControls;

namespace UIControls
{
    public class UIMessageScript : MonoBehaviour
    {
        public Text messageText;
        public Image myImage;

        List<string> players = new List<string>{"Аквамариновый", "Синий", "Зеленый", "Оранжевый", "Фиолетовый", "Красный", "Белый", "Желтый"};
        
        // Start is called before the first frame update
        void Start()
        {
            //myImage = gameObject.GetComponent<Image>();
        }

        public void SetStatus(bool isWin, int player = 0)
        {
            //Debug.Log("My image is: " + myImage);

            if (isWin)
            {
                messageText.text = players[player] + " игрок выиграл";
                myImage.sprite = CommonControl.instance.allGraphics.commonMenuPanels[1];
            }
            else
            {
                messageText.text = "Поражение :(";
                myImage.sprite = CommonControl.instance.allGraphics.commonMenuPanels[2];
            }
        }
    }
}
