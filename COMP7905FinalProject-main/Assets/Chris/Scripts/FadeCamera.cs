using System;
using UnityEngine;

public class FadeCamera : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How fast should the texture be faded out?")]
    private float fadeTime = 5.0f;

    [SerializeField]
    [Tooltip("Choose the color, which will fill the screen.")]
    private Color fadeColor = new Color(255.0f, 255.0f, 255.0f, 1.0f);

    [SerializeField]
    [Tooltip("How long will the screen stay black during FadeIn?")]
    private float blackScreenDuration;

    private float alpha = 1.0f;
    private Texture2D texture;

    private bool isFadingIn = false;
    private bool isFadingOut = false;

    private float currentTime = 0;

    public class Options
    {
        public Nullable<float> fadeTime;
        public Nullable<float> blackScreenDuration;
        public Color fadeColor;
    }

    private void Start()
    {
        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
        texture.Apply();

        FadeIn();
    }

    private void startFading(bool isFadingIn, bool isFadingOut, Options options = null)
    {
        currentTime = 0;

        if (options != null)
        {
            if (options.fadeTime != null) fadeTime = (float)options.fadeTime;

            if (options.blackScreenDuration != null) blackScreenDuration = (float)options.blackScreenDuration;

            if (options.fadeColor != null) fadeColor = options.fadeColor;
        }

        this.isFadingIn = isFadingIn;
        this.isFadingOut = isFadingOut;
    }

    public void FadeIn(Options options = null)
    {
        alpha = 1.0f;
        startFading(true, false, options);
    }

    public void FadeOut(Options options = null)
    {
        alpha = 0.0f;
        startFading(false, true, options);
    }

    public void OnGUI()
    {
        if (isFadingIn || isFadingOut)
        {
            showBlackScreen();
        }
    }

    private void showBlackScreen()
    {
        if (isFadingIn && alpha <= 0.0f)
        {
            isFadingIn = false;

            return;
        }

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);

        if (isFadingIn && blackScreenDuration > 0)
        {
            blackScreenDuration -= Time.deltaTime;

            return;
        }

        if (isFadingOut && alpha >= 1.0f) return;

        calculateTexture();
    }

    private void calculateTexture()
    {
        currentTime += Time.deltaTime;

        if (isFadingIn) alpha = 1.0f - currentTime / fadeTime;
        else alpha = currentTime / fadeTime;

        texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
        texture.Apply();
    }
}