using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class UIControl : MonoBehaviour
{
    public GameObject[] carros;
    public TextMeshProUGUI placar;
    public UnityEvent play;
    int contagem = 0;
    public TextMeshProUGUI contagemTxt;
    public bool[] finish;

    // Classe para armazenar as informa��es de cada jogador
    private class Jogador
    {
        public Carro car;
        public string nome;
        public int waypointCount;
    }

    // Lista de jogadores
    [SerializeField]private List<Jogador> jogadores = new List<Jogador>();

    private void Start()
    {
        // Adicionar os jogadores na lista com seus respectivos nomes e pontua��es
        jogadores.Add(new Jogador { nome = "Amarelo", car = carros[0].GetComponent<Carro>() });
        jogadores.Add(new Jogador { nome = "Vermelho", car = carros[1].GetComponent<Carro>() });
        jogadores.Add(new Jogador { nome = "Azul", car = carros[2].GetComponent<Carro>() });
        jogadores.Add(new Jogador { nome = "Roxo", car = carros[3].GetComponent<Carro>() });

        // Classificar a lista de jogadores em ordem decrescente baseado na pontua��o de cada jogador
        jogadores.Sort((j1, j2) => j2.waypointCount.CompareTo(j1.waypointCount));

        /*// Se houver empate na pontua��o, classificar pela dist�ncia ao objeto de refer�ncia
        jogadores.Sort(DesempatarPorDistancia);*/

        // Exibir o placar na tela
        for (int i = 0; i < jogadores.Count; i++)
        {
            Debug.Log((i + 1) + "� Lugar: " + jogadores[i].nome + " - " + jogadores[i].waypointCount/* + " pontos - Dist�ncia ao objeto de refer�ncia: " + jogadores[i].distanciaObjeto*/);
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < jogadores.Count; i++)
        {
            jogadores[i].waypointCount = jogadores[i].car.GetWayPointCount();
        }

        jogadores.Sort((j1, j2) => j2.waypointCount.CompareTo(j1.waypointCount));

        string txt = "";

        for (int i = 0; i < jogadores.Count; i++)
        {
            if (jogadores[i].car.GetFinish() && !finish[i])
            {
                finish[i] = true;
                txt += jogadores[i].nome + "\n" + (i + 1) + "� Lugar: ";
            }
            if (!jogadores[i].car.GetFinish())
            {
                txt += (i + 1) + "� Lugar: " + jogadores[i].nome + "\n";
                jogadores[i].car.SetRanking(i + 1);
            }
        }

        placar.text = txt;
    }

    public void PlayStart()
    {
        play.Invoke();
    }

    public void Contagem()
    {
        if (contagem == 0) contagemTxt.text = "3";
        if (contagem == 1) contagemTxt.text = "2";
        if (contagem == 2) contagemTxt.text = "1";
        if (contagem == 3) contagemTxt.text = "GO!!!";

        contagem++;
    }

    /*private int DesempatarPorDistancia(Jogador j1, Jogador j2)
    {
        // Se os jogadores tiverem a mesma pontua��o, calcular a dist�ncia ao objeto de refer�ncia
        if (j1.waypointCount == j2.waypointCount)
        {
            float distancia1 = Vector3.Distance(j1.car.transform.position, objetoReferencia);
            float distancia2 = Vector3.Distance(j2.car.transform.position, objetoReferencia);

            // Se a dist�ncia de j1 for menor, j1 fica na frente (retorno negativo)
            // Se a dist�ncia de j2 for menor, j2 fica na frente (retorno positivo)
            // Se a dist�ncia for a mesma, manter a ordem atual (retorno 0)
            return distancia1.CompareTo(distancia2);
        }

        // Se a pontua��o for diferente, manter a ordem atual (retorno 0)
        return 0;
    }*/
}
