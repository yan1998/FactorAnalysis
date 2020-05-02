using DomainModel.ForecastingTasks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Abstractions
{
    public interface IMachineLearningService
    {
        Task CreateForecastingTaskMLModel(string entityName, LearningAlgorithm learningAlgorith, bool isValidationNeeded = true);

        Task<float> PredictValue(string entityName, List<ForecastingTaskFieldValue> values, bool isValidationNeeded = true);

        Task<List<AlgorithmPredictionReport>> AnalyzePredictionAlgorithms(string entityName, List<LearningAlgorithm> algorithms);
    }
}
