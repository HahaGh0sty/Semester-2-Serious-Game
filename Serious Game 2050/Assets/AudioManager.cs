using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource Music;
    [SerializeField] List<AudioClip> Songs = new List<AudioClip>();
    [SerializeField] float silenceBetweenSongs = 2f;

    public static AudioManager instance;

    bool isPlayingPlaylist = false;
    bool skipRequested = false;
    bool isMusicMuted = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartPlaylist();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            skipRequested = true;
        }
        if (Input.GetKeyDown(KeyCode.M)) // ADD
        {
            ToggleMusicMute(); // ADD
        }
    }

    void ToggleMusicMute() // ADD
    {
        isMusicMuted = !isMusicMuted;
        Music.mute = isMusicMuted;
    }


    public void StartPlaylist()
    {
        if (!isPlayingPlaylist && Songs.Count > 1)
        {
            StartCoroutine(PlayMusicPlaylist());
        }
    }

    IEnumerator PlayMusicPlaylist()
    {
        isPlayingPlaylist = true;

        // Always play the first song
        AudioClip firstSong = Songs[0];
        Music.clip = firstSong;
        Music.Play();
        yield return WaitForTrackEnd(firstSong.length);

        while (true)
        {
            List<AudioClip> shuffled = new List<AudioClip>(Songs);
            shuffled.RemoveAt(0); // exclude first song
            Shuffle(shuffled);

            int index = 0;
            while (true)
            {
                if (index >= shuffled.Count)
                {
                    // Reshuffle and start over
                    Shuffle(shuffled);
                    index = 0;
                }

                AudioClip clip = shuffled[index];
                index++;

                Music.clip = clip;
                Music.Play();
                yield return WaitForTrackEnd(clip.length);
            }
        }
    }

    IEnumerator WaitForTrackEnd(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            if (skipRequested)
            {
                skipRequested = false;
                break;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        // Optional silence between songs
        float silenceTimer = 0f;
        while (silenceTimer < silenceBetweenSongs)
        {
            if (skipRequested)
            {
                skipRequested = false;
                break;
            }
            silenceTimer += Time.deltaTime;
            yield return null;
        }
    }

    void Shuffle(List<AudioClip> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = UnityEngine.Random.Range(i, list.Count);
            AudioClip temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
}
