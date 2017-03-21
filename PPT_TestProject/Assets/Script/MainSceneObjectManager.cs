using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneObjectManager : MonoBehaviour {

    public CanvasGroup FadeInOutSprite; //for fade in or out.
    public AudioSource MainAudioSourceController;
    public GameObject[] SceneCubeObject;

    private float FadeSpeed = 0.5f;
    private float FadeAlpha = 1f;
    private bool FadeState = true;// true = FadeIn , false = FadeOut.
    
	// Use this for initialization
	void Start () {
        InitObjectSet();
        MainAudioSourceController.Play();

        StartCoroutine(Fun_FadeInOut());
    }	

    private void InitObjectSet() {
        for (int i = 0; i < SceneCubeObject.Length; ++i) {
            float tmptime = (float)i / SceneCubeObject.Length;
            SceneCubeObject[i].GetComponent<Animator>().Play("Anim_Cube_Idle_4", -1, tmptime);
        }
    }

    
    /// <summary>
    /// For FadInOut Effect.
    /// </summary>
    private IEnumerator Fun_FadeInOut() {
        if (FadeState)
        {
            while (FadeAlpha > 0)
            {
                FadeAlpha -= Time.deltaTime * FadeSpeed;
                FadeInOutSprite.alpha = FadeAlpha;
                yield return null;
            }
        }
        else
        {
            while (FadeAlpha < 1)
            {
                FadeAlpha += Time.deltaTime * FadeSpeed;
                FadeInOutSprite.alpha = FadeAlpha;
                yield return null;
            }
        }
        FadeState = !FadeState;
    }

}
