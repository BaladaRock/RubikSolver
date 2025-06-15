using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace RubikSolver.Helpers.Moves
{
    /// <summary>
    ///  The move definitions for the Rubik's Cube.
    /// Used the Kociemba scheme for piece labeling.
    /// </summary>
    internal static class MoveDefinitions
    {
        private static readonly byte[] CornersDefaultOrientation = new byte[8];   // 8 × 0
        private static readonly byte[] EdgesDefaultOrientation = new byte[12];  // 12 × 0

        public static readonly MovesOperations U = new(
                CornerPermutations: [3, 0, 1, 2, 4, 5, 6, 7],
                CornerOrientations: CornersDefaultOrientation,
                EdgePermutations: [3, 0, 1, 2, 4, 5, 6, 7, 8, 9, 10, 11],
                EdgeOrientations: EdgesDefaultOrientation);

        public static readonly MovesOperations D = new(
            CornerPermutations: [0, 1, 2, 3, 5, 6, 7, 4],
            CornerOrientations: CornersDefaultOrientation,
            EdgePermutations: [0, 1, 2, 3, 5, 6, 7, 4, 8, 9, 10, 11],
            EdgeOrientations: EdgesDefaultOrientation);

        public static readonly MovesOperations R = new(
            CornerPermutations: [4, 1, 2, 0, 7, 5, 6, 3],
            CornerOrientations: [2, 0, 0, 1, 1, 0, 0, 2],
            EdgePermutations: [8, 1, 2, 3, 11, 5, 6, 7, 4, 9, 10, 0],
            EdgeOrientations: EdgesDefaultOrientation);

        public static readonly MovesOperations L = new(
            CornerPermutations: [0, 2, 6, 3, 4, 1, 5, 7],
            CornerOrientations: [0, 1, 2, 0, 0, 2, 1, 0],
            EdgePermutations: [0, 1, 10, 3, 4, 5, 9, 7, 8, 2, 6, 11],
            EdgeOrientations: EdgesDefaultOrientation);

        public static readonly MovesOperations F = new(
            CornerPermutations: [1, 5, 2, 3, 0, 4, 6, 7],
            CornerOrientations: [1, 2, 0, 0, 2, 1, 0, 0],
            EdgePermutations: [0, 9, 2, 3, 4, 8, 6, 7, 1, 5, 10, 11],
            EdgeOrientations: [0, 1, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0]);

        public static readonly MovesOperations B = new(
            CornerPermutations: [0, 1, 3, 7, 4, 5, 2, 6],
            CornerOrientations: [0, 0, 2, 1, 0, 0, 2, 1],
            EdgePermutations: [0, 1, 2, 10, 4, 5, 6, 11, 8, 9, 3, 7],
            EdgeOrientations: [0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 1]);
    }
}
