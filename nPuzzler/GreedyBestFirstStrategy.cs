﻿using System.Collections.Generic;

namespace solver
{

	public class GreedyBestFirstStrategy : SearchMethod
	{

		public GreedyBestFirstStrategy()
		{
			code = "GBFS";
			longName = "Greedy Best-First Search";
			Frontier = new LinkedList<PuzzleState>();
			Searched = new LinkedList<PuzzleState>();
		}

		public override bool addToFrontier(PuzzleState aState)
		{
			//We only want to add the new state to the fringe if it doesn't exist
			// in the fringe or the searched list.
			if (Searched.Contains(aState) || Frontier.Contains(aState))
			{
				return false;
			}
			else
			{
				Frontier.AddLast(aState);
				return true;
			}
		}

		public override direction[] Solve(nPuzzle aPuzzle)
		{
			//keep searching the fringe until it's empty.
			//Items are "popped" from the fringe in order of lowest heuristic value.

			//Add the start state to the fringe
			addToFrontier(aPuzzle.StartState);
			while (Frontier.Count > 0)
			{
				//get the next State
				PuzzleState thisState = popFrontier();

				//is this the goal state?
				if (thisState.Equals(aPuzzle.GoalState))
				{
					return thisState.GetPathToState();
				}

				//not the goal state, explore this node
				List<PuzzleState> newStates = thisState.explore();

				for (int i = 0; i < newStates.Count; i++)
				{
					PuzzleState newChild = newStates[i];
					//if you can add these new states to the fringe
					if (addToFrontier(newChild))
					{
						//then, work out it's heuristic value
						newChild.HeuristicValue = HeuristicValue(newStates[i], aPuzzle.GoalState);
						newChild.EvaluationFunction = newChild.HeuristicValue;
					}

				}

				//Sort the fringe by it's Heuristic Value. The PuzzleComparator uses each nodes EvaluationFunction
				// to determine a node's value, based on another. The sort method uses this to sort the Fringe.
				// 
				// TODO: is this the correct way to sort the frontier as specified in the Assignment: 
				// When all else is equal, nodes should be expanded according to the following order: 
				// the agent should try to move the empty cell UP before attempting LEFT, before 
				// attempting DOWN, before attempting RIGHT, in that order.
				Frontier.Sort(new PuzzleComparator());
			}

			//no more nodes and no path found?
			return null;
		}

		protected internal override PuzzleState popFrontier()
		{
			//remove a state from the top of the fringe so that it can be searched.
			PuzzleState lState = Frontier.RemoveFirst();

			//add it to the list of searched states so that duplicates are recognised.
			Searched.AddLast(lState);

			return lState;
		}

		private int HeuristicValue(PuzzleState aState, PuzzleState goalState)
		{
			//find out how many elements in aState match the goalState
			//return the number of elements that don't match
			int heuristic = 0;
			for (int i = 0; i < aState.Puzzle.Length; i++)
			{
				for (int j = 0; j < aState.Puzzle[i].Length; j++)
				{
					if (aState.Puzzle[i][j] != goalState.Puzzle[i][j])
					{
						heuristic++;
					}
				}
			}

			return heuristic;
		}

	}

}