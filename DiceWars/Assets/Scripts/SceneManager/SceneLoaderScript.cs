using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace GameControls
{
    public class SceneLoaderScript : MonoBehaviour
    {
        // Maybe this is not nessesary
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
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
