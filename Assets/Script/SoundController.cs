
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set; }
    private AudioSource[] sfx;
    private AudioSource music;
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

        SetVolume(volume);
    }

    public void SetVolume(float newVolume)
    {
        volume = newVolume;
        foreach(var AS in sfx)
        {
            AS.volume = newVolume;
        }
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
    public void SetMusic()
    {
        if(music.isPlaying)
            music.Stop();
        else
            music.Play();
    }
}
