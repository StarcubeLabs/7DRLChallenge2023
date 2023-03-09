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
    Camera camera;

    public void Start()
    {
        entity = GetComponentInParent<ActorController>();
        camera = FindObjectOfType<Camera>();
    }

    public void Update()
    {
        healthText.text = string.Format("Health: {0}/{1}", entity.visualHitPoints, entity.hitPoints.y);
        Vector3 globalRotate = Quaternion.LookRotation(-(camera.transform.position - this.transform.position)).eulerAngles;
        globalRotate.y = 0;
        globalRotate.z = 0;
        this.transform.rotation = Quaternion.Euler(globalRotate);
    }
}