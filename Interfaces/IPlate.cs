interface IPlate
{
    double getFintessFunction { get; }

    void addSegment(GeneticAlgorithms.Segment segment);

    int Size();

    void ShuffleSegments();

    void SetRandomDirectionsToSegments();

    double CalcSumIdlingLine();


}
