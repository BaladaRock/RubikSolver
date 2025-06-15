namespace RubikSolver.Helpers;

/// Use this record to define a move on the cube
internal sealed record MoveDef(
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
}