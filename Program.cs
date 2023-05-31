using System.Diagnostics.CodeAnalysis;

namespace DesignUndergroundSystem
{
	internal class Program
	{
		public class UndergroundSystem
		{
			private class StationInfo
			{
				public readonly string stationName;
				public readonly int time;

				public StationInfo(string stationName, int time)
				{
					this.stationName = stationName;
					this.time = time;
				}
			}

			private class StationDirection
			{
				public readonly string startStation;
				public readonly string endStation;

				public StationDirection(string startStation, string endStation)
				{
					this.startStation = startStation;
					this.endStation = endStation;
				}
			}

			private class StationDirectionEquality : IEqualityComparer<StationDirection>
			{
				public bool Equals(StationDirection? x, StationDirection? y)
				{
					return x?.startStation == y?.startStation && x?.endStation == y?.endStation;
				}

				public int GetHashCode([DisallowNull] StationDirection obj)
				{
					return (obj.startStation.GetHashCode() + obj.endStation.GetHashCode()).GetHashCode();
				}
			}


			private readonly Dictionary<int, StationInfo> checkInMap;
			private readonly Dictionary<StationDirection, double[]> totalMap;
			public UndergroundSystem()
			{
				checkInMap = new Dictionary<int, StationInfo>();
				totalMap = new Dictionary<StationDirection, double[]>(new StationDirectionEquality());
			}

			public void CheckIn(int id, string stationName, int t)
			{
				checkInMap[id] = new StationInfo(stationName, t);
			}

			public void CheckOut(int id, string stationName, int t)
			{
				StationInfo stationInfo = checkInMap[id];
				StationDirection stationDirection = new(stationInfo.stationName, stationName);
				if (!totalMap.ContainsKey(stationDirection))
				{
					totalMap[stationDirection] = new double[] { 0, 0 };
				}
				totalMap[stationDirection][0] += t - stationInfo.time;
				totalMap[stationDirection][1] += 1;
			}

			public double GetAverageTime(string startStation, string endStation)
			{
				StationDirection stationDirection = new(startStation, endStation);
				double totalTime = totalMap[stationDirection][0];
				double count = totalMap[stationDirection][1];
				return totalTime / count;
			}
		}

		static void Main(string[] args)
		{
			UndergroundSystem undergroundSystem1 = new();
			undergroundSystem1.CheckIn(45, "Leyton", 3);
			undergroundSystem1.CheckIn(32, "Paradise", 8);
			undergroundSystem1.CheckIn(27, "Leyton", 10);
			undergroundSystem1.CheckOut(45, "Waterloo", 15);  // Customer 45 "Leyton" -> "Waterloo" in 15-3 = 12
			undergroundSystem1.CheckOut(27, "Waterloo", 20);  // Customer 27 "Leyton" -> "Waterloo" in 20-10 = 10
			undergroundSystem1.CheckOut(32, "Cambridge", 22); // Customer 32 "Paradise" -> "Cambridge" in 22-8 = 14
            Console.WriteLine(undergroundSystem1.GetAverageTime("Paradise", "Cambridge")); // return 14.00000. One trip "Paradise" -> "Cambridge", (14) / 1 = 14
            Console.WriteLine(undergroundSystem1.GetAverageTime("Leyton", "Waterloo"));    // return 11.00000. Two trips "Leyton" -> "Waterloo", (10 + 12) / 2 = 11
            undergroundSystem1.CheckIn(10, "Leyton", 24);
			Console.WriteLine(undergroundSystem1.GetAverageTime("Leyton", "Waterloo"));    // return 11.00000
			undergroundSystem1.CheckOut(10, "Waterloo", 38);  // Customer 10 "Leyton" -> "Waterloo" in 38-24 = 14
			Console.WriteLine(undergroundSystem1.GetAverageTime("Leyton", "Waterloo"));    // return 12.00000. Three trips "Leyton" -> "Waterloo", (10 + 12 + 14) / 3 = 12
			Console.WriteLine("============");
			UndergroundSystem undergroundSystem2 = new();
			undergroundSystem2.CheckIn(10, "Leyton", 3);
			undergroundSystem2.CheckOut(10, "Paradise", 8); // Customer 10 "Leyton" -> "Paradise" in 8-3 = 5
            Console.WriteLine(undergroundSystem2.GetAverageTime("Leyton", "Paradise")); // return 5.00000, (5) / 1 = 5
            undergroundSystem2.CheckIn(5, "Leyton", 10);
			undergroundSystem2.CheckOut(5, "Paradise", 16); // Customer 5 "Leyton" -> "Paradise" in 16-10 = 6
            Console.WriteLine(undergroundSystem2.GetAverageTime("Leyton", "Paradise")); // return 5.50000, (5 + 6) / 2 = 5.5
            undergroundSystem2.CheckIn(2, "Leyton", 21);
			undergroundSystem2.CheckOut(2, "Paradise", 30); // Customer 2 "Leyton" -> "Paradise" in 30-21 = 9
            Console.WriteLine(undergroundSystem2.GetAverageTime("Leyton", "Paradise")); // return 6.66667, (5 + 6 + 9) / 3 = 6.66667
        }
	}
}