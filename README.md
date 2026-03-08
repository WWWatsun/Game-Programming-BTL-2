# 2D Local Multiplayer Tank Brawler (BTL 2)

A fast-paced, 2D local multiplayer top-down shooter built in Unity. Players control tanks, navigate dynamically generated destructible environments, and utilize bouncing projectiles and power-ups to defeat their opponents.

## 🚀 Key Features

* **Dynamic Local Multiplayer:** Automatically handles input device switching, seamlessly supporting both Keyboard and Gamepad for Player 1 and Player 2.
* **Custom Projectile Physics:** Bullets ricochet off walls using standard Euclidean vector reflection rather than relying solely on Unity's built-in physics materials.
* **Destructible Terrain:** Bullet explosions deal area-of-effect (AoE) damage to players and dynamically remove wall tiles from the tilemap.
* **Procedural Map Generation:** Levels are generated on the fly using 2D string arrays, automatically batching walls into optimized RuleTile chunks and setting player spawn points.
* **Power-Up System:** A dynamic spawner safely places power-ups in open areas, granting players temporary speed boosts, healing, or damage multipliers.
* **Optimized Performance:** Utilizes the Object Pooling pattern to manage bullet lifecycles, drastically reducing memory allocation and garbage collection spikes during intense firefights.

## 🎮 Controls

The game relies on Unity's modern Input System. 

* **Tank Movement:** Tank-style controls moving forward/backward along the local Y-axis and rotating left/right.
* **Turret Control:** Independent turret rotation to aim independently of the tank's body.
* **Shoot:** Fires a bouncing projectile that explodes after a set number of bounces or upon hitting a target.

The first player is keyboard by default, and the second player can also use keyboard or a controller.

## 📐 Technical Implementation Details

### Manual Bullet Reflection
To ensure precise and predictable bouncing, projectiles calculate their reflection vectors manually upon collision. 
$$R = I - 2(I \cdot N)N$$
Where $R$ is the new velocity, $I$ is the incoming velocity, and $N$ is the surface normal.

### Map Generation & Chunking
The `MapGenerator` reads string arrays (e.g., `"W.....WWWWW....WWW"`) to build the level. To optimize Unity Tilemap performance, the generator algorithm scans the grid to find the largest possible contiguous rectangular chunks of walls ('W') and places them in bulk, rather than placing them tile-by-tile.

### Object Pooling
Instead of instantiating and destroying bullets frequently, the game uses `UnityEngine.Pool.ObjectPool`. Bullets are created up to a max size, disabled upon explosion, and reactivated from the pool when a player shoots.


## 📂 Core Architecture (Scripts)

* `GameManager.cs`: Spawns players and manages dynamic input device assignment (Keyboard vs. Gamepad).
* `Player.cs` / `Turret.cs`: Handles tank locomotion, independent turret rotation, health management, and power-up states.
* `Bullet.cs`: Manages movement, bouncing logic, trail rendering, and explosion coroutines (AoE damage and wall destruction).
* `MapGenerator.cs`: Singleton that parses map layouts, builds the level, and exposes methods to destroy specific wall tiles based on world coordinates.
* `PowerSpawner.cs`: A coroutine-based spawner that uses `Physics2D.OverlapCircle` to ensure power-ups don't spawn inside walls.
* `HUD.cs` / `AudioManager.cs` / `Music.cs`: Singletons managing the user interface, sound effects, and persistent background music across scenes.

## 🛠️ Setup & Installation

1. Clone the repository.
2. Open the project in Unity (Ensure you have the "Input System" package installed).
3. Open the Main Menu or Game scene and press **Play**.
4. Alternatively, you can also play the finished build in ./BattleTanks/Game-Programming-BTL-2.exe

## 📦 Asset Sources

* **Sprites & Art Assets:** https://kenney.nl/assets/top-down-tanks-redux, https://kenney.nl/assets/top-down-shooter
* **Sound Effects (SFX):** https://harvey656.itch.io/8-bit-game-sound-effects-collection
* **Background Music:** https://beatscribe.itch.io/beatscribes-free-uge-music-asset-pack?download
