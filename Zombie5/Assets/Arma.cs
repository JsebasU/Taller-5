using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Arma : MonoBehaviour
{
    public Heroe posArma;
    public GameObject[] balas;
    public GameObject bala;
    public int contador = 0;
    public bool conBalas = true;
    /// <summary>
    /// Cargador Y un pool de objectos
    /// </summary>
    void Start()
    {
        posArma = FindObjectOfType<Heroe>();
        balas = new GameObject[100];

        for (int i = 0; i < balas.Length; i++)
        {
            bala = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bala.AddComponent<Rigidbody>();
            bala.GetComponent<Rigidbody>().useGravity = false;
            bala.GetComponent<Collider>().isTrigger = true;
            bala.transform.position = new Vector3(1000, 1000, 1000);
            bala.transform.localScale = new Vector3(.3f, .3f, .3f);
            bala.tag = "Bala";
            bala.SetActive(false);
            balas[i] = bala;
        }
    }
    /// <summary>
    /// Restablese el cantidad de bala
    /// </summary>
    void Update()
    {
        if (conBalas)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                balas[contador].transform.position = new Vector3(posArma.arma.transform.position.x, posArma.arma.transform.position.y, posArma.arma.transform.position.z);
                balas[contador].transform.rotation = posArma.arma.transform.rotation;
                balas[contador].SetActive(true);
                balas[contador].GetComponent<Rigidbody>().AddForce(transform.up * 500f);
                contador += 1;
            }
        }

        if (contador == 100)
            conBalas = false;
    }
}
