using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheEnd : MonoBehaviour
{

    [SerializeField] private Image victoryBackground;
    [SerializeField] private float timeDelayForVictoryAnimation;
    [SerializeField] private AudioClip oneStarAudioClip;
    [SerializeField] private AudioClip twoStarAudioClip;
    [SerializeField] private AudioClip threeStarAudioClip;
    [SerializeField] private Sprite oneStarBackground;
    [SerializeField] private Sprite twoStarBackground;
    [SerializeField] private Sprite threeStarBackground;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject == GameManager.instance.player.gameObject) {
            //TODO Pantalla de victoria
            this.StartCoroutine(this.StarCounterAnimation());
            InterfaceController.instance.setActiveVictory(true);
            InterfaceController.instance.setActiveInput(false);
        }
    }

    private IEnumerator StarCounterAnimation()
    {
        victoryBackground.sprite = oneStarBackground;
        audioSource.clip = oneStarAudioClip;
        audioSource.Play();
        yield return new WaitForSeconds(this.timeDelayForVictoryAnimation);
        
        if (GameManager.instance.getBubbleUseCount() <= GameManager.instance.getTwoStarThreshHold())
        {
            victoryBackground.sprite = twoStarBackground;
            audioSource.clip = twoStarAudioClip;
            audioSource.Play();
            yield return new WaitForSeconds(this.timeDelayForVictoryAnimation);
            if (GameManager.instance.getBubbleUseCount() <= GameManager.instance.getThreeStarThreshHold())
            {
                victoryBackground.sprite = threeStarBackground;
                audioSource.clip = threeStarAudioClip;
                audioSource.Play();
            }

        }
    }
}
