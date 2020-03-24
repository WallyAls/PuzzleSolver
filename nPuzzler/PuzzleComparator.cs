using System.Collections.Generic;

namespace solver
{

	public class PuzzleComparator : IComparer<PuzzleState>
	{

		public virtual int Compare(PuzzleState state1, PuzzleState state2)
		{
			return state1.EvaluationFunction - state2.EvaluationFunction;
		}

	}

}