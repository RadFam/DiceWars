using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControls;

namespace UIControls
{
    public class MenuTwoUIScript : MonoBehaviour
    {
        [SerializeField]
        private List<EnemyElementScript> allEnemies;
        [SerializeField]
        private List<HumanElementScript> allHumans;

        private ControlScript CS;
        private ControlSequenceOfActions CSoA;

        public HumanElementScript humanPrefab;
        public Transform humanParent;

        // Для контроля за тем, каких игроков мы выбираем
        private int maxEnemies = 1;

        // Start is called before the first frame update
        void Start()
        {
            CS = FindObjectOfType<ControlScript>();
            CSoA = FindObjectOfType<ControlSequenceOfActions>();

            allEnemies[0].OnClick();
        }

        public void OnEnemyClicked(int num) // num from 1 to 7
        {
            foreach (EnemyElementScript ees in allEnemies)
            {
                if (ees.myNum != num)
                {
                    ees.OnDeclick();
                }
            }
            maxEnemies = num;
            // Еще тут нужно в контроллер передать, сколько теперь у нас будет игроков
            // .........

            CS.playersCount = num + 1;
            CSoA.SetAllPlayersNum(num);
        }

        public void OnHumanPlayerIncrease()
        {
            if (maxEnemies + allHumans.Count < 8)
            {
                HumanElementScript hes = Instantiate(humanPrefab);
                hes.transform.parent = humanParent;

                int color = CSoA.AddHumanPlayer();
                Debug.Log("Color num is " + color.ToString());
                hes.SetInitials(allHumans.Count, color);

                allHumans.Add(hes);
            }
        }

        public void OnHumanPlayerDecrease()
        {
            if (allHumans.Count > 1)
            {
                CSoA.RemoveHumanPlayer();
                Destroy(allHumans[allHumans.Count - 1]);
                allHumans.RemoveAt(allHumans.Count - 1);
            }
        }

        public void OnHumanClicked(int num)
        {
            int color = CSoA.ChangeHumanPlayer(num);
            allHumans[num].SetInitials(num, color);
        }
    }
}
