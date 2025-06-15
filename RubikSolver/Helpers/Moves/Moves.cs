using System.Collections.Generic;

namespace RubikSolver.Helpers.Moves
{
    internal static class Moves
    {
        private static MovesOperations Pow(MovesOperations m, int n)
        {
            var r = m;
            for (var i = 1; i < n; i++)
                r = MovesOperations.ComposeMove(r, m);
            return r;
        }

        private static readonly MovesOperations U = MoveDefinitions.U;
        private static readonly MovesOperations R = MoveDefinitions.R;
        private static readonly MovesOperations F = MoveDefinitions.F;
        private static readonly MovesOperations D = MoveDefinitions.D;
        private static readonly MovesOperations L = MoveDefinitions.L;
        private static readonly MovesOperations B = MoveDefinitions.B;

        internal static readonly Dictionary<string, MovesOperations> FaceMove = new()
        {
            { "U",MoveDefinitions.U  }, { "U'", Pow(MoveDefinitions.U, 3) }, { "U2", Pow(MoveDefinitions.U, 2) },
            { "R",MoveDefinitions.R  }, { "R'", Pow(MoveDefinitions.R, 3) }, { "R2", Pow(MoveDefinitions.R, 2) },
            { "F",MoveDefinitions.F  }, { "F'", Pow(MoveDefinitions.F, 3) }, { "F2", Pow(MoveDefinitions.F, 2) },
            { "D",MoveDefinitions.D  }, { "D'", Pow(MoveDefinitions.D, 3) }, { "D2", Pow(MoveDefinitions.D, 2) },
            { "L",MoveDefinitions.L  }, { "L'", Pow(MoveDefinitions.L, 3) }, { "L2", Pow(MoveDefinitions.L, 2) },
            { "B",MoveDefinitions.B  }, { "B'", Pow(MoveDefinitions.B, 3) }, { "B2", Pow(MoveDefinitions.B, 2) }
        };
    }
}