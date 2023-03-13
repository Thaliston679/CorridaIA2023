using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour
{
    public GameObject[] carros;
    public TextMeshProUGUI placar;
    public UnityEvent play;
    int contagem = 0;
    public TextMeshProUGUI contagemTxt;
    public bool[] finish;

    //Vitoria
    bool victory;
    public GameObject panelInGame;
    public GameObject panelVictory;
    public TextMeshProUGUI[] nameCar;
    public TextMeshProUGUI[] timeCar;

    //Inicio
    public int maxLaps;
    public TMP_Dropdown selectMaxLaps;

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
        jogadores.Add(new Jogador { nome = "Carro Amarelo", car = carros[0].GetComponent<Carro>() });
        jogadores.Add(new Jogador { nome = "Carro Vermelho", car = carros[1].GetComponent<Carro>() });
        jogadores.Add(new Jogador { nome = "Carro Azul", car = carros[2].GetComponent<Carro>() });
        jogadores.Add(new Jogador { nome = "Carro Roxo", car = carros[3].GetComponent<Carro>() });

        // Classificar a lista de jogadores em ordem decrescente baseado na pontuação de cada jogador
        jogadores.Sort((j1, j2) => j2.waypointCount.CompareTo(j1.waypointCount));

        for (int i = 0; i < jogadores.Count; i++)
        {
            jogadores[i].car.maxLap = maxLaps;
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

    private void Update()
    {
        if(finish[0] && finish[1] && finish[2] && finish[3] && !victory)
        {
            victory = true;
            panelVictory.SetActive(true);
            panelInGame.SetActive(false);
            for(int i = 0; i < jogadores.Count; i++)
            {
                nameCar[i].text = jogadores[i].nome;
                timeCar[i].text = jogadores[i].car.timeCarString;
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void SetMaxLaps()
    {
        if(selectMaxLaps.value == 0) maxLaps = 3;
        if (selectMaxLaps.value == 1) maxLaps = 5;
        if (selectMaxLaps.value == 2) maxLaps = 7;
        if (selectMaxLaps.value == 3) maxLaps = 10;

        for (int i = 0; i < jogadores.Count; i++)
        {
            jogadores[i].car.maxLap = maxLaps;
        }

        string txt = "";

        for (int i = 0; i < jogadores.Count; i++)
        {
            txt += 1 + "º Lugar: " + jogadores[i].nome + "\n";
            jogadores[i].car.JpGUI();
        }

        placar.text = txt;
    }
}
