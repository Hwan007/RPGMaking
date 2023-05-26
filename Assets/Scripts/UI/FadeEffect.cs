using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Canvas = gameObject.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (onoff)
        {
            TimeElapsed += Time.deltaTime;
            float tempValue = TimeElapsed / FadeTime;

            // Fadein effect
            if (tempValue < 1.1f)
            {
                AlphaValue = Mathf.Clamp(tempValue, 0.0f, 1.0f);
                Fade(AlphaValue);
            }
            // Fadeout effect
            else if (ShowTime/FadeTime - tempValue < 0.1f)
            {
                AlphaValue = Mathf.Clamp(ShowTime/FadeTime+1.0f-tempValue, 0.0f, 1.0f);
                Fade(AlphaValue);
                if (AlphaValue <= 0.0f)
                    Destroy(gameObject);
            }
        }
    }

    private void OnEnable()
    {
        onoff = true;
    }

    private void OnDisable()
    {
        onoff = false;
    }

    public float FadeTime;
    public float ShowTime;
    bool onoff = false;
    float TimeElapsed = 0.0f;
    float AlphaValue = 0.0f;
    CanvasGroup Canvas;

    /// <summary>
    /// Use Canvas Group component alpha value to fadein/out effect.
    /// </summary>
    /// <param name="alpha"></param>
    public void Fade(float alpha)
    {
        Canvas.alpha = alpha;
    }
}
