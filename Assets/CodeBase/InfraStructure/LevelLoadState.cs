using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.InfraStructure
{
    public class LevelLoadState : IState, IEnter
    {
        private ILoadingCurtain _loadingCurtain;

        public LevelLoadState(ILoadingCurtain loadingCurtain)
        {
            _loadingCurtain = loadingCurtain;
        }

        public async void Enter()
        {
            _loadingCurtain.Show(0);

            AsyncOperation asyncOperation =  SceneManager.LoadSceneAsync(sceneBuildIndex: 1);

            while (asyncOperation.isDone == false)
            {
                await UniTask.Yield();
            }
            
            _loadingCurtain.Hide();
        }
    }
}