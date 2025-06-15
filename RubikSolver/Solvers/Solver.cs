using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RubikSolver.Helpers;

<<<<<<< TODO: Unmerged change from project 'RubikSolver (netstandard2.0)', Before:
=======
using RubikSolver.Helpers.Moves;
using RubikSolver.Helpers.Moves.Moves;
>>>>>>> After

<<<<<<< TODO: Unmerged change from project 'RubikSolver (net8.0)', Before:
=======
using RubikSolver.Helpers.Moves;
>>>>>>> After
using RubikSolver.Helpers.Moves;
using RubikSolver.Helpers.Moves.Moves;
using RubikSolver.Helpers.Moves.Moves.Moves;

namespace RubikSolver.Solvers
{
    public sealed class Solver(CubeState state)
    {
        private CubeState _state = state;

        public Solver() : this(CubeState.Solved)
        {
        }

        // This method will be used as an API endpoint to apply a sequence of moves
        public void ApplyMoves(string movesSequence)
        {
            var tokens = Regex.Matches(movesSequence.ToUpperInvariant(), @"[URFDLB]['2]?")
                .Cast<Match>()         // NU îl scoate
                .Select(m => m.Value); // "U", "U'", "U2" 

            foreach (var tok in tokens)
            {
                _state = Moves.FaceMove[tok].Apply(_state);
            }
        }

        public CubeState GetCubeConfiguration() => _state with { };  // copy state to avoid external modifications

        public string? FindSolution(int maxDepth)
        {
            var visited = new HashSet<string>();
            var path = new List<string>();

            return Search(_state, maxDepth, visited, path)
                ? CompressPath(path)
                : null;
        }

        private static string CompressPath(IReadOnlyList<string> src)
        {
            var sb = new System.Text.StringBuilder();
            var i = 0;
            while (i < src.Count)
            {
                var face = src[i];     
                var run = 1;
                while (i + run < src.Count && src[i + run] == face)
                {
                    run++;
                }

                switch (run % 4)
                {
                    case 1: sb.Append(face); break;    
                    case 2: sb.Append(face).Append('2'); break;       
                    case 3: sb.Append(face).Append('\''); break; 
                }
                if (i + run < src.Count) sb.Append(' ');
                i += run;
            }
            return sb.ToString();
        }

        private static bool Search(CubeState st, int depth,
            HashSet<string> visitedConfigurations, List<string> path)
        {
            if (IsSolved(st)) return true;
            if (depth == 0) return false;

            var key = string.Join(",", st.CornerPermutations) + "|"
                 + string.Join(",", st.EdgePermutations) + "|"
                 + string.Join(",", st.CornerOrientations) + "|"
                 + string.Join(",", st.EdgeOrientations);

            if (!visitedConfigurations.Add(key)) return false;

            foreach (var kv in Moves.FaceMove)
            {
                var mv = kv.Key;
                var def = kv.Value;

                var next = def.Apply(st);
                path.Add(mv);
                if (Search(next, depth - 1, visitedConfigurations, path)) return true;
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

}
