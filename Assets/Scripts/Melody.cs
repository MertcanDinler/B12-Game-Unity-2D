using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melody
{
    public string unique_name;
    public MyButton button;
    public AudioClip audio_clip;
    public AudioSource audio_source;
    private GameManager game_manager;

    public Melody(MyButton button)
    {
        this.button = button;
        unique_name = button.unique_name;
        audio_source = button.GetAudioSource();
        audio_clip = button.audio_clip;
    }

    public void Play()
    {
        button.SwitchColor();
        audio_source.clip = audio_clip;
        audio_source.Play();
    }
}
