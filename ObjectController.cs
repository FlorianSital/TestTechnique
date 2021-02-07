using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;

public class ObjectController : MonoBehaviour
{
    #region public variables
    public GameObject effetEnFeu;
    public GameObject effetCongele;
    public GameObject effetCharge;
    public GameObject effetVapeurDeau;
    public Sprite imageEnFeu;
    public Sprite imageCongele;
    public Sprite imageDetruit;
    public AudioClip sonDetruit;
    public bool peutSeMettreEnFeu;
    public bool peutSeGeler;
    public bool peutSeSouffler;
    public bool peutSeDetruire;
    public bool traversableParLeJoueur;
    public bool traversableEnFeu;
    public bool traversableEnGlace;
    public bool traversableEnFumee;
    public bool traversableDetruit;
	public bool canWalkAndRun;
    public bool canUseMenuActions;
    public bool estUnPerso;
    public bool estUnDecor;
    public bool estUnEffet;
    public int points;
    #endregion

    #region private variables
    private Direction currentDirection;
    private GameObject currentEffect;
    private bool estEnFeu;
    private bool estCongele;
    private bool estDetruit;
    private bool estDansVapeurDeau;
	private bool isWalk;
    private bool isRun;
    private bool isUsingMenuActions;
    private Animator animator;
    private DialogueSystemTrigger dialogueSystemTrigger;
    private float movementSpeedHorizontal;
    private float movementSpeedVertical;
    private float dirX, dirY;
    private GameObject menuActions;
    private List<DataSpells> actions;
    private bool actionsIsCreated;
    private AudioSource stepSound;
    private AudioSource sonDetruitSource;
    private SpriteRenderer spriteRender;
    private int layerOrigin;
    private bool peutSeSoufflerOrigin;
    private bool isReact;
    private bool isHitObject;
    private bool pointsObtenus;
    #endregion

    void Awake()
    {
        layerOrigin = gameObject.layer;
        peutSeSoufflerOrigin = peutSeSouffler;
        animator = GetComponent<Animator>();
        if(estUnPerso){
            animator?.SetInteger(Params.IntDirection, (int)Direction.Bottom);
            animator?.SetTrigger(Params.TriggerIdle);
        }
        if(tag == Params.TagFire){
            estEnFeu = true;
        }
            
        if(tag == Params.TagIce){
            estCongele = true;
        }

        if(tag == Params.TagSmoke){
            traversableEnFumee = true;
        }

        if(tag == Params.TagPlayer){
            peutSeMettreEnFeu = false;
        }
        
        dialogueSystemTrigger = GetComponent<DialogueSystemTrigger>();

        if(this.gameObject.layer == (int)UserLayer.Characters){
            stepSound = Methods.AddSound(gameObject, "pas", true, null);
        }

        if(sonDetruit){
            sonDetruitSource = Methods.AddSound(gameObject, null, false, sonDetruit);
        }

        spriteRender = GetComponent<SpriteRenderer>();
        currentDirection = Direction.Bottom;
		isWalk = false;
        isReact = false;
        isUsingMenuActions = false;
        isHitObject = false;
        currentEffect = null;
        menuActions = GameObject.Find(Params.MenuActions);
    }

