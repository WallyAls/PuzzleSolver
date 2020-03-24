using System;

namespace solver
{
	public class InvalidPuzzleException : Exception
	{
		public PuzzleState theState;
		internal const long serialVersionUID = 1988122501;


		public InvalidPuzzleException(PuzzleState aState)
		{
			//This puzzle is invalid for some reason
			theState = aState;
		}

	}

}