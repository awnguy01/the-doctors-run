# The Doctor’s Run

*Genre: Procedurally Generated Horror-lite Endless Sidescroll Runner*

*Objective: Run forward while collecting coins to increase the score and potions to stay alive*

## Room Design
-	Rooms randomly generated and destroyed as the player moves further right
-	Space cloud Skybox
-	Background wallpaper room tiles
-	Bloody text and blood splotches
-	Omnipresent force lurking in the windows

## Player Controls:
-	Left/right movement with arrow keys
-	Jump with space
-	Crouch with Ctrl key

## Enemy Design: 
-	Turret anchored to right side of screen moving randomly along the y-axis
-	Turret fires missiles on a set interval
-	Speed of turret and speed of missiles increases on a logarithmic scale relative to the player’s score
-	Collision trigger with a missile results in the loss of 1 player heart

## Game Over Condition:
-	Run out of hearts after hitting too many missiles
-	Can restart game on Game Over screen

## Game Start:
-	Player starts with 3 hearts (can collect more for a max of 5 hearts)
-	Turret movement speed at 1 unit/s 
-	Missile speed at 1 unit/s
