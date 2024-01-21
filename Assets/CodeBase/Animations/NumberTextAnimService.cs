using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CodeBase.Animations
{
    public class NumberTextAnimService
    {
        public async UniTask AnimateNumber(int startValue, int targetValue, float duration, TMP_Text targetText)
        {
            await DOTween.To(() => startValue, x => targetText.text = x.ToString(), targetValue, duration)
                .SetEase(Ease.Linear).AsyncWaitForCompletion().AsUniTask();
        }

        public async UniTask AnimateNumber(int startValue, int targetValue, float duration, TMP_Text targetText,
            AudioSource increaseSound)
        {
            increaseSound.volume = 1;
            increaseSound.Play();
            await DOTween.To(() => startValue, x => targetText.text = x.ToString(), targetValue, duration)
                .SetEase(Ease.Linear).AsyncWaitForCompletion().AsUniTask();
            await increaseSound.DOFade(0, 0.2f).OnComplete(increaseSound.Stop).AsyncWaitForCompletion().AsUniTask();
        }
        
        public async UniTask AnimateNumber(int startValue, int targetValue, float duration, TMP_Text targetText,
            AudioSource increaseSound, bool checkTargetValueOnNull)
        {
            if (checkTargetValueOnNull && targetValue == 0)
            {
                DOTween.To(() => startValue, x => targetText.text = x.ToString(), targetValue, 0f)
                    .SetEase(Ease.Linear);
                return;
            }
            
            increaseSound.volume = 1;
            increaseSound.Play();
            await DOTween.To(() => startValue, x => targetText.text = x.ToString(), targetValue, duration)
                .SetEase(Ease.Linear).AsyncWaitForCompletion().AsUniTask();
            await increaseSound.DOFade(0, 0.2f).OnComplete(increaseSound.Stop).AsyncWaitForCompletion().AsUniTask();
        }

        public async UniTask AnimateNumber(int startValue, int targetValue, float duration, TMP_Text targetText,
            char symbol)
        {
            await DOTween.To(() => startValue, x => targetText.text = $"{x}{symbol}", targetValue, duration)
                .SetEase(Ease.Linear).AsyncWaitForCompletion().AsUniTask();
        }

        public async UniTask AnimateNumber(int startValue, int targetValue, float duration, TMP_Text targetText,
            char symbol, AudioSource increaseSound)
        {
            increaseSound.volume = 1;
            increaseSound.Play();
            await DOTween.To(() => startValue, x => targetText.text = $"{x}{symbol}", targetValue, duration)
                .SetEase(Ease.Linear).AsyncWaitForCompletion().AsUniTask();
            await increaseSound.DOFade(0, 0.2f).OnComplete(increaseSound.Stop).AsyncWaitForCompletion().AsUniTask();
        }
    }
}