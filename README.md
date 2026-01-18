# 🏴‍☠️ Ahoy Matie!

**Ahoy Matie** is a 3D Pirate-Themed Party Game developed as a Final Semester Project for the **Game Development (IF581)** course at Universitas Multimedia Nusantara.

This project features two distinct game levels with different mechanics: a cannon battle simulation and a survival challenge.

## 👥 Development Team

| Name | Student ID | Role & Responsibilities |
| :--- | :--- | :--- |
| **Davin Christopher** | 00000085465 | **Core Gameplay & AI**<br>Level 1 Base, Player Movement, Health System, Camera Controller, Enemy AI, UI Implementation. |
| **Ferdiyanto** | 00000082619 | **Assets & Polish**<br>Asset Collection, Bug Fixing, Character Animation & Movement, Level 2 Polish, Video Documentation. |
| **Ananda C. Gunawan** | 00000084262 | **Level Design & Audio**<br>Level 2 Base (Terrain, Water), UI Design (Main Menu, Pause), Audio Management (BGM/SFX). |

---

## 🎮 Gameplay Overview

### Level 1: Cannon Fight 💣
In this level, the player engages in a ship-to-ship battle against an Enemy AI.
* **Objective**: Destroy the enemy before they destroy you.
* **Mechanics**:
    * Control the cannon trajectory.
    * Manage health (Hearts system).
    * **Win Condition**: Enemy health reaches 0 while Player is still alive.
    * **Lose Condition**: Player health reaches 0.

### Level 2: Survival Run ⏳
A survival mode where the environment becomes the enemy.
* **Objective**: Survive until the timer runs out.
* **Mechanics**:
    * Avoid rising water/lava and obstacles.
    * The difficulty increases as time progresses.
    * **Win Condition**: Survive until the timer hits 00:00.
    * **Lose Condition**: Player dies (falls or hits obstacles) before time runs out.

---

## 🛠️ Features

* **Game Architecture**: Built with Unity Engine (C#).
* **UI System**: Main Menu, Settings (Sound On/Off), Credits, Pause Menu, Win/Lose Screens.
* **Audio**: Background Music (BGM) and Sound Effects (SFX) integration.
* **AI System**: Enemy logic for aiming and shooting in Level 1.
* **Animation**: Character animations for movement, idle, and interactions.

---

## 🔗 Demo & Links

* **Itch.io Page**: [Play Ahoy Matie on Itch.io](https://davee43212.itch.io/ahoy-matie)
* **Gameplay Trailer**: [Watch on YouTube](https://youtu.be/jUTLzg5bBGw)

---

## 📂 Repository Structure

```text
├── Assets/
│   ├── Scenes/          # Main Menu, Level 1, Level 2, Credits
│   ├── Scripts/         # C# Scripts (PlayerController, EnemyAI, GameManager)
│   ├── Prefabs/         # Game Objects (Cannons, Player, UI Elements)
│   ├── Audio/           # BGM and SFX files
│   └── Materials/       # Textures and Shaders
├── ProjectSettings/     # Unity Project Configurations
└── README.md            # Project Documentation
```

---
*Created for IF581 - Game Development, Universitas Multimedia Nusantara (2025).*
