using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Or use TextMeshPro if you prefer

public class AIResponseManager : MonoBehaviour
{
    [Header("Config")]
    [Tooltip("JSON file containing AI responses (TextAsset).")]
    public TextAsset aiResponsesJson;

    [Header("UI")]
    [Tooltip("UI Text component where the AI response will appear.")]
    public Text uiText; // If using TextMeshPro, use TMP_Text instead.

    private AIResponseRoot _root;
    private Dictionary<string, AIContextGroup> _contextLookup;

    private void Awake()
    {
        LoadResponses();
    }

    private void LoadResponses()
    {
        if (aiResponsesJson == null)
        {
            Debug.LogError("AIResponseManager: No aiResponsesJson assigned.");
            return;
        }

        _root = JsonUtility.FromJson<AIResponseRoot>(aiResponsesJson.text);
        _contextLookup = new Dictionary<string, AIContextGroup>();

        if (_root?.contexts == null)
        {
            Debug.LogError("AIResponseManager: No contexts found in JSON.");
            return;
        }

        foreach (var ctx in _root.contexts)
        {
            string key = MakeKey(ctx.room, ctx.trigger);
            if (!_contextLookup.ContainsKey(key))
            {
                _contextLookup.Add(key, ctx);
            }
            else
            {
                Debug.LogWarning($"AIResponseManager: Duplicate context key {key}.");
            }
        }

        Debug.Log($"AIResponseManager: Loaded {_contextLookup.Count} AI contexts.");
    }

    private string MakeKey(string room, string trigger)
    {
        return $"{room}:{trigger}";
    }

    /// <summary>
    /// Returns a random response text for the given room + trigger.
    /// </summary>
    public string GetRandomResponse(string room, string trigger)
    {
        if (_contextLookup == null)
        {
            Debug.LogWarning("AIResponseManager: Context lookup not initialized.");
            return null;
        }

        string key = MakeKey(room, trigger);
        if (!_contextLookup.TryGetValue(key, out var ctx) || ctx.responses == null || ctx.responses.Length == 0)
        {
            Debug.LogWarning($"AIResponseManager: No responses for {key}.");
            return null;
        }

        int index = Random.Range(0, ctx.responses.Length);
        return ctx.responses[index].text;
    }

    /// <summary>
    /// Picks a random response for the given room + trigger and displays it in the UI.
    /// </summary>
    public void ShowResponse(string room, string trigger)
    {
        string text = GetRandomResponse(room, trigger);
        if (uiText != null && !string.IsNullOrEmpty(text))
        {
            uiText.text = text;
        }
    }
}