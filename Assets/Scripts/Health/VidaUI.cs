using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VidaUI : MonoBehaviour
{
    [SerializeField] private Vida vida;
    [SerializeField] private TextMeshProUGUI textoVida;
    [SerializeField] private Slider barraVida;
    [SerializeField] private Image preenchimentoBarra;

    private void Start()
    {
        AtualizarUI(vida.VidaAtual, vida.VidaMaxima);

        vida.AoMudarVida += AtualizarUI;
        vida.AoMorrer += MostrarMorte;
    }

    private void AtualizarUI(int vidaAtual, int vidaMaxima)
    {
        textoVida.text = vidaAtual + " / " + vidaMaxima;

        barraVida.maxValue = vidaMaxima;
        barraVida.value = vidaAtual;

        float porcentagemVida = (float)vidaAtual / vidaMaxima;

        if (porcentagemVida > 0.6f)
        {
            preenchimentoBarra.color = Color.green;
        }
        else if (porcentagemVida > 0.3f)
        {
            preenchimentoBarra.color = Color.yellow;
        }
        else
        {
            preenchimentoBarra.color = Color.red;
        }
    }

    private void MostrarMorte()
    {
        textoVida.text = "Morreu";
        preenchimentoBarra.color = Color.red;
        Debug.Log("A vida acabou!");
    }
}