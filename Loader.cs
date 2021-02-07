using CotcSdk;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Security.Permissions;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Classe à charger au début du jeu, contenant les variables globales
/// </summary>
[Serializable ()]
public class Loader : MonoBehaviour, ISerializable	
{
    public ListeScenes SceneADemarrer;
	public GameStatus EtatDuJeu;
	public static GameObject obj;
    public static string SceneDeDemarrage;
    public static string  ScenePrecedente = string.Empty;
    public static string  SceneActuelle = string.Empty;
	public static Dictionary<string, DataCharacters> ListCharacter = new Dictionary<string, DataCharacters>();
    public static List<DataCharacters> ListAllies = new List<DataCharacters>();
	public static List<DataCharacters> ListEnemies = new List<DataCharacters>();
	public static List<DataItems> Inventaire = new List<DataItems>();
	[NonSerialized]
	public static Vector3  PlayerPosition = default(Vector3);
    public static int PlayerXPosition = default(int);
    public static int PlayerYPosition = default(int);
    public static int PlayerZPosition = default(int);
    public static bool  ControlsBlocked = false;
    public static bool  JustEnteredTheScreen = false;
	public static GameStatus gameStatus;
	public static bool scoreEnabled = false;
	public static string score = String.Empty;
    public static Camera cameraUI;
    public static Camera cameraMap;
	// Vide par défaut. Si nécessaire, jouer Xtralife.AddXtralife() pour le rajouter automatiquement
	public static Xtralife cloudObj;
	public static CotcGameObject cloudSDK;
    public static bool IsGamePaused { 
		get{ return Time.timeScale ==0; } 
	}

    void Awake()
	{
		obj = this.gameObject;
		gameStatus = EtatDuJeu;
		SceneDeDemarrage = this.SceneADemarrer.ToString();
		if(string.IsNullOrEmpty(SceneDeDemarrage))
			Debug.LogError("Loader : Aucune scène à démarrer !");


		ScenePrecedente  = string.Empty;
		SceneActuelle  = SceneDeDemarrage;

		PlayerPosition  = default(Vector3);
		PlayerXPosition  = default(int);
		PlayerYPosition  = default(int);
		PlayerZPosition  = default(int);
		ControlsBlocked  = false;
		JustEnteredTheScreen  = false;

		// Remplit la database lors d'une nouvelle partie
		Database.PopulateDatabase(); 
		InitializeDatabase();

		DontDestroyOnLoad(gameObject);
		
		Debug.Log("Loader : Démarrage de la scène "+SceneDeDemarrage);
       	SceneManager.LoadScene(SceneDeDemarrage); 
	}

	public static void LoadScene(string nextScene)
	{
		ScenePrecedente = SceneActuelle;
		SceneActuelle = nextScene;
		Debug.Log("Loader : Changement de scène  : "+nextScene+", scène précédente : "+ScenePrecedente);
		SceneManager.LoadScene(nextScene); 
	}

    /// <summary>
    /// Mettre le jeu en pause
    /// </summary>
    /// <param name="pause">Si mis à true, jeu en pause</param>
    public static void PauseGame(bool pause)
	{
		if (pause)
			Time.timeScale = 0;
		else 
			Time.timeScale = 1;
	}

    public static void InitializePlayerPosition()
	{
		PlayerPosition = default(Vector3);
		PlayerXPosition = default(int);
		PlayerYPosition = default(int);
		PlayerZPosition = default(int);
	}

	public void InitializeDatabase(){
		foreach(KeyValuePair<string, DataCharacters> character in Database.DataCharacters){
			ListCharacter.Add(character.Value.PicturesName, character.Value);
		}

		ListAllies.Add(Database.DataCharacters["Dragonnet"]);
		ListAllies.Add(Database.DataCharacters["Nello"]);
		ListAllies.Add(Database.DataCharacters["Nello"]);
		ListAllies.Add(Database.DataCharacters["Nello"]);

		ListEnemies.Add(Database.DataCharacters["Fureuil"]);
		ListEnemies.Add(Database.DataCharacters["Nello"]);

		Debug.Log("Loader : InitializeDatabase() : personnages chargés : "+ListCharacter.Count()+", alliés chargés : "+ListAllies.Count()+", ennemis chargés : "+ListEnemies.Count());
	}

