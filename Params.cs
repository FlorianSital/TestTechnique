using System.Collections;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections.Generic;
using Holoville.HOTween;
using TMPro;

public class Params
{
    #region Attributs magie
    public const string AttributFeu = "Feu";
    public const string AttributGlace = "Glace";
    public const string AttributVent = "Vent";
    #endregion

    #region Tags
    public const string TagPlayer = "Player";
    public const string TagFire = "Fire";
    public const string TagIce = "Ice";
    public const string TagSmoke = "Smoke";
    public const string TagWind = "Wind";
    public const string TagLogic = "Logic";
    public const string TagClone = "Clone";
    public const string TagMusic = "Music";
    public const string TagCameraUI = "CameraUI";
    public const string TagCameraMap = "CameraMap";
    public const string TagUI = "UI";
    public const string TagUI2 = "UI2";
    public const string TagPanel = "Panel";
    public const string TagPanel2 = "Panel2";
    public const string TagPanel3 = "Panel3";
    public const string TagBattleVignette = "BattleVignette";
    public const string TagBattleName = "BattleName";
    public const string TagBattleLife = "BattleLife";
    public const string TagBattleMana = "BattleMana";
    public const string TagBattleMoral = "BattleMoral";
    public const string TagProgressBar = "ProgressBar";
    public const string TagProgressBar2 = "ProgressBar2";
    public const string TagVide = "Untagged";
    public const string TagInput = "Input";
    public const string TagInput2 = "Input2";
    public const string TagCanvas = "Canvas";
    public const string TagCanvas2 = "Canvas2";
    public const string TagButton = "Button";
    public const string TagButton2 = "Button2";
    public const string TagDelete = "ToDelete";
    #endregion

    #region Inputs
    public const string InputGachetteGauche = "LB";
    public const string InputValider = "Submit";
    public const string InputRetour = "Cancel";
    public const string InputAction = "Action";
    public const string InputMenu = "Menu";
    public const string InputHorizontal = "Horizontal";
    public const string InputVertical = "Vertical";
    public const string InputValiderMenu = "SubmitMenu";
    public const string InputRetourMenu = "CancelMenu";
    public const string InputHorizontalMenu = "HorizontalMenu";
    public const string InputVerticalMenu = "VerticalMenu";
    #endregion

    #region GameObjects
    public const string MenuActions = "MenuActions";
    public const string InputPress = "InputPress";
    public const string InputPressL = "InputPressL";
    public const string InputPressM = "InputPressM";
    public const string InputPressK = "InputPressK";
    public const string InputPressO = "InputPressO";
    public const string Skill0 = "Skill0";
    public const string Skill1 = "Skill1";
    public const string Skill2 = "Skill2";
    public const string Skill3 = "Skill3";
    public const string UICombat = "UICombat";
    public const string PopUp = "PopUp";
    #endregion

    #region Animator
    #region Animator Parameters
    public const string TriggerIdle = "Idle";
    public const string TriggerRun = "Run";
    public const string TriggerAtk = "Atk";
    public const string TriggerDestroy = "Destroy";
    public const string TriggerWalkInto = "WalkInto";
    public const string TriggerWalkOut = "WalkOut";
    public const string IntDirection = "Direction";
    public const string IntInputNumber = "InputNumber";
    #endregion

    #region Animator States
    public const string StateIdleBattle = "IdleBattle";
    public const string StateAtkBattleStart = "AtkBattleStart";
    public const string StateAtkBattleDpl1 = "AtkBattleDpl1";
    public const string StateAtkBattleCac = "AtkBattleCac";
    public const string StateAtkBattleDpl2 = "AtkBattleDpl2";
    public const string StateAtkBattleEnd = "AtkBattleEnd";
    public const string StateHitBattle = "HitBattle";
    public const string StateDeadBattle = "DeadBattle";
    public const string StateDefBattle = "DefBattle";
    public const string StateIdleBattleEnemy = "IdleBattleEnemy";
    public const string StateAtkBattleEnemyStart = "AtkBattleEnemyStart";
    public const string StateAtkBattleEnemyDpl1 = "AtkBattleEnemyDpl1";
    public const string StateAtkBattleEnemyCac = "AtkBattleEnemyCac";
    public const string StateAtkBattleEnemyDpl2 = "AtkBattleEnemyDpl2";
    public const string StateAtkBattleEnemyEnd = "AtkBattleEnemyEnd";
    public const string StateHitBattleEnemy = "HitBattleEnemy";
    public const string StateMagBattleStart = "MagBattleStart";
    public const string StateMagBattleCac = "MagBattleCac";
    public const string StateMagBattleEnemyStart = "MagBattleEnemyStart";
    public const string StateMagBattleEnemyCac = "MagBattleEnemyCac";
    #endregion
    #endregion

