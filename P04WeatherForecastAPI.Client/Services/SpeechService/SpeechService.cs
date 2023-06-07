using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P04WeatherForecastAPI.Client.Services.SpeechService
{
    public class SpeechService : ISpeechService
    {

        // Microsoft.CognitiveServices.Speech
        public async Task<string> RecognizeAsync()
        {
            string apiKey = "54d6b06f171346fdba73204de336869c";
            var conf =SpeechConfig.FromSubscription(apiKey, "northeurope");
            return await RecognizeFromMic(conf);
        }

        private async Task<string> RecognizeFromMic(SpeechConfig speechConfig)
        {
            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            using var recognizer = new SpeechRecognizer(speechConfig, "pl-PL", audioConfig);

            var result = await recognizer.RecognizeOnceAsync();
            return result.Text;
        }
    }
}
