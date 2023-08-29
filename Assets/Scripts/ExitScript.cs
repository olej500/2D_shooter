using UnityEngine;
using UnityEngine.UI;

public class ExitScript : MonoBehaviour
{
    LevelGenerator levelGeneratorScript;
    public Image fadeToBlack;
    bool fadeToBlackCheck;
    public bool fadeFromBlackCheck;
    GameObject entrance;

    float timeElapsed = 0;
    int zeroAlpha = 0;
    int fullAlpha = 255;
    float fadeTime = 2;
    float valueToLerp;

    void Start()
    {
        levelGeneratorScript = GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>();
        fadeToBlack = GameObject.Find("Image").GetComponent<Image>();
        entrance = GameObject.Find("Entrance(Clone)");
    }

    private void FixedUpdate()
    {
        if (fadeToBlackCheck)
        {
            if (timeElapsed < fadeTime)
            {
                valueToLerp = Mathf.Lerp(zeroAlpha, fullAlpha, timeElapsed / fadeTime);
                timeElapsed += Time.deltaTime;
                Color color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, valueToLerp / fullAlpha);
                fadeToBlack.color = color;
            }
            else
            {
                fadeToBlackCheck = false;
                levelGeneratorScript.GenerateLevel();
                Destroy(entrance);
                Destroy(gameObject);
            }
        }

        if (fadeFromBlackCheck)
        {
            if (timeElapsed < fadeTime)
            {
                valueToLerp = Mathf.Lerp(fullAlpha, zeroAlpha, timeElapsed / fadeTime);
                timeElapsed += Time.deltaTime;
                Color color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, valueToLerp / fullAlpha);
                fadeToBlack.color = color;
            }
            else
            {
                fadeFromBlackCheck = false;
                timeElapsed = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            fadeToBlackCheck = true;
        }
    }
}