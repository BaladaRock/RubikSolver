using RubikSolver.Helpers;
using RubikSolver.Solvers;

namespace SolverTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const string moves = "U";

            var solver = new Solver();
            PrintConfiguration(moves, CubeState.Solved);

            // Test applying moves
            solver.ApplyMoves(moves);

            var currentConfiguration = solver.GetCubeConfiguration();
            PrintConfiguration(moves, currentConfiguration, true);

            // Test finding solution
            var solution = solver.FindSolution(4);
            if (solution != null)
            {
                Console.WriteLine("Solution found:");
                Console.WriteLine(solution);
            }
            else
            {
                Console.WriteLine("No solution found within the given depth.");
            }

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
