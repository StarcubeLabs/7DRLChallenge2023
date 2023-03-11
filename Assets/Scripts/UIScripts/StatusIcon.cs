using System.Collections.Generic;
using RLDataTypes;
using UnityEngine;

public class StatusIcon : MonoBehaviour
{
    private const float ICON_CHANGE_TIME = 1;
    private SpriteRenderer sprite;
    private List<StatusType> statuses = new List<StatusType>();
    private List<Sprite> statusSprites = new List<Sprite>();
    private float timer;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        sprite.enabled = statusSprites.Count > 0;
        if (statusSprites.Count > 0)
        {
            sprite.sprite = statusSprites[(int)(timer % statusSprites.Count)];
            timer += Time.deltaTime * ICON_CHANGE_TIME;
        }
    }

    public void UpdateStatuses(List<StatusType> statuses)
    {
        this.statuses = statuses;
        statusSprites.Clear();
        foreach (StatusType status in statuses)
        {
            Sprite statusSprite = ServicesManager.MoveRegistry.GetStatusSprite(status);
            if (statusSprite)
            {
                statusSprites.Add(statusSprite);
            }
        }

        timer = 0;
    }

    public bool HasStatus(StatusType status)
    {
        return statuses.Contains(status);
    }
}
