using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UIControls;

namespace GameControls
{
    public class ControlArmyGrowthScript : MonoBehaviour
    {
        ControlScript myCS;
        int currPlayer;
        List<int> playerRegionNumbers;
        List<bool> canAddList;

        // Start is called before the first frame update
        void Start()
        {
            myCS = gameObject.GetComponent<ControlScript>();
            playerRegionNumbers = new List<int>();
            canAddList = new List<bool>();
        }

        public void StartArmyIncrease(int playerNum)
        {
            currPlayer = playerNum;
            canAddList.Clear();
            playerRegionNumbers.Clear();
            playerRegionNumbers = Enumerable.Range(0, myCS.GetRM.GetAccRegions.Count)
                .Where(x => myCS.GetRM.GetAccRegions[x].myPlayer == currPlayer)
                .ToList();
            for (int i = 0; i < playerRegionNumbers.Count; ++i)
            {
                canAddList.Add(true);
                //Debug.Log("CURRENT PLAYER REGION: " + playerRegionNumbers[i].ToString());
            }

            StartCoroutine(ConsequenceArmyIncrease());
        }

        IEnumerator ConsequenceArmyIncrease()
        {
            // Get number of dices, that we need to add to player
            int dices = myCS.GetRM.GetPlayerMaxConnectedTerritorySize(currPlayer);
            Debug.Log("MAX added dices value is: " + dices.ToString());

            // Set this dices to the reserve
            CommonControl.instance.SetToReserve(dices, currPlayer);
            bool canAdd = true;
            int regNum = 0;

            while (canAdd)
            {
                //Debug.Log("Come into while cycle");
                if (CommonControl.instance.GetFromReserve(currPlayer) > 0)
                {
                    //Debug.Log("Come into if station 1");
                    if (ChooseRegionToAddDice(currPlayer, out regNum))
                    {
                        //Debug.Log("Chosen reg to add: " + regNum.ToString());
                        CommonControl.instance.SetToReserve(-1, currPlayer);
                        myCS.SubdrawRegion(regNum, false);
                    }
                    else
                    {
                        canAdd = false;
                    }
                }
                else
                {
                    canAdd = false;
                }

                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(0.1f);
        }

        public bool ChooseRegionToAddDice(int playerNum, out int regNum)
        {
            regNum = -1;

            for (int i = 0; i < playerRegionNumbers.Count; ++i)
            {
                canAddList[i] = true;
            }

            int cntr = 0;

            while (cntr < playerRegionNumbers.Count)
            {
                // Choose random number of region
                int index = Random.Range(0, playerRegionNumbers.Count + 1);
                if (canAddList[index] == true)
                {
                    // Add dice to the region playerRegionNumbers[index]
                    // Кость уже добавляется (!)
                    myCS.GetRM.GetAccRegions[playerRegionNumbers[index]].AddArmy(1, ControlScript.ArmyTypes.Dice_d6);

                    // Check if fullfiled
                    if (myCS.GetRM.GetAccRegions[playerRegionNumbers[index]].CheckArmyFullfilled(ControlScript.ArmyTypes.Dice_d6))
                    {
                        canAddList[index] = false;
                        cntr++;
                    }
                    else
                    {
                        //regNum = playerRegionNumbers[index];
                        regNum = myCS.GetRM.GetAccRegions[playerRegionNumbers[index]].RegNum;
                        break;
                    }
                }
            }

            return regNum > -1;
        }
    }
}
