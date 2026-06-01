using System.Media;

namespace Chatbot_WPF
{
    public class AudioPlayer
    {
        public void PlayGreeting()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("greeting.wav");
                player.PlaySync();
            }
            catch
            {
                // Ignore audio errors
            }
        }
    }
}