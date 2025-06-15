using System;
using System.Collections.Generic;
using System.Text;

namespace RubikSolver.Helpers
{
    /// Our possible moves are defined in a dictionary, where the key is the move character (e.g., 'U', 'R', etc.)
    internal static class Moves
    {
        internal static readonly Dictionary<char, MoveDef> FaceMove = new()
        {
            { 'U', new MoveDef(
                CornerPermutations: [3, 0, 1, 2, 4, 5, 6, 7],
                CornerOrientations: new byte[8],
                EdgePermutations : [3, 0, 1, 2, 4, 5, 6, 7, 8, 9, 10, 11],
                EdgeOrientations: new byte[12])
            }
            
            // TO DO: Add other moves like 'D', 'L', 'R', 'F', 'B' with their respective definitions

        };
    }
}
