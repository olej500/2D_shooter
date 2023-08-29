using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    public Transform chestTransform;
    public Transform armTransform;
    public Transform headTransform;

    Vector3 mousePosition;
    Vector3 chestPosition;
    Vector3 armPosition;

    GameObject player;
    PlayerScript playerScript;
    private Vector3 playerScale;

    void Start()
    {
        player = GameObject.Find("Player");
        playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
        chestPosition = new Vector3(0, 0.3125f, 0);
        armPosition = new Vector3(-0.25f, 0.3125f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        HandleFlip();
        HandleAiming();

        if (playerScript.currentAmmo == 0 && !playerScript.isReloading)
        {
            GetComponent<AudioSource>().Play();
            playerScript.isReloading = true;
            Invoke("Reload", playerScript.reloadTime);
        }
    }

    void HandleFlip()
    {
        playerScale = player.transform.localScale;
        if (mousePosition.x < player.transform.position.x)
            playerScale.x = -1;
        else
            playerScale.x = 1;
        player.transform.localScale = playerScale;
    }

    void HandleAiming()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDirection = (mousePosition - armTransform.position);
        Vector3 aimPoint;
        Vector3 aim;
        float angle;

        if ((armTransform.position.x > mousePosition.x && mousePosition.x > player.transform.position.x && mousePosition.y < armTransform.position.y) ||
            (armTransform.position.x < mousePosition.x && mousePosition.x < player.transform.position.x && mousePosition.y < armTransform.position.y))
        {
            aimPoint = armTransform.position;
            aim = (mousePosition - aimPoint);
            angle = Mathf.Clamp(player.transform.localScale.x * -180 - Mathf.Atan2(aim.y, Mathf.Abs(aim.x)) * Mathf.Rad2Deg * player.transform.localScale.x, -110, 110);
        }
        else
        {
            aimPoint = new Vector3(player.transform.position.x, armTransform.position.y + Mathf.Tan(Mathf.Atan2(aimDirection.y, Mathf.Abs(aimDirection.x))) * Mathf.Abs(player.transform.position.x - armTransform.position.x) * Mathf.Sign(aimDirection.y), 0);
            aim = (mousePosition - aimPoint);
            angle = Mathf.Atan2(aim.y, Mathf.Abs(aim.x)) * Mathf.Rad2Deg * player.transform.localScale.x;
        }

        Debug.DrawLine(aimPoint, new Vector2(aimPoint.x + aim.x, aimPoint.y + aim.y), Color.green);

        chestTransform.eulerAngles = new Vector3(0, 0, angle / 2);
        armTransform.eulerAngles = new Vector3(0, 0, angle);

        if (angle * player.transform.localScale.x > 0)
        {
            chestTransform.localPosition = new Vector3(chestPosition.x - 0.1875f * player.transform.localScale.x * angle / 90f, chestPosition.y, chestPosition.z);
            armTransform.localPosition = new Vector3(armPosition.x + 0.1875f * player.transform.localScale.x * angle / 90f, armPosition.y - 0.05f * player.transform.localScale.x * angle / 90f, armTransform.position.z);
        }
        else
        {
            chestTransform.localPosition = new Vector3(chestPosition.x - 0.0625f * player.transform.localScale.x * angle / 90f, chestPosition.y, chestPosition.z);
            armTransform.localPosition = new Vector3(armPosition.x - 0.0991f * player.transform.localScale.x * angle / 90f, armPosition.y - 0.28f * player.transform.localScale.x * angle / 90f, armTransform.position.z);
        }
    }

    void Reload()
    {
        playerScript.currentAmmo = playerScript.magSize;
        playerScript.isReloading = false;
    }
}
