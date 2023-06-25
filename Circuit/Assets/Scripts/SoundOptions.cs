using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class SoundOptions : MonoBehaviour
{
    // 오디오 믹서
    public AudioMixer audioMixer;

    // 슬라이더
    public Slider musicSlider;
    public Slider effectsSlider;
        
    // 볼륨 조절
    public void SetBgmVolme()
    {
         // 로그 연산 값 전달
        audioMixer.SetFloat("BGM", Mathf.Log10(musicSlider.value) * 20);
    }

    public void SetSFXVolme()
    {
         // 로그 연산 값 전달
        audioMixer.SetFloat("SFX", Mathf.Log10(effectsSlider.value) * 20);
    }
}