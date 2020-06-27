using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameControls
{
    public struct ClashStack
    {
        public int playerA_num;
        public int playerA_sum_1;
        public int playerA_sum_2;
        public int playerA_sum_3;
        public List<int> playerA_res_1;
        public List<int> playerA_res_2;
        public List<int> playerA_res_3;

        public int playerB_num;
        public int playerB_sum_1;
        public int playerB_sum_2;
        public int playerB_sum_3;
        public List<int> playerB_res_1;
        public List<int> playerB_res_2;
        public List<int> playerB_res_3;

        public void SelfInitiate()
        {
            playerA_num = -1;
            playerA_sum_1 = 0;
            playerA_sum_2 = 0;
            playerA_sum_3 = 0;
            playerA_res_1 = new List<int>();
            playerA_res_2 = new List<int>();
            playerA_res_3 = new List<int>();

            playerB_num = -1;
            playerB_sum_1 = 0;
            playerB_sum_2 = 0;
            playerB_sum_3 = 0;
            playerB_res_1 = new List<int>();
            playerB_res_2 = new List<int>();
            playerB_res_3 = new List<int>();
        }

        public void SelfResert()
        {

        }
    }

    public class CommonControl : MonoBehaviour
    {
        public static CommonControl instance;
        public BattleUnitGraphics allGraphics;
        public ClashStack battleStack;

        List<int> playersReserve;

        // Start is called before the first frame update
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                battleStack.SelfInitiate();

                playersReserve = new List<int>();
                for (int i = 0; i < 8; ++i)
                {
                    playersReserve.Add(0);
                }
            }
        }

        public void SetToReserve(int value, int playerNum)
        {
            if (playerNum < 8)
            {
                playersReserve[playerNum] = Mathf.Max(playersReserve[playerNum] + value, 0);
            }
        }

        public int GetFromReserve(int playerNum)
        {
            return playersReserve[playerNum];
        }

        public void ResertStack()
        {
            battleStack.playerA_num = -1;
            battleStack.playerA_sum_1 = 0;
            battleStack.playerA_sum_2 = 0;
            battleStack.playerA_sum_3 = 0;
            battleStack.playerA_res_1.Clear();
            battleStack.playerA_res_2.Clear();
            battleStack.playerA_res_3.Clear();

            battleStack.playerB_num = -1;
            battleStack.playerB_sum_1 = 0;
            battleStack.playerB_sum_2 = 0;
            battleStack.playerB_sum_3 = 0;
            battleStack.playerB_res_1.Clear();
            battleStack.playerB_res_2.Clear();
            battleStack.playerB_res_3.Clear();
        }
    }
}
