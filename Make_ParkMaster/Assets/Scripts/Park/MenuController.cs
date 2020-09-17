using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] GameObject winPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] TextMeshProUGUI fillText;
    [SerializeField] Image fill;
    [SerializeField] SpriteRenderer bestDiamondFirst;
    [SerializeField] Sprite bestDiamondSprite;
    public TextMeshProUGUI coinText;

    public void SetCoinText(int count)
    {
        coinText.text = count.ToString();
    }
    public void OpenWinPanel()
    {

        StartCoroutine(OpenWinPanelForSecond());
        StartCoroutine(StartFill());
    }

    public void OpenGameOverPanel()
    {
        StartCoroutine(openGameoverPanelSecond());
    }
    IEnumerator StartFill(int startCount = 20, int endCount = 50)
    {
        yield return new WaitForSeconds(2);
        GameObject.Find("Main Camera").GetComponent<VoiceController>().playVoice(2);
        for (float i = startCount; i <= endCount; i++)
        {
            yield return new WaitForSeconds(.08f);
            fillText.text = i.ToString()+"%";
            fill.fillAmount = (i / 100);
        }
    }
    public void AddBestDiamond()
    {
        bestDiamondFirst.sprite = bestDiamondSprite;
    }
    IEnumerator OpenWinPanelForSecond()
    {
        winPanel.SetActive(true);                               //paneli aç
        yield return new WaitForSeconds(.3f);
        cam.GetChild(0).GetComponent<ParticleSystem>().Play(); //particle 1 patlat
        yield return new WaitForSeconds(.3f);
        cam.GetChild(1).GetComponent<ParticleSystem>().Play();//particle 2 patlat
        yield return new WaitForSeconds(.3f);
        cam.GetChild(2).GetComponent<ParticleSystem>().Play(); //particle 3 patlat
        yield return new WaitForSeconds(2.5f);
        winPanel.GetComponent<Animator>().StopPlayback();
    }

    IEnumerator openGameoverPanelSecond()
    {
        yield return new WaitForSeconds(0.5f);
        gameOverPanel.SetActive(true);

    }
}
