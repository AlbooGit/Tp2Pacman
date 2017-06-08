using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
namespace TP2PROF
{
  public class Pacman
  {
    #region Propriétés et Accèsseurs
    /// <summary>
    /// Position du pacman
    /// </summary>
    private Vector2i position;
    /// <summary>
    /// Accesseur de la position en ligne
    /// Propriété C#
    /// </summary>
    public int Row
    {
      get { return position.X; }
      set { Row = value; }
    }
    /// <summary>
    /// Accesseur de la position en colonne
    /// Propriété C#
    /// </summary>
    public int Column
    {
      get { return position.Y; }
      set { Row = value; }
    }
    /// <summary>
    /// Compteur du nombre de mise à jours effectués
    /// </summary>
    private int updateCounter;
    /// <summary>
    /// Fréquence à laquelle Pacman se met à jour
    /// </summary>
    private int updateFrequency;

    // Propriétés SFML pour l'affichage
    private Texture pacmanTexture = new Texture("Assets/Pacman.bmp");
    private Sprite pacmanSprite = null;
    #endregion

    #region Méthodes
    /// <summary>
    /// Constructeur
    /// </summary>
    /// <param name="row">Ligne de départ du pacman</param>
    /// <param name="column">Colonne de départ du pacman</param>
    public Pacman(int row, int column)
    {

      // Affectation de la position du pacman 
      // Ne pas oublier de lancer une exception si les paramètres sont invalides
      position.Y = column;
      position.X = row;
      updateCounter = 0;
      updateFrequency = 1;
      // Initialisation des propriétés SFML
      pacmanSprite = new Sprite(pacmanTexture);
      pacmanSprite.Origin = new Vector2f(pacmanTexture.Size.X / 2, pacmanTexture.Size.Y / 2);
    }
    /// <summary>
    /// Déplace le pacman selon une direction donnée.
    /// </summary>
    /// <param name="direction">Direction dans laquelle on veut déplacer le pacman</param>
    /// <param name="grid">Grille de référence. Utilisée pour ne pas que le pacman passe au travers des murs</param>
    public bool Move(Direction direction, Grid grid)
    {
      bool retval = true;

      if (direction == Direction.West)
      {
        if (position.Y == 0)
          retval = false;
        else if (grid.GetGridElementAt(position.X, position.Y - 1) == PacmanElement.Wall)
          retval = false;
        else
          position.Y--;
      }
      if (direction == Direction.East)
      {
        if (position.Y == grid.Height - 1)
          retval = false;
        else if (grid.GetGridElementAt(position.X, position.Y + 1) == PacmanElement.Wall)
          retval = false;
        else
          position.Y++;
      }
      if (direction == Direction.North)
      {
        if (position.X == 0)
          retval = false;
        else if (grid.GetGridElementAt(position.X - 1, position.Y) == PacmanElement.Wall)
          retval = false;
        else
          position.X--;
      }
      if (direction == Direction.South)
      {
        if (position.X == grid.Width - 1)
          retval = false;
        else if (grid.GetGridElementAt(position.X + 1, position.Y) == PacmanElement.Wall)
          retval = false;
        else
          position.X++;
      }

      return retval;
    }
    /// <summary>
    /// Affiche le pacman dans la fenêtre de rendu.
    /// </summary>
    /// <param name="window">Fenêtre de rendu</param>
    public void Draw(RenderWindow window)
    {
      pacmanSprite.Position = new Vector2f(PacmanGame.DEFAULT_GAME_ELEMENT_WIDTH * Column,
                                           PacmanGame.DEFAULT_GAME_ELEMENT_HEIGHT * Row) + pacmanSprite.Origin;
      window.Draw(pacmanSprite);
    }
    #endregion
  }
}
