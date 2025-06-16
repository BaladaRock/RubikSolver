namespace RubikSolver.Helpers.Moves;

/// Use this record to define a move on the cube
public sealed record MovesOperations(
    byte[] CornerPermutations,
    byte[] CornerOrientations,
    byte[] EdgePermutations,
    byte[] EdgeOrientations
)
{
    private const byte CornerLength = 8;
    private const byte EdgeLength = 12;

    public static (byte[] cornerPerms, byte[] cornerOri, byte[] edgePerms, byte[] edgeOri)
        ApplyArrays(CubeState s, MovesOperations m)
    {
        var cornerPerms = new byte[CornerLength];
        var cornerOri = new byte[CornerLength];
        for (var i = 0; i < CornerLength; i++)
        {
            int src = m.CornerPermutations[i];
            cornerPerms[i] = s.CornerPermutations[src];
            cornerOri[i] = (byte)((s.CornerOrientations[src] + m.CornerOrientations[i]) % 3);
        }

        var edgePerms = new byte[EdgeLength];
        var edgeOri = new byte[EdgeLength];
        for (var i = 0; i < EdgeLength; i++)
        {
            int src = m.EdgePermutations[i];
            edgePerms[i] = s.EdgePermutations[src];
            edgeOri[i] = (byte)((s.EdgeOrientations[src] + m.EdgeOrientations[i]) % 2);
        }

        return (cornerPerms, cornerOri, edgePerms, edgeOri);
    }


    internal static MovesOperations ComposeMove(MovesOperations firstMove, MovesOperations secondMove)
    {
        return new MovesOperations(
            CornerPermutations: ComposePermutation(firstMove.CornerPermutations, secondMove.CornerPermutations),
            CornerOrientations: ComposeOrientation
                (firstMove.CornerOrientations, secondMove.CornerOrientations, secondMove.CornerPermutations, 3),
            EdgePermutations: ComposePermutation(firstMove.EdgePermutations, secondMove.EdgePermutations),
            EdgeOrientations: ComposeOrientation
                (firstMove.EdgeOrientations, secondMove.EdgeOrientations, secondMove.EdgePermutations, 2)
        );

        byte[] ComposeOrientation(byte[] firstOri, byte[] secondOri, byte[] secondPerm, int mod)
        {
            var configuration = new byte[firstOri.Length];
            for (var i = 0; i < configuration.Length; i++)
                configuration[i] = (byte)((firstOri[secondPerm[i]] + secondOri[i]) % mod);
            return configuration;
        }

        byte[] ComposePermutation(byte[] srcPerm, byte[] map)
        {
            var configuration = new byte[map.Length];
            for (var i = 0; i < configuration.Length; i++)
                configuration[i] = srcPerm[map[i]];
            return configuration;
        }
    }

}