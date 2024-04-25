# Expérimentations avec la bibliothèque Unity de Machine Learning : ML-Agents

## Auteur: [Najib El khadir](https://github.com/NajibXY)

## 1. Motivations

<figure text-align="right">
  <img align="right" src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/gifs/car_controller_agent/advanced_training.gif" width="400">
</figure>

Afin de monter en compétence sur Unity et en Machine Learning, j'ai décidé de monter un projet de diverses expérimentations dans des environnements d'apprentissage par renforcement
que je conçois en parallèle dans le moteur Unity.
</br>
Cela implique plusieurs axes de développement :
- Concevoir des Scènes sur Unity.
- Concevoir des Controlleurs et des Agents apprenants sur Unity.
- Concevoir et dérouler des scénarios d'apprentissage automatique grâce à la bibliothèque ML-Agents.</br>

Ainsi qu'un objectif à court terme : développer un agent capable de controller une voiture et conduire sur n'importe quelle scène Unity adaptée. 

</br> 

## 2. Technologies Utilisées
![](https://skillicons.dev/icons?i=python,pytorch,anaconda,cs,unity)
- Python 3.9, PyTorch, Conda (pour mon environnement personnel)
- Bibliothèque Python ML-Agents 0.30.0
- C#, Unity 2022.3.14f1, package [ML-Agents](https://github.com/Unity-Technologies/ml-agents) : faire attention à ce que votre version de Unity soit compatible avec votre version du package.

### Configuration de votre environnement Python

- Vous devrez configurer un environnement Python (dans mon cas conda), de préférence 3.9.13 pour éviter les problèmes de compatibilité de bibliothèques.
- Pour installer les dépendances avec conda, vous pouvez simplement exécuter :
  > conda create --name `<votre_nom_env>` --file requirements.txt
- Ou si vous utilisez pip :
  > pip install -r requirements.txt
- Ensuite, vous pouvez tester si la commande `mlagents-learn` fonctionne. C'est cette commande qui déclenche l'apprentissage et écoute sur un port dédié votre fenêtre de projet Unity.
  
## 3. Références
- [Set up un repo Git sur un projet Unity](https://unityatscale.com/unity-version-control-guide/how-to-setup-unity-project-on-github/)
- [Documentation de la bibliothèque ML-Agents](https://unity-technologies.github.io/ml-agents/ML-Agents-Toolkit-Documentation/)
- [Tutoriel sur l'utilisation de ML-Agent dans Unity](https://www.youtube.com/watch?v=zPFU30tbyKs&list=PLzDRvYVwl53vehwiN_odYJkPBzcqFw110&ab_channel=CodeMonkey)

## 4. Environnement [Reach The Goal](https://github.com/NajibXY/Unity-ML-Agents/tree/master/unity/MLAgentsTests)

<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/images/reach_the_goal_scene.png" width="600">

### Préparation de la Scène

- L'important avec les scénarios d'apprentissage c'est de concevoir un environnement adapté.
- Pour l'agent dont le but est d'atteindre sa cible, la scène a été conçue, à partir d'un exemple de la bibliothèque ML-Agents, de manière a avoir un prefab controllable `Basic` dont le but est d'atteindre une balle, dans un environnement cloîsonné.
- Dans le contexte de l'apprentissage l'agent peut se déplacer à sa guise dans l'environnement sur l'axe <X,Z>. Il reçoit une récompense négative (e.g positive) et l'épisode d'apprentissage se termine quand l'agent rentre en contact avec les murs (e.g la balle).
- La logique de l'implémentation de l'agent est possible grâce à l'interface `Agent` des classes C# de la bibliothèque ML-Agent.
- Le script à rattacher à l'agent est consultable ici [MoveToGoalBasic.cs](https://github.com/NajibXY/Unity-ML-Agents/blob/master/unity/MLAgentsTests/Assets/Scripts/MoveToGoalBasic.cs)
- Les composants relatifs à la bibliothèque ML-Agents sont les suivants : </br>
  <img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/images/reach_the_goal_agent_params.png" width="250">

### Préparation de l'apprentissage

- Une fois que votre scène est adaptée à l'apprentissage, nous pouvons cloner l'environnement plusieurs fois pour avoir plusieurs environnements participants en parallèle et ainsi décupler la vitesse d'apprentissage. 
- De la randomization pour le spawn de l'agent et de la balle au début de chaque épisode est également implémentée dans le script.
- Une colorisation de la surface de la Scène permet aussi de savoir quel agent a récemment été en réussite (atteindre la balle) ou en échec (toucher un mur).

### Lancement de l'entraînement
- Afin de commencer l'entrainement retire le modèle et on met le comportement par défaut dans le composant `Behaviour Parameters` de l'agent.
- Puis on déroule dans l'environnement Python la commande :
  > mlagents-learn --run-id={un_id}
- Ensuite on démarre la scène côté Unity.

#### Début de l'apprentissage
<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/gifs/reach_the_goal_agent/training_begins.gif" width="650">
Au début l'agent apprend à se déplacer et tente des actions au hasard. Parfois il touche un mur.

#### Milieu de l'apprentissage 
<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/gifs/reach_the_goal_agent/middle_of_training.gif" width="650">
A partir de quelques milliers d'étapes, l'agent applique une heuristique qui se fiabilise au fil des épisodes.

#### Après 50000~ étapes
<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/gifs/reach_the_goal_agent/end_of_training.gif" width="650">
Après quelques dizaines de milliers d'étapes d'apprentissage, l'agent a développé une solide stratégie et a appris à maitriser le déplacement vers l'objectif.

### Résultats
- Le résulat est exporté dans un dossier [results](https://github.com/NajibXY/Unity-ML-Agents/tree/master/results).
- Le dossier de résultat comporte énormément d'informations qu'on peut visualiser grâce à `Tensorboard` ainsi qu'un [Brain](https://github.com/NajibXY/Unity-ML-Agents/blob/master/results/MoveToGoalWithParametersAndRandomization_03/MoveToGoalBasic.onnx) importable dans les assets Unity et utilisable dans les composants de l'agent.
- En chargeant le fichier `.onnx` dans le modèle utilisé par l'agent et en mettant le comportement à `Inference only`, on peut observer un comportement parfaitement automatisé et réutilisable :
<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/gifs/reach_the_goal_agent/trained_brain.gif" width="650">

## 4. Environnement [Car Controller](https://github.com/NajibXY/Unity-ML-Agents/tree/master/unity/CarTrackAgent) - Imitation Learning

<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/images/car_controller_scene.png" width="600">

### Préparation de la Scène

- Encore plus que pour l'agent `Reach_The_Goal`, il est très important ici de concevoir l'environnement d'apprentissage le plus adapté possible
- Pour l'agent dont le but est d'apprendre à conduire sur un circuit, la scène a été conçue en utilisant plusieurs assets. Cela a impliqué la construction d'une voiture et de ses différents composants, d'une route circulaire avec des barrières servant de cadre à l'entraînement, ainsi qu'un système de CheckPoint ordonné autour duquel les récompenses de l'apprentissage ont été conçues.
- L'agent peut accélérer, ralentir, reculer, tourner à gauche et tourner à droite.
- Si entre en collision avec les barrières, il reçoit une récompense négative.
- S'il reste au contact des barrières il reçoit une récompense négative tempérée.
- S'il atteint son prochain Checkpoint (l'ordre est important), il reçoit une récompense positive.
- S'il atteint le dernier Checkpoint (la ligne d'arrivée) et finit un tour, il reçoit une grosse récompense positive.
- S'il atteint un mauvais Checkpoint (en arrière), il reçoit une récompense négative.
- L'agent a également un composant `Ray Perception Sensor 3D` tunable, permettant de détecter les barrières et les checkpoints.
- Le script à rattacher à l'agent est consultable ici [CarController.cs](https://github.com/NajibXY/Unity-ML-Agents/blob/master/unity/CarTrackAgent/Assets/Scripts/CarController.cs)
- Les composants relatifs à la bibliothèque ML-Agents sont les suivants : </br>
  <img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/images/car_controller_params.png" width="250">

### Préparation de l'apprentissage

- Comme précédemment, une fois que votre scène est adaptée à l'apprentissage, nous pouvons cloner l'environnement plusieurs fois pour avoir plusieurs environnements participants en parallèle et ainsi décupler la vitesse d'apprentissage. Sauf qu'ici, nous allons simplement décupler le `CarController` pour avoir plusieurs voitures qui intéragissent avec le même environnement.
- Il faut bien veiller à désactiver les collisions entre les voitures, notre objectif se rapprochant plus d'un bot TrackMania que Forza.

### Utilisation d'un Demonstration Recorder

- Afin d'aider à l'apprentissage de l'agent, j'ai utilisé un composant de type Demonstration Recorder, afin de "filmer" un tour complet du circuit qui servira de base à l'entraînement. J'ai fait quelques erreurs
durant mon tour, volontaires (ou pas ?) afin de ne minimiser l'aide fournie par cette démo.

#### Fichier de configuration YAML
- Afin de pouvoir charger la démo pour l'apprentissage il faut rédiger un fichier de configuration YAML.
- J'ai rédigé un exemple de configuration [ici](https://github.com/NajibXY/Unity-ML-Agents/blob/master/config_custom/car_imitation.yaml), tunez-le comme souhaité.

### Lancement de l'entraînement
- Afin de commencer l'entrainement retire le modèle et on met le comportement par défaut dans le composant `Behaviour Parameters` de l'agent.
- Puis on déroule dans l'environnement Python la commande :
  > mlagents-learn config_custom/car_imitation.yaml --run-id={un_id}
- Ensuite on démarre la scène côté Unity.

#### Début de l'apprentissage
<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/gifs/car_controller_agent/begin_of_training.gif" width="650">
Au début l'agent apprend tâtonne, apprend à avancer, à reculer et bouge dans tous les sens.

#### Après 100 000 étapes
<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/gifs/car_controller_agent/after_begin_training.gif" width="650">
A partir de quelques 100 000 étapes, l'agent a presque appris à cibler les premiers checkpoints.

#### Après 500 000 étapes
<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/gifs/car_controller_agent/mid_training.gif" width="650">
Après quelques centaines de milliers d'étapes d'apprentissage, l'agent commence à bien reconnaître son objectif et à faire des tours. Certains agents font des tours en sens inverse, d'autres se crashent,
ce qui rassure par rapport à la propagation du bruit dans le modèle.

#### Après 1 500 000 étapes
<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/gifs/car_controller_agent/advanced_training.gif" width="650">
Après plus d'un million d'étapes d'apprentissage, l'agent est fluide dans le circuit. Il a appris à faire des tours !

#### Observation
Au delà de 2 000 000 d'étapes, l'agent passe dans une phase bizarre de surapprentissage où il "régresse" et les comportements observés sont tout autant intriguants que néfastes pour le résultat final. 
D'autres expérimentations seront menées pour mieux comprendre cela, même si c'est une limitation classique des algorithmes d'apprentissage par renforcement.


### Résultats
- Le résulat est exporté dans un dossier [results](https://github.com/NajibXY/Unity-ML-Agents/tree/master/results).
- Le dossier de résultat comporte énormément d'informations qu'on peut visualiser grâce à `Tensorboard` ainsi qu'un [CarControllerBrain](https://github.com/NajibXY/Unity-ML-Agents/blob/master/results/imitation_car_test_20/CarController/CarController-1499948.onnx) importable dans les assets Unity et utilisable dans les composants de l'agent.
- En chargeant le fichier `.onnx` dans le modèle utilisé par l'agent et en mettant le comportement à `Inference only`, on peut observer un comportement parfaitement automatisé et réutilisable :
<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/gifs/car_controller_agent/trained_brain_demo_game.gif" width="650">

### Objectif 
- L'objectif actuel étant de développer un agent capable de conduire sur n'importe quelle circuit sur Unity (tant que le script de l'agent expose certaines fonctions nécessaires du package ML-Agent), il faut
que je réalise l'apprentissage sur plusieurs types de circuits en même temps afin d'avoir un agent résultant adaptatif.
- Affaire à suivre !
<img src="https://github.com/NajibXY/Unity-ML-Agents/blob/master/assets/gifs/car_controller_agent/trained_brain_demo_scene.gif" width="650">

</br></br>

## 5. Améliorations Possibles

- Entrainer le modèle CarController sur d'autres circuits.
- Continuer les expérimentations avec d'autres modèles d'apprentissage profond par renforcement.
- [...]
