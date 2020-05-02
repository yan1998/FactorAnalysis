namespace DomainModel.ForecastingTasks
{
    public enum LearningAlgorithm
    {
        FastForest = 1,
        FastTree,
        FastTreeTweedie,
        Gam,
        LbfgsPoissonRegression,
        LightGbm,
        Sdca
    }
}
