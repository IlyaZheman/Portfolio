using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScripts.Misc
{
    public static class ExtensionMethods
    {
        public static void DestroyAllChildren(this Transform transform)
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(transform.GetChild(i).gameObject);
            }

            transform.DetachChildren();
        }

        public static int GetRandomWeightedIndex(this List<int> weights)
        {
            var totalSum = weights.Sum(i => i > 0 ? i : 0);

            var index = 0;
            var lastIndex = weights.Count - 1;
            while (index < lastIndex)
            {
                if (Random.Range(0, totalSum) < weights[index])
                {
                    return index;
                }

                totalSum -= weights[index++];
            }

            return index;
        }
    }
}