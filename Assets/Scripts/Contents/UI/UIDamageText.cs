using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class UIDamageText : MonoBehaviour
{
    [SerializeField]
    private RectTransform target;

    [SerializeField]
    private Vector3 direction;

    [SerializeField]
    private float distance;

    [SerializeField]
    private float time;

    [SerializeField]
    private float targetScaleSize;

    [SerializeField]
    private Color targetColor;

    [SerializeField]
    private TextMeshProUGUI damageText;

    private UniTask uiDamageTask;
    private CancellationTokenSource uiDamageCoroutineSource = new();

    private IObjectPool<UIDamageText> uiDamageTextPool;

    private Vector3 position;
    private Vector3 endPosition;

    private Vector3 startScale;
    private Vector3 targetScale;

    private Vector3 scale;
    private Color color;
    private float currentTime;

    private void OnDisable()
    {
        damageText.color = color;
        target.localScale = startScale;
    }

    private void Awake()
    {
        color = damageText.color;
        startScale = transform.localScale;
        targetScale = startScale * targetScaleSize;
    }

    private void OnEnable()
    {
        scale = target.localScale;
        currentTime = 0f;
    }
    //private void Start()
    //{
    //    StartCoroutine(CoEffect());
    //}

    private void Update()
    {
        currentTime += Time.deltaTime;
        var ratio = currentTime / time;
        target.position = Vector3.Lerp(position, endPosition, ratio);
        target.localScale = Vector3.Lerp(scale, targetScale, ratio);
        damageText.color = Color.Lerp(color, targetColor, ratio);

        if(currentTime > time)
            DestroyUIDamageText();
    }

    #region 미사용 코드
    private async UniTask CoUIDamageEffect()
    {
        float currentTime = 0f;

        Vector3 position = target.position;
        Vector3 endPosition = target.position + direction * distance;
        Vector3 scale = target.localScale;
        Color color = damageText.color;

        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            var ratio = currentTime / time;
            target.position = Vector3.Lerp(position, endPosition, ratio);
            target.localScale = Vector3.Lerp(scale, targetScale, ratio);
            damageText.color = Color.Lerp(color, targetColor, ratio);
            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: this.GetCancellationTokenOnDestroy());
        }

        DestroyUIDamageText();
    }

    private IEnumerator CoEffect()
    {
        float currentTime = 0f;

        Vector3 position = target.position;
        Vector3 endPosition = target.position + direction * distance;
        Vector3 scale = target.localScale;
        Color color = damageText.color;

        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            var ratio = currentTime / time;
            target.position = Vector3.Lerp(position, endPosition, ratio);
            target.localScale = Vector3.Lerp(scale, targetScale, ratio);
            damageText.color = Color.Lerp(color, targetColor, ratio);
            yield return new WaitForEndOfFrame();
        }

        DestroyUIDamageText();
    }
    #endregion
    public void SetDamage(string damage)
    {
        damageText.text = damage;

        position = target.position;
        endPosition = target.position + direction * distance;
    }

    public void SetPool(IObjectPool<UIDamageText> uiDamageTextPool)
    {
        this.uiDamageTextPool = uiDamageTextPool;
    }

    public IObjectPool<UIDamageText> GetObjectPool()
    {
        return uiDamageTextPool;
    }

    public void DestroyUIDamageText()
    {
        uiDamageTextPool.Release(this);
    }
}
