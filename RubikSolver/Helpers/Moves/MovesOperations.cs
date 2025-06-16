namespace RubikSolver.Helpers.Moves;

/// Use this record to define a move on the cube
internal sealed record MovesOperations(
    byte[] CornerPermutations,
    byte[] CornerOrientations,
    byte[] EdgePermutations,
    byte[] EdgeOrientations
)
{
    public CubeState Apply(CubeState src)
    {
        return new CubeState(
            Permute(src.CornerPermutations, CornerPermutations),
            AddMod(src.CornerOrientations, CornerOrientations, 3),
            Permute(src.EdgePermutations, EdgePermutations),
            Xor(src.EdgeOrientations, EdgeOrientations));

        byte[] AddMod(byte[] v, byte[] delta, int mod)
        {
            var r = new byte[v.Length];
            for (var i = 0; i < v.Length; i++)
                r[i] = (byte)((v[i] + delta[i]) % mod);
            return r;
        }

        byte[] Xor(byte[] v, byte[] mask)
        {
            var r = new byte[v.Length];
            for (var i = 0; i < v.Length; i++) r[i] = (byte)(v[i] ^ mask[i]);
            return r;
        }

        byte[] Permute(byte[] v, byte[] map)
        {
            var r = new byte[map.Length];
            for (var i = 0; i < map.Length; i++) r[i] = v[map[i]];
            return r;
        }
    }

    internal static MovesOperations ComposeMove(MovesOperations a, MovesOperations b)
    {
        return new MovesOperations(
            CornerPermutations: ComposePermutation(a.CornerPermutations, b.CornerPermutations),
            CornerOrientations: ComposeOrientation
                (a.CornerOrientations, b.CornerOrientations, b.CornerPermutations, 3),
            EdgePermutations: ComposePermutation(a.EdgePermutations, b.EdgePermutations),
            EdgeOrientations: ComposeOrientation
                (a.EdgeOrientations, b.EdgeOrientations, b.EdgePermutations, 2)
        );

        byte[] ComposeOrientation(byte[] firstOri, byte[] secondOri, byte[] secondPerm, int mod)
        {
            var r = new byte[firstOri.Length];
            for (var i = 0; i < r.Length; i++)
                r[i] = (byte)((firstOri[secondPerm[i]] + secondOri[i]) % mod);
            return r;
        }

        byte[] ComposePermutation(byte[] srcPerm, byte[] map)
        {
            var r = new byte[map.Length];
            for (var i = 0; i < r.Length; i++)
                r[i] = srcPerm[map[i]];
            return r;
        }
    }

}