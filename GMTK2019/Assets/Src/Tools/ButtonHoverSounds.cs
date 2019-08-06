using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHoverSounds : MonoBehaviour
{
    public AudioSource Source;
    public List<AudioClip> audioClips = new List<AudioClip>();
    public void PlayRandomSound()
    {
        int NumSoundToPlay = Random.Range(0, audioClips.Count);
        Source.PlayOneShot(audioClips[NumSoundToPlay]);
        Debug.Log("sound" + NumSoundToPlay);
    }
}
