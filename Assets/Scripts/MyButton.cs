using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton : MonoBehaviour
{
    public enum ButtonColorTypes
    {
        ACTIVE,
        PASSIVE
    }
    public string unique_name;
    public Color passive_color;
    public Color active_color;
    public AudioClip audio_clip;
    private SpriteRenderer sprite_renderer;
    private bool is_active = false;
    private AudioSource audio_source;
    private Melody melody;
    private GameManager game_manager;
    // Start is called before the first frame update
    void Start()
    {
        audio_source = FindObjectOfType<AudioSource>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        game_manager = FindObjectOfType<GameManager>();
        melody = new Melody(this);
        game_manager.AddUsableMelody(melody);
        SetColor(ButtonColorTypes.PASSIVE);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if (game_manager.GetState() != GameManager.GameState.USER) return;
        melody.Play();
        game_manager.CheckUserSelect(melody);
        
        
    }

    void SetColor(ButtonColorTypes color)
    {
        is_active = color == ButtonColorTypes.ACTIVE;
        if (is_active)
            sprite_renderer.color = active_color;
        else
            sprite_renderer.color = passive_color;
    }

    void SetColorActive()
    {
        sprite_renderer.sortingOrder = 1;
        SetColor(ButtonColorTypes.ACTIVE);
    }

    void SetColorPassive()
    {
        SetColor(ButtonColorTypes.PASSIVE);
        if(game_manager.GetState() == GameManager.GameState.WAIT_BUTTON_STATE)
        {
            game_manager.SetState(GameManager.GameState.USER);
        }
        sprite_renderer.sortingOrder = 0;
    }

    public void SwitchColor()
    {
        
        if(game_manager.GetState() == GameManager.GameState.USER)
        {
            game_manager.SetState(GameManager.GameState.WAIT_BUTTON_STATE);
        }
        SetColorActive();
        Invoke("SetColorPassive", .5f);
        
    }

    public AudioSource GetAudioSource()
    {
        return audio_source;
    }

    public Melody GetMelody()
    {
        return melody;
    }
}
