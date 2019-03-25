using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public GameManager gm;
    // Start is called before the first frame update
    private void OnMouseDown()
    {
        Destroy(gameObject);
        gm.Play();
    }
}
