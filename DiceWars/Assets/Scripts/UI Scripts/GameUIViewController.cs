using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameControls;

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
        [SerializeField]
        UIMessageScript uiMs;

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

        public UIMessageScript ShowMessageFrame()
        {
            uiMs.gameObject.SetActive(true);
            return uiMs;
        }

        public void OnRestartGame()
        {
            uiMs.gameObject.SetActive(false);
            ControlScript CS = FindObjectOfType<ControlScript>();
            CS.RestoreInitialGame();
        }

        public void OnMenuExit()
        {
            uiMs.gameObject.SetActive(false);
            ControlScript CS = FindObjectOfType<ControlScript>();
            CS.RestoreBeforeReload();
            SceneLoaderScript SLS = FindObjectOfType<SceneLoaderScript>();
            SLS.LoadMenuScene();
        }
    }
}