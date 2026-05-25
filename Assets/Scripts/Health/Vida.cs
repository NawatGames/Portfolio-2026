using UnityEngine;
using System;

public class Vida : MonoBehaviour
{
    [SerializeField] private int vidaMaxima = 100;
    private int vidaAtual;

    public int VidaAtual => vidaAtual;
    public int VidaMaxima => vidaMaxima;

    public event Action<int, int> AoMudarVida;
    public event Action AoMorrer;

    private void Start()
    {
        vidaAtual = vidaMaxima;
        AoMudarVida?.Invoke(vidaAtual, vidaMaxima);
    }

    public void AdicionarVida(int quantidade)
    {
        if (quantidade <= 0 || vidaAtual <= 0) return;

        vidaAtual += quantidade;

        if (vidaAtual > vidaMaxima)
        {
            vidaAtual = vidaMaxima;
        }

        AoMudarVida?.Invoke(vidaAtual, vidaMaxima);
    }

    public void SubtrairVida(int quantidade)
    {
        if (quantidade <= 0 || vidaAtual <= 0) return;

        vidaAtual -= quantidade;

        if (vidaAtual < 0)
        {
            vidaAtual = 0;
        }

        AoMudarVida?.Invoke(vidaAtual, vidaMaxima);

        if (vidaAtual == 0)
        {
            AoMorrer?.Invoke();
        }
    }
}