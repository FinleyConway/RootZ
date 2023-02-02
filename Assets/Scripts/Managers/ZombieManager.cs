using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    private void OnEnable()
    {
        FarmPlotObject.OnRemovePlot += SpawnEnemy;
    }

    private void OnDisable()
    {
        FarmPlotObject.OnRemovePlot -= SpawnEnemy;
    }

    private void SpawnEnemy(FarmPlotObject plot, bool killedByPlayer)
    {
        if (killedByPlayer) return;

        Transform temp = plot.GetPlacedObject().GetObjectData().ZombiePrefab;
        //Entity zombie = Instantiate(temp);

        //zombie.position = plot.GetPlacedObject().transform.position;
    }
}
