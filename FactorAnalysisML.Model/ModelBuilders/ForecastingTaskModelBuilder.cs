using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FactorAnalysisML.Model.ModelBuilders
{
    public class ForecastingTaskModelBuilder
    {
        private static MLContext mlContext = new MLContext(seed: 1);

        public static void CreateModel(dynamic data, string entityName, IEnumerable<string> factorNames, string predicatedValueName)
        {
            IDataView trainingDataView = mlContext.Data.LoadFromEnumerable(data);

            IEstimator<ITransformer> trainingPipeline = BuildTrainingPipeline(mlContext, factorNames, predicatedValueName);

            ITransformer dataPrepTransformer = trainingPipeline
                .Fit(trainingDataView);

            IDataView transformedData = dataPrepTransformer.Transform(trainingDataView);

            var trainedModel = trainingPipeline.Fit(transformedData);

            SaveModel(mlContext, trainedModel, transformedData.Schema, dataPrepTransformer, trainingDataView.Schema, entityName);
        }

        //public static void RetrainModel(IEnumerable<CurrencyExchangeModelInput> newData)
        //{
        //    DataViewSchema dataPrepPipelineSchema, modelSchema;

        //    ITransformer dataPrepPipeline = mlContext.Model.Load(GetAbsolutePath(preparationModelPath), out dataPrepPipelineSchema);
        //    ITransformer trainedModel = mlContext.Model.Load(GetAbsolutePath(modelPath), out modelSchema);

        //    LinearRegressionModelParameters originalModelParameters =
        //        ((RegressionPredictionTransformer<object>)trainedModel).Model as LinearRegressionModelParameters;
        //}

        private static IEstimator<ITransformer> BuildTrainingPipeline(MLContext mlContext, IEnumerable<string> factorNames, string predicatedValueNam)
        {
            var dataProcessPipeline = mlContext.Transforms.Concatenate("Features", factorNames.ToArray());
            IEstimator<ITransformer> trainer = mlContext.Regression.Trainers.LightGbm(labelColumnName: predicatedValueNam, featureColumnName: "Features");
            return dataProcessPipeline.Append(trainer);
        }

        private static void SaveModel(MLContext mlContext, ITransformer mlModel, DataViewSchema modelSchema, ITransformer dataPrepModel, DataViewSchema prepDataSchema, string entityName)
        {
            var preparationModelPath = $"preparation_{entityName}MLModel.zip";
            mlContext.Model.Save(dataPrepModel, prepDataSchema, GetAbsolutePath(preparationModelPath));

            var modelPath = $"{entityName}MLModel.zip";
            mlContext.Model.Save(mlModel, modelSchema, GetAbsolutePath(modelPath));
        }

        private static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(AppDomain.CurrentDomain.BaseDirectory);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            return Path.Combine(assemblyFolderPath, relativePath);
        }
    }
}
