using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        PC,
        USER,
        WAIT_BUTTON_STATE
    }
    public List<MyButton> buttons;
    public AudioClip game_over_clip;
    public SpriteRenderer b12;
    public Transform game_elements;
    List<Melody> usable_melodies = new List<Melody>();
    List<Melody> melody_sheet = new List<Melody>();
    private GameState game_state = GameState.PC;
    private int user_melody_index = 0;
    private float current_scale = 1;
    private ScoreManager score_manager;


    // Start is called before the first frame update
    void Start()
    {
        b12.color = new Color(255, 255, 255, 1);
        //b12.color = new Color(255, 255, 255, 0.1F);
        //StartCoroutine(AddAndPlay(2));
        StartCoroutine("Wait");
        TextMesh score_label = GameObject.Find("ScoreLabel").GetComponent<TextMesh>();
        TextMesh high_score_label = GameObject.Find("HighScoreLabel").GetComponent<TextMesh>();
        score_manager = new ScoreManager(score_label, high_score_label);
        //Play();
    }
    IEnumerator Wait()
    {
        int index = -1;
        yield return new WaitForSeconds(1);
        while (true)
        {
            yield return new WaitForSeconds(1.2f);
            index = Random.Range(0, usable_melodies.Count);
            usable_melodies[index].button.SwitchColor();
        }
        
    }
    public void Play()
    {
        StopCoroutine("Wait");
        StartCoroutine("_Play");
        b12.color = new Color(255, 255, 255, 0.1F);

    }
    IEnumerator _Play()
    {
        float current_scale = game_elements.localScale.x;
        float current_y = game_elements.position.y;
        bool scale = (current_scale.ToString("0.00") != "1,00" && current_scale.ToString("0.00") != "1.00");
        bool move = (current_y.ToString("0.00") != "0,00" || current_y.ToString("0.00") != "0,00");
        while (scale || move)
        {
            if (scale)
            {
                current_scale += .01f;
                
                game_elements.localScale = new Vector3(current_scale, current_scale);
                
            }

            if (move)
            {
                current_y -= .025f;
                game_elements.position = new Vector3(0, current_y);
            }
            scale = (current_scale.ToString("0.00") != "1,00" && current_scale.ToString("0.00") != "1.00");
            move = (current_y.ToString("0.00") != "0,00" || current_y.ToString("0.00") != "0,00");
            yield return new WaitForSeconds(.01f);
        }
        StartCoroutine(AddAndPlay(1));
        StopCoroutine("_Play");
        yield break;
    }
    IEnumerator AddAndPlay(int seconds = 2)
    {
        SetState(GameState.PC);
        
        yield return new WaitForSeconds(seconds);
        b12.color = new Color(255, 255, 255, 0.1F);
        Melody selected_melody = PickOne();
        melody_sheet.Add(selected_melody);
        foreach (Melody melody in melody_sheet)
        {
            melody.Play();
            yield return new WaitForSeconds(.7F);
        }
        SetState(GameState.USER);
        StopCoroutine("AddAndPlay");
        yield break;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    Melody PickOne()
    {
        int melody_index = Random.Range(0, usable_melodies.Count);
        return usable_melodies[melody_index];
    }

    public void AddUsableMelody(Melody melody)
    {
        usable_melodies.Add(melody);
    }

    public void SetState(GameState game_state)
    {
        this.game_state = game_state;
    }

    public GameState GetState()
    {
        return game_state;
    }

    public void CheckUserSelect(Melody user_selected)
    {
        Melody melody = melody_sheet[user_melody_index];
        if(melody.unique_name == user_selected.unique_name)
        {
            user_melody_index++;
            if (user_melody_index >= melody_sheet.Count)
            {
                user_melody_index = 0;
                score_manager.Add();
                b12.color = new Color(255, 255, 255, 1);
                StartCoroutine(AddAndPlay(2));
            }
            
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        SetState(GameState.PC);
        StartCoroutine(ScaleTo(.7f, 0));
        b12.color = Color.red;
        AudioSource audio_source = FindObjectOfType<AudioSource>();
        audio_source.clip = game_over_clip;
        audio_source.Play();
        Reset();
        StartCoroutine(ScaleTo(1, 3));
        StartCoroutine(AddAndPlay(4));

    }

    private IEnumerator ScaleTo(float to, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        print(current_scale.ToString("0.000"));
        print(to.ToString("0.000"));
        while (current_scale.ToString("0.000") != to.ToString("0.000"))
        {
            
            if (current_scale > to)
            {
                current_scale -= 0.01f;
                game_elements.localScale = new Vector3(current_scale, current_scale);
       
            }
            if (current_scale < to)
            {
                current_scale += 0.01f;
                game_elements.localScale = new Vector3(current_scale, current_scale);
            }
            print(current_scale);
            yield return new WaitForSeconds(.01f);
        }

        StopCoroutine("ScaleTo");
        yield break;
    }
    private void Reset()
    {
        melody_sheet = new List<Melody>();
        game_state = GameState.PC;
        user_melody_index = 0;
        score_manager.Reset();
    }
}
