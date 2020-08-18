using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UIControls;


namespace GameControls
{
    public class SceneLoaderScript : MonoBehaviour
    {
        private static SceneLoaderScript instance;
        // Maybe this is not nessesary
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);

            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        public void LoadGameScene()
        {
            StartCoroutine(LoadGameSceneCor());
        }

        public void LoadMenuScene()
        {
            StartCoroutine(LoadMenuSceneCor());
        }

        IEnumerator LoadMenuSceneCor()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenuScene", LoadSceneMode.Single);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            Debug.Log("Menu Scene Loaded");

            MainMenuControlUI mmCui = FindObjectOfType<MainMenuControlUI>();
            mmCui.OnMenuOneOpen();
            ControlScript cs = FindObjectOfType<ControlScript>();
            cs.CanTilePicking = false;
        }

        IEnumerator LoadGameSceneCor()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Single);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            ControlScript CS = gameObject.GetComponent<ControlScript>();
            CS.ActionsAfterSceneLoad();
        }
    }
}
