using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameAI;
using UIControls;

namespace GameControls
{
    public class ControlSequenceOfActions : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        private List<int> allgamerNums; // ordered sequence of all gamer numbers
        //private List<int> allgamerNums2; // ordered sequence of all gamer real numbers
        [SerializeField]
        private List<int> allplayerNums; // sequence of player numbers
        [SerializeField]
        private List<int> allNums;

        private ControlScript myCS;
        private ControlArmyGrowthScript myCAGS;
        private EnemyAI myAI;
        private int currPlay; // Number of current player (AI and human)

        private bool nextProceed = true;

        public List<int> GetAllGamerNums
        {
            get { return allgamerNums; }
        }

        public List<int> GetAllPlayerNums
        {
            get { return allplayerNums; }
        }

        void Start()
        {
            allNums = new List<int>();
            allplayerNums = new List<int>();
            allgamerNums = new List<int>();

            RestoreBeforeReload();

            /*
            allNums = Enumerable.Range(0, 8).ToList();

            // Temporary
            // .....let the list of allgamerNums is the range of 0 to 7
            allgamerNums = Enumerable.Range(0, 2).ToList(); // List of all players
            //allgamerNums2 = Enumerable.Range(0, 2).ToList(); // List of all player`s numbers (it is important)
            allplayerNums = Enumerable.Range(1, 1).ToList(); // List of human players

            myCS = gameObject.GetComponent<ControlScript>();
            myAI = gameObject.GetComponent<EnemyAI>();
            myCAGS = gameObject.GetComponent<ControlArmyGrowthScript>();
            myCS.CurrPlayerNum = allplayerNums[0];

            currPlay = -1;
            */
        }

        public void SetAllPlayersNum(int num)
        {
            if (num <= 8)
            {
                allgamerNums.Clear();
                //allgamerNums = Enumerable.Range(0, num).ToList();

                foreach (int nm in allplayerNums)
                {
                    allgamerNums.Add(nm);
                }

                allgamerNums.Sort();
                List<int> exNum = allNums.Except(allgamerNums).ToList();

                for (int i = 0; i < num - allplayerNums.Count; ++i)
                {
                    allgamerNums.Add(exNum[i]);
                }

                allgamerNums.Sort();
            }
        }

        public int AddHumanPlayer()
        {
            int plr = 0;

            bool tryNext = true;

            while (tryNext)
            {
                //plr = Random.Range(0, allgamerNums.Count-1);
                plr = Random.Range(0, 8);
                if (!allplayerNums.Contains(plr))
                {
                    allplayerNums.Add(plr);
                    tryNext = false;
                }
            }

            int plrs = allgamerNums.Count + 1;
            SetAllPlayersNum(plrs);

            return plr;
        }

        public void RemoveHumanPlayer()
        {
            if (allplayerNums.Count > 1)
            {
                allplayerNums.RemoveAt(allplayerNums.Count - 1);
                int plrs = allgamerNums.Count - 1;
                SetAllPlayersNum(plrs);
            }
        }

        public int ChangeHumanPlayer(int plNum)
        {
            int plr = 0;
            bool tryNext = true;

            /*
            if (allplayerNums.Count == allgamerNums.Count)
            {
                return -1;
            }
            */

            while (tryNext)
            {
                //plr = Random.Range(0, allgamerNums.Count - 1);
                plr = Random.Range(0, 8);
                if (!allplayerNums.Contains(plr))
                {
                    allplayerNums[plNum] = plr;
                    tryNext = false;
                }
            }

            int plrs = allgamerNums.Count;
            SetAllPlayersNum(plrs);

            return plr;
        }

        public void TempPrint()
        {
            Debug.Log("All gamers: " + allgamerNums.Count.ToString());
            foreach (int pl in allgamerNums)
            {
                Debug.Log(pl.ToString());
            }
            Debug.Log("Human players nums:");
            foreach (int pl in allplayerNums)
            {
                Debug.Log(pl.ToString());
            }
        }

