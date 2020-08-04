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

        public void OnEnemyClicked(int num) // num from 0 to 6
        {
            if (num + allHumans.Count < 8)
            {
                foreach (EnemyElementScript ees in allEnemies)
                {
                    if (ees.myNum != num)
                    {
                        ees.OnDeclick();
                    }
                }
                allEnemies[num].OnClick();
                maxEnemies = num + 1;
                // Еще тут нужно в контроллер передать, сколько теперь у нас будет игроков
                // .........

                CS.playersCount = num + 1 + allHumans.Count;
                CSoA.SetAllPlayersNum(num + 1 + allHumans.Count);
            }
            
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
                Debug.Log("Decrease players");
                CSoA.RemoveHumanPlayer();
                Destroy(allHumans[allHumans.Count - 1].gameObject); // Почему-то не удаляет из меню
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
