using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP2PROF
{
  public static class PathFinder
  {
    /// <summary>
    /// Initialise le tableau des coûts de déplacements, Le tableau est 
    /// initialisé à int.MaxValue partout sauf à l'endroit où se trouve le
    /// fantôme où le tableau est initialisé à 0.
    /// </summary>
    /// <param name="aGrid">La grille du jeu: pour connaitre les dimensions attendues</param>
    /// <param name="fromX">La position du pacman en colonne</param>
    /// <param name="fromY">La position du pacman en ligne</param>
    /// <returns>Le tableau initialisé correctement</returns>
    // A COMPLÉTER  Méthode InitCosts
    public static int[,] InitCost(Grid aGrid, int fromX, int fromY)
    {
      int[,] tabCosts = new int[aGrid.Height, aGrid.Width];

      for (int i = 0; i < aGrid.Height; i++)
      {
        for (int j = 0; j < aGrid.Width; j++)
        {
          if (aGrid.GetGridElementAt(i, j) != PacmanElement.Wall || aGrid.GetGridElementAt(i, j) != PacmanElement.GhostCage)
            tabCosts[i, j] = int.MaxValue;
          else
            tabCosts[i, j] = int.MinValue;
        }
      }

      tabCosts[fromX, fromY] = 0;

      return tabCosts;
    }
    /// <summary>
    /// Détermine le premier déplacement nécessaire pour déplacer un objet de la position (fromX, fromY)
    /// vers la position (toX, toY). 
    /// <param name="aGrid">La grille du jeu: pour connaitre les positions des murs</param>
    /// <param name="fromX">La position de départ en colonne</param>
    /// <param name="fromY">La position de départ en ligne</param>
    /// <param name="toX">La position cible en colonne</param>
    /// <param name="toY">La position cible en ligne</param>
    /// <remark>Typiquement, la position (fromX, fromY) est celle du fantôme tandis 
    /// que la position (toX, toY) est celle du pacman.</remark>
    /// <returns>La direction dans laquelle on doit aller. Direction.None si l'on
    /// est déjà rendu ou Direction.Undefined s'il est impossible d'atteindre la cible</returns>
    /// </summary>
    // A COMPLÉTER  Méthode FindShortestPath
    public static Direction FindShortestPath(Grid aGrid, int fromX, int fromY, int toX, int toY)
    {
      int[,] costs = InitCost(aGrid, fromX, fromY);
      ComputeCost(aGrid, fromX, fromY, toX, toY, costs);
      Direction direction = RecursiveFindDirection(costs, fromX, fromY, toX, toY);
      return direction;
    }


    /// <summary>
    /// Calcule le nombre de déplacements requis pour aller de la position (fromX, fromY)
    /// vers la position (toX, toY). 
    /// <param name="aGrid">La grille du jeu: pour connaitre les positions des murs</param>
    /// <param name="fromX">La position de départ en colonne</param>
    /// <param name="fromY">La position de départ en ligne</param>
    /// <param name="toX">La position cible en colonne</param>
    /// <param name="toY">La position cible en ligne</param>
    /// <param name="costs">Le tableau des coûts à remplir</param>
    /// <remark>Typiquement, la position (fromX, fromY) est celle du fantôme tandis 
    /// que la position (toX, toY) est celle du pacman.</remark>
    /// <remark>Cette méthode est récursive</remark>
    /// </summary>
    // A COMPLÉTER  Méthode ComputeCosts
    public static int ComputeCost(Grid aGrid, int fromX, int fromY, int toX, int toY, int[,] costs)
    {
      if (fromX == toX && fromY == toY)
        return costs[fromX, fromY];
      if (fromX > 1 && fromX < costs.GetLength(0) && fromY > 1 && fromY < costs.GetLength(1))
      {
        if (aGrid.GetGridElementAt(fromX + 1, fromY) != PacmanElement.Wall)
        {
          if (costs[fromX, fromY] < costs[fromX + 1, fromY])
          {
            costs[fromX + 1, fromY] = costs[fromX, fromY] + 1;
            ComputeCost(aGrid, fromX + 1, fromY, toX, toY, costs);
          }
        }
        if (aGrid.GetGridElementAt(fromX - 1, fromY) != PacmanElement.Wall)
        {
          if (costs[fromX, fromY] < costs[fromX - 1, fromY])
          {
            costs[fromX - 1, fromY] = costs[fromX, fromY] + 1;
            ComputeCost(aGrid, fromX - 1, fromY, toX, toY, costs);
          }
        }
        if (aGrid.GetGridElementAt(fromX, fromY + 1) != PacmanElement.Wall)
        {
          if (costs[fromX, fromY] < costs[fromX, fromY + 1])
          {
            costs[fromX, fromY + 1] = costs[fromX, fromY] + 1;
            ComputeCost(aGrid, fromX, fromY + 1, toX, toY, costs);
          }
        }
        if (aGrid.GetGridElementAt(fromX, fromY - 1) != PacmanElement.Wall)
        {
          if (costs[fromX, fromY] < costs[fromX, fromY - 1])
          {
            costs[fromX, fromY - 1] = costs[fromX, fromY] + 1;
            ComputeCost(aGrid, fromX, fromY - 1, toX, toY, costs);
          }
        }
      }
      return -1;
    }
    /// <summary>
    /// Parcourt le tableau de coûts pour trouver le premier déplacement requis pour aller de la position (fromX, fromY)
    /// vers la position (toX, toY). 
    /// <param name="costs">Le tableau des coûts prédédemment calculés</param>
    /// <param name="targetX">La position cible en colonne</param>
    /// <param name="targetY">La position cible en ligne</param>
    /// <remark>Typiquement, la position (targetX, targetY) est celle du pacman.</remark>
    /// <remark>Cette méthode est récursive</remark>
    /// </summary>
    /// <returns>La direction dans laquelle on doit aller. Direction.None si l'on
    /// est déjà rendu ou Direction.Undefined s'il est impossible d'atteindre la cible</returns>
    // A COMPLÉTER  Méthode RecurseFindDirection
    public static Direction RecursiveFindDirection(int[,] costs, int targetX, int targetY, int fromX, int fromY)
    {
      if (fromX < 1 || fromY < 1 || targetX < 1 || targetY < 1 || fromX >= costs.GetLength(0) || fromY >= costs.GetLength(1))
      {
        return Direction.Undefined;
      }
      else
      {
       // int temporaire = targetX;
      //  targetX = targetY;
       // targetY = temporaire;

        //Code fait avec des froms. C'est mélangeant ce qui est la bonne façon car le diagramme de classes dit que la fonction utilise from mais pas dans les params du summary.
        //if (costs[targetX - 1, targetY] == costs[targetX, targetY] -1)
        //{
        //  if (costs[fromX, fromY] == costs[targetX - 1, targetY])
        //    return Direction.West; 
        //  else
        //    return RecursiveFindDirection(costs, fromX, fromY, targetX - 1, targetY);
        //}
        //else if (costs[targetX, targetY + 1] == costs[targetX, targetY] - 1)
        //{
        //  if (costs[fromX, fromY] == costs[targetX, targetY + 1])
        //    return Direction.South;
        //  else
        //    return RecursiveFindDirection(costs, fromX, fromY, targetX, targetY + 1);
        //}
        //else if (costs[targetX + 1, targetY] == costs[targetX, targetY] - 1)
        //{
        //  if (costs[fromX, fromY] == costs[targetX + 1, targetY])
        //    return Direction.East;
        //  else
        //    return RecursiveFindDirection(costs, fromX, fromY, targetX + 1, targetY);
        //}
        //else if (costs[targetX, targetY - 1] == costs[targetX, targetY] - 1)
        //{
        //  if (costs[fromX, fromY] == costs[targetX, targetY - 1])
        //    return Direction.North;
        //  else
        //    return RecursiveFindDirection(costs, fromX, fromY, targetX, targetY - 1);
        //}
        //else if (fromX == targetX && fromY == targetY)
        //{
        //  return Direction.None;
        //}
        //else
        //{
        //  return Direction.Undefined;
        //}


        //Les directions ne sont peut-être mélangé. Je joue avec ceux-ci pour voir les réactions des fantômes pour débugger. Malheureusement je ne trouve pas la source du AI défectueux.
        int targetCosts = costs[targetX, targetY];
        int fromCosts = costs[fromX, fromY];

        if (costs[targetX, targetY - 1] == targetCosts + 1)
        {
          if (costs[targetX, targetY - 1] == fromCosts)
          {
            return Direction.North;
          }
          return RecursiveFindDirection(costs, targetX, targetY - 1, fromX, fromY);
        }
        if (costs[targetX - 1, targetY] == targetCosts + 1)
        {
          if (costs[targetX - 1, targetY] == fromCosts)
          {
            return Direction.West;
          }
          return RecursiveFindDirection(costs, targetX - 1, targetY, fromX, fromY);
        }
        if (costs[targetX, targetY + 1] == targetCosts + 1)
        {
          if (costs[targetX, targetY + 1] == fromCosts)
          {
            return Direction.South;
          }
          return RecursiveFindDirection(costs, targetX, targetY + 1, fromX, fromY);
        }
        if (costs[targetX + 1, targetY] == targetCosts + 1)
        {
          if (costs[targetX + 1, targetY] == fromCosts)
          {
            return Direction.East;
          }
          return RecursiveFindDirection(costs, targetX + 1, targetY, fromX, fromY);
        }

        return Direction.Undefined;

      }

    }

  }
}
