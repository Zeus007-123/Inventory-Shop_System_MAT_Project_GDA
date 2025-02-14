using UnityEngine;

public class SoundService
{
    private readonly SoundSO soundScriptableObject;
    private readonly AudioSource audioEffects;
    private EventService eventService;

    public SoundService(SoundSO soundScriptableObject, AudioSource audioEffectSource)
    {
        this.soundScriptableObject = soundScriptableObject;
        audioEffects = audioEffectSource;
    }

    public void Init(EventService eventService)
    {
        this.eventService = eventService;
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        // Item selection (2 parameters: ItemSO + bool)
        eventService.OnItemSelected.AddListener((_, __) => PlaySound(SoundTypes.SuccessfulClick));

        // Transactions (2 parameters: ItemSO + int)
        eventService.OnBuyTransaction.AddListener((_, __) => PlaySound(SoundTypes.TransactionClick));
        eventService.OnSellTransaction.AddListener((_, __) => PlaySound(SoundTypes.TransactionClick));

        // Transaction failure (1 parameter: string)
        eventService.OnTransactionFailed.AddListener(_ => PlaySound(SoundTypes.FailedClick));
    }

    private void UnsubscribeToEvents()
    {
        eventService.OnItemSelected.RemoveListener((_, __) => PlaySound(SoundTypes.SuccessfulClick));
        eventService.OnBuyTransaction.RemoveListener((_, __) => PlaySound(SoundTypes.TransactionClick));
        eventService.OnSellTransaction.RemoveListener((_, __) => PlaySound(SoundTypes.TransactionClick));
        eventService.OnTransactionFailed.RemoveListener(_ => PlaySound(SoundTypes.FailedClick));
    }

    private void PlaySound(SoundTypes soundType)
    {
        AudioClip clip = GetSoundClip(soundType);
        if (clip != null)
            audioEffects.PlayOneShot(clip);
    }

    private AudioClip GetSoundClip(SoundTypes soundType)
    {
        Sounds sound = System.Array.Find(
            soundScriptableObject.audioList,
            item => item.soundType == soundType
        );
        return sound.audio;
    }

    ~SoundService()
    {
        UnsubscribeToEvents();
    }
}