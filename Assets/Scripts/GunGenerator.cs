using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GunGenerator : MonoBehaviour
{
    private GameObject currentGun;

    private Color32 glass = new Color32(154, 217, 230, 255);
    private Color primary;
    private Color secondary;
    private Color tetriary;

    public List<GameObject> guns;
    public List<GameObject> sights;
    public List<GameObject> stocks;
    public List<GameObject> grips;
    public List<GameObject> barrels;

    int counter = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            InvokeRepeating("GenerateGun", 0f, 0.1f);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }

    public GameObject GenerateGun()
    {
        Destroy(currentGun);
        RandomRestrictedColors();

        int glow = Random.Range(0, 2);

        GameObject gun = new GameObject();
        currentGun = gun;

        GameObject randomGun = GetRandomPart(guns);
        GameObject chosenGun = Instantiate(randomGun, Vector3.zero, Quaternion.identity);
        chosenGun.transform.parent = gun.transform;
        chosenGun.transform.Find("primary").GetComponent<SpriteRenderer>().color = primary;
        chosenGun.transform.Find("secondary").GetComponent<SpriteRenderer>().color = secondary;
        chosenGun.transform.Find("tetriary").GetComponent<SpriteRenderer>().color = new Color32(128, 128, 128, 255);
        if(glow == 0)
            chosenGun.transform.Find("glow").GetComponent<SpriteRenderer>().color = new Color32(0, 44, 255, 255);
        else
            chosenGun.transform.Find("glow").GetComponent<SpriteRenderer>().color = new Color32(255, 44, 0, 255);
        chosenGun.name = "Main_part";

        if (randomGun.name == "medium_machine")
        {
            if (glow == 0)
            {
                gun.AddComponent<Assault_Rifle>();
            }
            else
            {
                gun.AddComponent<Machine_Gun>();
            }
        }
        else if (randomGun.name == "medium_rifle")
        {
            if (glow == 0)
            {
                gun.AddComponent<Semi_Auto_Rifle>();
            }
            else
            {
                gun.AddComponent<Bolt_Action_Rifle>();
            }
        }
        else
        {
            if (glow == 0)
            {
                gun.AddComponent<Shotgun_Buckshot>();
            }
            else
            {
                gun.AddComponent<Shotgun_Slugs>();
            }
        }

        Gun gunScript = gun.GetComponent<Gun>();

        GameObject randomSight = GetRandomPart(sights);
        GameObject chosenSight = Instantiate(randomSight, Vector3.zero, Quaternion.identity);
        chosenSight.transform.parent = gun.transform;
        chosenSight.transform.Find("tetriary").GetComponent<SpriteRenderer>().color = tetriary;
        if(chosenSight.transform.Find("glass") != null)
            chosenSight.transform.Find("glass").GetComponent<SpriteRenderer>().color = glass;

        if(randomSight.name == "medium_iron_sights")
        {
            gunScript.accuracyCounter++; // accuracy is spread, so adding spread decreases accuracy
            gunScript.fireRateCounter--; // fireRate is time between shots, so less is better
        }
        else if (randomSight.name == "medium_scope")
        {
            gunScript.accuracyCounter--;
            gunScript.fireRateCounter++;
        }

        GameObject randomGrip = GetRandomPart(grips);
        GameObject chosenGrip = Instantiate(randomGrip, Vector3.zero, Quaternion.identity);
        chosenGrip.transform.parent = gun.transform;
        chosenGrip.GetComponent<SpriteRenderer>().color = primary;

        if (randomGrip.name == "medium_open_grip")
        {
            gunScript.reloadTimeCounter--; // decreasing reload time is good
            gunScript.fireRateCounter++; // increasing firerate is bad
        }
        else if (randomGrip.name == "medium_bullpup_grip")
        {
            gunScript.reloadTimeCounter++;
            gunScript.fireRateCounter--;
        }

        GameObject randomStock = GetRandomPart(stocks);
        GameObject chosenStock = Instantiate(randomStock, Vector3.zero, Quaternion.identity);
        chosenStock.transform.parent = gun.transform;
        chosenStock.transform.Find("primary").GetComponent<SpriteRenderer>().color = primary;
        chosenStock.transform.Find("secondary").GetComponent<SpriteRenderer>().color = secondary;

        if (randomStock.name == "medium_light_stock")
        {
            gunScript.reloadTimeCounter--; // decreasing reload time is good
            gunScript.accuracyCounter++; // increasing accuracy is bad
        }
        else if (randomStock.name == "medium_heavy_stock")
        {
            gunScript.reloadTimeCounter++;
            gunScript.accuracyCounter--;
        }

        Transform socket = chosenGun.transform.Find("socket");

        GameObject randomBarrel = GetRandomPart(barrels);
        GameObject chosenBarrel = Instantiate(randomBarrel, socket.transform.position, Quaternion.identity);
        chosenBarrel.transform.parent = gun.transform;
        chosenBarrel.GetComponent<SpriteRenderer>().color = tetriary;
        chosenBarrel.name = "barrel";

        gun.name = "Gun";

        return currentGun;
    }

    GameObject GetRandomPart(List<GameObject> parts)
    {
        int randomNumber = Random.Range(0, parts.Count);
        return parts[randomNumber];
    }

    #region Colors

    void RandomColors()
    {
        primary = Random.ColorHSV(0, 1, 0, 0.6f, 0.1f, 0.9f);
        secondary = Random.ColorHSV(0, 1, 0, 0.6f, 0.1f, 0.9f);
        tetriary = Random.ColorHSV(0, 1, 0, 0.6f, 0.1f, 0.9f);

        ScreenCapture.CaptureScreenshot("C:/Files/Programs/Unity/Projects/GunGeneration/ScreenShots/Random/" + "Random" + counter + ".png");
        counter++;
    }

    void RandomRestrictedColors()
    {
        List<int> colorRange = new List<int>();
        colorRange.Add(20);
        colorRange.Add(50);
        colorRange.Add(80);

        int index = Random.Range(0, colorRange.Count);
        primary = Random.ColorHSV(0, 1, 0.1f, 0.9f, (float)(colorRange[index] - 10) / 100, (float)(colorRange[index] + 10) / 100);
        colorRange.RemoveAt(index);

        index = Random.Range(0, colorRange.Count);
        secondary = Random.ColorHSV(0, 1, 0.1f, 0.9f, (float)(colorRange[index] - 10) / 100, (float)(colorRange[index] + 10) / 100);
        colorRange.RemoveAt(index);

        index = Random.Range(0, colorRange.Count);
        tetriary = Random.ColorHSV(0, 1, 0.1f, 0.9f, (float)(colorRange[index] - 10) / 100, (float)(colorRange[index] + 10) / 100);
        colorRange.RemoveAt(index);

        //ScreenCapture.CaptureScreenshot("C:/Files/Programs/Unity/Projects/GunGeneration/ScreenShots/RandomRestricted/" + "RandomRestricted" + counter + ".png");
        //counter++;
    }

    void TwoAnalogousOneComplementaryColor()
    {
        List<int> colorRange = new List<int>();
        colorRange.Add(20);
        colorRange.Add(50);
        colorRange.Add(80);

        int index = Random.Range(0, colorRange.Count);
        primary = Random.ColorHSV(0, 1, 0.1f, 0.9f, (float)(colorRange[index] - 10) / 100, (float)(colorRange[index] + 10) / 100);
        colorRange.RemoveAt(index);

        float PH, PS, PV;
        Color.RGBToHSV(primary, out PH, out PS, out PV);

        index = Random.Range(0, colorRange.Count);
        secondary = Random.ColorHSV((float)(PH - 0.1), (float)(PH + 0.1), 0.1f, 0.9f, (float)(colorRange[index] - 10) / 100, (float)(colorRange[index] + 10) / 100);
        colorRange.RemoveAt(index);

        float SH, SS, SV;
        Color.RGBToHSV(secondary, out SH, out SS, out SV);

        index = Random.Range(0, colorRange.Count);
        tetriary = Random.ColorHSV((float)((PH + SH)/2 + 0.5) % 1, (float)((PH + SH)/2 + 0.5) % 1, 0.1f, 0.9f, (float)(colorRange[index] - 10) / 100, (float)(colorRange[index] + 10) / 100);
        colorRange.RemoveAt(index);

        ScreenCapture.CaptureScreenshot("C:/Files/Programs/Unity/Projects/GunGeneration/ScreenShots/TwoAnalogOneComplementary/" + "TwoAnalogOneComplementary" + counter + ".png");
        counter++;
    }

    void AnalogousTriad()
    {
        List<int> colorRange = new List<int>();
        colorRange.Add(20);
        colorRange.Add(80);

        int index = Random.Range(0, colorRange.Count);
        primary = Random.ColorHSV(0, 1, 0.1f, 0.9f, (float)(colorRange[index] - 10) / 100, (float)(colorRange[index] + 10) / 100);
        colorRange.RemoveAt(index);

        secondary = Random.ColorHSV(0, 1, 0.1f, 0.9f, (float)(colorRange[0] - 10) / 100, (float)(colorRange[0] + 10) / 100);
        colorRange.RemoveAt(0);

        float PH, PS, PV;
        Color.RGBToHSV(primary, out PH, out PS, out PV);
        float SH, SS, SV;
        Color.RGBToHSV(secondary, out SH, out SS, out SV);

        tetriary = Color.HSVToRGB((float)(PH + SH) / 2, (float)(PS + SS) / 2, (float)(PV + SV) / 2);

        ScreenCapture.CaptureScreenshot("C:/Files/Programs/Unity/Projects/GunGeneration/ScreenShots/AnalogousTriad/" + "AnalogousTriad" + counter + ".png");
        counter++;
    }

    void ComplementaryTriad()
    {
        List<int> colorRange = new List<int>();
        colorRange.Add(20);
        colorRange.Add(80);

        int index = Random.Range(0, colorRange.Count);
        primary = Random.ColorHSV(0, 1, 0.1f, 0.9f, (float)(colorRange[index] - 10) / 100, (float)(colorRange[index] + 10) / 100);
        colorRange.RemoveAt(index);

        secondary = Random.ColorHSV(0, 1, 0.1f, 0.9f, (float)(colorRange[0] - 10) / 100, (float)(colorRange[0] + 10) / 100);
        colorRange.RemoveAt(0);

        float PH, PS, PV;
        Color.RGBToHSV(primary, out PH, out PS, out PV);
        float SH, SS, SV;
        Color.RGBToHSV(secondary, out SH, out SS, out SV);

        tetriary = Color.HSVToRGB((float)((PH + SH) / 2 + 0.5f) % 1, (float)(PS + SS) / 2, (float)(PV + SV) / 2);

        ScreenCapture.CaptureScreenshot("C:/Files/Programs/Unity/Projects/GunGeneration/ScreenShots/ComplementaryTriad/" + "ComplementaryTriad" + counter + ".png");
        counter++;
    }

    #endregion
}
