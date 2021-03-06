﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//()안의 컴포넌트를 반드시 가지고 있어야만함
//없다면 자동으로 원하는 컴포넌트를 추가한다
//반드시 필요한 컴포넌트를 실수로 삭제할 수도 있기 때문에 강제로 붙어있게 만들어줌
[RequireComponent(typeof(Rigidbody))]

public class Enemy : MonoBehaviour
{
    //에너미의 역할
    //위에서 아래로 떨어진다
    //에너미가 플레이어를 향해서 총알 발사

    [SerializeField] float speed = 5.0f;
    [SerializeField] GameObject bloodFx;

    //충돌처리 - Rigidbody 사용

    private float cameraHeight;

    private void Start()
    {
        cameraHeight = Camera.main.orthographicSize;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Die();

            collision.gameObject.GetComponent<Player>().Die();
            //collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Dead Zone")
        {
            gameObject.SetActive(false);
            GameObject.Find("GameManager").GetComponent<GameManager>().enemyPool.Enqueue(gameObject);
        }
        else if (other.tag == "Bullet")
        {
            Die();
        }
    }

    public void Die()
    {
        GameObject fx = Instantiate(bloodFx);
        fx.transform.position = transform.position;
        ScoreManager.Instance.addScore();

        gameObject.SetActive(false);
        GameObject.Find("GameManager").GetComponent<GameManager>().enemyPool.Enqueue(gameObject);
    }
}
