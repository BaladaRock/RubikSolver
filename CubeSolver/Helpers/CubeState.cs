using CubeSolver.Helpers.Moves;
using System.Linq;

namespace CubeSolver.Helpers;

public sealed record CubeState(
    byte[] CornerPermutations, // 8 values values between 0-7
    byte[] CornerOrientations,  // 8 values (0, 1 or 2)  
    byte[] EdgePermutations,   // 12 values between 0-11
    byte[] EdgeOrientations     // 12 values (0 or 1)  
)
{
    // / <summary>
    /// The default solved state of the cube.
    // / <summary>
    public static CubeState Solved =>
        new(
            Enumerable.Range(0, 8).Select(i => (byte)i).ToArray(),
            new byte[8],
            Enumerable.Range(0, 12).Select(i => (byte)i).ToArray(),
            new byte[12]);

    public CubeState Apply(MovesOperations m)
    {
        var (ncPerm, ncOri, nePerm, neOri) = MovesOperations.ApplyArrays(this, m);

        return new CubeState(
            CornerPermutations: ncPerm,
            CornerOrientations: ncOri,
            EdgePermutations: nePerm,
            EdgeOrientations: neOri);
    }

}