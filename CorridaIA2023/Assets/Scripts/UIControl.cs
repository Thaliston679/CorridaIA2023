using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    public GameObject[] carros;

    // Classe para armazenar as informações de cada jogador
    private class Jogador
    {
        public string nome;
        public int pontuacao;
    }

    // Lista de jogadores
    [SerializeField]private List<Jogador> jogadores = new List<Jogador>();

    private void Start()
    {
        for (int i = 0; i < carros.Length; i++)
        {

        }
        // Adicionar os jogadores na lista com seus respectivos nomes e pontuações
        jogadores.Add(new Jogador { nome = "Carro 1", pontuacao = 20 });
        jogadores.Add(new Jogador { nome = "Carro 2", pontuacao = 50 });
        jogadores.Add(new Jogador { nome = "Carro 3", pontuacao = 10 });
        jogadores.Add(new Jogador { nome = "Carro 4", pontuacao = 35 });

        // Classificar a lista de jogadores em ordem decrescente baseado na pontuação de cada jogador
        jogadores.Sort((j1, j2) => j2.pontuacao.CompareTo(j1.pontuacao));

        // Exibir o placar na tela
        for (int i = 0; i < jogadores.Count; i++)
        {
            Debug.Log((i + 1) + "º Lugar: " + jogadores[i].nome + " - " + jogadores[i].pontuacao + " pontos");
        }
    }
}
