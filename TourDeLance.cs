using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;
using PixelCrushers.DialogueSystem;

public class TourDeLance : MonoBehaviour
{
    private List<DataCharacters> instantiatedListCharacter = new List<DataCharacters>();
    private int scenarioStep = 0;
    private GameObject leaderboard;
    private GameObject leaderboardLine;
    private List<LeaderboardRank> bestHighScores;
    public GameObject SelectedButton;
    public GameObject SelectedButtonMenu;

    // Start is called before the first frame update
    void Start()
    {
        // Mode exploration par défaut
        try{
            Loader.cameraUI = UnityEngine.GameObject.FindGameObjectWithTag(Params.TagCameraUI).GetComponent<Camera>();
            Loader.cameraMap = UnityEngine.GameObject.FindGameObjectWithTag(Params.TagCameraMap).GetComponent<Camera>();
        }catch(Exception e){
            Debug.LogError("Villarbor - Start() : Il manque une cameraMap, ou une cameraUi ! "+e);
        }

        Loader.gameStatus = GameStatus.Map;
        Loader.scoreEnabled = true;
        Loader.score = "Vide";
        PrepareLeaderboard();
        PositionPlayers();
        InfosGamer();
    }

    // Update is called once per frame
    void Update()
    {
        if(scenarioStep == 0){
            Methods.StartConversation("Xtralife", instantiatedListCharacter, "Itchi", "Dragonnet");
            scenarioStep++;
        }

        if (EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.activeSelf)
        {
            if(leaderboard.activeSelf)
                EventSystem.current.SetSelectedGameObject(SelectedButtonMenu, new BaseEventData(EventSystem.current));
            else if(SelectedButton != null)
			    EventSystem.current.SetSelectedGameObject(SelectedButton, new BaseEventData(EventSystem.current));
        }
        else
        {
            if(leaderboard.activeSelf)
                SelectedButton = SelectedButtonMenu;
            else
                SelectedButton = EventSystem.current.currentSelectedGameObject;
        }
    }

    public void PositionPlayers()
    {
        float PlayerXPosition = 5;
        float PlayerYPosition = -5;

        DataCharacters dragonnet = Loader.ListCharacter["Dragonnet"].Instantiate(PlayerXPosition, PlayerYPosition, PlayerType.PlayerOnMap);
        Destroy(dragonnet.obj.GetComponent<CircleCollider2D>());

        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        cam.GetComponent<CameraController>().TargetObject = dragonnet.obj.transform;

        DataCharacters nello = Loader.ListCharacter["Nello"].Instantiate(PlayerXPosition+2, PlayerYPosition+2, PlayerType.NPCOnMap);
        Methods.AddConversation(nello.obj, "Test", nello.obj, dragonnet.obj);

        DataCharacters itchi = Loader.ListCharacter["Itchi"].Instantiate(PlayerXPosition+2000, PlayerYPosition+2000, PlayerType.Other);

        instantiatedListCharacter.Add(dragonnet);
        instantiatedListCharacter.Add(nello);
        instantiatedListCharacter.Add(itchi);

        Debug.Log("TourDeLance : -PositionPlayers() - Liste de personnages instantiés : "+instantiatedListCharacter.Count);
    }

    void InfosGamer()
    {
        GameObject.FindGameObjectWithTag(Params.TagPanel).GetComponent<Text>().text = Xtralife.currentGamer["profile"]["displayName"];
        Loader.score = GameObject.FindGameObjectWithTag(Params.TagPanel2).GetComponent<Text>().text;
    }

    void PrepareLeaderboard()
    {
        Xtralife.AddXtralife();
        bestHighScores = new List<LeaderboardRank>();
        leaderboard = GameObject.FindGameObjectWithTag(Params.TagCanvas);
        leaderboardLine = GameObject.FindGameObjectWithTag(Params.TagClone);
        SelectedButtonMenu = GameObject.FindGameObjectWithTag(Params.TagButton);
        leaderboard.SetActive(false);
    }

    IEnumerator UpdateLeaderboard()
    {
        // Empêche le joueur de bouger pendant le panel
        GameObject.FindGameObjectWithTag(Params.TagPlayer).GetComponent<ObjectController>().SetWalkAndAct(false);

        // Préparation des données online
        Xtralife.coroutineRun = true;
        StartCoroutine(Loader.cloudObj.GetLeaderboard(value => bestHighScores = value));
        while(Xtralife.coroutineRun){
            yield return new WaitForSeconds(0.1f);
        }

        // Affichage des données à l'écran
        Destroy(GameObject.FindGameObjectWithTag(Params.TagDelete));
        foreach(LeaderboardRank highScore in bestHighScores){
            GameObject newLine = Instantiate(leaderboardLine);
            newLine.tag = Params.TagVide;
            newLine.transform.SetParent(leaderboardLine.transform.parent);
            Text[] infos = newLine.GetComponentsInChildren<Text>();
            infos[0].text = highScore.rank.ToString();
            infos[1].text = highScore.name.ToString();
            infos[2].text = highScore.score.ToString();
        }
    }

    public void EndLevel()
    {
        StartCoroutine(RetourMenu());
    }

    public IEnumerator RetourMenu()
    {
        // On déconnecte le compte joueur et on revient au menu
        StartCoroutine(Loader.cloudObj.Logout());
        yield return new WaitForSeconds(0.6f);
        Loader.LoadScene("MenuPrincipal");
    }
}

public class LeaderboardRank
{
    public int rank;
    public string name;
    public long score;
    public LeaderboardRank(int rank, string name, long score){
        this.rank = rank;
        this.name = name;
        this.score = score;
    }
}