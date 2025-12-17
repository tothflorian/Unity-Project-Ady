
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set; }
    private AudioSource[] sfx;
    public AudioSource music;
    public float volume;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        sfx = GetComponents<AudioSource>();
        music = sfx[0];

        music.Play();

        SetVolume(volume, true);
    }

    public void SetVolume(float newVolume, bool isOn)
    {
        volume = newVolume;
        
        if (isOn) sfx[0].volume = volume;
        sfx[1].volume = sfx[2].volume = volume;
    }
    public void SetClickSound()
    {
        foreach(var button in FindObjectsOfType<Button>())
        {
            button.onClick.AddListener(ClickSound);
        }
    }
    public void ClickSound()
    {
        sfx[1].Play();
    }
    public void SetMusic(bool isOn)
    {
        if(isOn)
            music.volume = volume;
        else
            music.volume = 0;
    }
}
