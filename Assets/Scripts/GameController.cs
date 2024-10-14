using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform PlayerSpawnPos;
    public Transform Player;
    public Transform[] AllEnemys;
    public Transform[] AllEnemysSpawnPos;
    public float InitTimeToRespawn;
    public float _currentTimeToRespawn;
    private void Start() {
        Player.position = PlayerSpawnPos.position;
        for (int i = 0; i < AllEnemysSpawnPos.Length; i++) {
            for (int j = 0; j < AllEnemys.Length; j++) {
                if (AllEnemysSpawnPos[i].GetComponent<EnemySpawnPos>().EnemySpawnType ==
                    AllEnemys[j].GetComponent<EnemyType>().MyType) {
                        Transform enemy = Instantiate(AllEnemys[j], AllEnemysSpawnPos[i].position, Quaternion.identity);
                        enemy.GetComponent<EnemyType>().MyEnemyController.MySpawnPosIndex = i;
                        enemy.GetComponent<EnemyType>().MyEnemyController.GameController = this;
                    }
            }
        }
    }

    public void StartCountdownToRes(int spawnPosId, GameObject animalToRespawn) {
        StartCoroutine(RespawnAnimal(spawnPosId, animalToRespawn));
    }
    private IEnumerator RespawnAnimal(int spawnPosId, GameObject animalToRespawn) {
        _currentTimeToRespawn = InitTimeToRespawn;
        while (_currentTimeToRespawn > 0) {
            _currentTimeToRespawn -= Time.deltaTime;
            yield return null;
        }
        animalToRespawn.GetComponentInChildren<EnemyController>().ResetAnimal();
    }
}
