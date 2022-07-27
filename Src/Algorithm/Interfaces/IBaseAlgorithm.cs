namespace Million_lords_Helper.Algorithm.Interfaces;

public interface IBaseAlgorithm
{
    double[] ResultArray { get; }
    double[] IncrementResultArray { get; }

    double GetResult(long level);

    double GetIncrementResult(long level);
}