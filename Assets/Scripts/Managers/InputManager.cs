using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = AudioManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Player.Instance.AutoFire = !Player.Instance.AutoFire;
            Player.Instance.autoFireChangeEventHandler?.Invoke(Player.Instance.AutoFire);

            _audioManager.PlayAudio(_audioManager.RandomPhraseFromArray(ref _audioManager.Okay));
        }
    }
}
