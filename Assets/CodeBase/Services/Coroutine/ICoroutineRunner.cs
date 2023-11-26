using System.Collections;

namespace CodeBase.Services.Coroutine
{
    public interface ICoroutineRunner
    {
        UnityEngine.Coroutine StartCoroutine(IEnumerator enumerator);
        void StopCoroutine(IEnumerator enumerator);
    }
}