| Lucas Alan Campbell|
|:----               |
|s208055|
|Proggramming Class 2022|
|02/12/2020|
# I. Requirements
1. Description of Problem
   1. name of game: cube tank
   2. problem statement: clear requirements on Assessment
   3. problem specs: make a game using math library
2. Input info
   1. player will move itself using WASD
   2. player will move it turret using arrow keys
   3. player will use spacebar to shoot bullets
3. Output info
   1. when using the movement keys the object player will move in the designated direction 

# II. Classes

## Game.cs

### properties

#### CurrentSceneIndex
- return type: int
- visability: public static
  - give the index of the current scene

#### DefaultColor
- return type: ConsoleColor
- visability: public static
  - returns a set default color for console

#### GameOver
- return type: bool
- visability: public static
  - returns the game status

#### Random
- return type: Random
- visability: public static
  - returns a random output

#### Debug
- return type: bool
- visability: public static
  - returns debug status

#### ShowControls
- return type: bool
- visability: public static
  - return control status

#### PlayerInfo
- return type: bool
- visability: public static
  - returns player info status

### Functions

#### GetScene(int)
- return type: Scene
- visability: public static
  - returns the scene of a curetent index

#### GetCurrentScene(void)
- return type: scene
- visability: public static
  - returns the current scene in the game

#### AddScene(Scene)
- return type: int 
- visability: public static
  - adds a scene to the game creates and index number of the scene added

#### SetCurrentScene(int)
- return type: void
- visibility: public static
  - Sets the current scene in the game to be the scene at the given index

#### GetKeyDown(int)
- return type: bool
- visibility: public static
  - returns true while key is being pressed

#### GetKeyPressed(int)
- return type: bool
- visibility: public static
  - returns true if the key was pressed once

#### Start(void)
- return type: void
- visibility: private
  - where thing only run once

#### Update(float deltaTime)
- return type: void
- visibility: private
  - updates positions and runs till game is over

#### Draw(void)
- return type: void
- visibility: private
  - draw the new position of game object and is repeated till game is over

#### End(void)
- return type: void
- visibility: private
  - runs code at the end of a game

#### Run(void)
- return type: void
- visibility: public
  - main game loop

## Scene.cs

### properties

#### World
- return type: Matrix4
- visability: public static
  - returns the Scene matrix4

#### Started
- return type: bool
- visability: public static
  - return true if the scene has started

### Functions

#### AddActor(Actor)
- return type: void
- visibility: public
  - adds an actor to scene. "object"

#### RemoveActor(int)
- return type: bool
- visibility: public
  - removes an actor from scene based on index

#### RemoveActor(Actor)
- return type: bool
- visibility: public
  - removes an actor from scene based on Actor

#### Start(void)
- return type: void
- visibility: public virtual
  - what should run on every scenes start

#### CheckCollision(void)
- return type: void
- visibility: public virtual
  - check the collision of every actor in the Scene

#### Update(float)
- return type: void
- visibility: public virtual
  - updates every Actor in the scene

#### Draw(void)
- return type: void
- visibility: public virtual
  - draws every actor in the scene

#### End(void)
- return type: void
- visibility: public virtual
  - ends actors that have started in the scene

## Actor.cs

### properties

#### Started
- return type: bool
- visability: public
  - return true if the scene has started

#### Parent
- return type: Actor
- visability: public
  - returns the parent of the Actor

#### Forward
- return type: vector3
- visability: public
  - returns the Actors forward axis

#### WorldPosition
- return type: vector3
- visability: public
  - returns the Actors global position

#### LocalPosition
- return type: vector3
- visability: public
  - returns the Actors local position

#### Velocity
- return type: vector3
- visability: public
  - returns the velocity of the actor

#### Acceleration
- return type: vector3
- visability: public
  - return the acceleration of an actor

#### Children
- return type: Actor[]
- visability: public
  - return the array of childrens in actor

#### Shape
- return type: Shape
- visability: public
  - returns the shape the actor has

#### RayColor
- return type: Color
- visability: public
  - returns the color the actor has

### Functions

#### AddChild(Actor)
- return type: void
- visibility: public
  - adds a child to the current Actor. 

#### RemoveChild(int)
- return type: bool
- visibility: public
  - removes a child from scene based on index

#### RemoveChild(Actor)
- return type: bool
- visibility: public
  - removes a child from scene based on child selected

#### SetTranslate(Vector3)
- return type: void
- visibility: public
  - set the postition of the actor 

