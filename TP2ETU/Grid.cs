using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
namespace TP2PROF
{
  public class Grid
  {
    #region Propriétés et Accèsseurs
    /// <summary>
    /// Grille logique du jeu.
    /// Tableau 2D de PacmanElement
    /// </summary>  
    private PacmanElement[,] tabElements;
    /// <summary>
    /// Position de la cage des fantômes
    /// </summary>
    private Vector2i ghostCagePosition;
    /// <summary>
    /// Accesseur du numéro de la ligne où se trouve la cage à fantômes
    /// Propriété C#
    /// </summary>
    public int GhostCagePositionRow
    {
      get { return ghostCagePosition.X; }
    }
    /// <summary>
    /// Accesseur du numéro de la colonne où se trouve la cage à fantômes
    /// Propriété C#
    /// </summary>
    public int GhostCagePositionColumn
    {
      get { return ghostCagePosition.Y; }
    }
    /// <summary>
    /// Position originale du pacman
    /// </summary>
    private Vector2i pacmanStartingPosition;
    /// <summary>
    /// Accesseur du numéro de la ligne où se trouve le pacman au début
    /// Propriété c#
    /// </summary>
    public int PacmanStartingPositionX
    {
      get { return pacmanStartingPosition.X; }
    }
    /// <summary>
    /// Accesseur du numéro de la colonne où se trouve le pacman au début
    /// Propriété C#
    /// </summary>
    public int PacmanStartingPositionY
    {
      get { return pacmanStartingPosition.Y; }
    }
    /// <summary>
    /// Accesseur de la hauteur
    /// Propriété C#
    /// </summary>
    public int Height
    {
      get { return tabElements.GetLength(0); }
    }
    /// <summary>
    /// Accesseur de la largeur
    /// Propriété C#
    /// </summary>
    public int Width
    {
      get { return tabElements.GetLength(1); }
    }
    #endregion

    #region Méthodes
    /// <summary>
    /// Constructeur sans paramètre
    /// </summary>
    public Grid()
    {
      tabElements = new PacmanElement[PacmanGame.DEFAULT_GAME_HEIGHT, PacmanGame.DEFAULT_GAME_WIDTH];

      pacmanStartingPosition.X = 1;
      pacmanStartingPosition.Y = 1;

      ghostCagePosition.X = PacmanGame.DEFAULT_GAME_WIDTH / 2;
      ghostCagePosition.Y = PacmanGame.DEFAULT_GAME_HEIGHT / 2;
    }


    //<aouellet>
    /// <summary>
    /// Charge un niveau à partir d'une chaine de caractères en mémoire.
    /// Voir l'énoncé du travail pour le format de la chaîne.
    /// </summary>
    /// <param name="content"> Le contenu du niveau en mémoire</param>
    /// <returns>true si le chargement est correct, false sinon</returns>
    public bool LoadFromMemory(string content)
    {
      bool retval = true;
      content = content.Replace(System.Environment.NewLine, "");
      string[] valeursFileContent = content.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
      int x = 0;
      int nbGhostCage = 0;
      int nbPacman = 0;
      if (valeursFileContent.Length < 462)
      {
        return false;
      }
      else
      {
        for (int i = 0; i < tabElements.GetLength(0); i++)
        {
          for (int j = 0; j < tabElements.GetLength(1); j++)
          {
            if (valeursFileContent[x] == "0")
            {
              tabElements[i, j] = PacmanElement.None;
              x++;
            }
            else if (valeursFileContent[x] == "1")
            {
              tabElements[i, j] = PacmanElement.Wall;
              x++;
            }
            else if (valeursFileContent[x] == "2")
            {
              tabElements[i, j] = PacmanElement.Ghost;
              x++;
            }
            else if (valeursFileContent[x] == "3")
            {
              tabElements[i, j] = PacmanElement.Pacman;
              x++;
              nbPacman++;
            }
            else if (valeursFileContent[x] == "4")
            {
              tabElements[i, j] = PacmanElement.Pill;
              x++;
            }
            else if (valeursFileContent[x] == "5")
            {
              tabElements[i, j] = PacmanElement.SuperPill;
              x++;
            }
            else if (valeursFileContent[x] == "6")
            {
              tabElements[i, j] = PacmanElement.GhostCage;
              x++;
              nbGhostCage++;
            }
            else
            {
              retval = false;
              return retval;
            }
          }
        }
        if (nbGhostCage > 1 || nbPacman > 1 || nbPacman == 0 || nbGhostCage == 0)
        {
          retval = false;
          return retval;
        }
      }

      return retval;
    }

    /// <summary>
    /// Retourne l'élément à la position spécifiée
    /// </summary>
    /// <param name="row">La ligne</param>
    /// <param name="column">La colonne</param>
    /// <returns>L'élément à la position spécifiée</returns>
    // A compléter
    public PacmanElement GetGridElementAt(int row, int column)
    {
      if (row >= 0 && row < tabElements.GetLength(0) && column >= 0 && column < tabElements.GetLength(1))
        return tabElements[row, column];
      else
        return PacmanElement.None;
    }




    /// <summary>
    /// Modifie le contenu du tableau à la position spécifiée
    /// </summary>
    /// <param name="row">La ligne</param>
    /// <param name="column">La colonne</param>
    /// <param name="element">Le nouvel élément à spécifier</param>
    // A compléter
    public void SetGridElementAt(int row, int column, PacmanElement element)
    {

      tabElements[row, column] = element;
    }
    //<aouellet>
    #endregion

  }
}
