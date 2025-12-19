using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatChangeAnimationScript : MonoBehaviour
{
    public static StatChangeAnimationScript Instance {get; private set;}
    private Animator statChangeAnimator;
    private ParticleSystem[] particles;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }
    public void BindUI()
    {
        statChangeAnimator = GameObject.Find("Stats").GetComponent<Animator>();
        particles = new ParticleSystem[4];
        for (int i = 0; i < 4; i++)
        {
            particles[i] = GameObject.Find("Particle" + (i+1)).GetComponent<ParticleSystem>();
        }
    }
    public void StatChange(Occurence occurence, Response response)
    {
        StartCoroutine(Help(occurence, response));
    }
    private IEnumerator Help(Occurence occurence, Response response)
{

    statChangeAnimator.SetTrigger("Start");

    yield return new WaitForSeconds(0.6f);

    GameManager.Instance.FixSize();

    foreach (int num in GameManager.Instance.ChangeResourceLevel(occurence, response))
        particles[num].Play();
    SoundController.Instance.UIMove();

    yield return new WaitForSeconds(1.5f);

    if (statChangeAnimator != null)
    {
        statChangeAnimator.SetTrigger("End");

        yield return new WaitForSeconds(0.6f);

        GameManager.Instance.FixSize();
    }
}

}
