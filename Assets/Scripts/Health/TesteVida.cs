using UnityEngine;

public class TesteVida : MonoBehaviour
{
    private Vida vida;

    private void Start()
    {
        vida = GetComponent<Vida>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            vida.SubtrairVida(10);
            Debug.Log("Tomou dano");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            vida.AdicionarVida(10);
            Debug.Log("Curou");
        }
    }
}