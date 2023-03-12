using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CreditsController: MonoBehaviour
{
    RectTransform rectTransform;

    public float scrollRate = 0.02f;

    public void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Update()
    {
        Vector3 nextPosition = this.transform.position;
        nextPosition.y += scrollRate;
        rectTransform.SetPositionAndRotation(nextPosition, this.transform.rotation);
    }
}