    #region Paths
    public const string LienResVignettes = "Vignettes/";
    public const string LienResSpells = "Magie/";
    public const string LienResMaterials = "Material/";
    public const string LienResShaders = "Shader/";
    public const string LienResMonsters = "Monstres/";
    public const string LienResSounds = "Sounds/";
    public const string LienResMusics = "Musics/";
    public const string LienResUICombat = "UI/Combat/";
    public const string LienResDialogueDatabase = "UI/Dialogue Database";
    #endregion

    #region Autre
    public const string MenuVide = "---";
    #endregion

    #region Options
    public const float CamOrthoSize = 5;
    #endregion

    #region Listes
    #region Liste sons
    public const string SonPas = "pas";
    public const string SonMiss = "miss";
    public const string SonCritique = "critique";
    public const string SonHit = "hit";
    public const string SonDead = "deadFizz";
    public const string SonKick = "kick";
    public const string SonKickEpee = "epeeHit";
    public const string SonBtnValider = "nav bouton valider";
    public const string SonBtnNav = "nav bouton";
    #endregion

    #region Liste musiques
    public const string MusCombatSimple = "combat2";
    #endregion

    #region Liste materials
    public const string MatWhiteSelection = "WhiteSelection";
    public const string MatPanelInfoSelection = "PanelInfoSelection";
    public const string MatDeadEnemy = "DeadEnemy";
    public const string MatHit = "Hit";
    #endregion

    #region Liste shaders
    public const string ShaChangeMaterial = "ChangeMaterial";
    public const string ShaDiffuseFlash = "DiffuseFlash";
    #endregion

    #region Liste textes
    public const string TxtPlayerTurn = "Que fais tu ?";
    public const string TxtEnemyTurn = "L'ennemi agit...";
    public const string TxtEnemyWon = "Outch, l'opposant a gagné.";
    public const string TxtPlayerWon = "Victoire !";
    public const string TxtEndOfTheBattle = "Fin du combat.";
    public const string TxtPlayerXP = "Les vainqueurs ont gagné ceci :";
    public const string TxtYouLost = "C'était un terrible adversaire. Vous sombrez dans l'inconscience.";
    #endregion
    #endregion
}

public static class Methods
{
    public static void DoDestruction(this ObjectController obj, Sprite destructionSprite){
        if(!obj.IsDetruit())
            obj.SetDetruit(true);
        if(destructionSprite != null){
            obj.GetComponent<SpriteRenderer>().sprite = destructionSprite;
        }
        else if(obj.imageDetruit != null){
            obj.GetComponent<SpriteRenderer>().sprite = obj.imageDetruit;
        }
        if(obj.GetAnimator() != null && obj.GetSonDetruitSource()){
            obj.GetAnimator().SetTrigger(Params.TriggerDestroy);
            obj.GetSonDetruitSource().Play();
        }
    }

