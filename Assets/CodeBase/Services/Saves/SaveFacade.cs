namespace CodeBase.Services.Saves
{
    public class SaveFacade
    {
        private readonly SaveOnEveryMinute _saveOnEveryMinute;

        public SaveFacade(SaveOnEveryMinute saveOnEveryMinute)
        {
            _saveOnEveryMinute = saveOnEveryMinute;
        }

        public void InitServices()
        {
            _saveOnEveryMinute.Init();
        }
    }
}