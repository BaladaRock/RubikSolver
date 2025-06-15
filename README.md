# CubeSolver DLL

A lightweight .NET class‑library that computes solution algorithms for a 3×3×3 Rubik’s Cube.
The project is part of my final thesis and is designed to be embedded in a Unity application (visualiser / trainer), but can be reused in any C# environment.

---

## 1. Motivation & Scope

* **Educational** – demonstrates search techniques (backtracking, heuristic pruning, future IDA\*) on a well‑known combinatorial puzzle.
* **Modular** – the solver is delivered as a clean *DLL* (`RubikSolver.dll`) with a minimal public API (`Solver`, `CubeState`) that hides internal details from the host (Unity).
* **Extensible** – the current implementation is deliberately simple (depth‑limited recursion); the scope is to find **all possible solutions** of a given length. Later extensions will limit the moves that can be applied, allowing the speedcuber to customise their desired algorithms.

---

## 2. Internal Cube Representation

| Component            | Count | Range | Meaning                      |
| -------------------- | :---: | :---: | ---------------------------- |
| `CornerPermutations` |   8   |  0‑7  | which corner is in each slot |
| `CornerOrientations` |   8   |  0‑2  | twist of each corner         |
| `EdgePermutations`   |   12  |  0‑11 | which edge is in each slot   |
| `EdgeOrientations`   |   12  |  0‑1  | flip of each edge            |

All four arrays are stored in an **immutable** record:

```csharp
public sealed record CubeState(
    byte[] CornerPermutations,
    byte[] CornerOrientations,
    byte[] EdgePermutations,
    byte[] EdgeOrientations
);
```

### Solved state

```
CornerPermutations = 0 1 2 3 4 5 6 7
CornerOrientations = 0 0 0 0 0 0 0 0
EdgePermutations   = 0 1 2 3 4 5 6 7 8 9 10 11
EdgeOrientations   = 0 0 0 0 0 0 0 0 0 0 0 0
```

---

## 3. Internal Representation of the Cube

Each face move is hard‑coded as four parallel look‑up tables (permutation + orientation delta).
For example, with the competition convention used in this project **`U` rotates the top layer clockwise when the solver views the cube from the front**:

```csharp
// URF → UFL → ULB → UBR
MoveDef U = new(
    new byte[]{ 3,0,1,2, 4,5,6,7 },   // corners
    new byte[8],                      // no corner twist on U
    new byte[]{ 3,0,1,2, 4,5,6,7, 8,9,10,11 }, // edges
    new byte[12]                      // no edge flip on U
);
```

---

## 4. Algorithmic Core

| Step | Description                                                                              |
| :--: | ---------------------------------------------------------------------------------------- |
|   1  | Depth‑limited DFS / backtracking (Iterative Deepening supported).                        |
|   2  | Visited‑state hash (all 40 values) to avoid loops.                                       |
|   3  | Optional pruning on last‑move repetition (cancellation for moves like `UU'`, `RR` etc.). |

**Complexity**: brute‑force, worst‑case *O(b^d)*; practical up to depth ≈ 8–10 (used for demo and unit tests).
In the thesis chapter *Future Work* we outline IDA\* + pattern databases that reach 20–22 moves optimally.

---

## 5. Public API

```csharp
using RubikSolver;

var solver = new Solver();              // solved cube
solver.ApplyMoves("U R U");             // any algorithm string
CubeState scrambled = solver.GetCubeConfiguration();

string? sol = solver.FindSolution(maxDepth: 8);
Console.WriteLine(sol ?? "No solution in chosen depth");
```

---

## 6. Build & Test

| Task            | Command / Setting                                      |
| --------------- | ------------------------------------------------------ |
| **Compile DLL** | `dotnet build -c Release` (targets **netstandard2.0**) |
| **Run demo**    | Start project **SolverTest** (see `src/SolverTest`)    |
| **IDE**         | Visual Studio 2022 / Rider (**C# 9** `LangVersion`)    |

Unity ≥ 2020.3 can load **netstandard2.0** DLLs directly; copy the compiled `RubikSolver.dll` into `Assets/Plugins`.

---

## 7. Licensing & Credits

* **Licence:** MIT – free for educational and commercial use.
* **Acknowledgements:** move tables and indexing scheme follow the Singmaster notation adapted by Thistlethwaite and Kociemba. Inspiration taken from *Cube Explorer* and **cubing.js** open‑source projects.

---
