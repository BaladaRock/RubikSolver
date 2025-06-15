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
            { "U" , U  }, { "U'", Pow(U,3) }, { "U2", Pow(U,2) },
            { "R" , R  }, { "R'", Pow(R,3) }, { "R2", Pow(R,2) },
            { "F" , F  }, { "F'", Pow(F,3) }, { "F2", Pow(F,2) },
            { "D" , D  }, { "D'", Pow(D,3) }, { "D2", Pow(D,2) },
            { "L" , L  }, { "L'", Pow(L,3) }, { "L2", Pow(L,2) },
            { "B" , B  }, { "B'", Pow(B,3) }, { "B2", Pow(B,2) }
        };
    }
}