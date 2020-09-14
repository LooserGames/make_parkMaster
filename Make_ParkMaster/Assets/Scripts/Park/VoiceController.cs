using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceController : MonoBehaviour
{
    public List<AudioClip> gameSfx = new List<AudioClip>();
    AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void playVoice(int sfxId) //0=alkış, 1=final, 2=ayakkabı, 3=toplama, 4=Çarpışma
    {
        audioSource.clip = gameSfx[sfxId];
        audioSource.Play();
    }
}
