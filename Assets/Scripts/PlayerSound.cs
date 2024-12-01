using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSound : MonoBehaviour
{
    private bool isCoroutineRunning = false;
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private float interval = 0.5f;

    private void OnEnable()
    {
        PlayerController.playerWalking += runSound;
    }

    private void OnDisable()
    {
        PlayerController.playerWalking -= runSound;

    }


    private void runSound()
    {
        if (!isCoroutineRunning)
        {
            StartCoroutine(PlayFootSound());
        }
    }
    
    private IEnumerator PlayFootSound()
    {
        isCoroutineRunning = true;
        if (footstepSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, footstepSounds.Length);
            footstepSource.PlayOneShot(footstepSounds[randomIndex]);
        }

        yield return new WaitForSeconds(interval);
        isCoroutineRunning = false;
    }
}
