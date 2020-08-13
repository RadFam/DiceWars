using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using GameControls;

namespace UIControls
{
    public class UISequenceScript : MonoBehaviour
    {
        public PlayerSequenceElementScript seqElementPrefab;
        List<PlayerSequenceElementScript> gamePlayers;

        void Start()
        {
            gamePlayers = new List<PlayerSequenceElementScript>();
        }

        public void CleanPrevoius()
        {
            foreach(PlayerSequenceElementScript pses in gamePlayers)
            {
                Destroy(pses.gameObject);
            }

            gamePlayers.Clear();
        }

        public void SetupSequeneceList()
        {
            CleanPrevoius();

            ControlSequenceOfActions CSoA = FindObjectOfType<ControlSequenceOfActions>();
            List<int> gamers = CSoA.GetAllGamerNums;

            int terrNum = 0;
            ControlScript myCS = FindObjectOfType<ControlScript>();

            foreach (int num in gamers)
            {
                terrNum = myCS.GetRM.GetPlayerMaxConnectedTerritorySize(num);
                /*
                terrNum = Enumerable.Range(0, myCS.GetRM.GetAccRegions.Count)
                    .Where(x => myCS.GetRM.GetAccRegions[x].myPlayer == num)
                    .ToList().Count;
                   */
                PlayerSequenceElementScript tmp = Instantiate(seqElementPrefab);
                tmp.transform.parent = gameObject.transform;
                tmp.SetMyPlayer(num, terrNum);
                gamePlayers.Add(tmp);
            }
        }

        public void UpdatePlayerFiledsVol(int playerNum)
        {
            StartCoroutine(UpdatePlayerFieldCoroutine(playerNum));
        }

        public void SetActiveElement(int playerNum)
        {
            int ind = gamePlayers.FindIndex(x => x.myNum == playerNum);

            if (ind != -1)
            {
                for (int i = 0; i < gamePlayers.Count; ++i)
                {
                    gamePlayers[i].UpLight(i == ind);
                }
            }
        }

        public void DisposeElement(int playerNum)
        {
            int ind = gamePlayers.FindIndex(x => x.myNum == playerNum);

            if (ind != -1)
            {
                Destroy(gamePlayers[ind].gameObject);
                gamePlayers.RemoveAt(ind);
            }
        }

        IEnumerator UpdatePlayerFieldCoroutine(int value)
        {
            yield return new WaitForSeconds(0.5f);

            int ind = gamePlayers.FindIndex(x => x.myNum == value);
            int terrNum = -1;

            if (ind != -1)
            {
                ControlScript myCS = FindObjectOfType<ControlScript>();
                /*
                terrNum = Enumerable.Range(0, myCS.GetRM.GetAccRegions.Count)
                    .Where(x => myCS.GetRM.GetAccRegions[x].myPlayer == playerNum)
                    .ToList().Count;
                    */
                terrNum = myCS.GetRM.GetPlayerMaxConnectedTerritorySize(value);
                gamePlayers[ind].SetMyFields(terrNum);
            }

            if (terrNum == 0)
            {
                DisposeElement(value);
            }
        }
    }
}

