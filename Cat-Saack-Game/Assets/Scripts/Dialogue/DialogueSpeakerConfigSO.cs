using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;


[CreateAssetMenu(fileName = "NewSpeakerConfig", menuName = "ScriptableObjects/SpeakerConfig")]
public class DialogueSpeakerConfigSO : ScriptableObject
{
    public string id = "default";

    [Header("Speaker Info")]
    public string speakerName = "";
    public UnityEngine.U2D.Animation.SpriteLibraryAsset portraitSpriteLibrary;


    [Header("Audio")]
    public AudioClip voiceClip;
    [Tooltip("Stop the previous audio clip before playing a new one")]
    public bool stopAudioSource;
    [Range(1,5)] public int textAudioPlayRate = 2;
    [Range(-3,3)] public float minPitch = 0.8f;
    [Range(-3,3)] public float maxPitch = 1.2f;
}
