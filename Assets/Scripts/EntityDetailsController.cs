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

    public void Start()
    {
        entity = GetComponentInParent<ActorController>();
    }

    public void Update()
    {
        healthText.text = string.Format("Health: {0}/{1}", entity.hitPoints.x, entity.hitPoints.y);
    }
}