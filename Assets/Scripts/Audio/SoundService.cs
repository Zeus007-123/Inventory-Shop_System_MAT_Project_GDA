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

    // Constructor to initialize the SoundService.
    public SoundService(SoundSO soundScriptableObject, AudioSource audioEffectSource)
    {
        this.soundScriptableObject = soundScriptableObject;
        audioEffects = audioEffectSource;
    }

    // Initializes the SoundService by subscribing to game events.
    public void Init(EventService eventService)
    {
        this.eventService = eventService;
        SubscribeToEvents();
    }

    // Subscribes to relevant game events and assigns corresponding sound effects.
    private void SubscribeToEvents()
    {
        _onItemSelectedHandler = (_, __) =>
        {
            PlaySound(SoundTypes.SuccessfulClick);
        };
        _onBuyTransactionHandler = (_, __) =>
        {
            PlaySound(SoundTypes.TransactionClick);
        };
        _onSellTransactionHandler = (_, __) =>
        {
            PlaySound(SoundTypes.TransactionClick);
        };
        _onTransactionFailedHandler = _ =>
        {
            PlaySound(SoundTypes.FailedClick);
        };

        // Subscribing event handlers to event listeners
        eventService.OnItemSelected.AddListener(_onItemSelectedHandler);
        eventService.OnBuyTransaction.AddListener(_onBuyTransactionHandler);
        eventService.OnSellTransaction.AddListener(_onSellTransactionHandler);
        eventService.OnTransactionFailed.AddListener(_onTransactionFailedHandler);

    }

    // Unsubscribes from all events to prevent memory leaks.
    private void UnsubscribeToEvents()
    {
        eventService.OnItemSelected.RemoveListener(_onItemSelectedHandler);
        eventService.OnBuyTransaction.RemoveListener(_onBuyTransactionHandler);
        eventService.OnSellTransaction.RemoveListener(_onSellTransactionHandler);
        eventService.OnTransactionFailed.RemoveListener(_onTransactionFailedHandler);

    }

    // Plays the specified sound effect if it exists.
    private void PlaySound(SoundTypes soundType)
    {
        AudioClip clip = GetSoundClip(soundType);
        if (clip != null)
        {
            audioEffects.PlayOneShot(clip);
        }
    }

    // Retrieves the audio clip associated with the given sound type.
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
            return null;
        }
    }

    // Destructor to ensure cleanup by unsubscribing from all events.
    ~SoundService()
    {
        UnsubscribeToEvents();
    }
}
