using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameAI;

namespace GameControls
{
    public class ControlSequenceOfActions : MonoBehaviour
    {
        // Start is called before the first frame update
        private List<int> allgamerNums; // ordered sequence of all gamer numbers
        private List<int> allplayerNums; // sequence of player numbers

        private ControlScript myCS;
        private ControlArmyGrowthScript myCAGS;
        private EnemyAI myAI;
        private int currPlay;

        void Start()
        {
            // Temporary
            // .....let the list of allgamerNums is the range of 0 to 7 (!!!!!!!!)
            allgamerNums = Enumerable.Range(0, 7).ToList(); // List of all players
            allplayerNums = Enumerable.Range(1, 1).ToList(); // List of human players

            myCS = gameObject.GetComponent<ControlScript>();
            myAI = gameObject.GetComponent<EnemyAI>();
            myCAGS = gameObject.GetComponent<ControlArmyGrowthScript>();
            myCS.CurrPlayerNum = allplayerNums[0];

            currPlay = -1;
        }

        public void SetAllPlayersNum(int num)
        {
            if (num <= 7)
            {
                allgamerNums.Clear();
                allgamerNums = Enumerable.Range(0, num).ToList();
            }
        }

        public int AddHumanPlayer()
        {
            int plr = 0;

            bool tryNext = true;

            while (tryNext)
            {
                plr = Random.Range(0, allgamerNums.Count);
                if (!allplayerNums.Contains(plr))
                {
                    allplayerNums.Add(plr);
                    tryNext = false;
                }
            }

            return plr;
        }

        public void RemoveHumanPlayer()
        {
            if (allplayerNums.Count > 1)
            {
                allplayerNums.RemoveAt(allplayerNums.Count - 1);
            }
        }

        public int ChangeHumanPlayer(int plNum)
        {
            int plr = 0;
            bool tryNext = true;

            if (allplayerNums.Count == allgamerNums.Count)
            {
                return -1;
            }

            while (tryNext)
            {
                plr = Random.Range(0, allgamerNums.Count);
                if (!allplayerNums.Contains(plr))
                {
                    allplayerNums[plNum] = plr;
                    tryNext = false;
                }
            }

            return plr;
        }

        public void ActionIteration()
        {
            int terrNum = 0;
            terrNum = Enumerable.Range(0, myCS.GetRM.GetAccRegions.Count)
                    .Where(x => myCS.GetRM.GetAccRegions[x].myPlayer == currPlay)
                    .ToList().Count;

            if (terrNum > 0)
            {
                myCS.CanTilePicking = false;
                int ind = allplayerNums.FindIndex(x => x == allgamerNums[currPlay]);
                if (ind == -1)
                {
                    // Block in ControlScript ability to click on tilemap
                    myCS.CanTilePicking = false;
                    // Invoke EnemyAI
                    myAI.EnemyPlayerAttacks(allgamerNums[currPlay]);
                }
                else
                {
                    // Deblock in ControlScript ability to click on tilemap
                    myCS.CanTilePicking = true;
                    myCS.CurrPlayerNum = allplayerNums[ind];
                }
            }
            else
            {
                GoAhead();
            }

            
        }

        public void GoAhead()
        {  
            StartCoroutine(BetweenPause()); // Maybe here we can invoke drawing of dice stashing
        }

        IEnumerator BetweenPause()
        {
            // Check if player have territories yet
            int terrNum = 0;
            if (currPlay != -1)
            {
                terrNum = Enumerable.Range(0, myCS.GetRM.GetAccRegions.Count)
                    .Where(x => myCS.GetRM.GetAccRegions[x].myPlayer == currPlay)
                    .ToList().Count;
            }

            if (currPlay != -1 && terrNum > 0)
            {
                // Вставить анимацию увеличения числа кубиков
                yield return StartCoroutine(myCAGS.ConsequenceArmyIncrease(currPlay));
            }
            currPlay += 1;
            currPlay = currPlay % allgamerNums.Count;

            yield return new WaitForSeconds(0.5f);
            ActionIteration();
                        
        }

    }
}
