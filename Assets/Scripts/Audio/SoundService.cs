using System;
using UnityEngine;

/// <summary>
/// The SoundService class handles playing sound effects based on in-game events.
/// It listens to various transaction-related events and plays appropriate sounds.
/// </summary>
public class SoundService
{
    private readonly SoundSO soundScriptableObject; // Reference to the sound data
    private readonly AudioSource audioEffects; // Audio source for playing sounds
    private EventService eventService; // Reference to the EventService

    // Event handlers for subscribing and unsubscribing events properly
    private Action<ItemSO, bool> _onItemSelectedHandler;
    private Action<ItemSO, int> _onBuyTransactionHandler;
    private Action<ItemSO, int> _onSellTransactionHandler;
    private Action<string> _onTransactionFailedHandler;

    /// <summary>
    /// Constructor to initialize the SoundService.
    /// </summary>
    /// <param name="soundScriptableObject">Scriptable object containing sound data.</param>
    /// <param name="audioEffectSource">AudioSource for playing sounds.</param>
    public SoundService(SoundSO soundScriptableObject, AudioSource audioEffectSource)
    {
        this.soundScriptableObject = soundScriptableObject;
        audioEffects = audioEffectSource;
    }

    /// <summary>
    /// Initializes the SoundService by subscribing to game events.
    /// </summary>
    /// <param name="eventService">EventService used for event-driven sound triggers.</param>
    public void Init(EventService eventService)
    {
        this.eventService = eventService;
        SubscribeToEvents();
        Debug.Log("[SoundService] Initialized and subscribed to events.");
    }

    /// <summary>
    /// Subscribes to relevant game events and assigns corresponding sound effects.
    /// </summary>
    private void SubscribeToEvents()
    {
        _onItemSelectedHandler = (_, __) =>
        {
            PlaySound(SoundTypes.SuccessfulClick);
            Debug.Log("[SoundService] Item selection sound triggered.");
        };
        _onBuyTransactionHandler = (_, __) =>
        {
            PlaySound(SoundTypes.TransactionClick);
            Debug.Log("[SoundService] Buy transaction sound triggered.");
        };
        _onSellTransactionHandler = (_, __) =>
        {
            PlaySound(SoundTypes.TransactionClick);
            Debug.Log("[SoundService] Sell transaction sound triggered.");
        };
        _onTransactionFailedHandler = _ =>
        {
            PlaySound(SoundTypes.FailedClick);
            Debug.Log("[SoundService] Transaction failed sound triggered.");
        };

        // Subscribing event handlers to event listeners
        eventService.OnItemSelected.AddListener(_onItemSelectedHandler);
        eventService.OnBuyTransaction.AddListener(_onBuyTransactionHandler);
        eventService.OnSellTransaction.AddListener(_onSellTransactionHandler);
        eventService.OnTransactionSuccess.AddListener(() => PlaySound(SoundTypes.TransactionClick));
        eventService.OnTransactionFailed.AddListener(_onTransactionFailedHandler);

        Debug.Log("[SoundService] Subscribed to game events.");
    }

    /// <summary>
    /// Unsubscribes from all events to prevent memory leaks.
    /// </summary>
    private void UnsubscribeToEvents()
    {
        eventService.OnItemSelected.RemoveListener(_onItemSelectedHandler);
        eventService.OnBuyTransaction.RemoveListener(_onBuyTransactionHandler);
        eventService.OnSellTransaction.RemoveListener(_onSellTransactionHandler);
        eventService.OnTransactionFailed.RemoveListener(_onTransactionFailedHandler);

        Debug.Log("[SoundService] Unsubscribed from all events.");
    }

    /// <summary>
    /// Plays the specified sound effect if it exists.
    /// </summary>
    /// <param name="soundType">The type of sound to play.</param>
    private void PlaySound(SoundTypes soundType)
    {
        AudioClip clip = GetSoundClip(soundType);
        if (clip != null)
        {
            audioEffects.PlayOneShot(clip);
            Debug.Log($"[SoundService] Playing sound: {soundType}");
        }
        else
        {
            Debug.LogWarning($"[SoundService] Clip not found for: {soundType}");
        }
    }

    /// <summary>
    /// Retrieves the audio clip associated with the given sound type.
    /// </summary>
    /// <param name="soundType">The type of sound to retrieve.</param>
    /// <returns>The corresponding AudioClip, or null if not found.</returns>
    private AudioClip GetSoundClip(SoundTypes soundType)
    {
        Sounds sound = System.Array.Find(
            soundScriptableObject.audioList,
            item => item.soundType == soundType
        );

        if (sound.soundType == soundType)
        {
            return sound.audio;
        }
        else
        {
            Debug.LogWarning($"[SoundService] Clip not found for: {soundType}");
            return null;
        }
    }

    /// <summary>
    /// Destructor to ensure cleanup by unsubscribing from all events.
    /// </summary>
    ~SoundService()
    {
        UnsubscribeToEvents();
        Debug.Log("[SoundService] Destructor called. Cleaning up resources.");
    }
}
