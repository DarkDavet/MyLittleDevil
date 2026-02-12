# MyLittleDevil

It is a 2D adventure game inspired by the Flappy Bird mechanics. Play as a little devil on a mission to defeat the forces of heaven. Collect powerful items, cast spells, summon loyal minions, and battle angels while navigating through challenging obstacles.

## Technical description
* Unity Version: 2021.3.16f1
* Render Pipeline: Built-In Render Pipeline
  
## Key features
* Architecture:
<br>- implemented abstract factory for spawning projectiles and allied units. The factory dynamically creates different entity types based on the player's chosen "Aspect"
<br>- developed a base AI logic designed for easy scalability, allowing rapid implementation of new enemy and ally types
<br>- Built a structured workflow for rapid level prototyping and assembly
<br>- integrated a behavior tree system to manage complex AI decision-making and combat patterns
* Optimization:
<br>- using object pooling for frequently reused projectiles, interactive game objects, dynamic UI icons
<br>- all 2D assets are organized into sprite atlases
<br>- UI elements are categorized into separate Canvas groups
* UI:
<br>- a functional in-game inventory allowing players to manage and use items during active playthroughs   

## Project structure
* Assets/Scripts - core game logic, including systems for combat, AI and factories.
* Assets/Prefabs - game entities, including enemies, items, minions and UI modules.


## How to launch?
1. Clone the repository
2. Open the project in Unity (2021.3.16f1)
3. Open and run the scene Assets/Scenes/MainMenu.unity


