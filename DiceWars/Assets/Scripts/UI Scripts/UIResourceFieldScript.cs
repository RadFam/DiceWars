using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameControls;

namespace UIControls
{
    public class UIResourceFieldScript : MonoBehaviour
    {
        public delegate void EndStep();
        public EndStep endStep;

        public Text resVolume;
        public List<UIDiceScript> diceReserve;

        private int resVol;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void OnEnable()
        {
            resVol = 0;
            resVolume.text = "00";
        }

        public void OnDisable()
        {
            ClearImages();
            resVolume.text = "00";
            resVol = 0;

            gameObject.SetActive(false);
        }

        void ClearImages()
        {
            foreach (UIDiceScript uids in diceReserve)
            {
                uids.ClearData();
            }
        }

        public void SetPlayer(int playerNum)
        {
            resVol = CommonControl.instance.GetFromReserve(playerNum);
            //Debug.Log("SAVED RESOURCE: " + resVol.ToString());

            int vl = Mathf.Min(resVol, diceReserve.Count);

            for (int i = 0; i < vl; ++i)
            {
                diceReserve[i].SetData(ControlScript.ArmyTypes.Dice_d6, playerNum, Random.Range(1, 7));
            }

            resVolume.text = resVol.ToString();
        }

        public void ClickMinus()
        {
            if (resVol > 0)
            {
                if (resVol <= diceReserve.Count)
                {
                    diceReserve[resVol-1].ClearData();
                }
                resVol--;

                resVolume.text = resVol.ToString();
            }
        }
    }
}
