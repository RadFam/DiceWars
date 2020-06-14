using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameControls
{
    public class ClashScript : MonoBehaviour
    {
        private ControlScript csControl;

        // Start is called before the first frame update
        void Start()
        {
            csControl = gameObject.GetComponent<ControlScript>();
        }

        public bool OnClash(Vector3Int attacker, Vector3Int defender)
        {
            bool victory = false;

            List<int> attackerResult = new List<int>();
            List<int> defenderResult = new List<int>();

            if (csControl.GetRM.GetRegionByCoords(attacker).GetArmyUnit(ControlScript.ArmyTypes.Dice_d6) != null)
            {
                attackerResult = csControl.GetRM.GetRegionByCoords(attacker).GetArmyUnit(ControlScript.ArmyTypes.Dice_d6).GetOnceDamage();
            }

            if (csControl.GetRM.GetRegionByCoords(defender).GetArmyUnit(ControlScript.ArmyTypes.Dice_d6) != null)
            {
                defenderResult = csControl.GetRM.GetRegionByCoords(defender).GetArmyUnit(ControlScript.ArmyTypes.Dice_d6).GetOnceDamage();
            }

            int attackVal = attackerResult.Sum();
            int defendVal = defenderResult.Sum();

            Debug.Log("Attack val: " + attackVal.ToString() + "  defend val: " + defendVal.ToString());

            victory = attackVal > defendVal;

            // Переносми данные в стек атаки в CommonControl
            CommonControl.instance.battleStack.playerA_num = csControl.GetRM.GetRegionByCoords(attacker).myPlayer;
            CommonControl.instance.battleStack.playerA_sum_1 = attackVal;
            CommonControl.instance.battleStack.playerA_res_1 = attackerResult;
            CommonControl.instance.battleStack.playerB_num = csControl.GetRM.GetRegionByCoords(defender).myPlayer;
            CommonControl.instance.battleStack.playerB_sum_1 = defendVal;
            CommonControl.instance.battleStack.playerB_res_1 = defenderResult;

            // Также запускаем визуализацию процесса атаки (или же это сделать в КонтроллерСкрипте?)

            return victory;
        }
    }
}
