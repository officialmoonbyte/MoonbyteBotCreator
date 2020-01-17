using System;
using System.Diagnostics;
using System.Globalization;
using System.Speech.Recognition;

namespace Moonbyte.BotCreator
{
    public class BotCreationAPI
    {

        #region EventArgs

        // Eventhandler used to pass the SpeechRecognizedEventArgs, used so that the programer can start
        // reading the user's microphone and pass that on to google.
        public EventHandler<SpeechRecognizedEventArgs> BotVoiceActivated;

        #endregion EventArgs

        #region Initialization

        /// <summary>
        /// If the botName is provided, uses System.Speech.Recognition lib to detect if the user
        /// is trying to interact with the bot
        /// e.g if the user says Hey "BotName" it will invoke the BotVoiceActivated script
        /// 
        /// The reason why we don't constantly have the Python script running, even though it is 
        /// more accurate then the System.Speech.Recognition lib is because all of that information
        /// is moved onto the google servers and we don't know if they are storing that information.
        /// 
        /// So we don't want people's information getting stored by google
        /// 
        /// f**k u google, please don't store my users private data. We take that data super seriously.
        /// </summary>
        /// <param name="botName">If the bot name is used, creates a SpeechRecognitionEngine service that detects if the user is trying to interact with the bot. </param>
        public BotCreationAPI(string botName = null)
        {
            if (botName != null)
            {
                using (SpeechRecognitionEngine recognizerEngine = new SpeechRecognitionEngine(new CultureInfo("en-US")))
                {
                    // Create and load a dictation grammar.  
                    recognizerEngine.LoadGrammar(new Grammar(new GrammarBuilder(new Choices(new string[] { "Hey " + botName, "Okay " + botName, "Hello " + botName }))));

                    // Add a handler for the speech recognized event.  
                    recognizerEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizerEngine_SpeechRecognized);

                    // Configure input to the speech recognizer.  
                    recognizerEngine.SetInputToDefaultAudioDevice();

                    // Start asynchronous, continuous speech recognition.  
                    recognizerEngine.RecognizeAsync(RecognizeMode.Multiple);
                }
            }
        }

        #endregion Initialization

        #region recognizerEngine_SpeechRecognized

        /// <summary>
        /// Invokes when the System.Speech detects that it's trying to talk to the bot. 
        /// Invokes the BotVoiceActivated EventHandler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void recognizerEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        { BotVoiceActivated?.Invoke(sender, e); }

        #endregion recognizerEngine_SpeechRecognized

        #region DetectSpeechCommand

        /// <summary>
        /// Loads the python script "voiceRecognition.py" that should be in the root directory of your application.
        /// This python script detects any input in the microphone, relays that information to google servers and
        /// then google servers return a http post request returning what it thinks the user said. Most of the time
        /// this is completely accurate in testing.
        /// </summary>
        /// <returns></returns>
        public string DetectSpeechCommand()
        {
            // Launches a new CMD process to execute the python script
            Process cmdProcess = new Process();
            cmdProcess.StartInfo.FileName = "cmd.exe";
            cmdProcess.StartInfo.RedirectStandardInput = true;
            cmdProcess.StartInfo.RedirectStandardOutput = true;
            cmdProcess.StartInfo.CreateNoWindow = true;
            cmdProcess.StartInfo.UseShellExecute = false;
            cmdProcess.Start();

            //Launches the python script
            cmdProcess.StandardInput.WriteLine("Python voiceRecognition.py");
            cmdProcess.StandardInput.Flush();
            cmdProcess.StandardInput.Close();
            cmdProcess.WaitForExit();

            //Returns the python script result. All data relayed to this python script may or may
            //not be stored on google servers, google seems like they are hiding it.

            //This doesn't use Google's cloud voice activation, but online is required to use this
            //lib so idk
            return cmdProcess.StandardOutput.ReadToEnd();
        }

        #endregion DetectSpeechCommand
    }
}
