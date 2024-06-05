using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using Bomberman.model;
using static System.Formats.Asn1.AsnWriter;


namespace Bomberman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private bool statusMoveCamera = true;
        private double ratioSpeed = 2;
        private double left = 0;
        private Random rdn = new(); 
        private List<string> diractions = new();
        private DispatcherTimer tick = new DispatcherTimer();
        private BoardGame pole = new BoardGame();
        private Player player;
        private Image playerImg;
        private Bomb bomb = null;
        private Image bombImg;
        private List<Image> blocks = new();
        private Dictionary<Brick, Image> bricksBreak = new();
        private Dictionary<Fire, Image> fires = new();
        private Dictionary<Enemy, Image> enemyes = new Dictionary<Enemy, Image>();
        private Dictionary<Brick, Image> bricks = new Dictionary<Brick, Image>();
        private Dictionary<(int, int), Brick> bricksPos = new Dictionary<(int, int), Brick>();




        public MainWindow()
        {
            InitializeComponent();
            StartGame();

        }
        private void StartGame()
        {
            pole.CreatePole();
            CreatePole();
            tick.Tick += new EventHandler(Update);
            tick.Interval = new TimeSpan(0, 0, 0, 0, Setting.Tick);
            tick.Start();
        }
        /// <summary>
        /// создание поля
        /// </summary>
        private void CreatePole()
        {
            for(int y = 0; y < pole.Map.Length; y++)
            {
                for(int x = 0; x < pole.Map[y].Length; x++)
                {
                    var item = new Image();
                    item.Width = Setting.CellSize;
                    item.Height = Setting.CellSize;
                    if (pole.Map[y][x] == '#')
                    {
                        item.Source = new BitmapImage(new Uri("/data/block/0.png", UriKind.Relative));
                        blocks.Add(item);
                        Panel.SetZIndex(item, 3);

                    }
                    if (pole.Map[y][x] == '@' || pole.Map[y][x] == 'E')
                    {
                        item.Source = new BitmapImage(new Uri("/data/brick/0.png", UriKind.Relative));
                        var brick = new Brick();
                        if (pole.Map[y][x] == 'E')
                        {
                            brick.Exit = true;
                            pole.Map[y][x] = '@';
                        }
                        bricks.Add(brick, item);
                        Panel.SetZIndex(item, 3);
                        bricksPos.Add((x, y), brick);

                    }
                    if (pole.Map[y][x] == '0')
                    {
                        item.Source = new BitmapImage(new Uri("/data/player/moveY/0.png", UriKind.Relative));
                        player = new(x * Setting.CellSize, y * Setting.CellSize, Setting.Right - x * Setting.CellSize, Setting.Bottom - y * Setting.CellSize);
                        playerImg = item;
                        Panel.SetZIndex(item, 2);

                    }
                    if (pole.Map[y][x] == '1')
                    {
                        item.Source = new BitmapImage(new Uri("/data/enemy/Valcom/move/0.png", UriKind.Relative));
                        var enemy = new Enemy(x * Setting.CellSize, y * Setting.CellSize, Setting.Right - x * Setting.CellSize, Setting.Bottom - y * Setting.CellSize);
                        diractions.Clear();
                        if (enemy.CheckFreeCell(pole.Map, x, y, "up"))
                        {
                            diractions.Add("down");
                            diractions.Add("up");
                        }
                        if (enemy.CheckFreeCell(pole.Map, x, y, "left"))
                        {
                            diractions.Add("right");
                            diractions.Add("left");
                        }
                        if(diractions.Count != 0)                        
                            enemy.Diraction = diractions[rdn.Next(diractions.Count)];
                        Panel.SetZIndex(item, 0);
                        enemyes.Add(enemy, item);

                    }
                    item.Stretch = Stretch.Fill;
                    item.Margin = new Thickness(x * Setting.CellSize, y * Setting.CellSize, Setting.Right - x * Setting.CellSize, Setting.Bottom - y * Setting.CellSize);
                    camera.Children.Add(item);
                }
            }
        }
        /// <summary>
        /// функция обновления игровых обьектов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update(object sender, EventArgs e)
        {
            UpdateBomb();
            UpdateEnemy();
            UpdatePlayerDead();
            UpdateBrick();
            UpdateFire();
        }
        /// <summary>
        /// логика анимации разрушения кирпича
        /// </summary>
        private void UpdateBrick()
        {
            foreach (var brick in bricksBreak)
            {
                brick.Value.Source = new BitmapImage(new Uri(brick.Key.Update(), UriKind.Relative));
            }
        }
        /// <summary>
        /// логика анимации взрыва
        /// </summary>
        private void UpdateFire()
        {

            foreach(var fire in fires)
            {
                if (fire.Key.FireOut())
                {
                    camera.Children.Remove(fire.Value);
                    fires.Remove(fire.Key);
                    foreach (var brick in bricksBreak)
                    {
                        if (brick.Key.Exit)
                        {
                            brick.Key.WholeBrick = false;
                            Panel.SetZIndex(brick.Value, -1);
                            continue;

                        }
                        camera.Children.Remove(brick.Value);
                        bricks.Remove(brick.Key);
                        bricksBreak.Remove(brick.Key);
                        bricksPos.Remove(((int)brick.Value.Margin.Left / Setting.CellSize, (int)brick.Value.Margin.Top / Setting.CellSize));
                    }
                }
                else
                    fire.Value.Source = new BitmapImage(new Uri(fire.Key.Update(), UriKind.Relative));
            }
            
        }
        /// <summary>
        /// логика анимации бомбы и запуска огня
        /// </summary>
        private void UpdateBomb()
        {
            if (!(bomb is null))
                bombImg.Source = new BitmapImage(new Uri(bomb.Update(), UriKind.Relative));
            if (!(bomb is null) && bomb.BombAtaka())
            {
                bomb = null;
                camera.Children.Remove(bombImg);
                CreateFire();
                bombImg = new Image();
            }
        }
        /// <summary>
        /// логика анимации смерти
        /// </summary>
        private void UpdatePlayerDead()
        {
            if (!player.Alive)
                playerImg.Source = new BitmapImage(new Uri(player.Dead(), UriKind.Relative));
            foreach(var fire in fires)
            {
                if (!fire.Key.FireOut() && player.CheckIntersection(fire.Value.Margin.Left, fire.Value.Margin.Top, true))
                    player.Alive = false;
            }
            if (player.Death)
            {
                tick.Stop();
                MessageBox.Show(QuotationBook.Citation);
                (new Menu()).Show();
                Close();
            }
        }
        /// <summary>
        /// логика передвижения и анимации
        /// </summary>
        private void UpdateEnemy()
        {
            foreach (var enemy in enemyes)
            {
                enemy.Key.move();
                enemy.Value.Source = new BitmapImage(new Uri(enemy.Key.Update(), UriKind.Relative));
                if (enemy.Key.Diraction == "right")
                    enemy.Value.FlowDirection = FlowDirection.LeftToRight;
                else
                    enemy.Value.FlowDirection = FlowDirection.RightToLeft;
                enemy.Value.Margin = new Thickness(enemy.Key.Left, enemy.Key.Top, enemy.Key.Right, enemy.Key.Bottom);
                foreach (var block in blocks)
                {
                    if (enemy.Key.CheckIntersection(block.Margin.Left, block.Margin.Top)) 
                        enemy.Key.SwapDiraction();
                    
                }
                foreach (var brick in bricks)
                {
                    if((brick.Key.Exit != true || brick.Key.WholeBrick) && enemy.Key.CheckIntersection(brick.Value.Margin.Left, brick.Value.Margin.Top))
                        enemy.Key.SwapDiraction();
                }
                foreach(var fire in fires)
                {
                    if(enemy.Key.CheckIntersection(fire.Value.Margin.Left, fire.Value.Margin.Top, true))
                        enemy.Key.Alive = false;
                }
                if (player.CheckIntersection(enemy.Key.Left, enemy.Key.Top, true))
                {
                    player.Alive = false;

                }
                if(!(bomb is null))
                    if(enemy.Key.CheckIntersection(bombImg.Margin.Left, bombImg.Margin.Top)) 
                        enemy.Key.SwapDiraction();
                enemy.Key.SwapRdnDiraction(pole.Map);
                if (!enemy.Key.Alive)
                {
                    enemy.Value.Source = new BitmapImage(new Uri(enemy.Key.Dead(), UriKind.Relative));
                    if (enemy.Key.EndDead)
                    {
                        score.Content = (int.Parse(score.Content.ToString()) + enemy.Key.Cost).ToString();
                        camera.Children.Remove(enemy.Value);
                        enemyes.Remove(enemy.Key);
                    }
                }
            }
        }
        /// <summary>
        /// отвечает за перемещение камеры
        /// </summary>
        /// <param name="diracation"></param>
        private void MoveCamera(string diracation)
        {
            /*
            камера пока плохо работает из-за подтягивания игрока
            */
            if (diracation == "left" && player.Left >= left + Width / 2 - Setting.CellSize &&
                    0 > left && !(player.Left > pole.Map[0].Length * Setting.CellSize - Width / 2))
                left += player.Speed * ratioSpeed ;
            if (diracation == "right" && player.Left >= left + Width / 2 + Setting.CellSize &&
                    (pole.Map[0].Length + 3) * Setting.CellSize - player.Speed * ratioSpeed > Math.Abs(left) + Width / 2 &&
                    !(player.Left > (pole.Map[0].Length - 1) * Setting.CellSize - Width / 2))
                left -= player.Speed * ratioSpeed;
            //тут есть баг
            camera.Margin = new Thickness(left, 80, 0, -13);
        }
        /// <summary>
        /// проверяет есть ли пересечение с блоками
        /// </summary>
        private void intersectionPlayer()
        {
            foreach (var block in blocks)
            {
                if (player.CheckIntersection(block.Margin.Left, block.Margin.Top))
                    statusMoveCamera = false;
            }
            foreach (var brick in bricks)
            {
                if ((brick.Key.Exit != true || brick.Key.WholeBrick) && player.CheckIntersection(brick.Value.Margin.Left, brick.Value.Margin.Top))
                {
                    statusMoveCamera = false;
                }
                else if (enemyes.Count == 0 && brick.Key.Exit && player.CheckIntersection(brick.Value.Margin.Left, brick.Value.Margin.Top))
                {
                    MessageBox.Show("вы переходите на следующий уровень");
                    Clear();
                    StartGame();
                    level.Content = int.Parse(level.Content.ToString()) + 1;
                    return;
                }
            }

            if (statusMoveCamera)
                MoveCamera(player.Diraction);
            statusMoveCamera = true;
        }
        private void Clear()
        {
            bricks.Clear();
            blocks.Clear();
            bricksPos.Clear();
            player = null;
            camera.Children.Clear();
            tick.Stop();
            tick = new DispatcherTimer();

        }
        /// <summary>
        /// обрабатывет клавиши 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PressKey(object sender, KeyEventArgs e)
        {
            if (!player.Alive)
                return;
            switch (e.Key)
            {
                case Key.W:
                    player.Diraction = "up";
                    break;
                case Key.S:
                    player.Diraction = "down";
                    break;
                case Key.A:
                    player.Diraction = "left";
                    playerImg.FlowDirection = FlowDirection.LeftToRight;
                    break;
                case Key.D:
                    player.Diraction = "right";
                    playerImg.FlowDirection = FlowDirection.RightToLeft;
                    break;
                case Key.L:
                    CreateBomb();
                    return;
                default: return;
            }
            MovePlayer();
        }
        /// <summary>
        /// создаёт бомбу на поле
        /// </summary>
        private void CreateBomb()
        {
            if(!(bomb is null))
            {
                return;
            }
            player.PutBomb();
            playerImg.Margin = new Thickness(player.Left, player.Top, player.Right, player.Bottom);
            foreach (var enemy in enemyes)
            {
                if (enemy.Key.CheckIntersection(playerImg.Margin.Left, playerImg.Margin.Top, true))
                    return;

            }
            bomb = new Bomb();
            bombImg = new();
            bombImg.Source = new BitmapImage(new Uri(bomb.Update(), UriKind.Relative));
            bombImg.Width = Setting.CellSize;
            bombImg.Height = Setting.CellSize;
            bombImg.Stretch = Stretch.Fill;
            bombImg.Margin = new Thickness(player.Left, player.Top, player.Right, player.Bottom);
            camera.Children.Add(bombImg);
        }
        /// <summary>
        /// создаёт огонь на поле
        /// </summary>
        private void CreateFire()
        {
            int posX = (int)bombImg.Margin.Left / Setting.CellSize;
            int posY = (int)bombImg.Margin.Top / Setting.CellSize;
            Fire fire;
            Image img;
            for (int y = -1; y <= 1; y+= 2)
            {
                if ("#@".IndexOf(pole.Map[posY + y][posX]) == -1)
                {
                    img = new Image();
                    fire = new Fire(bombImg.Margin.Left, bombImg.Margin.Top);
                    img.Source = new BitmapImage(new Uri(fire.Update(), UriKind.Relative));
                    img.Width = Setting.CellSize;
                    img.Height = Setting.CellSize;
                    img.Stretch = Stretch.Fill;
                    img.RenderTransformOrigin = new Point(0.5, 0.5);
                    if(y != 1)
                        img.RenderTransform = new RotateTransform(180);
                    img.Margin = new Thickness(bombImg.Margin.Left, (posY + y ) * Setting.CellSize,
                        bombImg.Margin.Right, Setting.Bottom - (posY + y) * Setting.CellSize);
                    fires.Add(fire, img);
                    Panel.SetZIndex(img, -1);
                    camera.Children.Add(img);
                }
                else if (pole.Map[posY + y][posX] == '@')
                {
                    pole.Map[posY + y][posX] = ' ';
                    var brick = bricksPos[(posX, posY + y)];
                    bricksBreak.Add(brick, bricks[brick]);
                }

            }
            for (int x = -1; x <= 1; x += 2)
            {
                if ("#@".IndexOf(pole.Map[posY][posX + x]) == -1)
                {
                    img = new Image();
                    fire = new Fire(bombImg.Margin.Left, bombImg.Margin.Top);
                    img.Source = new BitmapImage(new Uri(fire.Update(), UriKind.Relative));
                    img.RenderTransformOrigin = new Point(0.5, 0.5);
                    img.RenderTransform = new RotateTransform(90);
                    if (x == 1)
                        img.RenderTransform = new RotateTransform(-90);
                    img.Width = Setting.CellSize;
                    img.Height = Setting.CellSize;
                    img.Stretch = Stretch.Fill;
                    img.Margin = new Thickness((posX + x) * Setting.CellSize, bombImg.Margin.Top,
                        Setting.Right - (posX + x) * Setting.CellSize, bombImg.Margin.Bottom);
                    fires.Add(fire, img);
                    Panel.SetZIndex(img, -1);
                    camera.Children.Add(img);
                }
                else if(pole.Map[posY][posX + x] == '@')
                {
                    pole.Map[posY][posX + x] = ' ';
                    var brick = bricksPos[(posX + x, posY)];
                    bricksBreak.Add(brick, bricks[brick]);
                }
            }
            img = new Image();
            fire = new Fire(bombImg.Margin.Left, bombImg.Margin.Top, true);
            img.Source = new BitmapImage(new Uri(fire.Update(), UriKind.Relative));
            img.Width = Setting.CellSize;
            img.Height = Setting.CellSize;
            img.Stretch = Stretch.Fill;
            img.Margin = new Thickness(bombImg.Margin.Left, bombImg.Margin.Top,
                bombImg.Margin.Right, bombImg.Margin.Bottom);
            fires.Add(fire, img);
            Panel.SetZIndex(img, -1);
            camera.Children.Add(img);

        }
        /// <summary>
        /// перемещает игрока по полю
        /// </summary>
        private void MovePlayer()
        {
            player.move();
            playerImg.Source = new BitmapImage(new Uri(player.Update(), UriKind.Relative));
            intersectionPlayer();
            playerImg.Margin = new Thickness(player.Left, player.Top, player.Right, player.Bottom);
        }
    }
}