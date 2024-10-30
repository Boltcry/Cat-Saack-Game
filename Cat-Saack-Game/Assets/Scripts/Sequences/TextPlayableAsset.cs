using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using TMPro;

[System.Serializable]
public class TextPlayableAsset : PlayableAsset
{
    public string newText;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TextPlayableBehaviour>.Create(graph);
        playable.GetBehaviour().newText = newText;
        return playable;
    }
}

[System.Serializable]
public class TextPlayableBehaviour : PlayableBehaviour
{
    public string newText;

    // called each time playable is evaluated
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TextMeshProUGUI textMeshPro = playerData as TextMeshProUGUI;
        if (textMeshPro != null)
        {
            textMeshPro.text = newText;
        }
    }
}
