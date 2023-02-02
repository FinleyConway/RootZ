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

        Transform perfab = plot.GetPlacedObject().GetObjectData().ZombiePrefab;
        Transform temp = Instantiate(perfab);
        temp.position = plot.GetPlacedObject().transform.position;

        Zombie zombie = temp.GetComponent<Zombie>();
        //zombie.Init(_playerPosition.position);
    }
}
