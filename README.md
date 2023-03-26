# Rubiks-Cube
https://play.unity.com/mg/other/webgl-builds-327523

# GameController_abc:
  ### Controller for different gamemode, handle scramble algorithm, and make sure user input doesn't conflect each other (mouse and keyboard inputs).
  
  
# CubeHolder:
  ### Contains the cube with array of gameObject (Pieces), and numerical values of each piece.
  ### Contains basic functions that change the cube state.
  
  CheckSolved():  
      Check if the current cube is solved by first check the position of all pieces (relative to each other), then the orientation of individual pieces.
      
  GroupSide(side, piece): 
      Set the parent of all piece in the input side to input piece.
      
  UnGroupSide(side, piece):
      Ungroup the side from piece.
      
  StartNewStage(), GetStateChanged(): 
      Use for 3x3 and 3x3bld, check if the cube have been rotated (change of state).

  Switch Pieces:  
      Update the new numerical values of each piece after rotation. (Numerical Values of each piece can be seen at the top of CubeHolder file.)
      
  ### Side class:
      Group the pieces.
      For groups for side rotating, it starts with which piece the rotation it should follows with at index [0], and then the pieces in the side in clockwise direction (make easier for switch pieces).
  
  
# KeyboardActivities:
  ### Receive and process the keyboard input from the user.

  BUSY(): 
      Return wether there are any ongoing/pending task for keyboard input.
      
  KeyboardInputUpdate():
      Accept new Keyboard Inputs and add to queue (a list) using AddQ().
   
  ProcessingInput():
      Complete current taks or start a pending task.
      
  AddQ():
      Add new input to the queue or update queue if there are any cancelations. (UUUU = /, UUU = U', UU = U2...)
  
  CompleteTask():
      Finish ongoing task.
      
  FinishTask():
      Call by other calss to notify that rotation is finished.
     
     
# CubeRotate:
  ### Rotate the cube. Mainly used to assist processing Keyboard inputs.
  
  CompleteTask(s):
      Process string input by complete the rotation of the side.
  
  GroupAndRotateSide(side, n):
      Group the side and start rotating it. Integer n indecates the angle needed to be rotate.
      
  RotateSide():
      Actually rotating the side.


# MouseActivities:
  ### Receive and process mouse input.

  BUSY():
      Return wether there are ongoing mouse command or processing previous command (clicking or rotating).
      
  TriggerFreeRotate():
      Set FreeRotate to true or false. (Free rotate allow viewing cube by rotating multiple axies at once instead of only one.)
      
  MouseInputUpdate():
      Accept and process new mouse input.
      
  RightClickDrag():
      Process input from draging with right mouse click (Viewing the cube in different angle).
      
  LeftClickDrag():
      Process input from draging with left mouse click (Rotating the side).
      
  Decision():
      calculate which side should rotate from mouse input.
      
  RoundRotationg():
      Complete rotation of the side.
  


# controlls:
  ### outer layers: 
  R(right), L(left), U(up), D(down), F(front), B(back).
  
  ### middle layers:
  M(middle), E(equater), S(Standing).

  ### whole cube:
  X(similar to R), Y(similar to U), Z(similar to F).
  
  ### Combinations:
  - Using above command while holding ##shift## turn counter clock-wise instead.
  - Using outer layers command while holding ##control## turn two layers at once instead (ie control+L = L+M)
  
  ### Mouse Input:
  - Left click: Drag and rotate the side clicked on.
  - Right click: Rotating the whole cube to look around.
  
  ### Buttons:
  - 3x3: Start solving a 3x3 Rubik's cube, with 15 second inspecting time. 
  - 3x3 BLD: Start solving 3x3 Rubik's cube with blindfold, all sticker will be hidden once the user is ready. (Not Available yet)
  - FreePlay: Freely using all feature in the game and mess with the cube.
  - setting: Setting page. (Not Available yet)
  - quit: quit the game.
  
  - Scramble : Scramble the cube with 20 turns (Set sticker invisible if not in free play mode).
  - Reset (only in free play mode): Restart the scene and reset all movements.
  - Sticker (only in free play mode): show or hide stickers (Not yet implimented).
  - FreeRotate (Avialiable in game and setting): Switch between rotating along #one axis# at a time and freely rotate around #all axis# at once.
  - ~~Solved?: removed. Print out if the current cube is solved or not.~~
  - Menu: Back to main menu.
  
