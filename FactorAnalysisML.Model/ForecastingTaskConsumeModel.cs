using FactorAnalysisML.Model.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Linq;

namespace FactorAnalysisML.Model
{
    public class ForecastingTaskConsumeModel
    {
        // For more info on consuming ML.NET models, visit https://aka.ms/model-builder-consume
        // Method for consuming model in your app
        public static float Predict(dynamic input, string forecastingTaskName, Type type)
        {
            // Create new MLContext
            var mlContext = new MLContext();

            // Load model & create prediction engine
            var modelPath = AppDomain.CurrentDomain.BaseDirectory + $"{forecastingTaskName}MLModel.zip";
            var mlModel = mlContext.Model.Load(modelPath, out var schema);

            var method = mlContext.Model.GetType()
                .GetMethods()
                .Single(m => m.Name == "CreatePredictionEngine" && m.GetParameters().Length == 4)
                .MakeGenericMethod(type, typeof(ModelOutput));
            dynamic predEngine = method.Invoke(mlContext.Model, new object[] { mlModel, true, null, null });

            // Use model to make prediction on input data
            var result = predEngine.Predict(input);
            return (float)result.Score;
        }
    }
}
