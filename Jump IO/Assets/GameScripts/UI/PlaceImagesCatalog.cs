using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.UI
{
    [CreateAssetMenu(fileName = "PlaceImagesCatalog", menuName = "Jump.io/Place Images Catalog", order = 0)]
    public class PlaceImagesCatalog : ScriptableObject
    {
        [SerializeField] private List<Sprite> sprites;
        
        public Sprite GetSpriteByPlace(int place)
        {
            return sprites[place - 1];
        }
    }
}