namespace CodeBase.Services.Pause
{
    public class PauseService : IPauseService
    {
        public void Stop()
        {
            UnityEngine.Time.timeScale = 0f;
        }

        public void Run()
        {
            UnityEngine.Time.timeScale = 1f;
        }
    }
}