   #region ISerializable implementation
    /// <summary>
    /// Initializes a new instance of the <see cref="Main"/> class.
    /// </summary>
    public Loader (){}
    /// <summary>
    /// Initializes a new instance of the <see cref="Main"/> class.
    /// </summary>
    /// <param name="info">The information.</param>
    /// <param name="context">The context.</param>
    public Loader (SerializationInfo info, StreamingContext context)
	{
		ScenePrecedente = (string) info.GetValue ("scenePrecedente",typeof(string));
		SceneActuelle = (string) info.GetValue ("sceneActuelle",typeof(string));
		PlayerXPosition = (int) info.GetValue ("playerXPosition",typeof(int));
		PlayerYPosition = (int) info.GetValue ("playerYPosition",typeof(int));
		PlayerZPosition = (int) info.GetValue ("playerZPosition",typeof(int));
		PlayerPosition = new Vector3 (PlayerXPosition, PlayerYPosition, PlayerZPosition);
		ControlsBlocked = false;
		JustEnteredTheScreen = false;
	}
#pragma warning disable  // Type or member is obsolete
                              /// <summary>
                              /// Gets the object data.
                              /// </summary>
                              /// <param name="info">The information.</param>
                              /// <param name="context">The context.</param>
    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
#pragma warning restore  // Type or member is obsolete
    public void GetObjectData (SerializationInfo info, StreamingContext context)
	{
		info.AddValue ("scenePrecedente", ScenePrecedente);
		info.AddValue ("sceneActuelle", SceneActuelle);
		info.AddValue ("playerXPosition", PlayerPosition.x);
		info.AddValue ("playerYPosition", PlayerPosition.y);
		info.AddValue ("playerZPosition", PlayerPosition.z);
	}

    public static void Save() {
#pragma warning disable
        Main data = new Main ();
#pragma warning restore 
        Stream stream = File.Open(Settings.SavedFileName, FileMode.Create);
		BinaryFormatter bformatter = new BinaryFormatter();
		bformatter.Binder = new VersionDeserializationBinder(); 
		Debug.Log ("Ecriture des données");
		bformatter.Serialize(stream,  data);
		stream.Close();
	}

    public static void Load() {
		//Main data = new Main ();
		Stream stream = File.Open(Settings.SavedFileName, FileMode.Open);
		BinaryFormatter bformatter = new BinaryFormatter();
		bformatter.Binder = new VersionDeserializationBinder(); 
		Debug.Log ("Lecture des données");
		bformatter.Deserialize(stream);
		stream.Close();

		if (!string.IsNullOrEmpty (Main.CurrentScene))
			SceneManager.LoadScene (Main.CurrentScene);
		else
			SceneManager.LoadScene (Settings.MainMenuScene);
	}


    /// <summary>
    /// Class VersionDeserializationBinder. This class cannot be inherited.
    /// </summary>
    public sealed class VersionDeserializationBinder : SerializationBinder 
	{
        /// <summary>
        /// Binds to type.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>Type.</returns>
        public override Type BindToType( string assemblyName, string typeName )
		{ 
			if ( !string.IsNullOrEmpty( assemblyName ) && !string.IsNullOrEmpty( typeName ) ) 
			{ 
				Type typeToDeserialize = null; 
				assemblyName = Assembly.GetExecutingAssembly().FullName; 
				// The following line of code returns the type. 
				typeToDeserialize = Type.GetType( String.Format( "{0}, {1}", typeName, assemblyName ) ); 
				return typeToDeserialize; 
			} 
			return null; 
		} 
	}
	#endregion
}
