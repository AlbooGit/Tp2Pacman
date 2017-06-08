using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using SFML.Audio;
namespace TP2PROF
{
  public class PacmanGame
  {
    /// <summary>
    /// Nombre de cases en largeur dans le jeu du Pacman
    /// </summary>
    public const int DEFAULT_GAME_WIDTH = 21;

    /// <summary>
    /// Nombre de cases en hauteur dans le jeu du Pacman
    /// </summary>
    public const int DEFAULT_GAME_HEIGHT = 22;

    /// <summary>
    /// Largeur de rendu d'un élément de jeu
    /// </summary>
    public const int DEFAULT_GAME_ELEMENT_HEIGHT = 20;

    /// <summary>
    /// Hauteur de rendu d'un élément de jeu
    /// </summary>
    public const int DEFAULT_GAME_ELEMENT_WIDTH = 20;

    /// <summary>
    /// La grille principale de jeu. Elle est créée dans la méthode LoadGrid
    /// </summary>
    private Grid grid;


    /// <summary>
    /// Nombre de fantômes présents dans le jeu
    /// </summary>
    public const int NB_GHOSTS = 4;

    /// <summary>
    /// Les 4 fantômes du jeu
    /// </summary>
    // A COMPLETER
    public Ghost[] tabGhosts;
    /// <summary>
    /// Le pacman du jeu
    /// </summary>
    private Pacman pacman;

    /// <summary>
    /// Durée d'activation d'une superpastille (en secondes)
    /// </summary>
    private const int SUPERPILL_ACTIVATION_TIME = 5;


    /// <summary>
    /// Accesseur permettant de savoir si une super pastille est active
    /// Propriété C#
    /// </summary>
    public bool IsSuperPillActive
    {
      get;
      set;
    }

    // Propriétés SFML pour l'affichage des pastilles et super-pastilles
    const float SMALL_PILL_RADIUS = DEFAULT_GAME_ELEMENT_HEIGHT / 8;
    const float SUPER_PILL_RADIUS = 2 * SMALL_PILL_RADIUS;
    CircleShape smallPillShape = new CircleShape(SMALL_PILL_RADIUS);
    CircleShape superPillShape = new CircleShape(SUPER_PILL_RADIUS);

    // Propriétés SFML pour l'affichage du labyrinthe
    Texture wallTexture = new Texture("Assets/Wall.bmp");
    Sprite wallSprite = null;

    /// <summary>
    /// Constructeur du jeu de Pacman
    /// </summary>
    public PacmanGame()
    {
      // A COMPLETER   
      IsSuperPillActive = false;
      grid = new Grid();
      tabGhosts = new Ghost[NB_GHOSTS];

      // Initialisation SFML
      smallPillShape.Origin = new Vector2f((float)-(DEFAULT_GAME_ELEMENT_WIDTH - SMALL_PILL_RADIUS) / 2, -(float)(DEFAULT_GAME_ELEMENT_HEIGHT - SMALL_PILL_RADIUS) / 2);
      superPillShape.Origin = new Vector2f((float)-(DEFAULT_GAME_ELEMENT_WIDTH - SUPER_PILL_RADIUS) / 2, -(float)(DEFAULT_GAME_ELEMENT_HEIGHT - SUPER_PILL_RADIUS) / 2);
      wallSprite = new Sprite(wallTexture);
    }

    /// <summary>
    /// Charge un fichier de labyrinthe.
    /// </summary>
    /// <param name="path">Le fichier contenant la description du labyrinthe de jeu</param>
    /// <returns>true si le chargement s'est bien effectué, false sinon</returns>
    public bool LoadGrid(string path)
    {
      bool retval = System.IO.File.Exists(path);
      int compteurGhosts = 0;

      if (retval)
      {
        string fileContent = System.IO.File.ReadAllText(path);
        // ppoulin
        // Appelez la méthode LoadFromMemory ici
        retval = grid.LoadFromMemory(fileContent);


        // Si le chargement s'est correctement effectué
        if (retval)
        {
          // On parcourt la grille et, avec la méthode GetGridElementAt
          // On trouve les positions où il y a des fantômes
          //<aouellet>
          while (compteurGhosts < NB_GHOSTS)
          {
            for (int i = 0; i < grid.Width; i++)
            {
              for (int j = 0; j < grid.Height; j++)
              {
                if (grid.GetGridElementAt(i, j) == PacmanElement.Ghost)
                {
                  //instancier les ghosts ici
                  tabGhosts[compteurGhosts] = new Ghost(i, j);
                  grid.SetGridElementAt(i, j, PacmanElement.None);
                  compteurGhosts++;
                }
              }
            }
          }
          //<aouellet>
          // Ensuite, on crée le pacman à la position spécifiée par la grille.
          for (int i = 0; i < grid.Width; i++)
          {
            for (int j = 0; j < grid.Height; j++)
            {
              if (grid.GetGridElementAt(i, j) == PacmanElement.Pacman)
              {
                pacman = new Pacman(i, j);
                grid.SetGridElementAt(i, j, PacmanElement.None);
              }
            }
          }
        }
      }

      return retval;
    }

