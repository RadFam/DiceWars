using System;

namespace CreatingGraph
{
    //территория
    class Region
    {

        int graphVertexNum; //номер соотв вершины в графе
        int dicesNum; //число кубиков на территории
        int playerNum; //номер игрока, которому принадлежит территория

        public Region(int graphVertexNum, int dicesNum, int playerNum)
        {
            this.graphVertexNum = graphVertexNum;
            this.dicesNum = dicesNum;
            this.playerNum = playerNum;
        }

        public override string ToString()
        {
            // "номер_вершины число_кубиков номер_игрока"
            return this.graphVertexNum.ToString() + ' ' 
                + this.dicesNum.ToString() + ' '
                + this.playerNum.ToString();
        }

        // метод изменящий число костей на поле на заданную величину
    }
}