    public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag)where T:Component{
        Transform t = parent.transform;
        foreach(Transform tr in t)
        {
                if(tr.tag == tag)
                {
                    return tr.GetComponent<T>();
                }
        }
        return null;
    }

    public static AudioSource AddSound(GameObject go, string name, bool loop, AudioClip audioClip)
    {
        if(!audioClip)
            audioClip = Resources.Load(Params.LienResSounds+name) as AudioClip;
        AudioSource soundSource = go.AddComponent<AudioSource>();
        soundSource.clip = audioClip;
        soundSource.playOnAwake = false;
        soundSource.loop = loop;
        return soundSource;
    }

    public static void AddConversation(GameObject cible, string nameConversation, GameObject actor, GameObject conversant){
        DialogueSystemTrigger trigger = cible.GetComponent<DialogueSystemTrigger>();
        trigger.conversation = nameConversation;

        var overrideActorName2 = cible.GetComponent<OverrideActorName>() ?? cible.AddComponent<OverrideActorName>();
        overrideActorName2.overrideName = cible.name;

        Conversation conv = Database.dialogueDatabase.conversations.Find(c => c.Title.Equals(nameConversation));
        if(actor != null && Database.DataCharacters.TryGetValue(actor.name, out DataCharacters valueActor)){
            trigger.conversationActor = actor.transform;
        }

        if(conversant != null && Database.DataCharacters.TryGetValue(conversant.name, out DataCharacters valueConversant)){
            trigger.conversationConversant = conversant.transform;
        }
    }

    public static void StartConversation(string nameConversation, List<DataCharacters> listActors, string actor, string conversant){
        Transform actorTransform = listActors.Find(x => x.PicturesName.Equals("Itchi")).obj.transform;
        Transform conversantTransform = listActors.Find(x => x.PicturesName.Equals("Dragonnet")).obj.transform;
        DialogueManager.StartConversation(nameConversation, actorTransform, conversantTransform);
    }
    public static Direction ComparePositions(GameObject a, GameObject b)
    {
        Direction directionOfB = Direction.Bottom;
        Vector3 aPos = a.transform.position;
        Vector3 bPos = b.transform.position;
        float angleDirection = 0.3f;

        if(aPos.y > bPos.y && (aPos.x < bPos.x + angleDirection && aPos.x > bPos.x - angleDirection)){ /*b is bottom*/
            directionOfB = Direction.Bottom;
        }else if(aPos.y > bPos.y && aPos.x > bPos.x){ /*b is bottom left*/
            directionOfB = Direction.BottomLeft;
        }else if(aPos.x > bPos.x && (aPos.y < bPos.y + angleDirection/2 && aPos.y > bPos.y - angleDirection/2)){ /*b is left*/
            directionOfB = Direction.Left;
        }else if(aPos.x > bPos.x && aPos.y < bPos.y){ /*b is top left*/
            directionOfB = Direction.TopLeft;
        }else if(aPos.y < bPos.y && (aPos.x < bPos.x + angleDirection && aPos.x > bPos.x - angleDirection)){ /*b is top*/
            directionOfB = Direction.Top;
        }else if(aPos.y < bPos.y && aPos.x < bPos.x){ /*b is top right*/
            directionOfB = Direction.TopRight;
        }else if(aPos.x < bPos.x && (aPos.y < bPos.y + angleDirection/2 && aPos.y > bPos.y - angleDirection/2)){ /*b is right*/
            directionOfB = Direction.Right;
        }else if(aPos.x < bPos.x && aPos.y > bPos.y){ /*b is bottom right*/
            directionOfB = Direction.BottomRight;
        }

        return directionOfB;
    }

    public static Vector3 GetVector3ByDirection(bool isOpposite, Direction currentDirection){
        // 1 is x, 2 is y
        (int, int) xy = (0, 0);
        switch (currentDirection)
        {
            case Direction.Bottom: xy = isOpposite ? (0, 1) : (0, -1); break;
            case Direction.BottomLeft: xy = isOpposite ? (1, 1) : (-1, -1); break;
            case Direction.Left: xy = isOpposite ? (1, 0) : (-1, 0); break;
            case Direction.TopLeft: xy = isOpposite ? (1, -1) : (-1, 1); break;
            case Direction.Top: xy = isOpposite ? (0, -1) : (0, 1); break;
            case Direction.TopRight: xy = isOpposite ? (-1, -1) : (1, 1); break;
            case Direction.Right: xy = isOpposite ? (-1, 0) : (1, 0); break;
            case Direction.BottomRight: xy = isOpposite ? (-1, 1) : (1, -1); break;
            default: xy = (0, 0); break;
        }
        Vector3 force = new Vector3(xy.Item1, xy.Item2, 0);
        return force;
    }

    public static Vector3 GetVector3ByAngle(bool isOpposite, int currentAngle){
        // 1 is x, 2 is y
        (int, int) xy = (0, 0);
        switch (currentAngle)
        {
            case 180: xy = isOpposite ? (0, 1) : (0, -1); break;
            case 120: xy = isOpposite ? (1, 1) : (-1, -1); break;
            case 90: xy = isOpposite ? (1, 0) : (-1, 0); break;
            case 60: xy = isOpposite ? (1, -1) : (-1, 1); break;
            case 0: xy = isOpposite ? (0, -1) : (0, 1); break;
            case 360: xy = isOpposite ? (0, -1) : (0, 1); break;
            case 300: xy = isOpposite ? (-1, -1) : (1, 1); break;
            case 270: xy = isOpposite ? (-1, 0) : (1, 0); break;
            case 240: xy = isOpposite ? (-1, 1) : (1, -1); break;
            default: xy = (0, 0); break;
        }
        Vector3 force = new Vector3(xy.Item1, xy.Item2, 0);
        return force;
    }

    public static Direction GetDirectionByAngle(bool isOpposite, int currentAngle){
        Direction direction = Direction.Bottom;
        switch(currentAngle)
        {
            case 360: direction = isOpposite ? Direction.Bottom : Direction.Top; break;
            case 0: direction = isOpposite ? Direction.Bottom : Direction.Top; break;
            case 60: direction = isOpposite ? Direction.BottomRight : Direction.TopLeft; break;
            case 90: direction = isOpposite ? Direction.Right : Direction.Left; break;
            case 120: direction = isOpposite ? Direction.TopRight : Direction.BottomLeft; break;
            case 180: direction = isOpposite ? Direction.Top : Direction.Bottom; break;
            case 240: direction = isOpposite ? Direction.TopLeft : Direction.BottomRight; break;
            case 270: direction = isOpposite ? Direction.Left : Direction.Right; break;
            case 300: direction = isOpposite ? Direction.BottomLeft : Direction.TopRight; break;
            default : direction = Direction.Bottom; break;
        }
        return direction;
    }

    public static int GetAngleByDirection(bool isOpposite, Direction currentDirection)
    {
        int angle = 0;
        switch (currentDirection)
        {
            case Direction.Bottom: angle = isOpposite ? 0 : 180; break;
            case Direction.BottomLeft: angle = isOpposite ? 300 : 120; break;
            case Direction.Left: angle = isOpposite ? 270 : 90; break;
            case Direction.TopLeft: angle = isOpposite ? 240 : 60; break;
            case Direction.Top: angle = isOpposite ? 180 : 0; break;
            case Direction.TopRight: angle = isOpposite ? 120 : 300; break;
            case Direction.Right: angle = isOpposite ? 90 : 270; break;
            case Direction.BottomRight: angle = isOpposite ? 60 : 240; break;
            default: angle = 0; break;
        }
        return angle;
    }

    public static void ShowPopup(int? number, Vector3 beginPosition, string text, Color color)
	{
        Vector3 position = new Vector3();
        try{
            if(Loader.cameraUI == null)
                Loader.cameraUI = UnityEngine.GameObject.FindGameObjectWithTag(Params.TagCameraUI).GetComponent<Camera>();
            if(Loader.cameraMap == null)
                Loader.cameraMap = UnityEngine.GameObject.FindGameObjectWithTag(Params.TagCameraMap).GetComponent<Camera>();
            position = (Vector3)Loader.cameraUI.WorldToScreenPoint(beginPosition * Params.CamOrthoSize / Loader.cameraMap.orthographicSize);   
        }catch(Exception e){
            Debug.LogError("Params - ShowPopUp() : Il n'y a pas de tag 'CameraUI' ou de 'CameraMap' dans la scène."+e);
        }

		GameObject currentPopUp = Resources.Load<GameObject>(Params.LienResUICombat + Params.PopUp);
        currentPopUp = UnityEngine.Object.Instantiate(currentPopUp);
		currentPopUp.AddComponent<PopUp>();
        currentPopUp.transform.SetParent(UnityEngine.GameObject.FindGameObjectWithTag(Params.TagUI2).transform);
		TextMeshProUGUI currentPopUpText = currentPopUp.GetComponent<TextMeshProUGUI>();
		foreach (Transform child in currentPopUp.transform) {
			UnityEngine.Object.Destroy(child.gameObject);
		}
		currentPopUp.SetActive(true);
		float time = 0.7f;
		TweenParms parms = new TweenParms().Prop("color", color).Ease(EaseType.EaseOutQuart);
		parms.Prop("fontSize", currentPopUpText.fontSize * 3).Ease(EaseType.EaseOutBounce);
		TweenParms parmsReset = new TweenParms().Prop("color", color).Ease(EaseType.EaseOutQuart);
		parmsReset.Prop("fontSize", currentPopUpText.fontSize ).Ease(EaseType.EaseOutCirc);

		if(!text.Equals(string.Empty)){
			currentPopUpText.text += text;
		}
		if(number != null){
			if(!text.Equals(string.Empty))
				currentPopUpText.text += "\n";
			currentPopUpText.text += number.ToString();
		}

		Sequence actions = new Sequence(new SequenceParms());
		currentPopUp.transform.position = new Vector3(position.x, position.y+10, currentPopUp.transform.position.z);
		actions.Append(HOTween.To(currentPopUpText, time, parms));
		actions.Append(HOTween.To(currentPopUpText, time, parmsReset));
		actions.Play();
		UnityEngine.Object.Destroy(currentPopUp, 15);
	}
}

