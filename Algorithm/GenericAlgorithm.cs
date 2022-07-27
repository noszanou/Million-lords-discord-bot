using Million_lords_Helper.Algorithm.Interfaces;
using Million_lords_Helper.Extension;

namespace Million_lords_Helper.Algorithm
{
    public class GenericAlgorithm : IWallDefense, IGoldUpgradeCost, IXpNeeded, IRewardsLevelUp, IProductionHourly
    {
        public double[] ResultArray { get; }
        public double[] IncrementResultArray { get; }

        public GenericAlgorithm(AlgorithmConfiguration config)
        {
            double baseValue = config.InitialValue;
            ResultArray = new double[AlgorithmConstance.MaxLevel];
            IncrementResultArray = new double[AlgorithmConstance.MaxLevel];
            ResultArray[0] = baseValue;
            double nextValue = baseValue.DataPercent(config.Percent);
            IncrementResultArray[0] = Math.Round(nextValue);
            for (var i = 1; i != ResultArray.Length; i++)
            {
                nextValue = baseValue.DataPercent(config.Percent);
                baseValue += nextValue;
                ResultArray[i] = Math.Round(baseValue);
                IncrementResultArray[i] = Math.Round(nextValue);
            }
        }

        public double GetResult(long level)
        {
            return ResultArray[level - 1];
        }

        public double GetIncrementResult(long level)
        {
            return IncrementResultArray[level];
        }
    }
}