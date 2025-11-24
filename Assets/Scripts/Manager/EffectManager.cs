using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }
    [System.Serializable]

    public class EffectData
    {
        public string effectName;
        public GameObject effectPrefads;
        public float defaultDuration = 2f;             //이펙트 지속 시간
    }
    [Header("Effect Type")]
    [SerializeField] private List<EffectData> effectList = new List<EffectData>();
    private Dictionary<string, EffectData> effectDictionary = new Dictionary<string, EffectData>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void InitilalizeDictionary()
    {
        effectDictionary.Clear();
        foreach (var effect in effectList)
        {
            if (!effectDictionary.ContainsKey(effect.effectName))
            {
                effectDictionary.Add(effect.effectName, effect);
            }
            else
            {
                Debug.Log($"중복된 이펙트 이름: {effect.effectName}");
            }
        }

    }
    public GameObject PlayEffect (string effectName, Vector3 position, Quaternion rotation)
    {
        if(effectDictionary.TryGetValue(effectName, out EffectData data))
        {
            GameObject effect = Instantiate(data.effectPrefads, position, rotation);
            Destroy(effect, data.defaultDuration);
            return effect;
        }
        else
        {
            Debug.LogWarning($"이펙트를 찾을 수 없습니다. : {effectName}");
            return null;
        }
    }
    public GameObject PlayEffect(string effectName, Vector3 position, Quaternion rotation, float duration)
    {
        if (effectDictionary.TryGetValue(effectName, out EffectData data))
        {
            GameObject effect = Instantiate(data.effectPrefads, position, rotation);
            Destroy(effect, duration);
            return effect;
        }
        else
        {
            Debug.LogWarning($"이펙트를 찾을 수 없습니다. : {effectName}");
            return null;
        }
    }
    public GameObject PlayEffect(string effectName, Vector3 position)
    {
        return PlayEffect(effectName, position, Quaternion.identity);
    }
    public GameObject PlayEffect(string effectName, Vector3 position, float duration)
    {
        return PlayEffect(effectName, position, Quaternion.identity, duration);
    }
    public void PlayEffectWithDelay(string effectName, Vector3 position, Quaternion rotation, float delay, float duration)
    {
        StartCoroutine(PlayEffectDelayed(effectName, position, rotation, delay, duration));
    }
    private IEnumerator PlayEffectDelayed(string effectName, Vector3 position, Quaternion rotation, float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        if(duration > 0)
        {
            PlayEffect(effectName, position, rotation, duration);
        }
        else
        {
            PlayEffect(effectName, position, rotation);
        }

    }
}