#### SetRotationX(float)
- return type: void
- visibility: public
  - set the rotation of the x-axis in radians

#### SetRotationY(float)
- return type: void
- visibility: public
  - set the rotation of the y-axis in radians

#### SetRotaionZ(float)
- return type: void
- visibility: public
  - set the rotation of the z-axis in radians

#### RotateXYZ(float,float,float)
- return type: void
- visibility: public
  - rotate the Actor by the given radians on all axis

#### RotateX(float)
- return type: void
- visibility: public
  - rotate the x-axis of the Actor

#### RotateY(float)
- return type: void
- visibility: public
  - rotates the y-axis of the actor

#### RotateZ(float)
- return type: void
- visibility: public
  - rotates the z-axis of the actor

#### SetScale(Vector3)
- return type: void
- visibility: public
  - set the scale of the actor

#### CheckCollision(Actor)
- return type: bool
- visibility: public virtual
  - check the distance between collision spheres

#### OnCollision(Actor)
- return type: void
- visibility: public virtual
  - what the actor does on Collision

#### UpdateTransform(void)
- return type: void
- visibility: public
  - concatinates translation, rotation, and scale to make a new local transform

#### UpdateGlobalTransform(void)
- return type: void
- visibility: public
  - concatinates local transform with either a parent or world transform to give global transform

#### OnGround(void)
- return type: bool
- visibility: public
  - checks to see if Actor is on the ground

#### Destroy(void)
- return type: void
- visibility: public
  - destroy this actor

#### Start(void)
- return type: void
- visibility: public virtual
  - how every actor starts

#### Update(float)
- return type: void
- visibility: public virtual
  - updates the actor 

#### UpdateShape(void)
- return type: void
- visibility: public
  - update the actors selected shape

#### DrawShape(void)
- return type: void
- visibility: public
  - Draws shape to the screen

#### DrawShape(void)
- return type: void
- visibility: public virtual
   - draws the shape; if debug is true draw debug information

#### End(void)
- return type: void
- visibility: public virtual
  - ends the actor

#### Debug(void)
- return type: void
- visibility: public virtual
  - if game debug is true draws collision spheres and direction of the actors

## Bullet.cs
*inherited from actor

### Function

#### OnCollision(Actor)
- return type: void
- visibility: public override
  - when a bullet collides with a Collectible it repositions the collectible
  - adds a child to the array of rotation of player

#### Update(float)
- return type: void
- visibility: public override
  - when the bullet either hits the ground or goes out of bounds it destroys itself
  - bullet has constant gravity applied to it 

## Collectible.cs
*inherited from actor
- spawns an offset cubed that rotate on its own

### Function

#### Update(float)
- return type: void
- visibility: public override
  - puts a constant rotation on the collectible

## Player.cs
*inherited from actor

### Properties

#### Speed
- return type: float
- visability: public
  - returns the speed of the player

#### Rotations
- return type: Actor[]
- visability: public
  - returns the actor array of the player rotations

#### CubesCollected
- return type: int
- visability: public
  - returns how many cubes are attached to player

#### BulletSpeed
- return type: flaot
- visability: public
  - returns the speed the bullet will launch at.

### Functions

#### Shoot(void)
- return type: void
- visibility: public
  - spawns a bullet add applies a the forward vector for constant movement

#### AddObjectToPlayer(int, Actor)
- return type: void
- visibility: public
  - creates a new collectible that is child to players individual rotations.

#### InitBody(void)
- return type: void
- visibility: private
  - build the tank of the player

#### UpdateBody(void)
- return type: void
- visibility: private
  - updates the movements of the indiviual parts of player

#### DrawBody(void)
- return type: void
- visibility: private
  - draws the body to the screen 

#### Start(void)
- return type: void
- visibility: public override
  - initilizes body for player

#### Update(float)
- return type: void
- visibility: public override
  - uses the inputs from player to move himself

#### Draw(void)
- return type: void
- visibility: public override
  - Draws players body and direction

# Math library

## Vector2.cs

### properties

#### Magnitude
- return type: float
- visability: public
  - gives the magnitude of a vector2

#### Normilized
- return type: vector2
- visability: public
  - gives the normalized vector2

### operators 

#### +
- return type: vector2
- visability: public static
  - adds two vectors together

#### -
- return type: vector2
- visability: public static
  - subtracts two vector together

