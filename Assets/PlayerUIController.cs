using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text m_ScoreText;


    public void UdpateScore(int score)
    {
        m_ScoreText.text = score.ToString();   
    }

}
