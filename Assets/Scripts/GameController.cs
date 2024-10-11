using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform PlayerSpawnPos;
    public Transform Player;
    public Transform[] AllEnemys;
    public Transform[] AllEnemysSpawnPos;
    private void Start() {
        Player.position = PlayerSpawnPos.position;
        for (int i = 0; i < AllEnemysSpawnPos.Length; i++) {
            for (int j = 0; j < AllEnemys.Length; j++) {
                if (AllEnemysSpawnPos[i].GetComponent<EnemySpawnPos>().EnemySpawnType ==
                    AllEnemys[j].GetComponent<EnemyType>().MyType) {
                        Transform enemy = Instantiate(AllEnemys[j], AllEnemysSpawnPos[i].position, Quaternion.identity);
                        enemy.GetComponent<EnemyType>().MyEnemyController.MySpawnPosIndex = i;
                    }
            }
        }
    }
}
