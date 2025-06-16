using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RubikSolver.Helpers;
using RubikSolver.Helpers.Moves;

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
                .Cast<Match>()
                .Select(m => m.Value);

            foreach (var tok in tokens)
            {
                _state =  _state.Apply(Moves.FaceMove[tok]);
            }
        }

        public CubeState GetCubeConfiguration() => _state with { };  // copy state to avoid external modifications

        public string? FindSolution(int movesCount)
        {
            var path = new List<string>();
            return SearchExact(_state, movesCount, path)
                ? CompressMoves(path)
                : null;
        }

        public IEnumerable<string> FindSolutions(int maxMoves)
        {
            var foundSolutions = new HashSet<string>();

            for (var len = 1; len <= maxMoves; len++)
            {
                var path = new List<string>();
                foreach (var sol in EnumerateExact(_state, len, path))
                {
                    if (foundSolutions.Add(sol))
                    {
                        yield return sol;
                    }
                }
            }
        }

        public string? FindShortestSolution(int maxMoves)
        {
            for (var len = 1; len <= maxMoves; len++)
            {
                var sol = FindSolution(len);
                if (sol != null) return sol;
            }

            return null;
        }

        private static bool SearchExact(CubeState st, int left, List<string> path)
        {
            if (left == 0)
                return IsSolved(st);

            foreach (var kv in Moves.FaceMove)
            {
                var next = st.Apply(kv.Value);
                path.Add(kv.Key);
                if (SearchExact(next, left - 1, path))
                    return true;
                path.RemoveAt(path.Count - 1);
            }
            return false;
        }

        private static IEnumerable<string> EnumerateExact(CubeState st, int left, List<string> path)
        {
            if (left == 0)
            {
                if (IsSolved(st))
                    yield return CompressMoves(path);
                yield break;
            }

            foreach (var kv in Moves.FaceMove)
            {
                path.Add(kv.Key);
                foreach (var s in EnumerateExact(st.Apply(kv.Value), left - 1, path))
                    yield return s;
                path.RemoveAt(path.Count - 1);
            }
        }

        private static string CompressMoves(IReadOnlyList<string> src)
        {
            var sb = new System.Text.StringBuilder();
            var i = 0;
            while (i < src.Count)
            {
                var face = src[i];
                // if the Face move is already compressed (e.g., "U2", "R'", etc.),
                // do not compress it further
                if (face.Length > 1)
                {
                    sb.Append(face);
                    if (++i < src.Count) sb.Append(' ');
                    continue;
                }

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
                i += run;
                if (i < src.Count) sb.Append(' ');
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

                var next = st.Apply(def);
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
