using System;
using System.Collections.Generic;
using System.Linq;
using RubikSolver.Helpers;

namespace RubikSolver.Solvers
{
    public sealed class Solver(CubeState state)
    {
        private CubeState _state = state;

        public Solver() : this(CubeState.Solved)
        {
        }

        // This method will be used as an API endpoint to apply a sequence of moves
        public void ApplyMoves(string alg)
        {
            foreach (var mv in Tokenize(alg))
                _state = Moves.FaceMove[mv].Apply(_state);
        }

        public CubeState GetCubeConfiguration() => _state with { };  // copy state to avoid external modifications

        public string? FindSolution(int maxDepth)
        {
            var visited = new HashSet<string>();
            var path = new List<char>();
            return Search(_state, maxDepth, visited, path)
                ? new string(path.ToArray())
                : null;
        }

        private static IEnumerable<char> Tokenize(string s) =>
            s.Replace(" ", "").ToUpperInvariant().Where(c => c != '\'' && c != '2');

        private static bool Search(CubeState st, int depth,
            HashSet<string> vis, List<char> path)
        {
            if (IsSolved(st)) return true;
            if (depth == 0) return false;

            var key = string.Join(",", st.CornerPermutations) + "|"
                 + string.Join(",", st.EdgePermutations) + "|"
                 + string.Join(",", st.CornerOrientations) + "|"
                 + string.Join(",", st.EdgeOrientations);

            if (!vis.Add(key)) return false;

            foreach (var move in Moves.FaceMove)
            {
                var mv = move.Key;
                var def = move.Value;
                var next = def.Apply(st);
                path.Add(mv);
                if (Search(next, depth - 1, vis, path)) return true;
                path.RemoveAt(path.Count - 1);
            }
            return false;
        }

        private static bool IsSolved(CubeState s) =>
            s.CornerPermutations.SequenceEqual(Enumerable.Range(0, 8).Select(i => (byte)i)) &&
            s.EdgePermutations.SequenceEqual(Enumerable.Range(0, 12).Select(i => (byte)i)) &&
            s.CornerOrientations.All(o => o == 0) &&
            s.EdgeOrientations.All(o => o == 0);
    }



    //public class Solver
    //{
    //    // Initial state of the puzzle represented as a byte array of length 42
    //    // we also need to store the pieces' orientation, so we will use the
    //    // concept used in blindfolded approaches, where we store each piece's sticker in an array
    //    private Dictionary<char, byte[]> _cubeConfiguration;

    //    private static readonly Dictionary<char, char[]> FaceInterDependencies = new()
    //    {
    //        { 'U', ['U', 'F', 'L', 'R', 'B'] },
    //        { 'R', ['R', 'U', 'F', 'B', 'D'] },
    //        { 'D', ['D', 'F', 'L', 'B', 'D'] },
    //        { 'L', ['R', 'U', 'F', 'B', 'D'] },
    //        { 'F', ['R', 'U', 'F', 'B', 'D'] },
    //        { 'B', ['R', 'U', 'F', 'B', 'D'] }
    //    };

    //    public Solver()
    //    {
    //        _cubeConfiguration = new Dictionary<char, byte[]>
    //        {
    //            { 'U', DefaultConfigurations.SolvedUFaceConfiguration },
    //            { 'R', DefaultConfigurations.SolvedRFaceConfiguration },
    //            { 'D', DefaultConfigurations.SolvedDFaceConfiguration },
    //            { 'L', DefaultConfigurations.SolvedLFaceConfiguration },
    //            { 'F', DefaultConfigurations.SolvedFFaceConfiguration },
    //            { 'B', DefaultConfigurations.SolvedBFaceConfiguration }
    //        };
    //    }

    //    // Constructor that accepts a custom cube configuration
    //    public Solver(byte[] perm20)
    //    {
    //        if (perm20.Length != 20)
    //            throw new ArgumentException("Configuration must have 20 elements.");
    //        _cubeConfiguration = perm20.ToArray(); // deep copy
    //    }

    //    public string? FindSolution(int maxDepth)
    //    {
    //        var visited = new HashSet<string>();
    //        var path = new List<char>();
    //        return Search(_cubeConfiguration, maxDepth, visited, path) 
    //            ? new string(path.ToArray())
    //            : null;
    //    }

    //    private static bool Search(byte[] state, int depth, HashSet<string> visited, List<char> path)
    //    {
    //        if (IsSolved(state))
    //            return true;

    //        if (depth == 0)
    //            return false;

    //        var stateKey = string.Join(",", state);
    //        if (!visited.Add(stateKey))
    //            return false;

    //        foreach (var move in Moves)
    //        {
    //            var newState = ApplyMove(state, move.Value);
    //            path.Add(move.Key);
    //            if (Search(newState, depth - 1, visited, path))
    //                return true;
    //            path.RemoveAt(path.Count - 1); // backtrack
    //        }

    //        return false;
    //    }

    //    private static bool IsSolved(byte[] state)
    //    {
    //        for (var i = 0; i < 20; i++)
    //            if (state[i] != i)
    //                return false;
    //        return true;
    //    }

    //    public byte[] ApplyMoves(string move)
    //    {
    //        var result = new byte[20];
    //        for (var i = 0; i < 20; i++)
    //            result[i] = _cubeConfiguration[movePerm[i]];
    //        return result;
    //    }

    //    public Dictionary<char, byte[]> GetCurrentCubeConfiguration()
    //    {
    //        return _cubeConfiguration;
    //    }
    //}

}
