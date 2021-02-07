using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using System;

public class MenuPrincipal : MonoBehaviour
{
    public GameObject SelectedButton;
    public string loginMethod;
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;
    private bool runCoroutine = false;

    void Awake()
    {
        loginMethod = string.Empty;
        panel1 = GameObject.FindGameObjectWithTag(Params.TagPanel);
        panel2 = GameObject.FindGameObjectWithTag(Params.TagPanel2);
        panel3 = GameObject.FindGameObjectWithTag(Params.TagPanel3);
        panel2.SetActive(false);
        panel3.SetActive(false);
    }
    
    void Update()
    {
        // Gestion du retour en cas de sous-menu
        if((panel2.activeSelf || panel3.activeSelf) && Input.GetButtonDown(Params.InputRetourMenu)){
            Retour();
        }

        if (EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.activeSelf)
        {
            if(SelectedButton != null)
			    EventSystem.current.SetSelectedGameObject(SelectedButton, new BaseEventData(EventSystem.current));
            else{
                if(panel1.activeSelf)
                    EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag(Params.TagPanel).GetComponentsInChildren<Button>()[0].gameObject, new BaseEventData(EventSystem.current));
                else if(panel2.activeSelf)
                    EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag(Params.TagPanel2).GetComponentsInChildren<Button>()[0].gameObject, new BaseEventData(EventSystem.current));
                else if(panel3.activeSelf)
                    EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag(Params.TagPanel3).GetComponentsInChildren<InputField>()[0].gameObject, new BaseEventData(EventSystem.current));
            }
        }
        else
        {
            if(panel1.activeSelf && (SelectedButton == null || !SelectedButton.activeSelf))
                EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag(Params.TagPanel).GetComponentsInChildren<Button>()[0].gameObject, new BaseEventData(EventSystem.current));
            else if(panel2.activeSelf && (SelectedButton == null || !SelectedButton.activeSelf))
                EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag(Params.TagPanel2).GetComponentsInChildren<Button>()[0].gameObject, new BaseEventData(EventSystem.current));
            else if(panel3.activeSelf && (SelectedButton == null || !SelectedButton.activeSelf))
                EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag(Params.TagPanel3).GetComponentsInChildren<InputField>()[0].gameObject, new BaseEventData(EventSystem.current));
            SelectedButton = EventSystem.current.currentSelectedGameObject;
        }

        // Gestion navigation des input fields
        if(EventSystem.current.currentSelectedGameObject.TryGetComponent<InputField>(out InputField component)){
            if(Input.GetButtonDown(Params.InputValiderMenu) || Input.GetAxis(Params.InputVerticalMenu) < 0){
                StartCoroutine(NavigationInputFields(true));
            }else if(Input.GetAxis(Params.InputVerticalMenu) > 0){
                StartCoroutine(NavigationInputFields(false));
            }
        }
	}

    public void LoadNewGame(){
        BlackFade.instance.PlayFadeOutBlackChangeLevel("Intro");
    }

    public void LoadSaveGame(){
        Debug.Log("Sauvegardes non prises en charge pour l'instant.");
    }

    public void LoadNexusnet(){
        Xtralife.AddXtralife();
        StartCoroutine(SousMenuCoroutine(null));
    }

    public void LoadOptions(){
        Debug.Log("Options non prises en charge pour l'instant.");
    }

    public void QuitGame(){
        BlackFade.instance.PlayFadeOutBlackChangeLevel(SceneManager.GetActiveScene().name);
        Application.Quit();
    }

    // Partie Nexusnet (Nom temporaire : Xtr4L1f3)
    public void LoginAnonymous(){
        StartCoroutine(Loader.cloudObj.LoginAnonymous());
    }

    public void Login(){
        StartCoroutine(SousMenuCoroutine("email"));
    }

    public void LoginValider(){
        try{
            string gamerId = GameObject.FindGameObjectWithTag(Params.TagInput).GetComponent<Text>().text;
            string gamerMdp = GameObject.FindGameObjectWithTag(Params.TagInput2).GetComponent<Text>().text;
            StartCoroutine(Loader.cloudObj.Login(gamerId, gamerMdp));
        }catch(Exception e){
            Debug.LogError("MenuPrincipal - LoginValider() : Paramètres incorrects : "+e);
        }
    }

    public void LoginShortCode()
    {
        // TO DO
    }

    public void Retour(){
        StartCoroutine(RetourCoroutine());
    }

    private IEnumerator RetourCoroutine(){
        yield return new WaitForSeconds(0.1f);
        if(!panel1.activeSelf && panel2.activeSelf){
            panel2.SetActive(false);
            panel1.SetActive(true);
            SelectedButton = null;
        }
        else if(!panel2.activeSelf && panel3.activeSelf){
            panel3.SetActive(false);
            panel2.SetActive(true);
            SelectedButton = null;
        }
        loginMethod = string.Empty;
    }

    private IEnumerator SousMenuCoroutine(string loginMethod){
        yield return new WaitForSeconds(0.5f);
        if(panel1.activeSelf && !panel2.activeSelf){
            panel1.SetActive(false);
            panel2.SetActive(true);
            SelectedButton = null;
        }
        else if(panel2.activeSelf && !panel3.activeSelf){
            panel2.SetActive(false);
            panel3.SetActive(true);
            SelectedButton = null;
        }
        this.loginMethod = loginMethod;
    }

    private IEnumerator NavigationInputFields(bool isDown){
        if(!runCoroutine){
            runCoroutine = true;
            Selectable next = null;
            if(isDown)
                next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            else
                next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();

            if (next!= null) {
                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null) inputfield.OnPointerClick(new PointerEventData(EventSystem.current));
                EventSystem.current.SetSelectedGameObject(next.gameObject, new BaseEventData(EventSystem.current));
            }
            yield return new WaitForSeconds(0.2f);
            runCoroutine = false;
        }
    }
}
