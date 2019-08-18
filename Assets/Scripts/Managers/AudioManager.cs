using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal enum GenderOfVoice { Male, Female }

public class AudioManager : MonoBehaviour
{
    internal static AudioManager Instance;

    [SerializeField] private AudioClip[] Okay;
    [SerializeField] private AudioClip[] No;

    [SerializeField] private AudioClip[] StandingBy;

    [SerializeField] private AudioClip[] EnemySpotted;
    [SerializeField] private AudioClip[] EngagingEnemy;

    [SerializeField] private AudioClip[] UnderFire;
    [SerializeField] private AudioClip[] FallBack;
    [SerializeField] private AudioClip[] EnemyOutOfRange;

    [SerializeField] private AudioClip[] KillConfirmed;
    [SerializeField] private AudioClip[] EasyKill;

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    internal AudioClip RandomClipFromArray(ref AudioClip[] clips)
    {
        return clips[Random.Range(0, clips.Length)];
    }
}