        public void ActionIteration()
        {
            int terrNum = 0; // Number of territories that occupied current player (currPlayer)
            terrNum = Enumerable.Range(0, myCS.GetRM.GetAccRegions.Count)
                    .Where(x => myCS.GetRM.GetAccRegions[x].myPlayer == allgamerNums[currPlay])
                    .ToList().Count;

            if (terrNum > 0)
            {
                UISequenceScript uiSS = FindObjectOfType<UISequenceScript>();
                uiSS.SetActiveElement(allgamerNums[currPlay]);

                myCS.CanTilePicking = false;
                myCS.humanMove = false;
                int ind = allplayerNums.FindIndex(x => x == allgamerNums[currPlay]);
                if (ind == -1)
                {
                    // Block in ControlScript ability to click on tilemap
                    myCS.CanTilePicking = false;
                    myCS.humanMove = false;
                    // Invoke EnemyAI
                    myAI.EnemyPlayerAttacks(allgamerNums[currPlay]);
                }
                else
                {
                    // Deblock in ControlScript ability to click on tilemap
                    myCS.CanTilePicking = true;
                    myCS.humanMove = true;
                    myCS.CurrPlayerNum = allplayerNums[ind];
                }
            }
            else
            {
                Debug.Log("Suddenly invoke GoAhead from CSoA");
                GoAhead();
            }

            
        }

        public void GoAhead()
        {
            myCS.CanTilePicking = false;
            if (nextProceed)
            {
                StartCoroutine(BetweenPause()); // Maybe here we can invoke drawing of dice stashing
            }
        }

        IEnumerator BetweenPause()
        {
            // Check if player have territories yet
            int terrNum = 0;
            if (currPlay != -1)
            {
                terrNum = Enumerable.Range(0, myCS.GetRM.GetAccRegions.Count)
                    .Where(x => myCS.GetRM.GetAccRegions[x].myPlayer == allgamerNums[currPlay])
                    .ToList().Count;
            }

            if (currPlay != -1 && terrNum > 0)
            {
                // Вставить анимацию увеличения числа кубиков
                //yield return StartCoroutine(myCAGS.ConsequenceArmyIncrease(currPlay)); //(????)
                yield return StartCoroutine(myCAGS.ConsequenceArmyIncrease(allgamerNums[currPlay]));
            }
            currPlay += 1;
            //Debug.Log("currPlay: " + currPlay.ToString() + "  allgamerNums.Count: " + allgamerNums.Count.ToString());
            currPlay = currPlay % allgamerNums.Count;
            //Debug.Log("Real currPlay: " + currPlay.ToString());

            yield return new WaitForSeconds(0.5f);
            ActionIteration();
            //Debug.Log("End of Coroutine");
        }

        public void EmergencyStop()
        {
            nextProceed = false;
            myCS.CanTilePicking = false;
            myAI.EmergencyStop();
        }

        public void RestartSequence()
        {
            nextProceed = true;
            myCS.CurrPlayerNum = allplayerNums[0];
            currPlay = -1;
            myAI.RestoreBeforeReload();
        }

        public void RestoreBeforeReload()
        {
            nextProceed = true;

            allNums.Clear();
            allgamerNums.Clear();
            allplayerNums.Clear();
            myCS = null;
            myAI = null;
            myCAGS = null;

            allNums = Enumerable.Range(0, 8).ToList();

            // Temporary
            // .....let the list of allgamerNums is the range of 0 to 7
            allgamerNums = Enumerable.Range(0, 2).ToList(); // List of all players
            //allgamerNums2 = Enumerable.Range(0, 2).ToList(); // List of all player`s numbers (it is important)
            allplayerNums = Enumerable.Range(1, 1).ToList(); // List of human players

            myCS = gameObject.GetComponent<ControlScript>();
            myAI = gameObject.GetComponent<EnemyAI>();
            myCAGS = gameObject.GetComponent<ControlArmyGrowthScript>();
            myCS.CurrPlayerNum = allplayerNums[0];

            myAI.RestoreBeforeReload();

            currPlay = -1;
        }

    }
}
