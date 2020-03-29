using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource mainMusic;
    public AudioSource happierMusic;
    public AudioSource attackMusic;

    [Header("SFX")]
    public AudioSource dogFriendFX;
    public AudioSource wolfBarkFX;
    public AudioSource wolfBarkFX2;
    public List<AudioClip> wolfBarkList;
    private float volumeBark;

    public AudioSource dogFleeFX;
    public AudioSource dogFleeFX2;
    public List<AudioClip> dogFleeList;
    private float volumeDogFlee;


    public float timerForAttack = 0f;
    public float volumeHappier = 0;

    public void Start()
    {
        volumeBark = wolfBarkFX.volume;
        mainMusic.volume = 1;
        happierMusic.volume = volumeHappier;
    }

    public void UpdateMusic(float lerp)
    {
        Mathf.Clamp01(lerp);
        volumeHappier = lerp;

        mainMusic.volume = 1;
        happierMusic.volume = volumeHappier;
    }

    public void LaunchAttack()
    {
        timerForAttack = 1f;
    }

    public void Update()
    {
        if (timerForAttack > 0f)
        {
            timerForAttack -= Time.deltaTime;
            if (attackMusic.volume < 1)
            {
                attackMusic.volume += Time.deltaTime * 3f;
                mainMusic.volume = 1 - attackMusic.volume;
                happierMusic.volume = (1 - attackMusic.volume) * volumeHappier;
            }
        }
        else
        {
            if (attackMusic.volume > 0)
            {
                attackMusic.volume -= Time.deltaTime * 3f;
                mainMusic.volume = 1 - attackMusic.volume;
                happierMusic.volume = (1 - attackMusic.volume) * volumeHappier;
            }
        }
    }

    //SFX
    public void PlayDogFriendly()
    {
        dogFriendFX.Play();
    }


    public void PlayWolfBark(bool hero)
    {
        AudioSource aSAvailable = null;
        if (!wolfBarkFX.isPlaying)
        {
            aSAvailable = wolfBarkFX;
        }
        if (!wolfBarkFX2.isPlaying)
        {
            aSAvailable = wolfBarkFX2;
        }

        if (aSAvailable != null)
        {
            int random = Random.Range(0, wolfBarkList.Count); if (random == wolfBarkList.Count) random = wolfBarkList.Count - 1;
            aSAvailable.clip = wolfBarkList[random];
            aSAvailable.pitch = Random.Range(0.4f, 1.4f);
            aSAvailable.volume = volumeBark * (hero ? 1f : 0.2f);

            aSAvailable.Play();
        }
    }

    public void PlayDogFlee()
    {
        AudioSource aSAvailable = null;
        if (!dogFleeFX.isPlaying)
        {
            aSAvailable = dogFleeFX;
        }
        if (!dogFleeFX2.isPlaying)
        {
            aSAvailable = dogFleeFX2;
        }

        if (aSAvailable != null)
        {
            int random = Random.Range(0, dogFleeList.Count); if (random == dogFleeList.Count) random = dogFleeList.Count - 1;
            aSAvailable.clip = dogFleeList[random];
            aSAvailable.pitch = Random.Range(0.4f, 1.4f);
            aSAvailable.Play();
        }
    }

}
