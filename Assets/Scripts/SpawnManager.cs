﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private int _maxZombies = 50;
    [SerializeField]
    private float _spawnRadius = 20.0f;
    [SerializeField]
    private float _minSpawnDistance = 50.0f;
    [SerializeField]
    private float _maxSpawnDistance = 200.0f;

    [SerializeField]
    private GameObject[] _zombiePrefabs;
    private List<GameObject> _zombies = new List<GameObject>();

    [SerializeField]
    private GameObject[] _spawnPoints;

    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private GameObject _scoreUI;

    private ScoreController _scoreCtrl;

    // Start is called before the first frame update
    void Start()
    {
        _scoreCtrl = _scoreUI.GetComponent<ScoreController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        CheckForDestroyed();
        HandleSpawning();
    }

    private void CheckForDestroyed()
    {
        GameObject[] zombies = _zombies.ToArray();
        foreach(GameObject zombie in zombies)
        {
            if (zombie == null)
            {
                _zombies.Remove(zombie);
            }
        }
    }

    private void HandleSpawning()
    {
        if (_zombies.Count >= _maxZombies) return;

        List<GameObject> validSpawns = new List<GameObject>();

        foreach(GameObject spawnPoint in _spawnPoints)
        {
            float dist = Vector3.Distance(_player.transform.position, spawnPoint.transform.position);

            if (dist > _minSpawnDistance && dist < _maxSpawnDistance) validSpawns.Add(spawnPoint);
        }

        if (validSpawns.Count == 0) return;

        int index = 0;
        float xOffset = 0;
        float zOffset = 0;

        while(_zombies.Count < _maxZombies)
        {
            index = Random.Range(0, validSpawns.Count);
            xOffset = Random.Range(-_spawnRadius, _spawnRadius);
            zOffset = Random.Range(-_spawnRadius, _spawnRadius);

            Vector3 spawnLoc = validSpawns[index].transform.position;
            spawnLoc.x += xOffset;
            spawnLoc.y += 0.5f;
            spawnLoc.z += zOffset;

            if (Physics.CheckBox(spawnLoc, new Vector3(0.25f, 0.5f, 0.25f))) continue;

            GameObject zombie = Instantiate(_zombiePrefabs[Random.Range(0, _zombiePrefabs.Length)], spawnLoc, Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0));
            zombie.GetComponent<ZombieController>().SetScoreController(_scoreCtrl);
            _zombies.Add(zombie);
        }
    }
}
