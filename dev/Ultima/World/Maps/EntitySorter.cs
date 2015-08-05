/***************************************************************************
 *   EntitySorter.cs
 *   Copyright (c) 2015 UltimaXNA Development Team
 *   
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 3 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/
#region usings
using System.Collections.Generic;
using UltimaXNA.Ultima.World.Entities;
#endregion

namespace UltimaXNA.Ultima.World.Maps
{
    static class EntitySorter
    {
        public static void Sort(List<AEntity> items)
        {
            for (int i = 0; i < items.Count - 1; i++)
            {
                int j = i + 1;

                while (j > 0)
                {
                    int result = Compare(items[j - 1], items[j]);
                    if (result > 0)
                    {
                        AEntity temp = items[j - 1];
                        items[j - 1] = items[j];
                        items[j] = temp;

                    }
                    j--;
                }
            }
        }

        private static int Compare(AEntity a, AEntity b)
        {
            int aZ = a.SortZ + a.SortThreshold;
            int bZ = b.SortZ + b.SortThreshold;

            int sort = aZ - bZ;
            if (sort == 0)
            {
                sort = a.SortType - b.SortType;
            }
            if (sort == 0)
            {
                sort = a.SortThreshold - b.SortThreshold;
            }
            if (sort == 0)
            {
                sort = a.SortTiebreaker - b.SortTiebreaker;
            }
            return sort;
        }
    }
}
