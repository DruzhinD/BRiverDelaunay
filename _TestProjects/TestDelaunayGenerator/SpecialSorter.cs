using CommonLib.Geometry;
using System;
using System.Collections.Generic;

namespace TestDelaunayGenerator
{
    public class SpecialSorter
    {
        private IHPoint[] sortingArray;
        IComparer<IHPoint> comparer;

        public SpecialSorter(IHPoint[] sortingArray, IComparer<IHPoint> comparer)
        {
            this.sortingArray = sortingArray;
            this.comparer = comparer;
        }

        public IHPoint[] GetSortedArray()
        {
            //сортируем
            Array.Sort(sortingArray, comparer);

            double lastYValue = double.MinValue;
            List<int> swappingIndexes = new List<int>();
            //поиск индексов для свапа интервалов в массиве
            for (int i = 0; i < sortingArray.Length; i++)
            {
                if (lastYValue > sortingArray[i].Y)
                    swappingIndexes.Add(i);
                lastYValue = sortingArray[i].Y;
            }
            //добавляем индекс последнего элемента
            swappingIndexes.Add(sortingArray.Length - 1);
            for (int i = 0; i < swappingIndexes.Count; i += 2)
            {
                if (swappingIndexes[i] == sortingArray.Length - 1)
                    continue;
                int arrayLeft = swappingIndexes[i];
                int arrayRight = swappingIndexes[i + 1] - 1;
                while(arrayRight >= arrayLeft)
                {
                    (sortingArray[arrayLeft], sortingArray[arrayRight]) = (sortingArray[arrayRight], sortingArray[arrayLeft]);
                    arrayLeft++;
                    arrayRight--;
                }
            }

            return sortingArray;
        }
    }
}
