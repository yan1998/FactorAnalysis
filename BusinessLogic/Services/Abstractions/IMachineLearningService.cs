using DomainModel.ForecastingTasks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Abstractions
{
    public interface IMachineLearningService
    {
        Task CreateForecastingTaskMLModel(string entityName, LearningAlgorithm learningAlgorithm, bool isValidationNeeded = true);

        Task<float> PredictValueByFactors(string entityName, List<ForecastingTaskFieldValue> factors, bool isValidationNeeded = true);

        Task<List<AlgorithmPredictionReport>> AnalyzePredictionAlgorithms(string entityName, List<LearningAlgorithm> algorithms);
    }
}
