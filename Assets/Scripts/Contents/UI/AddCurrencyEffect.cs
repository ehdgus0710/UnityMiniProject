using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class AddCurrencyEffect : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    public TextMeshProUGUI Text {  get { return text; } }

    [SerializeField]
    private Vector3 direction;

    [SerializeField]
    private float distance;

    [SerializeField]
    private float time;

    [SerializeField]
    private Vector3 targetScale;

    [SerializeField]
    private Color targetColor;

    private Vector3 startScale;

    private UniTask currencyEffectUniTask;
    private IObjectPool<AddCurrencyEffect> addCurrencyEffectPool;

    private void Awake()
    {
        startScale = transform.localScale;
    }

    //private void Start()
    //{
    //    // StartCoroutine(CoEffect());
    //    currencyEffectUniTask = CoCurrencyEffect();
    //}

    private void OnDisable()
    {
        text.color = Color.white;
        transform.localScale = startScale;
    }

    public void StartEffect()
    {
        currencyEffectUniTask = CoCurrencyEffect();
    }

    private IEnumerator CoEffect()
    { 
        float currentTime = 0f;

        Vector3 position = transform.position;
        Vector3 endPosition = position + direction * distance;
        Vector3 scale = transform.localScale;
        Color color = text.color;

        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            var ratio = currentTime / time;
            transform.position = Vector2.Lerp(position, endPosition, ratio);
            transform.localScale = Vector2.Lerp(scale, targetScale, ratio);
            text.color = Color.Lerp(color, targetColor, ratio);
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }

    private async UniTask CoCurrencyEffect()
    {
        float currentTime = 0f;
        transform.localPosition = Vector3.zero;
        Vector3 position = transform.localPosition;
        Vector3 endPosition = position + direction * distance;
        Vector3 scale = transform.localScale;
        Color color = text.color;

        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            var ratio = currentTime / time;
            transform.localPosition = Vector2.Lerp(position, endPosition, ratio);
            transform.localScale = Vector2.Lerp(scale, targetScale, ratio);
            text.color = Color.Lerp(color, targetColor, ratio);
            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken:this.GetCancellationTokenOnDestroy());
        }

        DestroyUIDamageText();
    }

    public void SetPool(IObjectPool<AddCurrencyEffect> addCurrencyEffectPool)
    {
        this.addCurrencyEffectPool = addCurrencyEffectPool;
    }

    public IObjectPool<AddCurrencyEffect> GetObjectPool()
    {
        return addCurrencyEffectPool;
    }

    public void DestroyUIDamageText()
    {
        addCurrencyEffectPool.Release(this);
    }
}
