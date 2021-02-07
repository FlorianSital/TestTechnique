# TestTechnique
 Fichiers impactés par les fonctionnalités d'Xtralife

#Description des fichiers :
## Loader
Il s'agit du script principal du jeu, disponible partout, et dont l'objet n'est pas détruit même après un changement de sècne.
Il coordonne les données principales du joueur, de la partie en cours, ou le contenu du jeu venant de la classe Database.cs qui n'est pas montrée ici.
J'y stocke le SDK CotC si nécessaire, ainsi qu'un script Xtralife pour agir dessus, ou grâce à lui.

## Params
C'est la classe listant les constantes, enums et méthodes redondantes. Pas directement impliqué, mais je le mets à disposition car il est très présent partout.

## MenuPrincipal
## TourDeLance
Ce sont deux scripts correspondant à deux scènes, respectivement celle du Menu principal lorsqu'on lance l'application, ainsi que le script qui lance le niveau en ligne.
Les deux font appel à la classe Xtralife.cs lorsqu'une fonctionnalité en ligne est nécessaire, comme l'inscription et la connexion pour le menu, ou le leaderboard pour le niveau.
Les différentes méthodes sont appelées grâce à des boutons ou événements en jeu.

## ObjectController
Une grosse classe rattachée à chaque objet visible dans un niveau pouvant interagir physiquement avec le joueur d'une façon ou d'une autre.
J'y ai simplement rajouté l'attribution d'un score lors de la destruction de l'objet par une action du joueur, pour alimenter le leaderboard.

## Xtralife
Une classe créée spécifiquement pour l'occasion, où je fais appel aux différentes fonctionnalités expliquées dans la documentation du SDK d'Xtralife.
J'y ai rajouté des appels à des PopUp de messages à l'écran, pour l'informer des différentes opérations en cours.
