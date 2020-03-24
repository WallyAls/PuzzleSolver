using System.Collections.Generic;

namespace solver
{

	public class BFSStrategy : SearchMethod
	{

		public BFSStrategy()
		{
			code = "BFS";
			longName = "Breadth-First Search";
			Frontier = new LinkedList<PuzzleState>();
			Searched = new LinkedList<PuzzleState>();
		}

		protected internal override PuzzleState popFrontier()
		{
			//remove an item from the fringe to be searched
			PuzzleState thisState = Frontier.First.Value;
			Frontier.RemoveFirst();
			//Add it to the list of searched states, so that it isn't searched again
			Searched.AddLast(thisState);

			return thisState;
		}

		public override direction[] Solve(nPuzzle puzzle)
		{
			//This method uses the fringe as a queue.
			//Therefore, nodes are searched in order of cost, with the lowest cost
			// unexplored node searched next.
			//-----------------------------------------

			//put the start state in the Fringe to get explored.
			addToFrontier(puzzle.StartState);


			List<PuzzleState> newStates = new List<PuzzleState>();

			while (Frontier.Count > 0)
			{
				//get the next item off the fringe
				PuzzleState thisState = popFrontier();

				//is it the goal item?
				if (thisState.Equals(puzzle.GoalState))
				{
					//We have found a solution! return it!
					return thisState.GetPathToState();
				}
				else
				{
					//This isn't the goal, just explore the node
					newStates = thisState.explore();

					for (int i = 0; i < newStates.Count; i++)
					{
						//add this state to the fringe, addToFringe() will take care of duplicates
						//
						// TODO: is this the correct way to add to frontier as specified in the Assignment: 
						// When all else is equal, nodes should be expanded according to the following order: 
						// the agent should try to move the empty cell UP before attempting LEFT, before 
						// attempting DOWN, before attempting RIGHT, in that order.
						addToFrontier(newStates[i]);
					}
				}
			}

			//No solution found and we've run out of nodes to search
			//return null.
			return null;
		}

		public override bool addToFrontier(PuzzleState aState)
		{
			//if this state has been found before,
			if (Searched.Contains(aState) || Frontier.Contains(aState))
			{
				//discard it
				return false;
			}
			else
			{
				//else put this item on the end of the queue;
				Frontier.AddLast(aState);
				return true;
			}
		}

	}

}