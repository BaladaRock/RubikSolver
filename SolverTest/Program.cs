using RubikSolver.Helpers;
using RubikSolver.Solvers;
using System.Linq;

namespace SolverTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const string moves = "U B";

            var solver = new Solver();
            PrintConfiguration(moves, CubeState.Solved);

            // Test applying moves
            solver.ApplyMoves(moves);

            var currentConfiguration = solver.GetCubeConfiguration();
            PrintConfiguration(moves, currentConfiguration, true);

            // Test finding solution
            var solution = solver.FindSolution(1);
            if (solution != null)
            {
                Console.WriteLine("Solution found:");
                Console.WriteLine(solution);
            }
            else
            {
                Console.WriteLine("No solution found within the given depth.");
            }

            // Test finding solutions up to a given length
            var solutions = solver.FindSolutions(6);
            if (solutions == null || !solutions.Any())
            {
                Console.WriteLine($"No solution found!");
            }

            foreach (var sol in solutions)
            {
                Console.WriteLine($"Solution found: {sol}.");
            }

            // Test finding of the shortest solution up to a given length
            var shortestSolution = solver.FindShortestSolution(5);
            Console.WriteLine($"Shortest solution: {shortestSolution}");


            Console.ReadKey();
        }

        private static void PrintConfiguration(string appliedMoves, CubeState currentConfiguration, bool printMoves = false)
        {
            Console.WriteLine(printMoves ? $"Applied moves: {appliedMoves}" : "Default configuration: ");

            Console.WriteLine("CornerPermutation state: " + string.Join(' ', currentConfiguration.CornerPermutations));
            Console.WriteLine("CornerOrientation state: " + string.Join(' ', currentConfiguration.CornerOrientations));
            Console.WriteLine("EdgePermutation state: " + string.Join(' ', currentConfiguration.EdgePermutations));
            Console.WriteLine("EdgeOrientation state: " + string.Join(' ', currentConfiguration.EdgeOrientations));
            Console.WriteLine("");
        }
    }
}
