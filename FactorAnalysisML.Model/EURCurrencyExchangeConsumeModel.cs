// This file was auto-generated by ML.NET Model Builder. 

using System;
using System.Collections.Immutable;
using FactorAnalysisML.Model.Models;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace FactorAnalysisML.Model
{
    public class EURCurrencyExchangeConsumeModel
    {
        // For more info on consuming ML.NET models, visit https://aka.ms/model-builder-consume
        // Method for consuming model in your app
        public static ModelOutput Predict(CurrencyExchangeModelInput input)
        {
            // Create new MLContext
            var mlContext = new MLContext();

            // Load model & create prediction engine
            var modelPath = AppDomain.CurrentDomain.BaseDirectory + "EURCurrencyExchangeMLModel.zip";
            var mlModel = mlContext.Model.Load(modelPath, out _);
            var predEngine = mlContext.Model.CreatePredictionEngine<CurrencyExchangeModelInput, ModelOutput>(mlModel);

            // Use model to make prediction on input data
            var result = predEngine.Predict(input);
            return result;
        }
    }
}
