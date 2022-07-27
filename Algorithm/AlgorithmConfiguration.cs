namespace Million_lords_Helper.Algorithm;

public class AlgorithmConfiguration
{
    public AlgorithmConfiguration(double initialValue, double percent)
    {
        InitialValue = initialValue;
        Percent = percent;
    }

    public double InitialValue { get; }
    public double Percent { get; }
}