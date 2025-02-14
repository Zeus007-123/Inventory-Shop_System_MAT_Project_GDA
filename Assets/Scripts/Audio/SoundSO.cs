using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundSO", menuName = "ScriptableObjects/SoundSO")]
public class SoundSO : ScriptableObject
{
    public Sounds[] audioList;
}

[Serializable]
public struct Sounds
{
    public SoundTypes soundType;
    public AudioClip audio;
}
