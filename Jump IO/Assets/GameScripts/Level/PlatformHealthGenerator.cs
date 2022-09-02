using System;
using System.Collections.Generic;
using System.Linq;
using GameScripts.Misc;

namespace GameScripts.Level
{
    public class PlatformHealthGenerator
    {
        private readonly List<int> _hpAmount;
        private readonly List<WeightsPreset> _weightsPresets;

        public PlatformHealthGenerator(List<WeightsPreset> weightsPresets, List<int> hpAmount)
        {
            _weightsPresets = weightsPresets;
            _hpAmount = hpAmount;
        }

        public int GenerateHealth(float height)
        {
            var weights = _weightsPresets.Where(x => x.startingHeight <= height)
                .OrderByDescending(x => x.startingHeight)
                .First()
                .weights;
            var randomIndex = weights.GetRandomWeightedIndex();
            return _hpAmount[randomIndex];
        }

        [Serializable]
        public class WeightsPreset
        {
            public int startingHeight;
            public List<int> weights;
        }
    }
}