    void Update()
    { 
        if(Loader.gameStatus == GameStatus.Map && gameObject.tag.Equals(Params.TagPlayer)){
            if(canWalkAndRun && isHitObject){
                Moving(true);
                Rotate(null, null, currentDirection, false);
            }else if(canWalkAndRun && !isHitObject){
                Moving(false);
                Rotate(dirX, dirY, null, false);
            }

            // Affiche le menu des actions et le remplit
            if(canUseMenuActions){
                menuActions?.SetActive(true);
                ManetteAffichage menuActionsScript = menuActions?.GetComponentInChildren<ManetteAffichage>();
            
                if(Input.GetButton(Params.InputGachetteGauche) && !currentEffect)
                {
                    actions = menuActionsScript?.CreateActions(menuActions, this.name);
                    menuActionsScript?.ControlActions(actions, this);
                }
            }else if(!canUseMenuActions)
            {
                menuActions?.SetActive(false);
            }

            // Empêche le lanceur d'un souffle de vent de reculer comme ses cibles
            if(currentEffect != null && currentEffect.tag.Equals(Params.TagWind) && isReact && peutSeSouffler){
                peutSeSoufflerOrigin = peutSeSouffler;
                peutSeSouffler = false;
            }else if(currentEffect == null && !isReact && !peutSeSouffler){
                peutSeSouffler = peutSeSoufflerOrigin;
            }
        }

        // En cas de scores activés (mini jeu destruction)
        if(Loader.scoreEnabled && !pointsObtenus && (estDetruit || estEnFeu)){
            if(points != 0){
                pointsObtenus = true;
                int score = 0;
                if (!int.TryParse(Loader.score, out score)) score = 0;
                int result = score + points;
                Loader.score = result.ToString();
                GameObject.FindGameObjectWithTag(Params.TagPanel2).GetComponent<Text>().text = Loader.score;
                Methods.ShowPopup(null, this.transform.position, "+" +points, Color.green);
            }
        }
    }

