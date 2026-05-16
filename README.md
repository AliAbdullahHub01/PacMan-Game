# 🎮 PacmanGame

<div align="center">

```
 ____   _    ____ __  __    _    _   _ 
|  _ \ / \  / ___|  \/  |  / \  | \ | |
| |_) / _ \| |   | |\/| | / _ \ |  \| |
|  __/ ___ \ |___| |  | |/ ___ \| |\  |
|_| /_/   \_\____|_|  |_/_/   \_\_| \_|
```

**Console-Based Pacman Game — C# Edition**

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET_4.8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Console App](https://img.shields.io/badge/Console_App-000000?style=for-the-badge&logo=windowsterminal&logoColor=white)
![Status](https://img.shields.io/badge/Status-Active-brightgreen?style=for-the-badge)

</div>

---

## 📖 About

**PacmanGame** is a fully-featured, console-based remake of the classic Pacman arcade game built in C#. Navigate through a maze, eat all the dots, avoid 4 ghosts with different AI behaviors, and go for the highest score — all in your terminal!

---

## ✨ Features

### 🕹️ Gameplay
- Classic **28x31 Pacman maze** — faithful to the original
- Eat **dots** to score points
- Collect **power-ups (O)** to gain extra lives
- **3 lives** system with flash effect on death
- Win by eating **all dots** in the maze

### 👻 4 Ghost AI Types
| Ghost | Color | Behavior |
|---|---|---|
| G-1 | 🔴 Red | **Chase** — directly hunts you |
| G-2 | 🟣 Magenta | **Ambush** — targets ahead of you |
| G-3 | 🔵 Cyan | **Random** — unpredictable movement |
| G-4 | 🟡 Yellow | **Scatter** — patrols corners |

### 📊 Score System
- **+1 point** per dot eaten
- **High score** saved between sessions
- **Score history** — last 10 games tracked
- **Rating system** — ★ Good Start → ★★★ PERFECT!

### 🎨 UI & Menus
- Animated **color splash** on startup
- **Arrow key navigation** menu
- Styled **HUD** — score, lives, dots left
- Dedicated **Instructions screen**
- Animated **Game Over** & **Win** screens

---

## 🛠️ Tech Stack

| Component | Technology |
|---|---|
| Language | C# |
| Framework | .NET Framework 4.8 |
| UI | Console (colored, ASCII art) |
| Storage | File-based (txt) |

---

## 🚀 Getting Started

### Prerequisites
- Windows OS
- [Visual Studio](https://visualstudio.microsoft.com/) or [.NET SDK](https://dotnet.microsoft.com/)
- .NET Framework 4.8

### Run the App

**Option 1 — Visual Studio:**
1. Open `PacmanGame.sln`
2. Press `Ctrl + F5` or click **Run**

**Option 2 — Command Line:**
```bash
cd PacmanGame
dotnet run
```

---

## 🎮 Controls

| Key | Action |
|---|---|
| `↑ ↓ ← →` | Move Pacman |
| `ESC` | Quit current game |
| `Enter` | Select menu option |
| `↑ ↓` | Navigate menu |

---

## 📂 Project Structure

```
PacmanGame/
├── PacmanGame.cs      # All game logic
├── PacmanGame.csproj  # Project config
├── PacmanGame.sln     # Solution file
├── highscore.txt      # Auto-generated high score
└── scorehistory.txt   # Auto-generated score history
```

---

## 📸 Preview

```
 PACMAN  |  Arrow Keys: Move  |  ESC: Quit
===============================================================
            ############################
            #............##............#
            #.####.#####.##.#####.####.#
            #O####.#####.##.#####.####O#
            #C####.#####.##.#####.####.#   ← C = You!
            #..........................#
            #.####.##.########.##.####.#
            ...
===============================================================
SCORE:    0   LIVES: ***..   DOTS LEFT: 240
```

---

## 🧑‍💻 Author

**Ali Abdullah**
- GitHub: [@AliAbdullahHub01](https://github.com/AliAbdullahHub01)
- Location: Lahore, Pakistan 🇵🇰

---

## 📄 License

This project is open source and available under the [MIT License](LICENSE).

---

<div align="center">
  <sub>Built with ♥ by <a href="https://github.com/AliAbdullahHub01">AliAbdullahHub01</a></sub>
</div>
