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
        UIBattleAIFieldScript uiAIBfs;

        public void ShowClashAttack()
        {
            //UIBattleFieldScript uiBfs = gameObject.GetComponentInChildren<UIBattleFieldScript>();
            uiBfs.gameObject.SetActive(true);
            //uiBfs.OnEnable();
        }

        public void ShowAIClashAttack()
        {
            uiAIBfs.gameObject.SetActive(true);
        }
    }
}