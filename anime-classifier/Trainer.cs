using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace anime_classifier
{
    class Trainer
    {
        private string trainingKey;
        private string ENDPOINT;
        private string projId;
        private List<string> narutoImages;
        private List<string> dragonImages;
        private List<string> noteImages;
        private List<string> pokemonImages;
        private List<string> pieceImages;


        public Trainer(string train, string ender, string projectId)
        {
            trainingKey = train;
            ENDPOINT = ender;
            projId = projectId;
        }
        public void SetupClassifier()
        {
            CustomVisionTrainingClient trainer = new CustomVisionTrainingClient(new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.ApiKeyServiceClientCredentials(trainingKey))
            {
                Endpoint = ENDPOINT
            };

            Guid parseId = Guid.Parse(projId);
            var naruto = trainer.CreateTag(parseId, "Naruto");
            var dragon = trainer.CreateTag(parseId, "Dragon Ball");
            var note = trainer.CreateTag(parseId, "Death Note");
            var piece = trainer.CreateTag(parseId, "One Piece");
            var pokemon = trainer.CreateTag(parseId, "Pokemon");

            PrepareImages();
            foreach (var image in narutoImages)
            {
                using (var stream = new MemoryStream(File.ReadAllBytes(image)))
                {
                    trainer.CreateImagesFromData(parseId, stream, new List<Guid>() {naruto.Id});
                }
            }

            foreach (var image in dragonImages)
            {
                using (var stream = new MemoryStream(File.ReadAllBytes(image)))
                {
                    trainer.CreateImagesFromData(parseId, stream, new List<Guid>() {dragon.Id });
                }
            }
            foreach (var image in noteImages)
            {
                using (var stream = new MemoryStream(File.ReadAllBytes(image)))
                {
                    trainer.CreateImagesFromData(parseId, stream, new List<Guid>() { note.Id });
                }
            }
            foreach (var image in pieceImages)
            {
                using (var stream = new MemoryStream(File.ReadAllBytes(image)))
                {
                    trainer.CreateImagesFromData(parseId, stream, new List<Guid>() { piece.Id });
                }
            }
            foreach (var image in pokemonImages)
            {
                using (var stream = new MemoryStream(File.ReadAllBytes(image)))
                {
                    trainer.CreateImagesFromData(parseId, stream, new List<Guid>() { pokemon.Id });
                }
            }
            Console.WriteLine("Uploading images");


        }

        private void PrepareImages()
        {
            narutoImages = Directory.GetFiles(Path.Combine("Images", "naruto")).ToList();
            dragonImages = Directory.GetFiles(Path.Combine("Images", "dragon-ball")).ToList();
            noteImages = Directory.GetFiles(Path.Combine("Images", "death-note")).ToList();
            pokemonImages = Directory.GetFiles(Path.Combine("Images", "pokemon")).ToList();
            pieceImages = Directory.GetFiles(Path.Combine("Images", "one-piece")).ToList();

        }

        public void Train(int budget)
        {
            CustomVisionTrainingClient trainer = new CustomVisionTrainingClient(new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.ApiKeyServiceClientCredentials(trainingKey))
            {
                Endpoint = ENDPOINT
            };
            Console.WriteLine("Model has begun training");
            var iter = trainer.TrainProject(Guid.Parse(projId), "Advanced", budget);
            while (iter.Status == "Training")
            {
                Console.WriteLine("Currently training");
                Thread.Sleep(600000);                //ping every 10 min
                iter = trainer.GetIteration(Guid.Parse(projId), iter.Id);
            }

        }

    }
}
