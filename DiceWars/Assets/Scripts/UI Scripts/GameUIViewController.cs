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

        public void ShowClashAttack()
        {
            //UIBattleFieldScript uiBfs = gameObject.GetComponentInChildren<UIBattleFieldScript>();
            uiBfs.gameObject.SetActive(true);
            //uiBfs.OnEnable();
        }
    }
}