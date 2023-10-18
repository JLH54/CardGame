using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioEntityCollection audioEntityCollection;

    private Dictionary<SoundType, List<AudioClip>> audioClipsBySoundType = new Dictionary<SoundType, List<AudioClip>>(new SoundTypeComparer());

    private AudioPooler audioPooler;

    // Static singleton property
    public static AudioManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;

        audioPooler = FindObjectOfType<AudioPooler>();

        MapAudioClipsToSoundType();
    }

    private void MapAudioClipsToSoundType()
    {
        for (int i = 0; i < audioEntityCollection.audioEntities.Length; i++)
        {
            AddAudioClipToDictionary(audioEntityCollection.audioEntities[i].soundType,
                                     audioEntityCollection.audioEntities[i].audioClip);
        }
    }

    private void AddAudioClipToDictionary(SoundType soundType, AudioClip audioClip)
    {
        if (audioClipsBySoundType.ContainsKey(soundType))
        {
            List<AudioClip> audioClips = audioClipsBySoundType[soundType];
            if (audioClips.Contains(audioClip) == false)
            {
                audioClips.Add(audioClip);
            }
        }
        else
        {
            List<AudioClip> audioClips = new List<AudioClip>();
            audioClips.Add(audioClip);
            audioClipsBySoundType.Add(soundType, audioClips);
        }
    }

    private AudioClip GetRandomAudioClip(SoundType soundType)
    {
        AudioClip requestedAudioClip = null;

        if (audioClipsBySoundType.TryGetValue(soundType, out List<AudioClip> audioClips))
        {
            int selectedAudioClipIndex = Random.Range(0, audioClips.Count);

            requestedAudioClip = audioClips[selectedAudioClipIndex];
        }

        return requestedAudioClip;
    }

    public void PlaySound2D(SoundType soundType)
    {
        AudioSourceElement audioSourceElement = audioPooler.GetAvailablePooledObject();

        if (audioSourceElement == null) return;

        audioSourceElement.PlayAudio2D(GetRandomAudioClip(soundType));
    }
}