using System;
using System.IO;
using System.Threading;
using anime_classifier;
public class Manager
{
    static void Main(string[] args)
    {
        string projId = "projectId";
        string endpoint = "endpoint";
        string predKey = "predictionKey";
        string trainKey = "trainingKey";
        string iterName = "iteration name";
        

        //Trainer trainTest = new Trainer(trainKey, endpoint, projId);
        //trainTest.Train(2)
        var folder = "folderPath";
        Predictor predictor = new Predictor(predKey, iterName, endpoint, projId);
        while(true)
        {
            Thread.Sleep(3000);
            var files = Directory.GetFiles(folder);
            foreach (var file in files)
            {
                MemoryStream img = new MemoryStream(File.ReadAllBytes(file));
                predictor.SetupPrediction(img);
                File.Delete(file);
            }
        }
    }
}
