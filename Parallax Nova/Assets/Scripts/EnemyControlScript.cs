﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyControlScript : MonoBehaviour
{

    private float timer, speed, deathTimer;
    private int HP = 3;
    private bool dead = false;
    public GameObject bullet;
    AudioSource blasterShotSound, deathSound;

    void Start()
    {
        AudioSource[] audios = GetComponents<AudioSource>();
        blasterShotSound = audios[0];
        deathSound = audios[1];
        deathTimer = 1;
    }

    private void Awake()
    {
        timer = Random.Range(1, 6);
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            speed = 0.3f;
        }
        else if (SceneManager.GetActiveScene().name == "Level2")
        {
            speed = 0.6f;
        }
        else if (SceneManager.GetActiveScene().name == "Level3")
        {
            speed = 0.1f;
        }
    }

    void Update()
    {
        if (!dead)
        {
            blasterShotSound.volume = SettingsScript.sfxVolume;
            deathSound.volume = SettingsScript.sfxVolume;

            EnemyMove();

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z), Quaternion.Euler(90f, 0f, 0f));
                blasterShotSound.Play();
                timer = Random.Range(0, 6);
            }
        }
        else
        {
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void EnemyMove()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - (speed * Time.deltaTime), transform.position.z);

        if (transform.position.y < -1.4f)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControlScript>().MassiveDamagePlayer();
            Destroy(gameObject);
        }
    }

    public void DamageEnemy()
    {
        HP--;
        if (HP <= 0)
        {
            GameObject.FindGameObjectWithTag("ScoreText").GetComponent<UIControlScript>().AddScore();
            if (!deathSound.isPlaying)
            {
                deathSound.Play();
            }
            dead = true;
        }
    }
}
