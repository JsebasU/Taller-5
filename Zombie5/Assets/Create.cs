using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using zom = NPC.Enemy;
using zom2 = NPC.Enemy2;
using villa = NPC.Ally;

public sealed class Create : MonoBehaviour
{
    public TextMeshProUGUI numeroZombies;
    public TextMeshProUGUI numeroZombies2;
    public TextMeshProUGUI numeroAldeanos;
    public int numZombies;
    public int numZombies2;
    public int numAldeanos;
    public GameObject[] Zom2,Zom,Villa;
    public bool sinZom = false;
    public bool sinZom2 = false;
    void Start()
    {
        new CrearInstancias();

    }


    /// <summary>
    /// Hub concantidades de object aliados y enemigos
    /// </summary>
    private void Update()
    {
        Zom = GameObject.FindGameObjectsWithTag("Zombie");
        Zom2 = GameObject.FindGameObjectsWithTag("Esqueleto");
        Villa = GameObject.FindGameObjectsWithTag("Villager");

        foreach (GameObject item in Zom)
        {
            numZombies = Zom.Length;
        }
        foreach (GameObject item2 in Zom2)
        {
            numZombies2 = Zom2.Length;
        }
        foreach (GameObject item in Villa)
        {
            numAldeanos = Villa.Length;
        }

        if (Zom.Length == 0)
        {
            numeroZombies.text = 0.ToString();
            sinZom = true;
        }
        else
        {
            numeroZombies.text = numZombies.ToString();
        }

        if (Zom2.Length == 0)
        {
            numeroZombies2.text = 0.ToString();
            sinZom2 = true;
        }
        else
        {
            numeroZombies2.text = numZombies2.ToString();
        }

        if (Villa.Length == 0)
        {
            numeroAldeanos.text = 0.ToString();
        }
        else
        {
            numeroAldeanos.text = numAldeanos.ToString();
        }
        
        numeroZombies.text = numZombies.ToString();
        numeroZombies2.text = numZombies2.ToString();

        if (sinZom && sinZom2 == true)
        {
            SceneManager.LoadScene(0);
        }
    }
}
/// <summary>
/// Objects con su respectivo 
/// </summary>
 class CrearInstancias 
{
    public GameObject cube;
    public GameObject Botiquin;
    public readonly int minInstancias = Random.Range(5, 16);
    int selector = 0;
    public int tipoZombie;
    const int MAX = 26;
    public CrearInstancias()
    {
        for (int i = 0; i < 3; i++)
        {
            Botiquin = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            Botiquin.tag = "Medikit";
            Botiquin.AddComponent<Rigidbody>().useGravity = false;
            Botiquin.GetComponent<Collider>().isTrigger = true;
            Botiquin.transform.Rotate(90, 0, 0);
            Botiquin.GetComponent<Renderer>().material.color = Color.blue;
            Botiquin.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));
        }

        for (int i = 0; i < Random.Range(minInstancias,MAX); i++)
        {
            
            if (selector == 0)
            {
                cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.AddComponent<Camera>();
                cube.AddComponent<Heroe>();
                cube.AddComponent<Heroe.MirarH>();
                cube.AddComponent<Heroe.MoverH>();
                cube.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));
                selector += 1;
            }

            int selec = Random.Range(selector, 3);

            if (selec == 1)
            {
                cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.AddComponent<villa.Villager>();
                cube.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));
            }
            if (selec == 2)
            {
                cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                tipoZombie = Random.Range(0, 2);
                if(tipoZombie == 0)
                    cube.AddComponent<zom.Zombie>();
                else
                    cube.AddComponent<zom2.Zombie2>();
                cube.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));
            }
        }
    }
}