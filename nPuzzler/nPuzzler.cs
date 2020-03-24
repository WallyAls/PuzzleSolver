using System;
using System.IO;

namespace solver
{

	/// <summary>
	/// nPuzzler --- main class/initiate search methods/read problem file/etc.
	/// @author    COS30019
	/// </summary>
	internal class nPuzzler
	{

		//the number of methods programmed into nPuzzler
		public const int METHOD_COUNT = 2;
		public static nPuzzle gPuzzle;
		public static SearchMethod[] lMethods;

		public static void Main(string[] args)
		{
			//Create method objects
			InitMethods();

			//args contains:
			//  [0] - filename containing puzzle(s)
			//  [1] - method name

			if (args.Length < 2)
			{
				Console.WriteLine("Usage: nPuzzler <filename> <search-method>.");
				Environment.Exit(1);
			}

			//Get the puzzle from the file
			gPuzzle = readProblemFile(args[0]);

			string method = args[1];
			SearchMethod thisMethod = null;

			//determine which method the user wants to use to solve the puzzles
			for (int i = 0; i < METHOD_COUNT; i++)
			{
				//do they want this one?
				if (lMethods[i].code.CompareTo(method) == 0)
				{
					//yes, use this method.
					thisMethod = lMethods[i];
				}
			}

			//Has the method been implemented?
			if (thisMethod == null)
			{
				//No, give an error
				Console.WriteLine("Search method identified by " + method + " not implemented. Methods are case sensitive.");
				Environment.Exit(1);
			}

			//Solve the puzzle, using the method that the user chose
			direction[] thisSolution = thisMethod.Solve(gPuzzle);

			//Print information about this solution
			Console.WriteLine(args[0] + "   " + method + "   " + thisMethod.Searched.Count);
			if (thisSolution == null)
			{
				//No solution found :(
				Console.WriteLine("No solution found.");
			}
			else
			{
				//We found a solution, print all the steps to success!
				for (int j = 0; j < thisSolution.Length; j++)
				{
					Console.Write(thisSolution[j].ToString() + ";");
				}
				Console.WriteLine();
			}
			//Reset the search method for next use.
			thisMethod.reset();
			Environment.Exit(0);
		}

		private static void InitMethods()
		{
			lMethods = new SearchMethod[METHOD_COUNT];
			lMethods[0] = new BFSStrategy();
			lMethods[1] = new GreedyBestFirstStrategy();
		}

		private static nPuzzle readProblemFile(string fileName) // this allow only one puzzle to be specified in a problem file
		{

			try
			{
				//create file reading objects
				StreamReader reader = new StreamReader(fileName);
				StreamReader puzzle = new StreamReader(reader);
				nPuzzle result;

				string puzzleDimension = puzzle.ReadLine();
				//split the string by letter "x"
				string[] bothDimensions = puzzleDimension.Split("x", true);

				//work out the "physical" size of the puzzle
				//here we only deal with NxN puzzles, so the puzzle size is taken to be the first number
				int puzzleSize = int.Parse(bothDimensions[0]);

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: int[][] startPuzzleGrid = new int[puzzleSize][puzzleSize];
				int[][] startPuzzleGrid = RectangularArrays.RectangularIntArray(puzzleSize, puzzleSize);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: int[][] goalPuzzleGrid = new int[puzzleSize][puzzleSize];
				int[][] goalPuzzleGrid = RectangularArrays.RectangularIntArray(puzzleSize, puzzleSize);

				//fill in the start state
				string startStateString = puzzle.ReadLine();
				startPuzzleGrid = ParseStateString(startStateString, startPuzzleGrid, puzzleSize);

				//fill in the end state
				string goalStateString = puzzle.ReadLine();
				goalPuzzleGrid = ParseStateString(goalStateString, goalPuzzleGrid, puzzleSize);

				//create the nPuzzle object...
				result = new nPuzzle(startPuzzleGrid, goalPuzzleGrid);

				puzzle.Close();
				return result;
			}
			catch (FileNotFoundException)
			{
				//The file didn't exist, show an error
				Console.WriteLine("Error: File \"" + fileName + "\" not found.");
				Console.WriteLine("Please check the path to the file.");
				Environment.Exit(1);
			}
			catch (IOException)
			{
				//There was an IO error, show and error message
				Console.WriteLine("Error in reading \"" + fileName + "\". Try closing it and programs that may be accessing it.");
				Console.WriteLine("If you're accessing this file over a network, try making a local copy.");
				Environment.Exit(1);
			}

			//this code should be unreachable. This statement is simply to satisfy Eclipse.
			return null;
		}

		private static int[][] ParseStateString(string stateString, int[][] puzzleGrid, int pWidth)
		{
			//Parse state string converts the text file's format for each puzzle into
			// multidimensional arrays.

			//split the string by spaces
			string[] tileLocations = stateString.Split(" ", true);

			// the top-left corner of the puzzle has a coordinate of [0,0]
			int x = 0;
			int y = 0;

			for (int i = 0; i < tileLocations.Length; i++)
			{
				//tileLocations[i] holds the (i + 1)th tile
				int tileNumber = int.Parse(tileLocations[i]);

				//now, check the location of this tile
				if (x >= pWidth)
				{
					//reset x to 0 and go to next row (increase y by 1)
					x = 0;
					y++;
				}

				puzzleGrid[x][y] = tileNumber;
				x++;
			}

			return puzzleGrid;
		}
	}
}