public enum Direction
{
    Top = 1
    ,TopLeft = 8
    ,Left = 7
    ,BottomLeft = 6
    ,Bottom = 5
    ,BottomRight = 4
    ,Right = 3
    ,TopRight = 2
}

public enum UserLayer
{
    Default = 0
    ,TransparentFX = 1
    ,Water = 4
    ,UI = 5
    ,Player = 8
    ,PostProcess = 9
    ,IgnorePostProcess = 10
    ,Magic = 11
    ,Characters = 12
    ,ColliderIgnorePlayer = 13
    ,ColliderIgnoreMagic = 14
    ,ColliderIgnoreAll = 15
    ,FadeInBlack = 20
    ,IgnoreFadeInBlack = 21
    ,UIPremierPlan = 25
    ,Ignore = 28
}

public enum SortingLayer
{
    Background
    ,Sol
    ,Default
    ,Player
    ,Magic
    ,UI
    ,PostProcess
    ,IgnorePostProcess
    ,FadeInBlack
    ,IgnoreFadeInBlack
    ,UIPremierPlan
}

public enum TypeEffect
{
    Action
    , Effect
    , MirrorEffect
}

public enum ChannelEffect
{
    Atk
    , Aura
    , Other
}

public enum BattleActor
{
    Ally
    , Enemy
}

