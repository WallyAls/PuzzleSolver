//====================================================================================================
//The Free Edition of Java to C# Converter limits conversion output to 100 lines per file.

//To purchase the Premium Edition, visit our website:
//https://www.tangiblesoftwaresolutions.com/order/order-java-to-csharp.html
//====================================================================================================

using System;
using System.Collections.Generic;

namespace solver
{

	/// <summary>
	/// @author COS30019
	/// 
	/// </summary>
	public class PuzzleState : IComparable<PuzzleState>
	{
		public int[][] Puzzle;
		public PuzzleState Parent;
		public List<PuzzleState> Children;
		public int Cost;
		public int HeuristicValue;
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private int EvaluationFunction_Conflict;
		public direction PathFromParent;


		public int CompareTo(PuzzleState aState)
		{
			return EvaluationFunction - aState.getEvaluationFunction();
		}

		public PuzzleState(PuzzleState aParent, direction aFromParent, int[][] aPuzzle)
		{
			Parent = aParent;
			PathFromParent = aFromParent;
			Puzzle = aPuzzle;
			Cost = Parent.Cost + 1;
			EvaluationFunction_Conflict = 0;
			HeuristicValue = 0;
		}

		public PuzzleState(int[][] aPuzzle)
		{
			Parent = null;
			PathFromParent = null;
			Cost = 0;
			Puzzle = aPuzzle;
			EvaluationFunction_Conflict = 0;
			HeuristicValue = 0;
		}

		public virtual int EvaluationFunction
		{
			get
			{
				return EvaluationFunction_Conflict;
			}
			set
			{
				EvaluationFunction_Conflict = value;
			}
		}


		public virtual direction[] PossibleActions
		{
			get
			{
				//find where the blank cell is and store the directions.
				direction[] result;
				int[] blankLocation = new int[] { 0, 0 }; //dummy value to avoid errors.

				try
				{
					blankLocation = findBlankCell();
				}
				catch (InvalidPuzzleException)
				{
					Console.WriteLine("There was an error in processing! Aborting...");
					Environment.Exit(1);
				}
				result = new direction[countMovements(blankLocation)];
				int thisIndex = 0;
				if (blankLocation[0] == 0)
				{
					//the blank cell is already as far left as it will go, it can move right
					result[thisIndex++] = direction.Right;
				}
				else if (blankLocation[0] == (Puzzle.Length - 1))
				{
					result[thisIndex++] = direction.Left;
				}
				else
				{
					result[thisIndex++] = direction.Left;
					result[thisIndex++] = direction.Right;
				}

				if (blankLocation[1] == 0)
				{
					//the blank cell is already as far up as it will go, it can move down
					result[thisIndex++] = direction.Down;
				}
				else if (blankLocation[1] == (Puzzle.Length - 1))
				{
					result[thisIndex++] = direction.Up;
				}
				else
				{
					result[thisIndex++] = direction.Up;
					result[thisIndex++] = direction.Down;
				}
				return result;
			}
		}

		private int countMovements(int[] blankLocation)
		{
			int result = 2;
			try
			{
				blankLocation = findBlankCell();

				for (int i = 0; i <= 1; i++)
				{
					if (blankLocation[i] == 0 || blankLocation[i] == (Puzzle.Length - 1))
					{
						//do nothing
					}
					else
					{
						result++;
					}
				}
			}
			catch (InvalidPuzzleException)
			{
				//do something
			}
			return result;
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: private int[] findBlankCell() throws InvalidPuzzleException
		private int[] findBlankCell()
		{
			for (int i = 0; i < Puzzle.Length; i++)
			{
				for (int j = 0; j < Puzzle[i].Length; j++)
				{
					if (Puzzle[i][j] == 0)
					{
						int[] result = new int[] { i, j };
						return result;
					}
				}
			}
			//No blank cell found?
			throw new InvalidPuzzleException(this);
		}

		private int[][] cloneArray(int[][] cloneMe)
		{
			//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
			//ORIGINAL LINE: int[][] result = new int[cloneMe.Length][cloneMe[0].Length];
			int[][] result = RectangularArrays.RectangularIntArray(cloneMe.Length, cloneMe[0].Length);
			for (int i = 0; i < cloneMe.Length; i++)
			{
				for (int j = 0; j < cloneMe[i].Length; j++)
				{
					result[i][j] = cloneMe[i][j];
				}
			}
			return result;
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: public PuzzleState move(direction aDirection) throws CantMoveThatWayException
		public virtual PuzzleState move(direction aDirection)
		{
			//Moving up moves the empty cell up (and the cell above it down)
			//first, create the new one (the one to return)
			PuzzleState result = new PuzzleState(this, aDirection, cloneArray(this.Puzzle));

			//now, execute the changes: move the blank cell aDirection
			//find the blankCell
			int[] blankCell = new int[] { 0, 0 };
			try
			{
				blankCell = findBlankCell();
			}
			catch (InvalidPuzzleException)
			{
				Console.WriteLine("There was an error in processing! Aborting...");
				Environment.Exit(1);
			}
			try
			{
				//move the blank cell in the new child puzzle

				if (aDirection == direction.Up)
				{
					result.Puzzle[blankCell[0]][blankCell[1]] = result.Puzzle[blankCell[0]][blankCell[1] - 1];
					result.Puzzle[blankCell[0]][blankCell[1] - 1] = 0;
				}
				else if (aDirection == direction.Down)
				{
					result.Puzzle[blankCell[0]][blankCell[1]] = result.Puzzle[blankCell[0]][blankCell[1] + 1];
					result.Puzzle[blankCell[0]][blankCell[1] + 1] = 0;
				}
				else if (aDirection == direction.Left)
				{
					result.Puzzle[blankCell[0]][blankCell[1]] = result.Puzzle[blankCell[0] - 1][blankCell[1]];
					result.Puzzle[blankCell[0] - 1][blankCell[1]] = 0;
				}
				else    //aDirection == Right;
				{
					result.Puzzle[blankCell[0]][blankCell[1]] = result.Puzzle[blankCell[0] + 1][blankCell[1]];
					result.Puzzle[blankCell[0] + 1][blankCell[1]] = 0;
				}
				return result;
			}
			catch (IndexOutOfBoundsException ex)
			{
				throw new CantMoveThatWayException(this, aDirection);
			}
		}



















	}
}

//====================================================================================================
//End of the allowed output for the Free Edition of Java to C# Converter.

//To purchase the Premium Edition, visit our website:
//https://www.tangiblesoftwaresolutions.com/order/order-java-to-csharp.html
//====================================================================================================
