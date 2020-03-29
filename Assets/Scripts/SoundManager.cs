using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource mainMusic;
    public AudioSource happier3Music;
    public AudioSource happier5Music;
    public AudioSource happier8Music;
    public AudioSource happier10Music;
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

    public AudioSource askHelp;

    public float timerForAttack = 0f;
    public int volumeHappier = 0;

    public void Start()
    {
        volumeBark = wolfBarkFX.volume;
        mainMusic.volume = 1;
        happier3Music.volume = 0;
        happier5Music.volume = 0;
        happier8Music.volume = 0;
        happier10Music.volume = 0;
    }

    public void UpdateFriendNumer(int numberOfFriends)
    {
        volumeHappier = numberOfFriends;

        UpdateMusic();
    }

    void UpdateMusic()
    {
        mainMusic.volume = 1;

        happier3Music.volume = lerpBetween(volumeHappier, 0, 3);
        happier5Music.volume = lerpBetween(volumeHappier, 3, 5);
        happier8Music.volume = lerpBetween(volumeHappier, 5, 8);
        happier10Music.volume = lerpBetween(volumeHappier, 8, 10);
    }
    void UpdateMusic(float atten)
    {
        mainMusic.volume = 1 * atten;

        happier3Music.volume = lerpBetween(volumeHappier, 0, 3) * atten;
        happier5Music.volume = lerpBetween(volumeHappier, 3, 5) * atten;
        happier8Music.volume = lerpBetween(volumeHappier, 5, 8) * atten;
        happier10Music.volume = lerpBetween(volumeHappier, 8, 10) * atten;
    }

    public float lerpBetween(float value, int min, int max)
    {
        float res = (float)(value - min) / (float)(max - min);
        return Mathf.Clamp01(res);
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
            }
        }
        else
        {
            if (attackMusic.volume > 0)
            {
                attackMusic.volume -= Time.deltaTime * 3f;
                UpdateMusic(1 - attackMusic.volume);
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
            aSAvailable.volume = volumeBark * (hero ? GameManager.Instance.feedbackValue.soundBarkHero : GameManager.Instance.feedbackValue.soundBarkDoggo);

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

    public void PlayAskHelp()
    {
        askHelp.Play();
    }

}
