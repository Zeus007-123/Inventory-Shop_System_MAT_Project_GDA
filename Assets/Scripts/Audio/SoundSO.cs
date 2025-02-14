using System;
using UnityEngine;

/// <summary>
/// A ScriptableObject that stores a list of sound effects used in the game.
/// This allows easy management of sound assets without hardcoding them in scripts.
/// </summary>

[CreateAssetMenu(fileName = "SoundSO", menuName = "ScriptableObjects/SoundSO")]
public class SoundSO : ScriptableObject
{
    [Header("List of Sound Effects")]
    public Sounds[] audioList; // Array of all sound effects used in the game
}

/// <summary>
/// Struct that holds information about a specific sound.
/// </summary>

[Serializable]
public struct Sounds
{
    public SoundTypes soundType; // Type/category of the sound effect.
    public AudioClip audio; // The corresponding audio clip.
}