    #region evenements
    void OnTriggerStay2D(Collider2D other) {
        if(!Input.GetButton(Params.InputGachetteGauche) && !currentEffect && !estEnFeu && !estCongele && !estDetruit){
            DialogueStartByPlayer(other);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        // Animation en cas d'entrée dans l'objet
        if(animator && estUnDecor){
            #pragma warning disable 0168
            try{
                animator.ResetTrigger(Params.TriggerWalkOut);
                animator.SetTrigger(Params.TriggerWalkInto);
            }catch(Exception e){
                
                // Ne possède pas ces triggers
            }
            #pragma warning restore 0168
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        // Animation en cas de sortie de l'objet
        if(animator){
            animator?.ResetTrigger(Params.TriggerWalkInto);
            animator?.SetTrigger(Params.TriggerWalkOut);
        }
    }

    void OnParticleCollision(GameObject other) {
        float durationIgnoreLayer = 1.5f;
        // Animation en cas de vapeur d'eau -- (commun & childs)
        if(traversableEnFumee && other.gameObject.layer == (int)UserLayer.Magic && other.tag != Params.TagWind && other.tag != Params.TagFire){
            StartCoroutine(SetOnIgnoreLayer(durationIgnoreLayer));
        }
        if((estEnFeu && other.tag == Params.TagIce) || (estCongele && other.tag == Params.TagFire)){
            if(estEnFeu){
                estEnFeu = false;
                ObjectController childFire = gameObject.FindComponentInChildWithTag<ObjectController>(Params.TagFire);
                if(childFire)
                    Destroy(childFire.gameObject);
            }
            if(estCongele){
                estCongele = false;
                ObjectController childIce = gameObject.FindComponentInChildWithTag<ObjectController>(Params.TagIce);
                childIce?.SetDetruit(true);
                childIce?.DoDestruction(null);
            }
            peutSeMettreEnFeu = false;
            estDansVapeurDeau = true;
            spriteRender.color = new Color(255, 255, 255, 255);
            if(animator != null && !estDetruit){
                animator.enabled = true;
            }
            if(effetVapeurDeau){
                StartCoroutine(PlayEffect(null, false, effetVapeurDeau, 0, false, true, TypeEffect.Effect));
            }
        }
        // Animation en cas de vapeur d'eau -- (du feu sur l'objet glacé)
        if(tag == Params.TagIce && other.tag == Params.TagFire && !estDansVapeurDeau){
            StartCoroutine(SetOnIgnoreLayer(durationIgnoreLayer));
            estDansVapeurDeau = true;
            StartCoroutine(PlayEffect(null, false, effetVapeurDeau, 0, false, true, TypeEffect.Effect));
            SetDetruit(true);
            this.DoDestruction(null);
        }
        // Animation en cas de vapeur d'eau -- (de la glace sur l'objet enflammé)
        if(tag == Params.TagFire && other.tag == Params.TagIce && !estDansVapeurDeau){
            StartCoroutine(SetOnIgnoreLayer(durationIgnoreLayer));
            estDansVapeurDeau = true;
            StartCoroutine(PlayEffect(null, false, effetVapeurDeau, 0, false, true, TypeEffect.Effect));
            SetDetruit(true);
            this.DoDestruction(imageEnFeu);
        }
        
        // Animation en cas de glace
        if(traversableEnGlace && other.gameObject.layer == (int)UserLayer.Magic && other.tag != Params.TagWind){
            StartCoroutine(SetOnIgnoreLayer(durationIgnoreLayer));
        }
        if(peutSeGeler && estCongele == false && other.tag == Params.TagIce){
            estCongele = true;
            estEnFeu = false;
            if(animator != null){
                animator.enabled = false;
            }
            if(imageCongele != null){
                //Si personnage, tourner le sprite selon sa direction
                spriteRender.sprite = imageCongele;
                if(this.gameObject.layer == (int)UserLayer.Characters)
                {
                    this.transform.Rotate(0, 0, Methods.GetAngleByDirection(true, Methods.ComparePositions(this.gameObject, other)), Space.World);
                }
            }
            if(effetCongele){
                StartCoroutine(PlayEffect(null, false, effetCongele, 0, false, true, TypeEffect.Effect));
            }
            spriteRender.color = new Color(0, 255, 255, 255);
        }

        // Animation en cas de feu
        if(traversableEnFeu && other.gameObject.layer == (int)UserLayer.Magic && other.tag != Params.TagWind && other.tag != Params.TagIce){
            StartCoroutine(SetOnIgnoreLayer(durationIgnoreLayer));
        }
        if(peutSeMettreEnFeu && estEnFeu == false && other.tag == Params.TagFire && !estDansVapeurDeau){
            estEnFeu = true;
            foreach (Transform child in transform) {
                GameObject.Destroy(child.gameObject);
            }
            if(animator != null){
                animator.enabled = false;
            }
            if(!estDetruit){
                //Si personnage, tourner le sprite selon sa direction
                this.DoDestruction(imageEnFeu);
                if(this.gameObject.layer == (int)UserLayer.Characters && (imageEnFeu || imageDetruit))
                {
                    this.transform.Rotate(0, 0, Methods.GetAngleByDirection(true, Methods.ComparePositions(this.gameObject, other)), Space.World);
                }
            }
            if(effetEnFeu){
                StartCoroutine(PlayEffect(null, false, effetEnFeu, 0, false, true, TypeEffect.Effect));
            }
            spriteRender.color = new Color(255, 0, 0, 255);
        }

        // Animation en cas de vent
        if(other.tag == Params.TagWind){
            if(estDansVapeurDeau){
                estDansVapeurDeau = false;
                ObjectController childSmoke = gameObject.FindComponentInChildWithTag<ObjectController>(Params.TagSmoke);
                if(childSmoke)
                    Destroy(childSmoke.gameObject);
            }
            if(tag == Params.TagSmoke){
                Destroy(gameObject);
            }else if(peutSeSouffler && GetComponent<Rigidbody2D>() != null){
                float intensity = 5;
                Vector3 force = Methods.GetVector3ByDirection(true, Methods.ComparePositions(this.gameObject, other));
                GetComponent<Rigidbody2D>().AddForce(new Vector3(force.x * intensity, force.y * intensity,0));
            }else if(estEnFeu && !isReact){
                ObjectController childFire = gameObject.FindComponentInChildWithTag<ObjectController>(Params.TagFire);
                if(childFire != null && childFire.peutSeSouffler){
                    Destroy(childFire.gameObject);
                    currentDirection = Methods.GetDirectionByAngle(false, (int)other.transform.eulerAngles.z);
                    StartCoroutine(PlayEffect(Database.DataSpells["Souffle de feu"], true, null, 0, false, true, TypeEffect.Effect));
                }
                spriteRender.color = new Color(255, 255, 255, 255);
                estEnFeu = false;
            }else{
                StartCoroutine(SetOnIgnoreLayer(durationIgnoreLayer));
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other){
        var currentCollid = gameObject.GetComponent<Collider2D>();
        var otherCollid = other.gameObject.GetComponent<Collider2D>();
        var currentEdgeCollid = gameObject.GetComponent<EdgeCollider2D>();
        var otherEdgeCollid = other.gameObject.GetComponent<EdgeCollider2D>();
        var currentCapsuleCollid = gameObject.GetComponent<CapsuleCollider2D>();
        var otherCapsuleCollid = other.gameObject.GetComponent<CapsuleCollider2D>();

        if(tag != Params.TagPlayer){
            ObjectController otherObjectScript = other.gameObject.GetComponent<ObjectController>();
            // Layers à ignorer lors d'une collision
            if(traversableParLeJoueur && other.gameObject.layer == (int)UserLayer.Player
            || traversableEnFeu && estEnFeu && other.gameObject.layer == (int)UserLayer.Player
            || traversableEnGlace && estCongele && other.gameObject.layer == (int)UserLayer.Player
            || traversableDetruit && estDetruit && other.gameObject.layer == (int)UserLayer.Player){
                if(currentCollid && otherCollid)
                    Physics2D.IgnoreCollision(currentCollid, otherCollid, true);
                if(currentEdgeCollid && otherCapsuleCollid)
                    Physics2D.IgnoreCollision(currentEdgeCollid, otherCapsuleCollid, true);
            }
            // Lors d'une charge du joueur
            else if(otherObjectScript.isRun
                && (other.gameObject.layer == (int)UserLayer.Player || other.gameObject.layer == (int)UserLayer.Characters))
            {
                if(peutSeDetruire && !estDetruit
                && (other.gameObject.layer == (int)UserLayer.Player || other.gameObject.layer == (int)UserLayer.Characters)){
                    this.DoDestruction(imageDetruit);
                    spriteRender.color = new Color(255, 255, 255, 255);
                    if(animator != null && !estDetruit){
                        animator.enabled = true;
                    }
                    ObjectController parentObj = gameObject.transform.parent.GetComponent<ObjectController>();
                    if(parentObj){
                        parentObj.estCongele = false;
                        if(parentObj.GetSpriteRenderer())
                            parentObj.GetSpriteRenderer().color = new Color(255, 255, 255, 255);
                        if(parentObj.GetAnimator() && !parentObj.estDetruit)
                            parentObj.GetAnimator().enabled = true;
                    }
                }

                // Joueur assomé, objet repoussé
                SoundsManager.instance.PlayOneShot(Params.SonHit);
                StartCoroutine(otherObjectScript.SetHitObject(0.1f));
                StartCoroutine(PlayEffect(Database.DataSpells["Coup"], true, null, 0, false, false, TypeEffect.Effect));
                if(peutSeSouffler && GetComponent<Rigidbody2D>() != null){
                    float intensity = 200;
                    Vector3 force = Methods.GetVector3ByDirection(true, Methods.ComparePositions(this.gameObject, other.gameObject));
                    GetComponent<Rigidbody2D>().AddForce(new Vector3(force.x * intensity, force.y * intensity,0));
                    StartCoroutine(SetHitObject(0.1f));
                }
            }
        }
    }

    void OnConversationStart(Transform NPC) {
        GameObject playerObj = DialogueManager.CurrentConversant.gameObject;
        GameObject NPCobj = NPC.gameObject;
        ObjectController playerScript = playerObj.GetComponent<ObjectController>();
        ObjectController NPCScript = NPCobj.GetComponent<ObjectController>();

        if(NPCScript && NPCScript.estUnPerso){
            NPCScript.CantMove();
            NPCScript.Rotate(null,null,Methods.ComparePositions(NPCobj, playerObj), false);
        }
        if(playerScript && playerScript.estUnPerso){
            playerScript.CantMove();
            playerScript.Rotate(null,null,Methods.ComparePositions(NPCobj, playerObj), true);
        }
    }

    void OnConversationEnd(Transform NPC) {
        ObjectController NPCScript = NPC.gameObject.GetComponent<ObjectController>();

        if(NPCScript != null && NPCScript.estUnPerso){
            NPCScript.CanMove();
        }
        if(NPC.tag != Params.TagPlayer){
            GameObject.FindGameObjectWithTag(Params.TagPlayer).GetComponent<ObjectController>().CanMove();
        }
    }

    #endregion



    #region public
    public AudioSource GetSonDetruitSource(){
        return sonDetruitSource;
    }
    
    public Animator GetAnimator(){
        return animator;
    }

    public SpriteRenderer GetSpriteRenderer(){
        return spriteRender;
    }

    public bool IsDetruit(){
        return estDetruit;
    }

    public bool IsRun(){
        return isRun;
    }

    public void SetDetruit(bool detruit){
        estDetruit = detruit;
    }

    public void DialogueStartByPlayer(Collider2D player){
        if(!DialogueManager.isConversationActive && dialogueSystemTrigger && player.CompareTag(Params.TagPlayer) && Input.GetButtonDown(Params.InputValider)){
            dialogueSystemTrigger.conversationConversant = player.gameObject.transform;
            dialogueSystemTrigger.OnUse();
        }
    }

	public void CantMove()
	{
        canUseMenuActions = false;
		canWalkAndRun = false;
        isWalk = false;
        isRun = false;
		animator.ResetTrigger(Params.TriggerRun);
		animator.ResetTrigger(Params.TriggerAtk);
		animator.SetTrigger(Params.TriggerIdle);
	}

	public void CanMove()
	{
        canUseMenuActions = true;
		canWalkAndRun = true;
		animator.ResetTrigger(Params.TriggerIdle);
	}

    public void Rotate(float? x, float? y, Direction? dir, bool isOpposite)
	{
        string state = string.Empty;
        state = (isWalk && canWalkAndRun) ? Params.TriggerRun : Params.TriggerIdle;

		if ((x == 0 && y == 1 && !isOpposite) || (x == 0 && y == -1 && isOpposite) ||(dir == Direction.Top && !isOpposite) || (dir == Direction.Bottom && isOpposite)) {
			currentDirection = Direction.Top;
			animator.SetInteger (Params.IntDirection, (int)Direction.Top);
            animator.Play(state+Direction.Top);
		}

		else if ((x == 1 && y == 1 && !isOpposite) || (x == -1 && y == -1 && isOpposite) || (dir == Direction.TopRight && !isOpposite) || (dir == Direction.BottomLeft && isOpposite)) {
			currentDirection = Direction.TopRight;
			animator.SetInteger (Params.IntDirection, (int)Direction.TopRight);
            animator.Play(state+Direction.TopRight);
		}

		else if ((x == 1 && y == 0 && !isOpposite) || (x == -1 && y == 0 && isOpposite) || (dir == Direction.Right && !isOpposite) || (dir == Direction.Left && isOpposite)) {
			currentDirection = Direction.Right;
			animator.SetInteger (Params.IntDirection, (int)Direction.Right);
            animator.Play(state+Direction.Right);
		}

		else if ((x == 1 && y == -1 && !isOpposite) || (x == -1 && y == 1 && isOpposite) || (dir == Direction.BottomRight && !isOpposite) || (dir == Direction.TopLeft && isOpposite)) {
			currentDirection = Direction.BottomRight;
			animator.SetInteger (Params.IntDirection, (int)Direction.BottomRight);
            animator.Play(state+Direction.BottomRight);
		}

		else if ((x == 0 && y == -1 && !isOpposite) || (x == 0 && y == 1 && isOpposite) || (dir == Direction.Bottom && !isOpposite) || (dir == Direction.Top && isOpposite)) {
			currentDirection = Direction.Bottom;
			animator.SetInteger (Params.IntDirection, (int)Direction.Bottom);
            animator.Play(state+Direction.Bottom);
		}

		else if ((x == -1 && y == -1 && !isOpposite) || (x == 1 && y == 1 && isOpposite) || (dir == Direction.BottomLeft && !isOpposite) || (dir == Direction.TopRight && isOpposite)) {
			currentDirection = Direction.BottomLeft;
			animator.SetInteger (Params.IntDirection, (int)Direction.BottomLeft);
            animator.Play(state+Direction.BottomLeft);
		}

		else if ((x == -1 && y == 0 && !isOpposite) || (x == 1 && y == 0 && isOpposite) || (dir == Direction.Left && !isOpposite) || (dir == Direction.Right && isOpposite)) {
			currentDirection = Direction.Left;
			animator.SetInteger (Params.IntDirection, (int)Direction.Left);
            animator.Play(state+Direction.Left);
		}

		else if ((x == -1 && y == 1 && !isOpposite) || (x == 1 && y == 1 && isOpposite) || (dir == Direction.TopLeft && !isOpposite) || (dir == Direction.BottomRight && isOpposite)) {
			currentDirection = Direction.TopLeft;
			animator.SetInteger (Params.IntDirection, (int)Direction.TopLeft);
            animator.Play(state+Direction.TopLeft);
		}
	}

    public void PlaySound(AudioClip soundClip){
        SoundsManager.instance.PlayOneShot(soundClip);
    }
    
    #endregion



    #region private
    private void Moving(bool isOpposite)
	{
		dirX = Mathf.RoundToInt(Input.GetAxis(Params.InputHorizontal));
		dirY = Mathf.RoundToInt(Input.GetAxis(Params.InputVertical));

        if(isOpposite){
            dirX = -dirX;
            dirY = -dirY;
        }

        // Etat immobile
		if(dirX == 0 && dirY == 0 && !Input.GetButton(Params.InputRetour))
		{
			animator.ResetTrigger(Params.TriggerRun);
			animator.SetTrigger(Params.TriggerIdle);
			isWalk = false;
            isRun = false;
            stepSound.Stop();
		}
        // Etat mouvement
		else
		{
			animator.ResetTrigger(Params.TriggerIdle);
			animator.SetTrigger(Params.TriggerRun);
            if(Input.GetButton(Params.InputRetour) && !Input.GetButton(Params.InputGachetteGauche) && (dirX != 0 || dirY != 0)){
                animator.speed = 3;
                movementSpeedHorizontal = 9f;
                movementSpeedVertical = 4.5f;
                isRun = true;
                if(!stepSound.isPlaying){
                    stepSound.Play();
                }
                if(effetCharge != null && currentEffect == null){
                    StartCoroutine(PlayEffect(null, false, effetCharge, 0, false, true, TypeEffect.MirrorEffect));
                }else if(effetCharge != null && currentEffect != null){
                    currentEffect.transform.position = transform.position;
                    currentEffect.transform.eulerAngles = new Vector3(0,0,Methods.GetAngleByDirection(true, currentDirection));
                }
            }else{
                animator.speed = 1;
                movementSpeedHorizontal = 3f;
                movementSpeedVertical = 1.5f;
                stepSound.Stop();
                isRun = false;
                if(canWalkAndRun && !isUsingMenuActions && currentEffect){
                    DestroyEffect(TypeEffect.MirrorEffect);
                }
            }
            
			isWalk = true;
			transform.position = new Vector2 (dirX * movementSpeedHorizontal * Time.deltaTime + transform.position.x,
			dirY * movementSpeedVertical * Time.deltaTime + transform.position.y);
		}
	}

    public void SetWalkAndAct(bool can)
    {
        canWalkAndRun = can;
        canUseMenuActions = can;
    }

    private void StartEffect(ParticleSystem[] pss, bool oppositeAngle, SortingLayer sortingLayer, UserLayer userLayer, string tag){
        foreach(ParticleSystem ps in pss){
            if(ps != null && pss != null){
                ps.GetComponent<ParticleSystemRenderer>().sortingLayerName = sortingLayer.ToString(); 
                ps.transform.eulerAngles = new Vector3(0,0,Methods.GetAngleByDirection(oppositeAngle, currentDirection));
                ps.tag = tag;
                ps.gameObject.layer = (int)userLayer;
                ps.Play();
            }
        }
    }

    private void DestroyEffect(TypeEffect type)
    {
        Destroy(currentEffect);
        currentEffect = null;
        if(type == TypeEffect.Action)
        {
            isUsingMenuActions = false;
            canWalkAndRun = true;
            animator.ResetTrigger(Params.TriggerAtk);
            animator.SetTrigger(Params.TriggerIdle);
        }
    }

    #endregion



    #region coroutines
    public IEnumerator PlayEffect(DataSpells s, bool stopEffect, GameObject effetAlternatif, float demarrageAlternatif, bool afficherAlerte, bool isChild, TypeEffect type)
    {
        if(!isReact){
            isReact = true;
            if(s == null){
                s = new DataSpells(effetAlternatif.name, effetAlternatif.tag);
                s.StartSpell = demarrageAlternatif;
                s.ParticleEffect = effetAlternatif.name;
            }

            // Create effect
            if(estUnPerso && type == TypeEffect.Action)
                isUsingMenuActions = true;

            currentEffect = s.Instantiate(this.transform.position.x, this.transform.position.y).obj;
            currentEffect.tag = s.tag;
            if(isChild && s.tag == Params.TagIce){
                float fixedScale = 1f;
                SpriteRenderer spriteRender = GetComponent<SpriteRenderer>();
                SpriteRenderer childSpriteRender = currentEffect.GetComponent<SpriteRenderer>();
                currentEffect.transform.parent = transform;
                currentEffect.transform.position =
                    new Vector2 (currentEffect.transform.position.x
                    , currentEffect.transform.position.y - 0.01f);
                currentEffect.transform.localScale =
                    new Vector3 (fixedScale/transform.localScale.x + spriteRender.bounds.extents.x
                    , fixedScale/transform.localScale.y + spriteRender.bounds.extents.y
                    , 1);
                currentEffect.layer = (int)UserLayer.Default;
            }else if(isChild && s.tag == Params.TagFire){
                currentEffect.transform.parent = transform;
                currentEffect.layer = (int)UserLayer.Magic;
            }else if(isChild && s.tag == Params.TagSmoke){
                currentEffect.transform.parent = transform.parent;
                currentEffect.layer = (int)UserLayer.Magic;
            }else{
                currentEffect.layer = (int)UserLayer.Default;
            }

            ParticleSystem[] pss = currentEffect.GetComponentsInChildren<ParticleSystem>();
            if(afficherAlerte)
                DialogueManager.ShowAlert(s.Name);

            // Start effect
            yield return new WaitForSeconds(s.StartSpell);
            if(type == TypeEffect.MirrorEffect){
                StartEffect(pss, true, SortingLayer.Default, UserLayer.Default, s.tag);
            }else if(type == TypeEffect.Effect){
                StartEffect(pss, false, SortingLayer.Default, UserLayer.Magic, s.tag);
            }else{
                if(estUnPerso){ 
                    animator.ResetTrigger(Params.TriggerIdle);
                    animator.ResetTrigger(Params.TriggerRun);
                    animator.SetTrigger(Params.TriggerAtk);
                }
                canWalkAndRun = false;
                StartEffect(pss, false, SortingLayer.Magic, UserLayer.Magic, s.tag);
            }

            if(stopEffect){
                // Stop effect
                yield return new WaitForSeconds(s.StopSpell);
                foreach(ParticleSystem ps in pss){
                    if(ps != null && pss != null)
                        ps.Stop();
                }

                // Destroy effect
                yield return new WaitForSeconds(s.DestroySpell);
                DestroyEffect(type);
            }
            isReact = false;
        }
    }

    public IEnumerator SetOnIgnoreLayer(float duration){
        if(tag != Params.TagPlayer){
            gameObject.layer = (int)UserLayer.Ignore;
            yield return new WaitForSeconds(duration);
            gameObject.layer = layerOrigin;
        }
    }

    public IEnumerator SetHitObject(float duration){
        isHitObject = true;
        yield return new WaitForSeconds(duration);
        isHitObject = false;
    }

    #endregion
}
