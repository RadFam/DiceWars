using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GameControls;
using RegionStructure;

namespace GameAI
{
    public class EnemyAI : MonoBehaviour
    {
        private ControlScript myCS;

        // Start is called before the first frame update
        void Start()
        {
            myCS = gameObject.GetComponent<ControlScript>();
        }

        public void EnemyPlayerAttacks(int numPlayer)
        {
            // Get list fo regions that belongs to definite player
            List<int> regionAccessPlayerIndicies = new List<int>();
            List<int> regionEnemiesIndicies = new List<int>();
            List<float> regionEnemiesArmiesSuccess = new List<float>();
            bool cannotFurtherAttack = false;

            while (cannotFurtherAttack)
            {
                // indicies of acessRegions with regions that belongs to the player
                regionAccessPlayerIndicies = Enumerable.Range(0, myCS.GetRM.GetAllRegions.Count)
                    .Where(x => myCS.GetRM.GetAccRegions[x].myPlayer == numPlayer)
                    .ToList();

                if (regionAccessPlayerIndicies.Count == 0)
                {
                    cannotFurtherAttack = true;
                }

                foreach (int ind in regionAccessPlayerIndicies)
                {
                    // Check if region has neighbours with other player
                    List<int> regNeib = myCS.GetRM.GetAccRegions[ind].RegBorder;
                    regionEnemiesArmiesSuccess.Clear();
                    regionEnemiesIndicies.Clear();
                    foreach (int neib in regNeib)
                    {
                        Region reg = myCS.GetRM.GetAccRegions.Find(x => x.RegNum == neib);
                        if (reg.myPlayer != numPlayer)
                        {
                            float success = CheckWorthToAttack(myCS.GetRM.GetAccRegions[ind].RegNum, reg.RegNum);
                            if (success >= 1.0f)
                            {
                                regionEnemiesArmiesSuccess.Add(success);
                                regionEnemiesIndicies.Add(reg.RegNum);
                            }
                        }
                    }
                    if (regionEnemiesIndicies.Count != 0)
                    {
                        // Choose enemy to attack
                        float mx = regionEnemiesArmiesSuccess.Max();
                        int mInd = regionEnemiesArmiesSuccess.FindIndex(x => x == mx);
                        AttackRegion(regionEnemiesIndicies[mInd]);
                    }
                }

            }
        }

        public float CheckWorthToAttack(int regNumA, int regNumD)
        {
            float worthAttack = 0.0f;

            Region regA = myCS.GetRM.GetAccRegions.Find(x => x.RegNum == regNumA);
            Region regD = myCS.GetRM.GetAccRegions.Find(x => x.RegNum == regNumD);

            int dice6_A = regA.myArmyOnRegion[0].myCount;
            int dice6_D = regD.myArmyOnRegion[0].myCount;

            worthAttack = ((float)dice6_A) / ((float)dice6_D);

            return worthAttack;
        }

        public void AttackRegion(int regionNum)
        {
            // Darken attack territory

            StartCoroutine(PauseCoroutine());
            // Darken defend territory
            StartCoroutine(PauseCoroutine());

            // Undark both territories
            myCS.UndarkRegions();
        }

        IEnumerator PauseCoroutine()
        {
            yield return new WaitForSeconds(.4f);
        }
    }
}

