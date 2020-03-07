# The Hoard

## Ideation
During the ideation process I tried to come up with everything I could think of that correlated with "spreading". The ideas I came up with were:
- Virus (Zombie)
- Butter
- Jam
- Disco Fever
- Power
- Influence

## Culling
During the culling phase I determined that the easiest idea to work with given the time frame was the Zombie Virus. I decided on a top down zombie survival game where the objective is to kill as many zombies as possible to reduce the spread of the zombie virus. I utilized the Simple Apocalypse assets that I purchased on HumbleBundle because they came with animations and had zombie assets and weapons. 

## Implementation Information
This game was developed using unity.

### Weapons
There are 2 weapons in the game:
- Desert Eagle
- AK-47

Weapons have spotlights mounted to them to simulate a flashlight. They also enable/disable a point light when firing to simulate muzzle flash.

### Zombies
There are 25 different zombie prefabs. There are always 50 zombies spawned into the map, but they localize to near the player area. Their spawn location and orientation is randomized, as are their running speed, health value, and damage value. Zombies wander aimlessly until they are either shot by the player, or they see the player. Both of these result in them chasing the player.

## Screenshots
![](https://github.com/gmfereday/GameJam/blob/master/StartPrompt.PNG)
This screenshot shows the prompt shown to the player at the beginning of the game.

![](https://github.com/gmfereday/GameJam/blob/master/ScoreAndEnemy.PNG)
This screenshot shows the score having been updated by killing zombies and shows what zombies look like.

![](https://github.com/gmfereday/GameJam/blob/master/Death.PNG)
This screenshot shows what is displayed to the player when they die.

## Controls
The game's controls are fairly simple:
- Movement: WASD (LShift for sprint)
- Aim: Mouse Movement
- Shoot: Spacebar
- Change Weapon: F

## [Gameplay Video](https://github.com/gmfereday/GameJam/blob/master/GameplayVideo.mp4)
