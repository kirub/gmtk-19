using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextOverTime : MonoBehaviour
{
    [Header("if 0 text will not appear without calling StartWriting()")]
    public float DelayTextAppear = 1f;
    public float DelayTextHide = 10f;
    [Header("char '£' will switch delay to DelayBtwnLetters1 (default)")]
    public float DelayBtwnLetters1 = 0.1f;
    [Header("char '%' will switch delay to DelayBtwnLetters2")]
    public float DelayBtwnLetters2 = 0.2f;
    [Header("char '§' will cause one delay of PauseCharTimer")]
    public float PauseCharTimer = 0.5f;
    [Header("char '|' will jump to next line")]
    [TextArea]
    public string FullText = "";

    private string CurrentText = "";
    private float DelayUsed;
    private Text T;
    // Start is called before the first frame update
    void Start()
    {
        DelayUsed = DelayBtwnLetters1;
        T = this.GetComponent<Text>();
        if(DelayTextAppear>0)
            StartCoroutine(TimerShow());
    }
    
    public void StartWriting()
    {
        StartCoroutine(ShowText());
    }
    IEnumerator TimerShow()
    {
        yield return new WaitForSeconds(DelayTextAppear);
        StartCoroutine(ShowText());
    }
    IEnumerator TimerHide()
    {
        yield return new WaitForSeconds(DelayTextHide);
        this.gameObject.SetActive(false);
    }


    IEnumerator ShowText()
    {
        for(int i=0; i<FullText.Length ;i++)
        {
            if(FullText[i].Equals('£')|| FullText[i].Equals('%')|| FullText[i].Equals('§')|| FullText[i].Equals('|'))
            {
                if (FullText[i].Equals('£'))
                    DelayUsed = DelayBtwnLetters1;
                else if (FullText[i].Equals('%'))
                    DelayUsed = DelayBtwnLetters2;
                else if (FullText[i].Equals('§'))
                    yield return new WaitForSeconds(PauseCharTimer);
                else if (FullText[i].Equals('|'))
                    CurrentText += "\n";
            }
            else
            {
                CurrentText += FullText[i];
                T.text = CurrentText;
                yield return new WaitForSeconds(DelayUsed);
            }
        }
        StartCoroutine(TimerHide());
    }



}
