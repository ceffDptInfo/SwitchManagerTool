# SwitchManagerTool
Une application qui permet de lancé un server et un site web pour gérer les switches du département Informatique

## Installation Out-Of-The-Box
### Requirements
- Une machine linux (Ubuntu server, Alpine Linux ...)
- Docker installé
- Docker-Compose installé
- Connexion à internet

### Etapes
1. Connecté docker à Github GHCR (GitHub Container Registry) https://github.com/orgs/ceffDptInfo/packages
  - Faire parti de l'organisation Github CeffDptInfo
  - Utiliser la commande ```sudo docker login ghcr.io -u <USERNAME>```
  - Utiliser un Personnal Access Token de github comme mot de passe
2. Lancé l'environement docker-compose
  - Copier le fichier docker-compose.yml sur la machine linux
  - Entré la commande ```sudo docker-compose up -d```

## Mise à jour de l'environement Docker
Voici les étapes à suivre si vous voulez que les nouvelles images sur GHCR.io
### Etapes
1. Allez sur le site suivant : https://github.com/orgs/ceffDptInfo/packages
2. Copier la commande de pull: exemple ```docker pull ghcr.io/ceffdptinfo/switch-manager-tool-api:latest```
3. Exécuter ces deux commandes
   - ```sudo docker-compose down```
   - ```sudo docker-compose up -d```

## Publish le code comme images docker
Le build et Release du code sont gérés par l'outil DevOps "Github Actions". Cet outils permet lors d'un Git Push sur la branche "Main" de lancé des automatisations, commme:
  - L'execution des tests unitaire
  - Le build du projet API
  - Le Publish du projet API comme container dans GHCR
  - Le build du projet Frontend
  - Le Publish du projet Frontend comme container dans GHCR

Alors si vous voulez deployé le programme il suffit de faire ```git push``` a la racine du projet.
