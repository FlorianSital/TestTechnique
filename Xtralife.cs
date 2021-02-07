using CotcSdk;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class Xtralife : MonoBehaviour
{
    private CotcGameObject cotc;
    private Cloud currentCloud;
    public static Gamer currentGamer;
    public static bool coroutineRun;

	public void Start()
    {
        coroutineRun = false;
        Loader.cloudObj = this;
		cotc = FindObjectOfType<CotcGameObject>();
        if(cotc == null){
            Debug.LogError("Xtralife - Start() : Objet CotcGameObject non trouvé. Insérez le dans la scène.");
            return;
        }else{
            Debug.Log("Xtralife - Start() : Setup start");
        }

		Promise.UnhandledException += (object sender, ExceptionEventArgs e) => {
			Debug.LogError("Xtralife - Start() : Exception non gérée : " + e.Exception.ToString());
		};

		cotc.GetCloud().Done(cloud => {
            currentCloud = cloud;
			cloud.HttpRequestFailedHandler = (HttpRequestFailedEventArgs e) => {
				if (e.UserData == null) {
					e.UserData = new object();
					e.RetryIn(1000);
                    Debug.LogWarning("Xtralife - Start() : Echec du setup. Nouvelle tentative...");
				}
				else
					e.Abort();
			};
			Debug.Log("Xtralife - Start() : Setup done");
		});
	}

    public static void AddXtralife()
    {
        Loader.obj.AddComponent<Xtralife>();
    }

    public IEnumerator LoginAnonymous()
    {
        DialogueManager.ShowAlert("Connexion en cours.... Patientez.");

        currentCloud.LoginAnonymously().Done(gamer => {
            currentGamer = gamer;
            Debug.Log("Xtralife - LoginAnonymous() :"+
            "\nConnexion anonyme réussie (ID = " + gamer.GamerId + ")"+
            "\nLogin data: " +gamer+
            "\nServer time: " + gamer["servertime"]);
            DialogueManager.ShowAlert("Connexion en tant que '"+currentGamer["profile"]["displayName"]+"'");
        }, ex => {
            CotcException error = (CotcException)ex;
            DialogueManager.ShowAlert("Connexion échouée. Mauvais format, ou mauvais mot de passe sur un compte existant.");
            Debug.LogWarning("Xtralife - LoginAnonymous() : Connexion échouée: " + error.ErrorCode + " (" + error.HttpStatusCode + ")");
        });
        while(currentGamer == null)
            yield return new WaitForSeconds(0.1f);
        BlackFade.instance.PlayFadeOutBlackChangeLevel("TourDeLance");
    }

    public IEnumerator Login(string gamerId, string gamerMdp)
    {
        DialogueManager.ShowAlert("Connexion en cours.... Patientez.");

        currentCloud.Login(
            network: "email",
            networkId: gamerId,
            networkSecret: gamerMdp)
        .Done(gamer => {
            currentGamer = gamer;
            Debug.Log("Xtralife - Login() :"+
            "\nConnexion réussie (ID = " + gamer.GamerId + ")"+
            "\nLogin data: " +gamer+
            "\nServer time: " + gamer["servertime"]);
            DialogueManager.ShowAlert("Connexion en tant que '"+currentGamer["profile"]["displayName"]+"'");
        }, ex => {
            CotcException error = (CotcException)ex;
            DialogueManager.ShowAlert("Connexion échouée. Mauvais format, ou mauvais mot de passe sur un compte existant.");
            Debug.LogWarning("Xtralife - Login() : Connexion échouée: " + error.ErrorCode + " (" + error.HttpStatusCode + ")");
        });
        while(currentGamer == null)
            yield return new WaitForSeconds(0.1f);
        BlackFade.instance.PlayFadeOutBlackChangeLevel("TourDeLance");
    }

    public IEnumerator Logout()
    {
        DialogueManager.ShowAlert("Déconnexion en cours...");
        cotc.GetCloud().Done(cloud => {
        currentCloud.Logout(currentGamer)
            .Done(result => {
                Debug.Log("Xtralife - Logout() : Joueur "+ currentGamer.NetworkId +" déconnecté.");
                DialogueManager.ShowAlert("Vous êtes déconnecté.");
            }, ex => {
                CotcException error = (CotcException)ex;
                Debug.LogError("Xtralife - Logout() : Déconnexion échouée: " + error.ErrorCode + " (" + error.HttpStatusCode + ")");
                DialogueManager.ShowAlert("Déconnexion échouée.");
            });
        });  
        yield return null;
    }

    public IEnumerator GetLeaderboard(System.Action<List<LeaderboardRank>> result)
    {
        coroutineRun = true;
        Debug.Log("Xtralife - GetLeaderboard() : Début de la mise à jour du leaderboard...");
        // Mise à jour du leaderboard
        bool firstStep = true;
        DialogueManager.ShowAlert("Connexion en cours.... Patientez.");
        long score = 0;
        if (!long.TryParse(Loader.score, out score)) score = 0;
        currentGamer.Scores.Domain("private").Post(score, "destruction", ScoreOrder.HighToLow,
        "Score de destruction", false)
        .Done(postScoreRes => {
            DialogueManager.ShowAlert("Leaderboard mis à jour.");
            Debug.Log("Xtralife - GetLeaderboard() : Leaderboard mis à jour");
            firstStep = false;
        }, ex => {
            CotcException error = (CotcException)ex;
            DialogueManager.ShowAlert("Connexion échouée.");
            Debug.LogError("Xtralife - GetLeaderboard() : Impossible de poster le score : " + error.ErrorCode + " (" + error.ErrorInformation + ")");
            firstStep = false;
            return;
        }); 
        while(firstStep)
            yield return new WaitForSeconds(0.1f);
        
        // Requête des meilleurs joueurs
        List<LeaderboardRank> bestHighScores = new List<LeaderboardRank>();
        currentGamer.Scores.Domain("private").BestHighScores("destruction", 10, 1)
        .Done(bestHighScoresRes => {
            foreach(var highScore in bestHighScoresRes){
                bestHighScores.Add(new LeaderboardRank(highScore.Rank, highScore.GamerInfo["profile"]["displayName"], highScore.Value));
            }
            DialogueManager.ShowAlert("Vous avez réalisé "+Loader.score+" points !");
            Debug.Log("Xtralife - GetLeaderboard() : Score du joueur : "+Loader.score);
            Loader.score = "0";
            result(bestHighScores);
            coroutineRun = false;
        }, ex => {
            CotcException error = (CotcException)ex;
            DialogueManager.ShowAlert("Connexion échouée.");
            Debug.LogError("Xtralife - GetLeaderboard() : Impossible de récupérer le classement : " + error.ErrorCode + " (" + error.ErrorInformation + ")");
            result(bestHighScores);
            coroutineRun = false;
        });
    }
}
