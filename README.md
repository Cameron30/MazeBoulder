# MazeBoulder
Partial Unity Project with fully integrated Oculus Multiplayer

This project was created as part of an unpaid internship that I participated in.
## NOTE: ALL BOUGHT ASSETS HAVE BEEN REMOVED

# Setup
The unity project is intended for Unity 2019.3 or lower (down to 2018.0). 

To implement multiplayer functionality, you need to create a new app at dashboard.oculus.com, then add that appID to Unity. Next, you must upload the app at least once to dashboard.oculus.com, then you should be free to go.

# About
This Unity project implements multiple different scenes. These scenes include a randomly generating maze with a guaranteed path, a set maze with carefully implemented traps that are fully working, and others.

The game has the following intended 'classes':
  - Cartographer (given a map of the RNG maze)
  - Drone Operator (given a drone that the player can fly to reveal traps on the map)
  - Adventurer (given a grappling gun to traverse many traps)
  - Doctor (given the ability to turn flowers into potions for health regeneration)
  
# Highlights
My proudest implementations follow:
  - Complete integration into the Oculus ecosystem. From-scratch implementation of multiplayer aspects:
    - Object orientation, rotation, acceleration and Avatar information converted to bits, and sent as a packet to the end-users, who decode and apply the information.
    - Newest version of Oculus Avatars, which inludes more detailed faces and expressions.
    - Invitations through the Oculus menu and Oculus Home.
    - Voice chat with moving Avatar mouths.
  - A randomly generated maze with guaranteed exits, with randomly placed traps and walls that can be rotated in any (possible) orientation, leading to nearly infinite maze combinations. 

