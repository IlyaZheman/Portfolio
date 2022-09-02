using System.Collections.Generic;
using GameScripts.Level;
using UnityEngine;

namespace GameScripts.Providers.Module
{
    [CreateAssetMenu(fileName = "LevelConfiguration", menuName = "Jump.io/Level Configuration", order = 0)]
    public class LevelConfiguration : ScriptableObject
    {
        public int levelHeight;
        public List<int> hpAmount;
        public List<PlatformHealthGenerator.WeightsPreset> weightsPresets;
    }
}