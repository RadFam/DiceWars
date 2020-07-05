using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIControls
{
    public class GameUIViewController : MonoBehaviour
    {

        [SerializeField]
        UIBattleFieldScript uiBfs;
        [SerializeField]
        UIBattleAIFieldScript uiAIBfs;
        [SerializeField]
        UIResourceFieldScript uiRfs;

        public void ShowClashAttack()
        {
            //UIBattleFieldScript uiBfs = gameObject.GetComponentInChildren<UIBattleFieldScript>();
            uiBfs.gameObject.SetActive(true);
        }

        public UIBattleAIFieldScript ShowAIClashAttack()
        {
            uiAIBfs.gameObject.SetActive(true);
            return uiAIBfs;
        }

        public UIResourceFieldScript ShowResourceGrowth()
        {
            uiRfs.gameObject.SetActive(true);            
            return uiRfs;
        }
    }
}