using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PatternHypercubeService {
	public const int HYPERCUBES_COUNT = 10;
	public const int SYMBOLS_COUNT = HYPERCUBES_COUNT * 8 + 8 * 5;
	public const int COMMON_SYMBOLS_COUNT = 40;

	private static readonly int[] DEFAULT_COMMON_SYMBOLS = new[] {
		20, 21, 22, 23, 0, 1, 2, 10,
		11, 12, 13, 5, 7, 8, 15, 18,
		30, 113, 114, 33, 37, 45, 48, 52,
		57, 65, 66, 67, 73, 105, 107, 83,
		87, 92, 96, 97, 98, 100, 103, 104,
	};

	private static bool _init = false;
	private static int _seed;
	private static int[][][] _symbols;

	public static int[][] GetHypercube(int seed, int index) {
		if (!_init || _seed != seed) {
			_init = true;
			_seed = seed;
			MonoRandom rnd = new MonoRandom(seed);
			int[] commonSymbols = seed == 1 ? DEFAULT_COMMON_SYMBOLS : Shuffled(Enumerable.Range(0, SYMBOLS_COUNT), rnd).Take(COMMON_SYMBOLS_COUNT).ToArray();
			HashSet<int> commonSymbolsSet = new HashSet<int>(commonSymbols);
			int[] uncommonSymbols = Shuffled(Enumerable.Range(0, SYMBOLS_COUNT).Where(i => !commonSymbolsSet.Contains(i)), rnd);
			// Debug.Log(uncommonSymbols.Join(", "));
			_symbols = Enumerable.Repeat<int[][]>(null, HYPERCUBES_COUNT).ToArray();
			for (int hypercubeIndex = 0; hypercubeIndex < HYPERCUBES_COUNT; hypercubeIndex++) {
				// Debug.LogFormat("HC: {0}", hypercubeIndex);
				_symbols[hypercubeIndex] = Enumerable.Repeat<int[]>(null, 8).ToArray();
				int[] commons = Shuffled(commonSymbols, rnd);
				// Debug.Log(uncommonSymbols.Skip(hypercubeIndex * 8).Take(8).Join(", "));
				for (int hyperfaceIndex = 0; hyperfaceIndex < 8; hyperfaceIndex++) {
					IEnumerable<int> hyperfaceCommons = Enumerable.Range(hyperfaceIndex * 5, 5).Select(i => commons[i]);
					int uncommon = uncommonSymbols[hypercubeIndex * 8 + hyperfaceIndex];
					_symbols[hypercubeIndex][hyperfaceIndex] = Shuffled(hyperfaceCommons.Concat(new[] { uncommon }), rnd);
					// Debug.Log(_symbols[hypercubeIndex][hyperfaceIndex].Join(", "));
				}
			}
		}
		return _symbols[index];
	}

	public static int[][] GetPlaceholder() {
		return Enumerable.Range(0, 8).Select(hyperfaceIndex => Enumerable.Range(0, 6).Select(faceIndex => hyperfaceIndex * 6 + faceIndex).ToArray()).ToArray();
	}

	private static T[] Shuffled<T>(IEnumerable<T> arr, MonoRandom rnd) {
		return arr.OrderBy(_ => rnd.NextDouble()).ToArray();
	}
}
