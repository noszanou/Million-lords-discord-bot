using Million_lords_Helper.Enums;

namespace Million_lords_Helper.Algorithm;

public static class AlgorithmConstance
{
    public static readonly long MaxLevel = 2000;

    public static readonly Dictionary<AlgorithmDataType, AlgorithmConfiguration> Constance = new()
    {
        [AlgorithmDataType.XP] = new AlgorithmConfiguration(50, 30),
        [AlgorithmDataType.WallDefense] = new AlgorithmConfiguration(70, 20),
        [AlgorithmDataType.Rewards] = new AlgorithmConfiguration(50, 24.5), // is not really 24.5 but Yolo
        [AlgorithmDataType.UpgradeGoldsRequired] = new AlgorithmConfiguration(10, 20),
        [AlgorithmDataType.Production] = new AlgorithmConfiguration(20, 11.5),
    };
}