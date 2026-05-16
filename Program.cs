using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace PacmanGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "PACMAN - C# Console Edition";
            Console.CursorVisible = false;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            try
            {
                int w = Math.Min(80, Console.LargestWindowWidth);
                int h = Math.Min(46, Console.LargestWindowHeight);
                Console.SetWindowSize(w, h);
                Console.SetBufferSize(w, h + 5);
            }
            catch { }
            new GameApp().Run();
        }
    }

    class GameApp
    {
        private int _highScore = 0;
        private readonly List<int> _scoreHistory = new List<int>();
        private const string HighScoreFile = "highscore.txt";
        private const string HistoryFile = "scorehistory.txt";

        public void Run()
        {
            LoadData();
            ShowStartup();
            bool running = true;
            while (running)
            {
                string choice = ShowMenu();
                switch (choice)
                {
                    case "play":
                        var level = new GameLevel();
                        level.Play();
                        int s = level.Score;
                        if (s > _highScore) _highScore = s;
                        _scoreHistory.Add(s);
                        SaveData();
                        break;
                    case "instructions": ShowInstructions(); break;
                    case "highscore": ShowHighScore(); break;
                    case "history": ShowHistory(); break;
                    case "exit": running = false; break;
                }
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            PrintCenter("Thanks for playing PACMAN!", Console.WindowHeight / 2);
            Console.ResetColor();
            Thread.Sleep(1200);
        }

        void ShowStartup()
        {
            ConsoleColor[] cycle = { ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Cyan, ConsoleColor.Green, ConsoleColor.Magenta, ConsoleColor.Yellow };
            foreach (var col in cycle) { Console.Clear(); Console.ForegroundColor = col; DrawBigTitle(); Thread.Sleep(260); }
        }

        string ShowMenu()
        {
            string[] labels = { "  > Start Game    ", "  ? Instructions  ", "  * High Score    ", "  # Score History ", "  X Exit          " };
            string[] retvals = { "play", "instructions", "highscore", "history", "exit" };
            int selected = 0;
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                DrawBigTitle();
                int boxW = 26, topRow = 13;
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                PrintCenter("+" + new string('=', boxW) + "+", topRow++);
                Console.ForegroundColor = ConsoleColor.Cyan;
                PrintCenter("|" + CenterPad(" MAIN MENU ", boxW) + "|", topRow++);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                PrintCenter("+" + new string('=', boxW) + "+", topRow++);
                for (int i = 0; i < labels.Length; i++)
                {
                    if (i == selected)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        PrintCenter("|" + PadRight(labels[i], boxW) + "|", topRow++);
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        PrintCenter("|" + PadRight(labels[i], boxW) + "|", topRow++);
                    }
                }
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                PrintCenter("+" + new string('=', boxW) + "+", topRow++);
                topRow++;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                PrintCenter("[ Up/Down ] Navigate   [ ENTER ] Select", topRow);
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow) selected = (selected - 1 + labels.Length) % labels.Length;
                else if (key.Key == ConsoleKey.DownArrow) selected = (selected + 1) % labels.Length;
                else if (key.Key == ConsoleKey.Enter) return retvals[selected];
                else if (key.KeyChar >= '1' && key.KeyChar <= '5') return retvals[key.KeyChar - '1'];
            }
        }

        void ShowInstructions()
        {
            Console.Clear();
            int top = 2;
            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintCenter("+========================================+", top++);
            PrintCenter("|        I N S T R U C T I O N S        |", top++);
            PrintCenter("+========================================+", top++);
            PrintCenter("|                                        |", top++);
            var lines = new (ConsoleColor col, string text)[] {
                (ConsoleColor.Yellow,     "|  C (Yellow) = You, PACMAN              |"),
                (ConsoleColor.Red,        "|  G (Colors) = Ghost enemies            |"),
                (ConsoleColor.Gray,       "|  . = Dot        +1 score per dot       |"),
                (ConsoleColor.Green,      "|  O = Power-up   +1 extra life          |"),
                (ConsoleColor.Cyan,       "|                                        |"),
                (ConsoleColor.White,      "|  Arrow Keys  =  Move Pacman            |"),
                (ConsoleColor.White,      "|  ESC         =  Quit current game      |"),
                (ConsoleColor.Cyan,       "|                                        |"),
                (ConsoleColor.DarkRed,    "|  Touch ghost  ->  lose 1 life          |"),
                (ConsoleColor.Red,        "|  Lives = 0    ->  Game Over            |"),
                (ConsoleColor.Green,      "|  Eat all dots ->  YOU WIN!             |"),
                (ConsoleColor.Cyan,       "|                                        |"),
                (ConsoleColor.DarkYellow, "|  TIP: Red ghost chases you directly!   |"),
                (ConsoleColor.Cyan,       "|                                        |"),
            };
            foreach (var (col, text) in lines) { Console.ForegroundColor = col; PrintCenter(text, top++); }
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            PrintCenter("+========================================+", top++);
            top++;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            PrintCenter("Press any key to return...", top);
            Console.ReadKey(true);
        }

        void ShowHighScore()
        {
            Console.Clear(); Console.ForegroundColor = ConsoleColor.Yellow; DrawBigTitle();
            int top = 14;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            PrintCenter("+================================+", top++);
            PrintCenter("|         HIGH SCORE             |", top++);
            PrintCenter("+================================+", top++);
            Console.ForegroundColor = _highScore > 0 ? ConsoleColor.Yellow : ConsoleColor.DarkGray;
            string st = _highScore > 0 ? "***  " + _highScore + "  ***" : "No score yet!";
            PrintCenter("|" + CenterPad(st, 32) + "|", top++);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            PrintCenter("+================================+", top++);
            top++;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            PrintCenter("Press any key to return...", top);
            Console.ReadKey(true);
        }

        void ShowHistory()
        {
            Console.Clear(); Console.ForegroundColor = ConsoleColor.Yellow; DrawBigTitle();
            int top = 13;
            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintCenter("+================================+", top++);
            PrintCenter("|        SCORE HISTORY           |", top++);
            PrintCenter("+================================+", top++);
            if (_scoreHistory.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                PrintCenter("|    No games played yet.        |", top++);
            }
            else
            {
                int shown = Math.Min(_scoreHistory.Count, 10);
                for (int i = 0; i < shown; i++)
                {
                    int idx = _scoreHistory.Count - 1 - i;
                    Console.ForegroundColor = i == 0 ? ConsoleColor.Yellow : i < 3 ? ConsoleColor.Cyan : ConsoleColor.White;
                    string r2 = string.Format("  #{0,-3}  Score: {1,6}", i + 1, _scoreHistory[idx]);
                    PrintCenter("|" + PadRight(r2, 32) + "|", top++);
                }
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintCenter("+================================+", top++);
            top++;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            PrintCenter("Press any key to return...", top);
            Console.ReadKey(true);
        }

        void SaveData()
        {
            try
            {
                File.WriteAllText(HighScoreFile, _highScore.ToString());
                File.WriteAllLines(HistoryFile, _scoreHistory.ConvertAll(x => x.ToString()));
            }
            catch { }
        }

        void LoadData()
        {
            try
            {
                if (File.Exists(HighScoreFile) && int.TryParse(File.ReadAllText(HighScoreFile).Trim(), out int h)) _highScore = h;
                if (File.Exists(HistoryFile))
                    foreach (var line in File.ReadAllLines(HistoryFile))
                        if (int.TryParse(line.Trim(), out int v)) _scoreHistory.Add(v);
            }
            catch { }
        }

        static void PrintCenter(string text, int row)
        {
            int col = Math.Max(0, (Console.WindowWidth - text.Length) / 2);
            Console.SetCursorPosition(col, row);
            Console.Write(text);
        }

        static string CenterPad(string text, int width)
        {
            if (text.Length >= width) return text.Substring(0, width);
            int left = (width - text.Length) / 2;
            return new string(' ', left) + text + new string(' ', width - text.Length - left);
        }

        static string PadRight(string text, int width)
        {
            if (text.Length >= width) return text.Substring(0, width);
            return text + new string(' ', width - text.Length);
        }

        static void DrawBigTitle()
        {
            string[] art = {
                @" ____   _    ____ __  __    _    _   _ ",
                @"|  _ \ / \  / ___|  \/  |  / \  | \ | |",
                @"| |_) / _ \| |   | |\/| | / _ \ |  \| |",
                @"|  __/ ___ \ |___| |  | |/ ___ \| |\  |",
                @"|_| /_/   \_\____|_|  |_/_/   \_\_| \_|",
            };
            int row = 3;
            foreach (var line in art) PrintCenter(line, row++);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            PrintCenter("--- C# Console Edition ---", row + 1);
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
    }

    // ═══════════════════════════════════════════════
    //  GAME LEVEL
    // ═══════════════════════════════════════════════
    class GameLevel
    {
        public int Score { get; private set; } = 0;

        private char[,] _grid = new char[0, 0];
        private int _rows, _cols;
        private int _totalDots;
        private int _px, _py;
        private int _lives = 3;
        private readonly List<Ghost> _ghosts = new List<Ghost>();

        private const int OX = 12;   // horizontal offset (centers 28*2=56 in 80)
        private const int OY = 3;    // row 0=topbar, row1=divider, row2=blank, row3=maze start

        // ── ORIGINAL MAZE — 28 cols x 31 rows ──────
        private static readonly string[] MazeRows =
        {
            "############################",
            "#............##............#",
            "#.####.#####.##.#####.####.#",
            "#O####.#####.##.#####.####O#",
            "#.####.#####.##.#####.####.#",
            "#..........................#",
            "#.####.##.########.##.####.#",
            "#.####.##.########.##.####.#",
            "#......##....##....##......#",
            "######.#####.##.#####.######",
            "######.#####.##.#####.######",
            "######.##          ##.######",
            "######.##.########.##.######",
            "######.##.#      #.##.######",
            "      .   .#      #.   .    ",
            "######.##.########.##.######",
            "######.##          ##.######",
            "######.##.########.##.######",
            "######.##.########.##.######",
            "#............##............#",
            "#.####.#####.##.#####.####.#",
            "#O..##...................##O#",
            "###.##.##.########.##.##.###",
            "#......##....##....##......#",
            "#.##########.##.##########.#",
            "#.##########.##.##########.#",
            "#..........................#",
            "#.####.#####.##.#####.####.#",
            "#.####.#####.##.#####.####.#",
            "#............##............#",
            "############################",
        };

        public void Play()
        {
            InitMaze();
            SpawnActors();
            DrawHUD();
            FullRedraw();

            var sw = Stopwatch.StartNew();
            long lastGhostTick = 0;
            bool running = true;

            while (running)
            {
                // ── Input ──
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    int nx = _px, ny = _py;
                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow: ny--; break;
                        case ConsoleKey.DownArrow: ny++; break;
                        case ConsoleKey.LeftArrow: nx--; break;
                        case ConsoleKey.RightArrow: nx++; break;
                        case ConsoleKey.Escape: running = false; continue;
                    }
                    if (nx != _px || ny != _py) TryMovePacman(nx, ny);
                }

                // ── Ghost tick — every 350ms (slower!) ──
                long now = sw.ElapsedMilliseconds;
                if (now - lastGhostTick >= 350)
                {
                    lastGhostTick = now;
                    MoveGhosts();
                }

                // ── Collision ──
                foreach (var g in _ghosts)
                {
                    if (g.X == _px && g.Y == _py)
                    {
                        _lives--;
                        UpdateHUD();
                        if (_lives <= 0) { running = false; ShowGameOver(); }
                        else { FlashScreen(); ReplacePacman(); }
                        break;
                    }
                }

                if (!running) break;
                if (Score >= _totalDots) { running = false; ShowWin(); }

                Thread.Sleep(16);
            }
        }

        void InitMaze()
        {
            _rows = MazeRows.Length;        // 31
            _cols = MazeRows[0].Length;     // 28
            _grid = new char[_rows, _cols];
            _totalDots = 0;

            for (int r = 0; r < _rows; r++)
            {
                string row = MazeRows[r];
                for (int c = 0; c < _cols; c++)
                {
                    char ch = c < row.Length ? row[c] : ' ';
                    _grid[r, c] = ch;
                    if (ch == '.' || ch == 'O') _totalDots++;
                }
            }
        }

        void SpawnActors()
        {
            _px = 1; _py = 1;

            // All 4 ghosts spawn in open corridors — NOT inside ghost house walls
            // Row 14 is the open corridor row in the original maze: "      .   .#      #.   .    "
            // Cols 1,6,21,26 are open spaces in that row
            _ghosts.Add(new Ghost(1, 14, 1, 0, ConsoleColor.Red, GhostMode.Chase));
            _ghosts.Add(new Ghost(22, 14, -1, 0, ConsoleColor.Magenta, GhostMode.Ambush));
            _ghosts.Add(new Ghost(1, 5, 1, 0, ConsoleColor.Cyan, GhostMode.Random));
            _ghosts.Add(new Ghost(22, 5, -1, 0, ConsoleColor.DarkYellow, GhostMode.Scatter));
        }

        // ── DRAWING ──────────────────────────────────
        void FullRedraw()
        {
            for (int r = 0; r < _rows; r++)
                for (int c = 0; c < _cols; c++)
                    DrawCell(c, r);
            DrawPacman();
            foreach (var g in _ghosts) RenderGhost(g);
        }

        void DrawCell(int cx, int cy)
        {
            if (cy < 0 || cy >= _rows || cx < 0 || cx >= _cols) return;
            SetPos(cx, cy);
            switch (_grid[cy, cx])
            {
                case '#': Console.ForegroundColor = ConsoleColor.Blue; Console.Write("█ "); break;
                case '.': Console.ForegroundColor = ConsoleColor.Gray; Console.Write("· "); break;
                case 'O': Console.ForegroundColor = ConsoleColor.Green; Console.Write("● "); break;
                default: Console.ForegroundColor = ConsoleColor.Black; Console.Write("  "); break;
            }
        }

        void DrawPacman()
        {
            SetPos(_px, _py);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("C ");
        }

        void RenderGhost(Ghost g)
        {
            SetPos(g.X, g.Y);
            Console.ForegroundColor = g.Color;
            Console.Write("G ");
        }

        void SetPos(int cx, int cy) =>
            Console.SetCursorPosition(OX + cx * 2, OY + cy);

        // ── HUD ──────────────────────────────────────
        void DrawHUD()
        {
            Console.Clear();

            // Row 0 — top bar
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write((" PACMAN  |  Arrow Keys: Move  |  ESC: Quit").PadRight(80));
            Console.BackgroundColor = ConsoleColor.Black;

            // Row 1 — divider
            Console.SetCursorPosition(0, 1);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write(new string('=', 79));

            // Row 2 — blank
            Console.SetCursorPosition(0, 2);
            Console.Write(new string(' ', 79));

            UpdateHUD();
        }

        void UpdateHUD()
        {
            // Bottom HUD rows
            int divRow = OY + _rows;
            int statRow = OY + _rows + 1;

            // Clamp to window
            if (divRow >= Console.WindowHeight) return;
            if (statRow >= Console.WindowHeight) statRow = Console.WindowHeight - 1;

            // Divider
            Console.SetCursorPosition(0, divRow);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write(new string('=', 79));

            // Stats
            Console.SetCursorPosition(2, statRow);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("SCORE: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(Score.ToString().PadLeft(5));

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("   LIVES: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(new string('*', Math.Max(0, _lives)));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(new string('.', Math.Max(0, 5 - _lives)));

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("   DOTS LEFT: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write((_totalDots - Score).ToString().PadLeft(4));
        }

        // ── MOVEMENT ──────────────────────────────────
        void TryMovePacman(int nx, int ny)
        {
            if (ny < 0 || ny >= _rows || nx < 0 || nx >= _cols) return;
            char dest = _grid[ny, nx];
            if (dest == '#') return;

            DrawCell(_px, _py);
            _px = nx; _py = ny;

            if (dest == '.')
            {
                Score++;
                _grid[_py, _px] = ' ';
                UpdateHUD();
            }
            else if (dest == 'O')
            {
                _lives = Math.Min(_lives + 1, 9);
                _grid[_py, _px] = ' ';
                UpdateHUD();
            }

            DrawPacman();
        }

        void ReplacePacman()
        {
            DrawCell(_px, _py);
            _px = 1; _py = 1;
            DrawPacman();
        }

        void MoveGhosts()
        {
            foreach (var g in _ghosts)
            {
                DrawCell(g.X, g.Y);
                g.Move(_grid, _rows, _cols, _px, _py);
                RenderGhost(g);
            }
            DrawPacman();
        }

        void FlashScreen()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.BackgroundColor = i % 2 == 0 ? ConsoleColor.DarkRed : ConsoleColor.Black;
                Thread.Sleep(80);
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }

        // ── END SCREENS ───────────────────────────────
        void ShowGameOver()
        {
            Thread.Sleep(400);
            Console.Clear();
            int mid = Console.WindowHeight / 2 - 4;
            Console.ForegroundColor = ConsoleColor.Red;
            PC("+======================================+", mid++);
            PC("|                                      |", mid++);
            PC("|       G A M E   O V E R  !           |", mid++);
            PC("|                                      |", mid++);
            Console.ForegroundColor = ConsoleColor.Yellow;
            PC(string.Format("|       Final Score  :  {0,6}          |", Score), mid++);
            Console.ForegroundColor = ConsoleColor.Red;
            PC("|                                      |", mid++);
            PC("+======================================+", mid++);
            mid++;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            PC("Press any key to continue...", mid);
            Console.ReadKey(true);
        }

        void ShowWin()
        {
            Thread.Sleep(400);
            Console.Clear();
            int mid = Console.WindowHeight / 2 - 4;
            Console.ForegroundColor = ConsoleColor.Green;
            PC("+======================================+", mid++);
            PC("|                                      |", mid++);
            PC("|        Y O U   W I N !               |", mid++);
            PC("|                                      |", mid++);
            Console.ForegroundColor = ConsoleColor.Yellow;
            PC(string.Format("|       Final Score  :  {0,6}          |", Score), mid++);
            Console.ForegroundColor = ConsoleColor.Cyan;
            string grade = Score > 200 ? "|       Rating  :  *** PERFECT! ***   |"
                         : Score > 100 ? "|       Rating  :  **  Great!          |"
                                       : "|       Rating  :  *   Good Start!     |";
            PC(grade, mid++);
            Console.ForegroundColor = ConsoleColor.Green;
            PC("|                                      |", mid++);
            PC("+======================================+", mid++);
            mid++;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            PC("Press any key to continue...", mid);
            Console.ReadKey(true);
        }

        static void PC(string text, int row)
        {
            int col = Math.Max(0, (Console.WindowWidth - text.Length) / 2);
            Console.SetCursorPosition(col, row);
            Console.Write(text);
        }
    }

    // ═══════════════════════════════════════════════
    //  GHOST
    // ═══════════════════════════════════════════════
    enum GhostMode { Chase, Ambush, Random, Scatter }

    class Ghost
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public ConsoleColor Color { get; }
        private int _dx, _dy;
        private readonly GhostMode _mode;
        private static readonly Random _rng = new Random();
        private int _tick = 0;

        public Ghost(int x, int y, int dx, int dy, ConsoleColor color, GhostMode mode)
        { X = x; Y = y; _dx = dx; _dy = dy; Color = color; _mode = mode; }

        public void Move(char[,] grid, int rows, int cols, int px, int py)
        {
            _tick++;
            if (_mode == GhostMode.Random) { MoveRandom(grid, rows, cols); return; }

            int tx = px, ty = py;

            if (_mode == GhostMode.Scatter)
            {
                // Patrol between two corners
                tx = _tick % 120 < 60 ? cols - 2 : 1;
                ty = _tick % 120 < 60 ? 1 : rows - 2;
            }
            else if (_mode == GhostMode.Ambush)
            {
                // Target slightly ahead of player
                tx = Math.Max(1, Math.Min(cols - 2, px + 3));
                ty = Math.Max(1, Math.Min(rows - 2, py + 3));
            }
            // Chase: tx=px, ty=py already set

            // Choose best direction toward target
            int bestDx = _dx, bestDy = _dy;
            double bestDist = double.MaxValue;
            int[,] dirs = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

            for (int i = 0; i < 4; i++)
            {
                int ndx = dirs[i, 0], ndy = dirs[i, 1];
                int nx2 = X + ndx, ny2 = Y + ndy;
                if (!CanEnter(nx2, ny2, grid, rows, cols)) continue;
                // No U-turns
                if (ndx == -_dx && ndy == -_dy && (_dx != 0 || _dy != 0)) continue;
                double dist = Math.Sqrt(Math.Pow(nx2 - tx, 2) + Math.Pow(ny2 - ty, 2));
                // Add randomness so it's not perfectly predictable
                dist += _rng.NextDouble() * (_mode == GhostMode.Chase ? 2.5 : 6.0);
                if (dist < bestDist) { bestDist = dist; bestDx = ndx; bestDy = ndy; }
            }

            int nnx = X + bestDx, nny = Y + bestDy;
            if (CanEnter(nnx, nny, grid, rows, cols)) { _dx = bestDx; _dy = bestDy; X = nnx; Y = nny; }
            else MoveRandom(grid, rows, cols);
        }

        void MoveRandom(char[,] grid, int rows, int cols)
        {
            int nx = X + _dx, ny = Y + _dy;
            if (CanEnter(nx, ny, grid, rows, cols) && _rng.Next(5) != 0) { X = nx; Y = ny; return; }
            int[,] dirs = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
            // Shuffle
            for (int i = 3; i > 0; i--)
            {
                int j = _rng.Next(i + 1);
                for (int k = 0; k < 2; k++) { int tmp = dirs[i, k]; dirs[i, k] = dirs[j, k]; dirs[j, k] = tmp; }
            }
            for (int i = 0; i < 4; i++)
            {
                int vx = X + dirs[i, 0], vy = Y + dirs[i, 1];
                if (CanEnter(vx, vy, grid, rows, cols)) { _dx = dirs[i, 0]; _dy = dirs[i, 1]; X = vx; Y = vy; return; }
            }
        }

        static bool CanEnter(int nx, int ny, char[,] grid, int rows, int cols)
        {
            if (nx < 0 || nx >= cols || ny < 0 || ny >= rows) return false;
            return grid[ny, nx] != '#';
        }
    }
}