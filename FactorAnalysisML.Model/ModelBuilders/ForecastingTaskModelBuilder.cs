using DomainModel.ForecastingTasks;
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

        public static void CreateModel(dynamic data, string entityName, IEnumerable<string> factorNames, string predicatedValueName, LearningAlgorithm algorithm)
        {
            IDataView trainingDataView = mlContext.Data.LoadFromEnumerable(data);

            IEstimator<ITransformer> trainingPipeline = BuildTrainingPipeline(mlContext, factorNames, predicatedValueName, algorithm);

            ITransformer dataPrepTransformer = trainingPipeline
                .Fit(trainingDataView);

            IDataView transformedData = dataPrepTransformer.Transform(trainingDataView);

            var trainedModel = trainingPipeline.Fit(transformedData);

            SaveModel(mlContext, trainedModel, transformedData.Schema, dataPrepTransformer, trainingDataView.Schema, entityName);
        }

        private static IEstimator<ITransformer> BuildTrainingPipeline(MLContext mlContext, IEnumerable<string> factorNames, string predicatedValueName, LearningAlgorithm algorithm)
        {
            var dataProcessPipeline = mlContext.Transforms.Concatenate("Features", factorNames.ToArray());
            IEstimator<ITransformer> trainer;
            switch (algorithm)
            {
                case LearningAlgorithm.FastForest:
                    trainer = mlContext.Regression.Trainers.FastForest(labelColumnName: predicatedValueName, featureColumnName: "Features");
                    break;
                case LearningAlgorithm.FastTree:
                    trainer = mlContext.Regression.Trainers.FastTree(labelColumnName: predicatedValueName, featureColumnName: "Features");
                    break;
                case LearningAlgorithm.FastTreeTweedie:
                    trainer = mlContext.Regression.Trainers.FastTreeTweedie(labelColumnName: predicatedValueName, featureColumnName: "Features");
                    break;
                case LearningAlgorithm.Gam:
                    trainer = mlContext.Regression.Trainers.Gam(labelColumnName: predicatedValueName, featureColumnName: "Features");
                    break;
                case LearningAlgorithm.LbfgsPoissonRegression:
                    trainer = mlContext.Regression.Trainers.LbfgsPoissonRegression(labelColumnName: predicatedValueName, featureColumnName: "Features");
                    break;
                case LearningAlgorithm.LightGbm:
                    trainer = mlContext.Regression.Trainers.LightGbm(labelColumnName: predicatedValueName, featureColumnName: "Features");
                    break;
                //case LearningAlgorithm.OnlineGradientDescent:
                //    trainer = mlContext.Regression.Trainers.OnlineGradientDescent(labelColumnName: predicatedValueName, featureColumnName: "Features");
                //    break;
                case LearningAlgorithm.Sdca:
                    trainer = mlContext.Regression.Trainers.Sdca(labelColumnName: predicatedValueName, featureColumnName: "Features");
                    break;
                default:
                    throw new Exception($"Algorithm {algorithm} was not implemented!");
            }
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
