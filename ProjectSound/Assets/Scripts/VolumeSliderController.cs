using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSliderController : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private AudioMixer audioMixer;


    private void Awake()
    {
        slider = GetComponent<Slider>();
        if(StayThroughScenesObject.instance != null)
        {
            slider.value = StayThroughScenesObject.instance.getSoundVolume();
        }
        else
        {
            slider.value = 1;
        }
        
    }
    
    public void ChangeVolume(float volume)
    {
        if(StayThroughScenesObject.instance != null)
        StayThroughScenesObject.instance.setSoundVolume(slider.value);
        audioMixer.SetFloat("volume", Mathf.Log10(slider.value) * 20);
    }
}
