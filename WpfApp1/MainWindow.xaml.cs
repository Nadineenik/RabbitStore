using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MazeGame
{
    public partial class MainWindow : Window
    {
        private int playerX = 1, playerY = 1;
        private const int cellSize = 50;
        private Rectangle player;
        private Rectangle goal;
        private int[,] maze;
        private int currentLevel = 0;

        private int[][,] mazes =
        {
            // Уровень 1
            new int[,]
            {
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 0, 0, 1, 0, 1 },
                { 1, 0, 0, 0, 1, 0, 1 },
                { 1, 0, 1, 0, 0, 0, 1 },
                { 1, 0, 1, 1, 1, 1, 1 },
                { 1, 0, 0, 0, 0, 0, 1 },
                { 1, 1, 1, 1, 1, 1, 1 }
            },
            // Уровень 2
            new int[,]
            {
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 0, 0, 1, 0, 1, 1 },
                { 1, 1, 0, 1, 0, 1, 0 },
                { 1, 0, 0, 0, 1, 0, 1 },
                { 1, 1, 1, 0, 0, 1, 1 },
                { 1, 0, 1, 0, 0, 0, 1 },
                { 1, 1, 1, 1, 1, 1, 1 }
            },
            // Уровень 3
            new int[,]
            {
                { 1, 1, 1, 1, 1, 1, 1 },
                { 1, 0, 1, 0, 1, 0, 1 },
                { 1, 0, 1, 0, 0, 0, 1 },
                { 1, 0, 0, 0, 1, 0, 1 },
                { 1, 1, 1, 0, 1, 0, 1 },
                { 1, 0, 0, 0, 1, 0, 1 },
                { 1, 1, 1, 1, 1, 1, 1 }
            },
            // Уровень 4
            new int[,]
            {
                { 1, 1, 1, 1, 1, 1, 1, 1},
                { 1, 0, 1, 0, 0, 0, 1, 1},
                { 1, 0, 0, 0, 1, 0, 0, 1},
                { 1, 1, 1, 0, 1, 1, 0, 1},
                { 1, 0, 1, 0, 0, 1, 0, 1},
                { 1, 0, 0, 1, 0, 0, 0, 1},
                { 1, 1, 1, 1, 1, 1, 1, 1}
            },
            // Уровень 5
            new int[,]
            {
                { 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 0, 1, 0, 1, 0, 1, 1},
                { 1, 0, 0, 0, 0, 0, 0, 1},
                { 1, 1, 1, 0, 1, 1, 0, 1},
                { 1, 0, 0, 0, 0, 0, 0, 1},
                { 1, 1, 1, 1, 1, 0, 1, 1},
                { 1, 1, 1, 1, 1, 1, 1 , 1}
            },
            // Уровень 6
            new int[,]
            {
                { 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 0, 0, 0, 0, 1, 1 },
                { 1, 0, 0, 0, 1, 0, 1,1 },
                { 1, 0, 1, 0, 1, 0, 0,1 },
                { 1, 1, 0, 1, 0, 1, 0,1 },
                { 1, 0, 0, 1, 0, 0, 0,1 },
                { 1, 1, 1, 1, 1, 0, 0,1 },
                { 1, 1, 1, 1, 1, 1, 1,1 }
            },
            // Уровень 7
            new int[,]
            {
                {1, 1, 1, 1, 1, 1, 1, 1 },
                {1, 0, 0, 0, 0, 0, 1, 1 },
                {1, 0, 1, 0, 1, 0, 0, 1 },
                {1, 0, 0, 1, 0, 0, 0, 1 },
                {1, 1, 0, 0, 0, 1, 1, 1 },
                {1, 0, 0, 1, 0, 0, 0, 1 },
                {1, 0, 0, 0, 0, 1, 1, 1 },
                {1, 1, 1, 1, 1, 1, 1, 1 }
            }
        };

        public MainWindow()
        {
            InitializeComponent();
            LoadLevel(currentLevel);
            CreateLevelSelector();
            CreateControls();
        }

        private void LoadLevel(int level)
        {
            maze = mazes[level];
            playerX = 1;
            playerY = 1;
            DrawMaze();
            UpdatePlayerPosition();
        }

        private void DrawMaze()
        {
            MazeCanvas.Children.Clear();

            for (int row = 0; row < maze.GetLength(0); row++)
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    Rectangle rect = new Rectangle
                    {
                        Width = cellSize,
                        Height = cellSize,
                        Fill = (maze[row, col] == 1) ? Brushes.DarkGray : Brushes.White, // Стены - темно-серые
                        Stroke = Brushes.Black, // Черная обводка для контраста
                        StrokeThickness = 1 // Толщина обводки
                    };
                    Canvas.SetLeft(rect, col * cellSize);
                    Canvas.SetTop(rect, row * cellSize);
                    MazeCanvas.Children.Add(rect);
                }
            }

            // Игрок - красный 
            player = new Rectangle
            {
                Width = cellSize,
                Height = cellSize,
                Fill = Brushes.Red // Красный цвет для игрока
            };
            UpdatePlayerPosition();
            MazeCanvas.Children.Add(player);

            // Финиш
            goal = new Rectangle
            {
                Width = cellSize,
                Height = cellSize,
                Fill = Brushes.Green // Финиш - зеленый
            };
            Canvas.SetLeft(goal, 5 * cellSize); // Установите координаты финиша
            Canvas.SetTop(goal, 5 * cellSize);
            MazeCanvas.Children.Add(goal);
        }



        private void UpdatePlayerPosition()
        {
            Canvas.SetLeft(player, playerX * cellSize);
            Canvas.SetTop(player, playerY * cellSize);
        }

        private void MovePlayer(int dx, int dy)
        {
            int newX = playerX + dx;
            int newY = playerY + dy;
            if (maze[newY, newX] == 0)
            {
                playerX = newX;
                playerY = newY;
                UpdatePlayerPosition();

                // Проверка на достижение финиша
                if (playerX == 5 && playerY == 5)
                {
                    MessageBox.Show("You reached the finish!");
                    currentLevel = (currentLevel + 1) % mazes.Length;
                    LoadLevel(currentLevel);
                }
            }
        }

        private void CreateLevelSelector()
        {
            ComboBox levelSelector = new ComboBox
            {
                Width = 100,
                Height = 30,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10),
                Background = Brushes.LightBlue,
                Foreground = Brushes.DarkBlue,
                FontSize = 14,
                FontWeight = FontWeights.Bold
            };

            for (int i = 0; i < mazes.Length; i++)
            {
                levelSelector.Items.Add($"Level {i + 1}");
            }

            levelSelector.SelectedIndex = currentLevel;
            levelSelector.SelectionChanged += (s, e) =>
            {
                currentLevel = levelSelector.SelectedIndex;
                LoadLevel(currentLevel);
            };

            MainGrid.Children.Add(levelSelector);
        }

        private void CreateControls()
        {
            // Создаем Grid для кнопок
            Grid controlGrid = new Grid();
            controlGrid.HorizontalAlignment = HorizontalAlignment.Center;
            controlGrid.VerticalAlignment = VerticalAlignment.Bottom;
            controlGrid.RowDefinitions.Add(new RowDefinition());
            controlGrid.RowDefinitions.Add(new RowDefinition());
            controlGrid.RowDefinitions.Add(new RowDefinition());
            controlGrid.RowDefinitions.Add(new RowDefinition());
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition());
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition());
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition());

            // Добавление кнопок
            Button upButton = new Button
            {
                Content = "↑",
                Width = 50,
                Height = 50,
                Margin = new Thickness(5),
                Background = Brushes.SkyBlue,
                FontSize = 18,
                FontWeight = FontWeights.Bold
            };
            upButton.Click += (s, e) => MovePlayer(0, -1);
            Grid.SetRow(upButton, 0);
            Grid.SetColumn(upButton, 1);
            controlGrid.Children.Add(upButton);

            Button leftButton = new Button
            {
                Content = "←",
                Width = 50,
                Height = 50,
                Margin = new Thickness(5),
                Background = Brushes.SkyBlue,
                FontSize = 18,
                FontWeight = FontWeights.Bold
            };
            leftButton.Click += (s, e) => MovePlayer(-1, 0);
            Grid.SetRow(leftButton, 1);
            Grid.SetColumn(leftButton, 0);
            controlGrid.Children.Add(leftButton);

            Button rightButton = new Button
            {
                Content = "→",
                Width = 50,
                Height = 50,
                Margin = new Thickness(5),
                Background = Brushes.SkyBlue,
                FontSize = 18,
                FontWeight = FontWeights.Bold
            };
            rightButton.Click += (s, e) => MovePlayer(1, 0);
            Grid.SetRow(rightButton, 1);
            Grid.SetColumn(rightButton, 2);
            controlGrid.Children.Add(rightButton);

            Button downButton = new Button
            {
                Content = "↓",
                Width = 50,
                Height = 50,
                Margin = new Thickness(5),
                Background = Brushes.SkyBlue,
                FontSize = 18,
                FontWeight = FontWeights.Bold
            };
            downButton.Click += (s, e) => MovePlayer(0, 1);
            Grid.SetRow(downButton, 2);
            Grid.SetColumn(downButton, 1);
            controlGrid.Children.Add(downButton);

            MainGrid.Children.Add(controlGrid); // Добавляем контроллеры в основной Grid
        }
    }
}
