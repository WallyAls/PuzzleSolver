using System.Collections.Generic;

namespace solver
{

	public abstract class SearchMethod
	{
		public string code; //the code used to identify the method at the command line
		public string longName; //the actual name of the method.
		//public List<PuzzleState> nodeList;	//this is for catching repeated states and counting total nodes.
		public abstract direction[] Solve(nPuzzle aPuzzle);

		//The fringe needs to be a Queue and a Stack.
		//LinkedList implements both interfaces.
		//LinkedList also implements List, which allows it to be sorted easily.
		public LinkedList<PuzzleState> Frontier;

		//the searched list simply needs to be able to store nodes for the purpose of checking
		//Fast addition and removal is crucial here.
		//HashSet provides constant time for add, contains and size.
		public LinkedList<PuzzleState> Searched;

		public abstract bool addToFrontier(PuzzleState aState);
		protected internal abstract PuzzleState popFrontier();

		public virtual void reset()
		{
			this.Frontier.Clear();
			this.Searched.Clear();
		}
	}
}