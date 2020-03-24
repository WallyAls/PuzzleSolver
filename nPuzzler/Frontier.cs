using System.Collections.Generic;

namespace solver
{

	public class Frontier
	{
		//We can use a linked list here because it's more efficient than an array
		//and because we only want to add/remove items from the start or end.
		public LinkedList<PuzzleState> Items;

		public Frontier()
		{
			Items = new LinkedList<PuzzleState>();
		}

		public virtual PuzzleState Pop()
		{
			return Items.RemoveFirst();
		}

		public virtual void Push(PuzzleState aState)
		{
			Items.AddFirst(aState);
		}

		public virtual void Push(PuzzleState[] aStates)
		{
			for (int i = 0; i < aStates.Length; i++)
			{
				//add each item at the start of the list.
				Items.AddFirst(aStates[i]);
			}
		}

		public virtual void Enqueue(PuzzleState aState)
		{
			//This can be done using the addToArray method from the nPuzzler class
			Items.AddLast(aState);
		}

		public virtual void Enqueue(PuzzleState[] aStates)
		{
			//This can be done using the addToArray method from the nPuzzler class
			for (int i = 0; i < aStates.Length; i++)
			{
				Items.AddLast(aStates[i]);
			}
		}

		public virtual void SortByCostAsc()
		{
			//TODO: Implement SortByCostAsc()

		}

		public virtual void SortByCostDesc()
		{
			//TODO: Implement SortByCostDesc() 
		}

		public virtual void SortByHeuristicAsc()
		{
			//TODO: Implement SortByHeuristicAsc()
		}

		public virtual void SortByHeuristicDesc()
		{
			//TODO: Implement SortByHeuristicDesc()
		}
	}

}