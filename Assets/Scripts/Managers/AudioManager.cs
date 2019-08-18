using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GenderOfVoice { Male, Female }

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource _audioSource;

    [SerializeField] public AudioClip[] Okay;
    [SerializeField] public AudioClip[] No;
    [SerializeField] public AudioClip[] StandingBy;

    [SerializeField] public AudioClip[] EnemySpotted;
    [SerializeField] public AudioClip[] EngagingEnemy;
    [SerializeField] public AudioClip[] UnderFire;
    [SerializeField] public AudioClip[] FallBack;

    [SerializeField] public AudioClip[] EnemyOutOfRange;
    [SerializeField] public AudioClip[] KillConfirmed;
    [SerializeField] public AudioClip[] EasyKill;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip audioClip)
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.clip = audioClip;
            _audioSource.Play();
        }
    }

    public AudioClip RandomPhraseFromArray(ref AudioClip[] clips)
    {
        return clips[Random.Range(0, clips.Length)];
    }
}
