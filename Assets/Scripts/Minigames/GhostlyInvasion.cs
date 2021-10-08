using System.Collections;
using UnityEngine;

public class GhostlyInvasion : MonoBehaviour
{
    public GameObject[] spirits;
    public int maxSpirits = 3;
    public int spiritCount = 0;
    public float spawnFrequency = 5f;
    private float _spawnCountdown = 0f;
    private bool _isSpawning;
    private Transform _person;

    [Header("Spawn Area Fields")]
    [Tooltip("Range of spawning enemies")]
    [SerializeField] private float _spawnRange = 10f;
    [Tooltip("Offset from person to start spawning enemies")]
    [SerializeField] private float _offset = 10f;

    private void Start() {
        _person = GameObject.FindGameObjectWithTag("Person").GetComponent<Transform>();
    }
    private void Update() {
        if(_spawnCountdown <= 0) {
            if(!_isSpawning) {
                StartCoroutine(SpawnSpirits());
            }
        } else {
            _spawnCountdown -= Time.deltaTime;
        }

        spiritCount = GameObject.FindGameObjectsWithTag("Spirit").Length;
    }

    private IEnumerator SpawnSpirits() {
        _isSpawning = true;
        while(spiritCount< maxSpirits) {
            SpawnSpirit();
            yield return new WaitForSeconds(spawnFrequency);
        }
        _isSpawning = false;
        yield break;
    }

    private void SpawnSpirit() {
        int index = Random.Range(0, spirits.Length);
        // returns 0 or 1 for direction
        float xDir = Random.Range(0, 2)==0? -1: 1;
        float yDir = Random.Range(0, 2)==0? -1: 1;
        
        float xPos = Random.Range(_person.position.x + _offset, 
                                    _person.position.x + (_spawnRange * xDir));
        float yPos = Random.Range(_person.position.y + _offset, 
                                    _person.position.y + (_spawnRange * yDir));

        GameObject spirit = spirits[index];
        Instantiate(spirit, new Vector2(xPos, yPos), spirit.transform.rotation, gameObject.transform);
        spiritCount++;
    }
}
