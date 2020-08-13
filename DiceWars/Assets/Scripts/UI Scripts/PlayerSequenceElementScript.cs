using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameControls;

namespace UIControls
{
    public class PlayerSequenceElementScript : MonoBehaviour
    {
        public Image bkgrImage;
        public Image frameImage;
        public Text fieldsNum;
        public int myNum;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void SetMyPlayer(int num, int vol)
        {
            bkgrImage.sprite = CommonControl.instance.allGraphics.unitSprite_class01[num];
            myNum = num;
            SetMyFields(vol);
        }

        public void SetMyFields(int vol)
        {
            fieldsNum.text = vol.ToString();
        }

        public void UpLight(bool uplight)
        {
            if (uplight)
            {
                frameImage.sprite = CommonControl.instance.allGraphics.playerSequenceElementFrames[1];
            }
            else
            {
                frameImage.sprite = CommonControl.instance.allGraphics.playerSequenceElementFrames[0];
            }
        }
    }
}
