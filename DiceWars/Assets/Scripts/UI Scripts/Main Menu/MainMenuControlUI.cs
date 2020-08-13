using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControls;

namespace UIControls
{
    public class MainMenuControlUI : MonoBehaviour
    {
        [SerializeField]
        private MenuOneUIScript dialOne;
        [SerializeField]
        private MenuTwoUIScript dialTwo;
        // Start is called before the first frame update
        void Start()
        {

        }

        public void OnMenuOneOpen()
        {
            dialTwo.gameObject.SetActive(false);
            dialOne.gameObject.SetActive(true);
        }

        public void OnMenuTwoOpen()
        {
            dialOne.gameObject.SetActive(false);
            dialTwo.gameObject.SetActive(true);
        }

        public void OnStartGame()
        {
            ControlSequenceOfActions CSoA = FindObjectOfType<ControlSequenceOfActions>();
            CSoA.TempPrint();

            SceneLoaderScript SLS = FindObjectOfType<SceneLoaderScript>();
            SLS.LoadGameScene();
        }

        public void OnAppExit()
        {
            Application.Quit();
        }
    }
}

