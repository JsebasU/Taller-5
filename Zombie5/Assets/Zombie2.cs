using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using villa = NPC.Ally;

namespace NPC
{
    namespace Enemy2
    {

        /// <summary>
        /// le damos sus edades, sus objetivos y gustos
        /// </summary>
        public sealed class Zombie2 : MonoBehaviour
        {
            public Heroe laVida;
            public DatosZombie datosZombie;
            bool Infected = false;
            public string Gus;
            public int D = 0;
            public float edad = 0;
            public float tiempo = 0;
            public bool mirar = false;
            public float porsuingSpeed;
            public Estado zombieEstado;
            public Vector3 direccion;
            float distancia;
            float distanciaH;
            public bool pursuingState = false;
            GameObject Target, heroe;
            GameObject[] aldeanos;
            /// <summary>
            /// Su forma de buscar una presa
            /// </summary>
            /// <returns></returns>
            IEnumerator buscaAldeanos()
            {
                heroe = GameObject.FindGameObjectWithTag("Hero");
                aldeanos = GameObject.FindGameObjectsWithTag("Villager");
                foreach (GameObject item in aldeanos)
                {
                    yield return new WaitForEndOfFrame();
                    villa.Villager componenteAldeano = item.GetComponent<villa.Villager>();
                    if (componenteAldeano != null)
                    {
                        distanciaH = Mathf.Sqrt(Mathf.Pow((heroe.transform.position.x - transform.position.x), 2) + Mathf.Pow((heroe.transform.position.y - transform.position.y), 2) + Mathf.Pow((heroe.transform.position.z - transform.position.z), 2));
                        distancia = Mathf.Sqrt(Mathf.Pow((item.transform.position.x - transform.position.x), 2) + Mathf.Pow((item.transform.position.y - transform.position.y), 2) + Mathf.Pow((item.transform.position.z - transform.position.z), 2));
                        if (!pursuingState)
                        {

                            if (distancia < 5f)
                            {
                                zombieEstado = Estado.Pursuing;
                                Target = item;
                                pursuingState = true;
                            }
                            else if (distanciaH < 5f)
                            {
                                zombieEstado = Estado.Pursuing;
                                Target = heroe;
                                pursuingState = true;
                            }
                        }
                        if (distancia < 5f && distanciaH < 5f)
                        {
                            Target = item;
                        }
                    }
                }

                if (pursuingState)
                {
                    if (distancia > 5f && distanciaH > 5f)
                    {
                        pursuingState = false;
                    }
                }

                yield return new WaitForSeconds(0.1f);
                StartCoroutine(buscaAldeanos());
            }

            // gustos del zombie
            public enum Gusto
            {
                Femur, Omoplato, Craneo, Pulgares, Falos
            }

            //estados del zombie
            public enum Estado
            {
                Moving, Idle, Rotating, Pursuing
            }

            /// <summary>
            /// asignacion de los datos basicos y sus respectivos rb
            /// </summary>
            void Start()
            {
                if (!Infected)
                {
                    edad = (int)Random.Range(15, 101);
                    datosZombie = new DatosZombie();
                    Rigidbody rb;
                    rb = this.gameObject.AddComponent<Rigidbody>();
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                    rb.useGravity = false;
                    this.gameObject.name = "Zombie";
                }
                else
                {
                    edad = datosZombie.edad;
                    this.gameObject.name = datosZombie.nombre;                    
                }
                StartCoroutine(buscaAldeanos());
                laVida = GameObject.FindGameObjectWithTag("Hero").GetComponent<Heroe>();
                porsuingSpeed = 5 / edad;
                this.gameObject.tag = "Esqueleto";
                Gusto gusto;
                gusto = (Gusto)Random.Range(0, 5);
                Gus = gusto.ToString();
                datosZombie.gusto = Gus;
                this.gameObject.GetComponent<Renderer>().material.color = Color.magenta;

            }
            /// <summary>
            /// estados aleatorios
            /// </summary>
            void Update()
            {
                tiempo += Time.deltaTime;
                if (!pursuingState)
                {
                    if (tiempo >= 3)
                    {
                        D = Random.Range(0, 3);
                        mirar = true;
                        tiempo = 0;
                        if (D == 0)
                        {
                            zombieEstado = Estado.Idle;
                        }
                        else if (D == 1)
                        {
                            zombieEstado = Estado.Moving;
                        }
                        else if (D == 2)
                        {
                            zombieEstado = Estado.Rotating;
                        }
                    }
                }

                // Casos de acciones
                switch (zombieEstado)
                {
                    case Estado.Idle:
                        break;

                    case Estado.Moving:
                        if (mirar)
                        {
                            this.gameObject.transform.Rotate(0, Random.Range(0, 361), 0);
                        }
                        this.gameObject.transform.Translate(0, 0, 0.05f);
                        mirar = false;
                        break;

                    case Estado.Rotating:
                        this.gameObject.transform.Rotate(0, Random.Range(1, 50), 0);
                        break;

                    case Estado.Pursuing:
                        direccion = Vector3.Normalize(Target.transform.position - transform.position);
                        transform.position += direccion * porsuingSpeed;
                        break;
                }
            }

            /// <summary>
            /// collision encargada de la infeccion de los humanos y la condicion de perder
            /// </summary>
            private void OnCollisionEnter(Collision collision)
            {
                if (collision.gameObject.tag == "Villager")
                {
                    collision.gameObject.AddComponent<Zombie2>().datosZombie = collision.gameObject.GetComponent<NPC.Ally.Villager>().datosAldeano;
                    collision.gameObject.GetComponent<Zombie2>().Infected = true;
                    Destroy(collision.gameObject.GetComponent<NPC.Ally.Villager>());
                }

                if (collision.gameObject.tag == "Hero")
                {
                    laVida.vida -= 20;
                }
            }

            private void OnTriggerEnter(Collider other)
            {
                if (other.gameObject.tag == "Bala")
                {
                    this.gameObject.tag = "Muerto";
                    this.gameObject.SetActive(false);
                    other.gameObject.SetActive(false);
                }
            }
        }
    }
}
