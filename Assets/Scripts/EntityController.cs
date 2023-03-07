using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EntityController: MonoBehaviour
{
    public Vector3Int gridPosition
    {
        get;
        set;
    }

    public virtual void SaveEntity(EntityManager entityManager)
    {
    }

    public virtual void LoadEntity(EntityManager entityManager)
    {
    }
}