public enum BattleState
{
    Winner
    , GameOver
    , Beginning
    , PlayerTurn
    , EnemyTurn
    , SelectingTarget
    , SelectedTarget
    , None
}

// ATTENTION : Les premières actions doivent être celles du menu dans l'ordre, pour faire marcher leur désactivation
public enum BattleAction
{
    Attaque
    , Jobcard
    , Inventaire
    , Parler
    , Defendre
    , Passer
    , None
}

public enum BattleCharState
{
    Idle
    , Atk
    , Walking
    , Running
    , Jobcard
    , Defending
    , Hit
    , Dead
}

public enum ItemType
{
    None
    , Epee
    , Main
}

// Les spécialisations d'un personnage, détermine ses facultés et son stuff équipable
public enum CharacterType
{
    None,
    Epeiste,
    Boxeur
}

// Périmètre d'une technique lors du ciblage
public enum SelectingPerimeter
{
    OneEnemy,
    OneAlly,
    OneEntity,
    AllEnemies,
    AllAllies,
    AllAlliesOrEnemies,
    AllEntities
}

// Type d'animation d'un utilisateur de sort, technique ou objet
public enum InitiatorAnimationType
{
    AtkCac,
    AtkDist,
    MagicCac,
    MagicDist,
    ObjCac,
    ObjDist,
    ShieldCac,
    ShieldDist
}

// Type d'animation d'un sort, technique ou objet
public enum ActionAnimationType
{
    Souffle,
    Fixe
}

// Scènes chargeables par le loader
public enum ListeScenes
{
    MenuPrincipal,
    Intro,
    Villarbor,
    BattleScene,
    TourDeLance
}

public enum PlayerType
{
    PlayerOnMap,
    PlayerOnBattle,
    NPCOnMap,
    NPCOnBattle,
    Other
}

// Etat du jeu, pour définir si le héros peut bouger ou pas
public enum GameStatus
{
    Cinematic,
    Map,
    Battle
}