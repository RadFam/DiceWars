using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameControls;


namespace UIControls
{
    public class EnemyElementScript : MonoBehaviour, IPointerClickHandler
    {
        public int myNum;
        public Image myImage;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Enemy clicked " + myNum.ToString()); 
            OnClick();
            MenuTwoUIScript mtUIS = FindObjectOfType<MenuTwoUIScript>();
            mtUIS.OnEnemyClicked(myNum);
        }

        public void OnClick()
        {
            myImage.sprite = CommonControl.instance.allGraphics.enemyElements[1];
        }

        public void OnDeclick()
        {
            myImage.sprite = CommonControl.instance.allGraphics.enemyElements[0];
        }
    }
}
    
