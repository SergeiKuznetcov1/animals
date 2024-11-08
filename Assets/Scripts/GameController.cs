using System;
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
    private void OnEnable() {
        AnimationEvents.OnRespawn += RespawnPlayer;
    }
    private void OnDisable() {
        AnimationEvents.OnRespawn -= RespawnPlayer;
    }

    private void RespawnPlayer()
    {
        Player.gameObject.SetActive(false);
        Player.position = PlayerSpawnPos.position;
        Player.gameObject.SetActive(true);
    }

    private void Start() {
        Player.position = PlayerSpawnPos.position;
        for (int i = 0; i < AllEnemysSpawnPos.Length; i++) {
            for (int j = 0; j < AllEnemys.Length; j++) {
                if (AllEnemysSpawnPos[i].GetComponent<EnemySpawnPos>().EnemySpawnType ==
                    AllEnemys[j].GetComponentInChildren<EnemyType>().MyType) {
                        Transform enemy = Instantiate(AllEnemys[j], AllEnemysSpawnPos[i].position, Quaternion.identity);
                        enemy.GetComponentInChildren<EnemyType>().MyEnemyController.MySpawnPosIndex = i;
                        enemy.GetComponentInChildren<EnemyType>().MyEnemyController.GameController = this;
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
