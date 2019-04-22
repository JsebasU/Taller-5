using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using zom = NPC.Enemy;
using zom2 = NPC.Enemy2;
using villa = NPC.Ally;

/// <summary>
/// calculamos las distancias con respecto a las demas entidades
/// </summary>
public sealed class Heroe : MonoBehaviour
{
    float distancia;
    float distanciaZ;
    public float time;
    public int vida;
    public TextMeshProUGUI textoZombie;
    public TextMeshProUGUI textoZombie2;
    public TextMeshProUGUI textoAldeano;
    public TextMeshProUGUI vidaHeroe;
    GameObject[] aldeanos, zombie, zombie2;
    public GameObject arma;
    DatosVillager datosAldeano = new DatosVillager();
    DatosZombie datosZombie = new DatosZombie();

    /// <summary>
    /// asignamos datos basicos del heroe
    /// </summary>
    void Start()
    {
        arma = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        arma.transform.position = new Vector3(this.gameObject.transform.position.x + .313f, this.gameObject.transform.position.y - .224f, this.gameObject.transform.position.z + .232f);
        arma.transform.localScale = new Vector3(.3f, .3f, .3f);
        arma.transform.Rotate(90, 0, 0);
        arma.transform.SetParent(this.gameObject.transform);
        arma.AddComponent<Arma>();
        vida = 100;
        Rigidbody hero = this.gameObject.AddComponent<Rigidbody>();
        this.gameObject.tag = "Hero";
        this.gameObject.name = "Hero";
        hero.constraints = RigidbodyConstraints.FreezeAll;
        hero.useGravity = false;
        StartCoroutine(BuscaEntidades());
        textoZombie = GameObject.FindGameObjectWithTag("TextZombie").GetComponent<TextMeshProUGUI>();
        textoZombie2 = GameObject.FindGameObjectWithTag("TextZombie2").GetComponent<TextMeshProUGUI>();
        textoAldeano = GameObject.FindGameObjectWithTag("TextAldeano").GetComponent<TextMeshProUGUI>();
        vidaHeroe = GameObject.FindGameObjectWithTag("TextVida").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// contamos un tiempo para hub
    /// </summary>
    public void Update()
    {
        time += Time.fixedDeltaTime;
        vidaHeroe.text = vida.ToString();

        if (vida <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Medikit")
        {
            vida += 5;
            if (vida >= 100)
                vida = 100;
            other.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Indica  lo del hub por distancia
    /// </summary>
    /// <returns></returns>
    IEnumerator BuscaEntidades()
    {
        zombie = GameObject.FindGameObjectsWithTag("Zombie");
        zombie2 = GameObject.FindGameObjectsWithTag("Esqueleto");
        aldeanos = GameObject.FindGameObjectsWithTag("Villager");

        // retroalimentacion para el aldeano
        foreach (GameObject item in aldeanos)
        {
            yield return new WaitForEndOfFrame();
            villa.Villager componenteAldeano = item.GetComponent<villa.Villager>();
            if (componenteAldeano != null)
            {              
                distancia = Mathf.Sqrt(Mathf.Pow((item.transform.position.x - transform.position.x), 2) + Mathf.Pow((item.transform.position.y - transform.position.y), 2) + Mathf.Pow((item.transform.position.z - transform.position.z), 2));
                if (distancia < 5f)
                {
                    time = 0;
                    datosAldeano = item.GetComponent<villa.Villager>().datosAldeano;
                    textoAldeano.text = "Hola, soy " + datosAldeano.nombre + " y tengo " + datosAldeano.edad.ToString() + " años";
                }
                if (time > 3)
                {
                    textoAldeano.text = " ";
                }
            }
        }

        // retroalimentacion para el zombie
        foreach (GameObject itemZ in zombie)
        {
            yield return new WaitForEndOfFrame();
            zom.Zombie componenteZombie = itemZ.GetComponent<zom.Zombie>();
            if (componenteZombie != null)
            {              
                distanciaZ = Mathf.Sqrt(Mathf.Pow((itemZ.transform.position.x - transform.position.x), 2) + Mathf.Pow((itemZ.transform.position.y - transform.position.y), 2) + Mathf.Pow((itemZ.transform.position.z - transform.position.z), 2));
                if (distanciaZ < 5f)
                {
                    time = 0;
                    datosZombie = itemZ.GetComponent<zom.Zombie>().datosZombie;
                    textoZombie.text = "Waaaarrrr quiero comer " + datosZombie.gusto;
                }
                if (time > 3)
                {
                    textoZombie.text = " ";
                }
            }
        }

        foreach (GameObject itemZ2 in zombie2)
        {
            yield return new WaitForEndOfFrame();
            zom2.Zombie2 componenteZombie = itemZ2.GetComponent<zom2.Zombie2>();
            if (componenteZombie != null)
            {
                distanciaZ = Mathf.Sqrt(Mathf.Pow((itemZ2.transform.position.x - transform.position.x), 2) + Mathf.Pow((itemZ2.transform.position.y - transform.position.y), 2) + Mathf.Pow((itemZ2.transform.position.z - transform.position.z), 2));
                if (distanciaZ < 5f)
                {
                    time = 0;
                    datosZombie = itemZ2.GetComponent<zom2.Zombie2>().datosZombie;
                    textoZombie2.text = "Waaaarrrr quiero comer " + datosZombie.gusto;
                }
                if (time > 3)
                {
                    textoZombie2.text = " ";
                }
            }
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(BuscaEntidades());
    }

    /// <summary>
    /// Movimiento Con input  
    /// </summary>
    public sealed class MoverH : MonoBehaviour
    {

        Velocidad velocidad;

        private void Start()
        {
            velocidad  = new Velocidad(Random.Range(0.25f, 0.5f));
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.W))
            {
                this.gameObject.transform.Translate(0, 0, velocidad.velo);
            }
            if (Input.GetKey(KeyCode.S))
            {
                this.gameObject.transform.Translate(0, 0, -velocidad.velo);
            }
        }
    }

    /// <summary>
    /// rotacion del personaje por medio inputs
    /// </summary>
    public sealed class MirarH : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                this.gameObject.transform.Rotate(0, -3, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                this.gameObject.transform.Rotate(0, 3, 0);
            }
        }
    }
}

/// <summary>
/// Read only Veloc
/// </summary>
public sealed class Velocidad
{
    public readonly float velo;
    public Velocidad(float vel)
    {
        velo = vel;
    }
}
