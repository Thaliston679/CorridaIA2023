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

    // Classe para armazenar as informações de cada jogador
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
        // Adicionar os jogadores na lista com seus respectivos nomes e pontuações
        jogadores.Add(new Jogador { nome = "Amarelo", car = carros[0].GetComponent<Carro>() });
        jogadores.Add(new Jogador { nome = "Vermelho", car = carros[1].GetComponent<Carro>() });
        jogadores.Add(new Jogador { nome = "Azul", car = carros[2].GetComponent<Carro>() });
        jogadores.Add(new Jogador { nome = "Roxo", car = carros[3].GetComponent<Carro>() });

        // Classificar a lista de jogadores em ordem decrescente baseado na pontuação de cada jogador
        jogadores.Sort((j1, j2) => j2.waypointCount.CompareTo(j1.waypointCount));

        /*// Se houver empate na pontuação, classificar pela distância ao objeto de referência
        jogadores.Sort(DesempatarPorDistancia);*/

        // Exibir o placar na tela
        for (int i = 0; i < jogadores.Count; i++)
        {
            Debug.Log((i + 1) + "º Lugar: " + jogadores[i].nome + " - " + jogadores[i].waypointCount/* + " pontos - Distância ao objeto de referência: " + jogadores[i].distanciaObjeto*/);
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
                txt += jogadores[i].nome + "\n" + (i + 1) + "º Lugar: ";
            }
            if (!jogadores[i].car.GetFinish())
            {
                txt += (i + 1) + "º Lugar: " + jogadores[i].nome + "\n";
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
        // Se os jogadores tiverem a mesma pontuação, calcular a distância ao objeto de referência
        if (j1.waypointCount == j2.waypointCount)
        {
            float distancia1 = Vector3.Distance(j1.car.transform.position, objetoReferencia);
            float distancia2 = Vector3.Distance(j2.car.transform.position, objetoReferencia);

            // Se a distância de j1 for menor, j1 fica na frente (retorno negativo)
            // Se a distância de j2 for menor, j2 fica na frente (retorno positivo)
            // Se a distância for a mesma, manter a ordem atual (retorno 0)
            return distancia1.CompareTo(distancia2);
        }

        // Se a pontuação for diferente, manter a ordem atual (retorno 0)
        return 0;
    }*/
}
