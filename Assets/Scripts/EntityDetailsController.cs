using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class EntityDetailsController: MonoBehaviour
{
    public TextMeshProUGUI healthText;
    ActorController entity;
    Camera cam;

    public void Start()
    {
        entity = GetComponentInParent<ActorController>();
        cam = FindObjectOfType<Camera>();
    }

    public void Update()
    {
        if (healthText)
        {
            healthText.text = string.Format("Health: {0}/{1}", entity.visualHitPoints, entity.hitPoints.y);
        }
        Vector3 globalRotate = Quaternion.LookRotation(-(cam.transform.position - this.transform.position)).eulerAngles;
        globalRotate.y = 0;
        globalRotate.z = 0;
        this.transform.rotation = Quaternion.Euler(globalRotate);
    }
}