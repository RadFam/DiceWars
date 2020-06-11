using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIControls
{
    public class GameUIViewController : MonoBehaviour
    {
        public void ShowClashAttack()
        {
            UIBattleFieldScript uiBfs = gameObject.GetComponentInChildren<UIBattleFieldScript>();
            uiBfs.gameObject.SetActive(true);
        }
    }
}