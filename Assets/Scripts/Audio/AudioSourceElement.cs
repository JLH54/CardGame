using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceElement : MonoBehaviour
{
    private Transform myTransform;
    private AudioSource audioSource;

    private void Awake()
    {
        myTransform = gameObject.transform;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            gameObject.SetActive(false);
        }
    }

    public void PlayAudio2D(AudioClip audioClip)
    {
        gameObject.SetActive(true);

        audioSource.spatialBlend = 0f;
        PlayAudioClip(audioClip);
    }

    private void PlayAudioClip(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
