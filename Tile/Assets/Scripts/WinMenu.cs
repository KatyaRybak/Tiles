using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    [SerializeField] Canvas mainCanvas;
    [SerializeField] Text topText;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        mainCanvas.gameObject.SetActive(true);
    }
    public  void ChangeTextOnCanvas(bool isAlive)
    {
        if(isAlive)
        {
            topText.text = "CONGRATULATION! YOU WIN!";
        }
        else
        {
            topText.text = "YOU LOSE!";
        }
    }
}
