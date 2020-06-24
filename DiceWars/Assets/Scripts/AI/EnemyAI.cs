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
        private ControlSequenceOfActions myCSoA;
        private int stageCntr;
        private int attackerReg;
        private int attackReg;

        private bool cannotFurtherAttack;
        private List<int> regionAccessPlayerIndicies;
        private List<int> regionEnemiesIndicies;
        private List<float> regionEnemiesArmiesSuccess;

        // Start is called before the first frame update
        void Start()
        {
            myCS = gameObject.GetComponent<ControlScript>();
            myCSoA = gameObject.GetComponent<ControlSequenceOfActions>();
            regionAccessPlayerIndicies = new List<int>();
            regionEnemiesIndicies = new List<int>();
            regionEnemiesArmiesSuccess = new List<float>();
        }

        public void EnemyPlayerAttacks(int numPlayer)
        {
            Debug.Log("Player " + numPlayer.ToString() + " will attack");

            // Get list fo regions that belongs to definite player
            regionAccessPlayerIndicies.Clear();
            regionEnemiesIndicies.Clear();
            regionEnemiesArmiesSuccess.Clear();
            cannotFurtherAttack = false;

            StartCoroutine(FullAttackCoroutine(numPlayer));

            /*
            while (!cannotFurtherAttack)
            {
                // indicies of acessRegions with regions that belongs to the player
                regionAccessPlayerIndicies.Clear();
                regionAccessPlayerIndicies = Enumerable.Range(0, myCS.GetRM.GetAccRegions.Count)
                    .Where(x => myCS.GetRM.GetAccRegions[x].myPlayer == numPlayer)
                    .ToList();

                if (regionAccessPlayerIndicies.Count == 0)
                {
                    cannotFurtherAttack = true;
                }

                foreach (int ind in regionAccessPlayerIndicies)
                {
                    Debug.Log("Region " + myCS.GetRM.GetAccRegions[ind].RegNum.ToString() + " search for chance to attack");

                    // Check if region has neighbours with other player
                    List<int> regNeib = myCS.GetRM.GetAccRegions[ind].RegBorder;
                    regionEnemiesArmiesSuccess.Clear();
                    regionEnemiesIndicies.Clear();

                    Debug.Log("Current territory: " + myCS.GetRM.GetAccRegions[ind].RegNum.ToString());
                    foreach (int neib in regNeib)
                    {
                        //Debug.Log("Neighbour: " + neib.ToString());
                    }

                    foreach (int neib in regNeib)
                    {
                        int indd = myCS.GetRM.GetAccRegions.FindIndex(x => x.RegNum == neib);
                        if (indd != -1)
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
                    }
                    if (regionEnemiesIndicies.Count != 0)
                    {
                        // Choose enemy to attack
                        
                        Debug.Log("Ready to attack");
                        float mx = regionEnemiesArmiesSuccess.Max();
                        int mInd = regionEnemiesArmiesSuccess.FindIndex(x => x == mx);

                        stageCntr = 0;
                        attackerReg = myCS.GetRM.GetAccRegions[ind].RegNum;
                        attackReg = regionEnemiesIndicies[mInd];

                        //AttackRegion(attackerReg, attackReg);
                        //StartCoroutine(AttackCoroutine(attackerReg, attackReg, 0.4f));
                    }
                }

                // tmp stub
                cannotFurtherAttack = true;
            }
            */
        }

        public float CheckWorthToAttack(int regNumA, int regNumD)
        {
            float worthAttack = 0.0f;

            Region regA = myCS.GetRM.GetAccRegions.Find(x => x.RegNum == regNumA);
            Region regD = myCS.GetRM.GetAccRegions.Find(x => x.RegNum == regNumD);

            int dice6_A = regA.myArmyOnRegion[0].myCount;
            int dice6_D = regD.myArmyOnRegion[0].myCount;

            if (dice6_A == 1)
            {
                return 0.0f;
            }

            worthAttack = ((float)dice6_A) / ((float)dice6_D);

            return worthAttack;
        }

        public void AttackRegion(int regionNumA, int regionNumD)
        {
            if (stageCntr == 0)
            {
                // Darken attack territory
                Debug.Log("Region " + regionNumA.ToString() + " attacks region " + regionNumD.ToString());
                myCS.DarkRegion(regionNumA);
                StartCoroutine(PauseCoroutine(0.4f));
            }
            
            if (stageCntr == 1)
            {
                // Darken defend territory
                myCS.DarkRegion(regionNumD);
                StartCoroutine(PauseCoroutine(0.4f));
            }
            
            if (stageCntr == 2)
            {
                // Undark both territories
                myCS.UndarkRegions();
                stageCntr = 0;
            }   
        }

        IEnumerator AttackCoroutine(int regionNumA, int regionNumD, float time)
        {
            Debug.Log("Region " + regionNumA.ToString() + " attacks region " + regionNumD.ToString());
            myCS.DarkRegion(regionNumA);
            yield return new WaitForSeconds(time);

            myCS.DarkRegion(regionNumD);
            yield return new WaitForSeconds(time);

            myCS.UndarkRegions();
            yield return new WaitForSeconds(time);
        }

        IEnumerator FullAttackCoroutine(int numPlayer)
        {
            int attackCounter = 0;

            while (!cannotFurtherAttack)
            {
                // indicies of acessRegions with regions that belongs to the player
                regionAccessPlayerIndicies.Clear();
                regionAccessPlayerIndicies = Enumerable.Range(0, myCS.GetRM.GetAccRegions.Count)
                    .Where(x => myCS.GetRM.GetAccRegions[x].myPlayer == numPlayer)
                    .ToList();

                if (regionAccessPlayerIndicies.Count == 0)
                {
                    cannotFurtherAttack = true;
                }

                attackCounter = 0;
                Debug.Log("STAGE FOR ATTACK");
                foreach (int ind in regionAccessPlayerIndicies)
                {
                    Debug.Log("Region " + myCS.GetRM.GetAccRegions[ind].RegNum.ToString() + " search for chance to attack");

                    // Check if region has neighbours with other player
                    List<int> regNeib = myCS.GetRM.GetAccRegions[ind].RegBorder;
                    regionEnemiesArmiesSuccess.Clear();
                    regionEnemiesIndicies.Clear();

                    foreach (int neib in regNeib)
                    {
                        int indd = myCS.GetRM.GetAccRegions.FindIndex(x => x.RegNum == neib);
                        if (indd != -1)
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
                    }
                    if (regionEnemiesIndicies.Count != 0)
                    {
                        // Choose enemy to attack
                        attackCounter++;

                        Debug.Log("Ready to attack");
                        float mx = regionEnemiesArmiesSuccess.Max();
                        int mInd = regionEnemiesArmiesSuccess.FindIndex(x => x == mx);

                        stageCntr = 0;
                        attackerReg = myCS.GetRM.GetAccRegions[ind].RegNum;
                        attackReg = regionEnemiesIndicies[mInd];

                        Debug.Log("Region " + attackerReg.ToString() + " attacks region " + attackReg.ToString());
                        myCS.DarkRegion(attackerReg);
                        yield return new WaitForSeconds(0.4f);

                        myCS.DarkRegion(attackReg);
                        yield return new WaitForSeconds(0.4f);

                        ClashScript CS = gameObject.GetComponent<ClashScript>();
                        bool res = CS.OnClash(myCS.GetRM.GetCoordByRegion(myCS.GetDarkenedRegions[0]), myCS.GetRM.GetCoordByRegion(myCS.GetDarkenedRegions[1]));
                        myCS.ChangeArmyDistribution(res);

                        myCS.UndarkRegions();
                        yield return new WaitForSeconds(0.4f);
                    }
                }

                if (attackCounter == 0)
                {
                    cannotFurtherAttack = true;
                }

                // tmp stub
                //cannotFurtherAttack = true;
            }

            myCSoA.GoAhead();
        }


        IEnumerator PauseCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
            stageCntr++;
            AttackRegion(attackerReg, attackReg);
        }

        public void AddProportionalArmy(int numPlayer)
        {
            // Get List of all Player Territories
            regionAccessPlayerIndicies.Clear();
            regionAccessPlayerIndicies = Enumerable.Range(0, myCS.GetRM.GetAccRegions.Count)
                .Where(x => myCS.GetRM.GetAccRegions[x].myPlayer == numPlayer)
                .ToList();

            if (regionAccessPlayerIndicies.Count > 0)
            {
                // Get number of connected territories

                // Add army to every territory
            }

        }
    }
}

