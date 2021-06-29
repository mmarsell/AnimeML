using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Predictor
{
	private string predKey;
	private string iterationName;
	private string endP;
	private string projId;
	private Dictionary<string, double> results;

	public Predictor(string predict, string iter, string ender, string id)
	{
		predKey = predict;
		iterationName = iter;
		endP = ender;
		projId = id;
	}

	public void SetupPrediction(MemoryStream image)
    {
		CustomVisionPredictionClient endpoint = new CustomVisionPredictionClient(new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.ApiKeyServiceClientCredentials(predKey))
		{
			Endpoint = endP
		};

		var pred = endpoint.ClassifyImage(Guid.Parse(projId), iterationName, image);
		results = new Dictionary<string, double>(); 
		foreach (var p in pred.Predictions)
        {
			results[p.TagName] = p.Probability * 100;
        }

		var maxVal = results.Aggregate((curr, next) => curr.Value > next.Value ? curr : next).Key;
		Console.WriteLine("You are watching " + maxVal + " with confidence " + results[maxVal]);
    }


}