#### *
- return type: vector2
- visability: public static
  - multiplies a vector with a scalar

#### /
- return type: vector2
- visability: public static
  - Divides a vector with a scalar

### Functions

#### Normalized(Vector2)
- return type: vector2
- visability: public static
  - Returns the normalized version of a the vector passed in.

#### DotProduct(vector2,vector2)
- return type: float
- visability: public
  - Returns the dot product of the two vectors given.

## Vector3.cs

### properties

#### Magnitude
- return type: float
- visability: public
  - gives the magnitude of a vector3

#### Normilized
- return type: vector3
- visability: public
  - gives the normalized vector3

### operators 

#### +
- return type: vector3
- visability: public static
  - adds two vectors together

#### -
- return type: vector3
- visability: public static
  - subtracts two vectors together

#### *
- return type: vector3
- visability: public static
  - multiplies a vector with a scalar

#### /
- return type: vector3
- visability: public static
  - Divides a vector with a scalar

### Functions

#### Normalized(Vector3)
- return type: vector3
- visability: public static
  - Returns the normalized version of a vector given.

#### DotProduct(vector3,vector3)
- return type: float
- visability: public
  - Returns the dot product of the two vectors given.

#### CrossProduct(Vector3,vector3)
- return type: vector3
- visability: public static
  - Returns the cross product of the two vectors given.

## Vector4.cs

### properties

#### Magnitude
- return type: float
- visability: public
  - gives the magnitude of a vector4

#### Normilized
- return type: vector4
- visability: public
  - gives the normalized vector4

### operators 

#### +
- return type: vector4
- visability: public static
  - adds two vectors together

#### -
- return type: vector4
- visability: public static
  - subtracts two vectors together

#### *
- return type: vector4
- visability: public static
  - multiplies a vector with a scalar

#### /
- return type: vector4
- visability: public static
  - Divides a vector with a scalar

### Functions

#### Normalized(Vector4)
- return type: vector3
- visability: public static
  - Returns the normalized version of a vector given.

#### DotProduct(vector4,vector4)
- return type: float
- visability: public
  - Returns the dot product of the two vectors given.

#### CrossProduct(Vector4,vector4)
- return type: vector4
- visability: public static
  - Returns the cross product of the two vectors given.

## Matrix3.cs

### operators 

#### +
- return type: matrix3
- visability: public static
  - adds properties of one matrix with another

#### -
- return type: matrix3
- visability: public static
  - subtracts properties of one matrix with another

#### *
- return type: matrix3
- visability: public static
  - concatinates the properties of both matricies
  - concatinates the properties of a matrix3 and a vector3

### Functions

#### CreateRotation(float)
- return type: matrix3
- visability: public static
  - creates a new matrix3 with a rotation property

#### CreateTranslation(Vector2)
- return type: matrix3
- visability: public static
  - creates a new matrix3 with a translation property

#### CreateTranslation(float,float)
- return type: matrix3
- visability: public static
  - creates a new matrix3 with a translation property

#### CreateScale(vector2)
- return type: matrix3
- visability: public static
  - creates a new matrix3 with a scaled property

#### CreateScale(float,float)
- return type: matrix3
- visability: public static
  - creates a new matrix3 with a scaled property

## Matrix4.cs

### operators 

#### +
- return type: matrix4
- visability: public static
  - adds properties of one matrix with another

#### -
- return type: matrix4
- visability: public static
  - subtracts properties of one matrix with another

#### *
- return type: matrix4
- visability: public static
  - concatinates the properties of both matricies
  - concatinates the properties of a matrix4 and a vector4

### Functions

#### CreateRotationX(float)
- return type: matrix4
- visability: public static
  - creates a rotation matrix4 on the x-axis form radians

#### CreateRotationY(float)
- return type: matrix4
- visability: public static
  - creates a rotation matrix4 on the y-axis form radians

#### CreateRotationZ(float)
- return type: matrix4
- visability: public static
  - creates a rotation matrix4 on the z-axis form radians

#### CreateTranslation(Vector3)
- return type: matrix4
- visability: public static
  - creates a new matrix4 with a translation property

#### CreateTranslation(float,float)
- return type: matrix4
- visability: public static
  - creates a new matrix4 with a translation property

#### CreateScale(vector3)
- return type: matrix4
- visability: public static
  - creates a new matrix4 with a scaled property

#### CreateScale(float,float)
- return type: matrix4
- visability: public static
  - creates a new matrix4 with a scaled property