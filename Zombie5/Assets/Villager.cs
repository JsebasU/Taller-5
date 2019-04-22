
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zom = NPC.Enemy;
using zom2 = NPC.Enemy2;

namespace NPC
{
    namespace Ally
    {
        /// <summary>
        /// le damos su ID  y una velocidad 
        /// </summary>
        public sealed class Villager : MonoBehaviour
        {
            public DatosVillager datosAldeano = new DatosVillager();
            public Estado aldeanoEstado;
            float tiempo;
            public float distancia;
            public float runningSpeed;
            public float edad;
            int D;
            public bool runningState = false;
            bool mirar = false;
            public Vector3 direccion;
            GameObject Target;
            GameObject[] zombie, Esqueleto;

            // los nombres de los aldeanos
            public enum nombres
            {
                Carlos, Sebastian, Eduardo, Daniel, Cata,
                Danilo, Felipe, Tatiana, Juan, Ronald,
                Geremias, Rene, Eugenia, Eulari, Gala,
                Gurtza, Gudula, Hebe, Fara, Fedora
            }

            // estados 
            public enum Estado
            {
                Idle, Moving, Rotating, Running
            }

            /// <summary>
            /// Designar el escape si se acerca zombie
            /// </summary>
            /// <returns></returns>
            IEnumerator buscaZombies()
            {
                zombie = GameObject.FindGameObjectsWithTag("Zombie");
                Esqueleto = GameObject.FindGameObjectsWithTag("Esqueleto");
                foreach (GameObject item in zombie)
                {
                    zom.Zombie componenteZombie = item.GetComponent<zom.Zombie>();
                    if (componenteZombie != null)
                    {
                        distancia = Mathf.Sqrt(Mathf.Pow((item.transform.position.x - transform.position.x), 2) + Mathf.Pow((item.transform.position.y - transform.position.y), 2) + Mathf.Pow((item.transform.position.z - transform.position.z), 2));
                        if (!runningState)
                        {
                            if (distancia < 5f)
                            {
                                aldeanoEstado = Estado.Running;
                                Target = item;
                                runningState = true;
                            }
                        }
                    }
                }

                foreach (GameObject item in Esqueleto)
                {
                    zom2.Zombie2 componenteZombie = item.GetComponent<zom2.Zombie2>();
                    if (componenteZombie != null)
                    {
                        distancia = Mathf.Sqrt(Mathf.Pow((item.transform.position.x - transform.position.x), 2) + Mathf.Pow((item.transform.position.y - transform.position.y), 2) + Mathf.Pow((item.transform.position.z - transform.position.z), 2));
                        if (!runningState)
                        {
                            if (distancia < 5f)
                            {
                                aldeanoEstado = Estado.Running;
                                Target = item;
                                runningState = true;
                            }
                        }
                    }
                }

                if (runningState)
                {
                    if (distancia > 5f)
                    {
                        runningState = false;
                    }
                }

                yield return new WaitForSeconds(0.1f);
                StartCoroutine(buscaZombies());
            }
            /// <summary>
            /// Designar los estandares a aldeano
            /// </summary>
            void Start()
            {
                Rigidbody Villa;
                this.gameObject.tag = "Villager";
                Villa = this.gameObject.AddComponent<Rigidbody>();
                Villa.constraints = RigidbodyConstraints.FreezeAll;
                Villa.useGravity = false;
                nombres nombre;
                nombre = (nombres)Random.Range(0, 20);
                datosAldeano.nombre = nombre.ToString();
                edad = (int)Random.Range(15, 101);
                datosAldeano.edad = (int)edad;
                runningSpeed = 10 / edad;
                this.gameObject.name = nombre.ToString();
                StartCoroutine(buscaZombies());
            }

            /// <summary>
            /// Toma Estado aleaterio
            /// </summary>
            void Update()
            {
                tiempo += Time.deltaTime;
                if (!runningState)
                {
                    if (tiempo >= 3)
                    {
                        D = Random.Range(0, 3);
                        mirar = true;
                        tiempo = 0;
                        if (D == 0)
                        {
                            aldeanoEstado = Estado.Idle;
                        }
                        else if (D == 1)
                        {
                            aldeanoEstado = Estado.Moving;
                        }
                        else if (D == 2)
                        {
                            aldeanoEstado = Estado.Rotating;

                        }
                    }
                }

                switch (aldeanoEstado)
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

                    case Estado.Running:
                        direccion = Vector3.Normalize(Target.transform.position - transform.position);
                        transform.position -= direccion * runningSpeed;
                        break;
                }
            }
        }
    }
}