    /// <summary>
    /// Met à jour la logique de jeu
    /// </summary>
    /// <param name="key">La touche entrée par le joueur pour contrôle le pacman</param>
    /// <returns>EndGameResult.NotFinished si la partie est toujours en cours, EndGameResult.Win
    /// si le joueur a mangé toutes les pastilles ou EndGameResult.Losse si le joueur s'est fait
    /// mangé par un fantôme</returns>
    public EndGameResult Update(Keyboard.Key key)
    {

      EndGameResult gameState = EndGameResult.NotFinished;
      // Déplacement du joueur
      if (key == Keyboard.Key.Left)
      {
        pacman.Move(Direction.West, grid);
      }
      else if (key == Keyboard.Key.Right)
      {
        pacman.Move(Direction.East, grid);
      }
      else if (key == Keyboard.Key.Up)
      {
        pacman.Move(Direction.North, grid);
      }
      else if (key == Keyboard.Key.Down)
      {
        pacman.Move(Direction.South, grid);
      }


      Vector2i vector = new Vector2i(1, 1);
      // Mise à jour des fantômes
      //<aouellet>
      for (int i = 0; i < tabGhosts.Length; i++)
      {
        Direction direction = PathFinder.FindShortestPath(grid, tabGhosts[i].Row, tabGhosts[i].Column, pacman.Row, pacman.Column);
        tabGhosts[i].Move(direction, grid);
        tabGhosts[i].Update(grid, vector, IsSuperPillActive);
      }
      //<aouellet>



      // Gestion des collisions avec le pacman
      for (int i = 0; i < tabGhosts.Length; i++)
      {
        if (tabGhosts[i].Row == pacman.Row && tabGhosts[i].Column == pacman.Column)
        {
          if (IsSuperPillActive)
          {
            Direction direction = PathFinder.FindShortestPath(grid, tabGhosts[i].Row, tabGhosts[i].Column, pacman.Row, pacman.Column);
            tabGhosts[i].Move(direction, grid);
            tabGhosts[i].Update(grid, vector, IsSuperPillActive);
          }
          else
            gameState = EndGameResult.Loss;
        }
      }




      // Vérification du ramassage d'une pastille
      if (grid.GetGridElementAt(pacman.Row, pacman.Column) == PacmanElement.Pill)
        grid.SetGridElementAt(pacman.Row, pacman.Column, PacmanElement.None);


      // Vérification de l'activation d'un superpill
      // A COMPLETER    
      if (grid.GetGridElementAt(pacman.Row, pacman.Column) == PacmanElement.SuperPill)
      {
        IsSuperPillActive = true;
        grid.SetGridElementAt(pacman.Row, pacman.Column, PacmanElement.None);
      } 

      // Validations de fin de partie
      if (ComputePillsLeft() == 0)
        gameState = EndGameResult.Win;

      return gameState;
    }

    /// <summary>
    /// Calcule le nombre de pastille non encore ramassées par le pacman
    /// </summary>
    /// <returns>Le nombre de pastille non encore ramassées</returns>
    public int ComputePillsLeft()
    {
      int pillsLeft = 0;

      for (int i = 0; i < grid.Height; i++)
      {
        for (int j = 0; j < grid.Width; j++)
        {
          if (grid.GetGridElementAt(i, j) == PacmanElement.Pill || grid.GetGridElementAt(i, j) == PacmanElement.SuperPill)
            pillsLeft++;
        }
      }

      return pillsLeft;
    }


    

    /// <summary>
    /// Dessine les éléments du jeu à l'écran
    /// </summary>
    /// <param name="window">Le contexte de rendu</param>
    public void Draw(RenderWindow window)
    {
      // PPOULIN
      // A DECOMMENTER LORSQUE LES CLASSES AURONT ÉTÉ CODÉES
      for (int row = 0; row < grid.Height; row++)
      {
        for (int col = 0; col < grid.Width; col++)
        {
          // Pastille régulière
          if (grid.GetGridElementAt(row, col) == PacmanElement.Pill)
          {
            smallPillShape.Position = new Vector2f(col * DEFAULT_GAME_ELEMENT_WIDTH, row * DEFAULT_GAME_ELEMENT_HEIGHT);
            window.Draw(smallPillShape);
          }
          // Super pastille
          else if (grid.GetGridElementAt(row, col) == PacmanElement.SuperPill)
          {

            superPillShape.Radius = SUPER_PILL_RADIUS;
            superPillShape.Position = new Vector2f(col * DEFAULT_GAME_ELEMENT_WIDTH, row * DEFAULT_GAME_ELEMENT_HEIGHT);
            window.Draw(superPillShape);
          }
          // Mur
          else if (grid.GetGridElementAt(row, col) == PacmanElement.Wall)
          {
            wallSprite.Position = new Vector2f(col * DEFAULT_GAME_ELEMENT_WIDTH, row * DEFAULT_GAME_ELEMENT_HEIGHT);
            window.Draw(wallSprite);
          }
        }
      }

      // Les 4 fantômes
      for (int i = 0; i < NB_GHOSTS; i++)
      {
        if (tabGhosts[i] != null)
          tabGhosts[i].Draw(window, IsSuperPillActive);
      }

      // Le pacman
      if (null != pacman)
        pacman.Draw(window);

    }
  }
}
