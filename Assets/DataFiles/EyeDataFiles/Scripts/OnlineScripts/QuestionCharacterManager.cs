using System.Collections;
using System.Collections.Generic;
using ReadyPlayerMe;
using UnityEngine.Localization.Settings;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class QuestionCharacterManager : MonoBehaviour
{
    public AudioClip productDataClip_AR;
    public AudioClip solutionProductData_AR;
    public AudioClip howTeacherExplain_AR;

    public AudioClip productDataClip_EN;
    public AudioClip solutionProductData_EN;
    public AudioClip howTeacherExplain_EN;

    public AudioSource characterAudio, tapEffect;
    public Animator character;

    public Image imageButton;
    public Sprite spriteAR, spriteEN;

    [HideInInspector] private bool isAudioFinished;

    IEnumerator Start()
    {
        // Wait for the localization system to initialize, loading Locales, preloading etc.
        yield return LocalizationSettings.InitializationOperation;
    }

    private void Update()
    {
        if (isAudioFinished == true && !characterAudio.isPlaying)
        {
            character.runtimeAnimatorController = null;
            character.enabled = false;
            isAudioFinished = true;
        }
    }

    public void PlayCharacterAudioQuestion1()
    {
        tapEffect.Play();
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1])
        {
            characterAudio.clip = productDataClip_EN;
            character.GetComponent<VoiceHandler>().AudioClip = productDataClip_EN;
        }
        else
        {
            characterAudio.clip = productDataClip_AR;
            character.GetComponent<VoiceHandler>().AudioClip = productDataClip_AR;
        }

        StartCoroutine("PlayAnswerQuestion");
    }

    public void PlayCharacterAudioQuestion2()
    {
        tapEffect.Play();
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1])
        {
            characterAudio.clip = solutionProductData_EN;
            character.GetComponent<VoiceHandler>().AudioClip = solutionProductData_EN;
        }
        else
        {
            characterAudio.clip = solutionProductData_AR;
            character.GetComponent<VoiceHandler>().AudioClip = solutionProductData_AR;
        }

        StartCoroutine("PlayAnswerQuestion");
    }

    public void PlayCharacterAudioQuestion3()
    {
        tapEffect.Play();
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1])
        {
            characterAudio.clip = howTeacherExplain_EN;
            character.GetComponent<VoiceHandler>().AudioClip = howTeacherExplain_EN;
        }
        else
        {
            characterAudio.clip = howTeacherExplain_AR;
            character.GetComponent<VoiceHandler>().AudioClip = howTeacherExplain_AR;
        }

        StartCoroutine("PlayAnswerQuestion");
    }


    public IEnumerator PlayAnswerQuestion()
    {
        yield return new WaitForSeconds(0f);
        character.runtimeAnimatorController =
            Resources.Load("Avatar Animator") as RuntimeAnimatorController;
        character.enabled = true;
        if (characterAudio.clip == productDataClip_AR)
        {
            characterAudio.Play();
        }
        else if (characterAudio.clip == solutionProductData_AR)
        {
            characterAudio.Play();
        }
        else if (characterAudio.clip == howTeacherExplain_AR)
        {
            characterAudio.Play();
        }

        else if (characterAudio.clip == productDataClip_EN)
        {
            characterAudio.Play();
        }
        else if (characterAudio.clip == solutionProductData_EN)
        {
            characterAudio.Play();
        }
        else if (characterAudio.clip == howTeacherExplain_EN)
        {
            characterAudio.Play();
        }

        isAudioFinished = true;
    }

    public void ChangeLanguage()
    {
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
        {
            Debug.Log("EnglishLanguage");
            LocaleSelected(1);
            imageButton.sprite = spriteAR;
            characterAudio.Stop();
            isAudioFinished = true;
        }
        else
        {
            Debug.Log("ArabicLanguage");
            LocaleSelected(0);
            imageButton.sprite = spriteEN;
            characterAudio.Stop();
            isAudioFinished = true;
        }
    }

    static void LocaleSelected(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}