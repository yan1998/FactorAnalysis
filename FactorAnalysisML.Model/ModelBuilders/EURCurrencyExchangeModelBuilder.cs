using FactorAnalysisML.Model.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FactorAnalysisML.Model.ModelBuilders
{
    public class EURCurrencyExchangeModelBuilder
    {
        private static MLContext mlContext = new MLContext(seed: 1);
        private static readonly string modelPath = @"EURCurrencyExchangeMLModel.zip";
        private static readonly string preparationModelPath = @"preparation_EURCurrencyExchangeMLModel.zip";

        public static void CreateModel(IEnumerable<CurrencyExchangeModelInput> data)
        {
            IDataView trainingDataView = mlContext.Data.LoadFromEnumerable(data);

            IEstimator<ITransformer> trainingPipeline = BuildTrainingPipeline(mlContext);

            ITransformer dataPrepTransformer = trainingPipeline.Fit(trainingDataView);

            IDataView transformedData = dataPrepTransformer.Transform(trainingDataView);

            var trainedModel = trainingPipeline.Fit(transformedData);

            SaveModel(mlContext, trainedModel, transformedData.Schema, dataPrepTransformer, trainingDataView.Schema);
        }

        public static void RetrainModel(IEnumerable<CurrencyExchangeModelInput> newData)
        {
            DataViewSchema dataPrepPipelineSchema, modelSchema;

            ITransformer dataPrepPipeline = mlContext.Model.Load(GetAbsolutePath(preparationModelPath), out dataPrepPipelineSchema);
            ITransformer trainedModel = mlContext.Model.Load(GetAbsolutePath(modelPath), out modelSchema);

            LinearRegressionModelParameters originalModelParameters =
                ((RegressionPredictionTransformer<object>)trainedModel).Model as LinearRegressionModelParameters;
        }

        private static IEstimator<ITransformer> BuildTrainingPipeline(MLContext mlContext)
        { 
            var dataProcessPipeline = mlContext.Transforms.Concatenate("Features", new[] { "CreditRate", "GDPIndicator", "ImportIndicator", "ExportIndicator", "InflationIndex" });
            IEstimator<ITransformer> trainer = mlContext.Regression.Trainers.LightGbm(labelColumnName: "ExchangeRateEUR", featureColumnName: "Features");
            //trainer = mlContext.Regression.Trainers.FastForest(labelColumnName: "ExchangeRateEUR", featureColumnName: "Features");
            return dataProcessPipeline.Append(trainer);
        }

        private static void SaveModel(MLContext mlContext, ITransformer mlModel, DataViewSchema modelSchema, ITransformer dataPrepModel, DataViewSchema prepDataSchema)
        {
            mlContext.Model.Save(dataPrepModel, prepDataSchema, GetAbsolutePath(preparationModelPath));

            mlContext.Model.Save(mlModel, modelSchema, GetAbsolutePath(modelPath));
        }

        private static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(AppDomain.CurrentDomain.BaseDirectory);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }
    }
}
