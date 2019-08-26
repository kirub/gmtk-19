using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximitySensorComponent : MonoBehaviour {
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private float basePitch = 1f;
    [SerializeField] private bool isPitchRelativeToDistance = true;
    [SerializeField] private float volume = .6f;
    [SerializeField] private float distanceThreshold = 100f;
    [SerializeField] private float baseDelay = .4f;

    private float LastTimeSoundWasPlayed = 0f;

    void Start() {
        LastTimeSoundWasPlayed = Time.time;

        if (!audioSource) return;

        audioSource.pitch = basePitch;
        audioSource.volume = volume;
    }

    // Update is called once per frame
    void Update() {
        if (!audioSource || !Supernova.Instance) return;

        float distance = Supernova.Instance.GetPlayerDistanceFromBorder();

        if (distance == 0 || distance > distanceThreshold) return;

        float dividedDistance = distance / 100f;
        float relativeDelay = baseDelay + dividedDistance;
        
        if (Time.time - LastTimeSoundWasPlayed < relativeDelay) return;

        LastTimeSoundWasPlayed = Time.time;

        if (isPitchRelativeToDistance) {
            float relativePitch = basePitch + (distanceThreshold / 100f - dividedDistance);
            audioSource.pitch = relativePitch;
        }

        audioSource.Play();
    }
}
