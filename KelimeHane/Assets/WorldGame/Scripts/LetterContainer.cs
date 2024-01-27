using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterContainer : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private TextMeshPro letter;
    [SerializeField] private SpriteRenderer letterContainer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialized()
    {
        letter.text = "";
        letterContainer.color = Color.white;
    }

    public void SetLetter(char letter, bool ishint = false)
    {
        if (ishint)
        {
            this.letter.color = Color.gray;
        }
        else
        {
            this.letter.color = Color.black;
        }

        this.letter.text = letter .ToString();
    }
    public void SetValid()
    {
        letterContainer.color = Color.green;
    }

    public void SetPotaniel()
    {
        letterContainer.color = Color.yellow;

    }
    public void SetInvalid()
    {
        letterContainer.color = Color.gray;

    }
    public char GetLetter()
    {
        return letter.text[0];
    }
   
}
