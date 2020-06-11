using System;

namespace CreatingGraph
{
    public class RegionsGraph
    {
        private int[,] matrix;//матрица смежности
        private Region[] regions;
        private int playersNum = 5;
        private int maxDicesNum = 8;

        public RegionsGraph()
        {
            this.СreateGraphMatrix();
            this.WriteMatrToFile();
            this.CreateRegionsList();
            this.WriteRegionsListToFile();
        }

        // создает матрицу смежности для графа
        public void СreateGraphMatrix()
        {
            matrix = new int[10, 10];

            //расставляем едицины рандомно, пропуская диагональные элементы
            for (int i = 0; i < 9; ++i)
            {
                for (int j = i + 1; j < 10; ++j)
                {
                    matrix[i, j] = new Random().Next(100) % 2;
                    matrix[j, i] = matrix[i, j];
                }
            }

        }

        private void WriteMatrToFile()
        {
            // матрицу запишем в файл
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter("matr.txt"))
            {
                for (int i = 0; i < 10; ++i)
                {
                    for (int j = 0; j < 10; ++j)
                    {
                        file.Write(matrix[i, j]);
                        file.Write(' ');
                    }
                    file.WriteLine();
                }
            }
        }

        public void CreateRegionsList()
        {
            int regionsNum = 10; //пока 10 регионов
            regions = new Region[regionsNum];
            for (int i = 0; i < 10; ++i)
            {
                int playerNum = new Random().Next(1, playersNum);//генерируем номер игрока
                int dicesNum = new Random().Next(1, maxDicesNum);//генерируем число кубиков
                regions[i] = new Region(i, dicesNum, playerNum);
            }
        }

        private void WriteRegionsListToFile()
        {
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter("regions_list.txt"))
            {
                for (int i = 0; i < 10; ++i)
                {
                    file.WriteLine(regions[i].ToString());
                }
            }
        }
    }
}