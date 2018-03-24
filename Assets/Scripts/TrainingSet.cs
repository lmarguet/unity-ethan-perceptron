using System;

namespace Assets.Scripts
{
    [Serializable]
    public class TrainingSet
    {
        public double[] Input  { get; private set; }
        public double   Output { get; private set; }

        public TrainingSet(double[] input, double output)
        {
            Input = input;
            Output = output;
        }
    }
}