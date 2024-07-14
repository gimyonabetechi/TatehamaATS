using System;
using System.Collections.Generic;
using System.Linq;

namespace ATSCommon
{
    internal class Pattern
    {
        /// <summary>
        /// 目標距離
        /// </summary>
        private double TargetDistance;
        /// <summary>
        /// 目標速度
        /// </summary>
        private double TargetSpeed;
        /// <summary>
        /// 減速度
        /// </summary>
        private double Deceleration;
        /// <summary>
        /// 空走時間
        /// </summary>
        private double IdleTime;

        /// <summary>
        /// 勾配情報リスト
        /// </summary>
        private List<List<double>> GradientInfo;

        /// <summary>
        /// パターン線
        /// </summary>
        private Dictionary<double, double> SpeedProfile = new Dictionary<double, double>();

        /// <summary>
        /// 並べ替え済みキー
        /// </summary>
        private List<double> SortedKeys;

        /// <summary>
        /// パターンを発生させます。
        /// </summary>
        /// <param name="targetDistance">目標距離</param>
        /// <param name="targetSpeed">目標速度</param>
        /// <param name="deceleration">減速度</param>
        /// <param name="idleTime">空走時間</param>        
        /// <param name="gradientInfo">勾配情報リスト</param>
        public Pattern(double targetDistance, double targetSpeed, double deceleration, double idleTime, List<List<double>> gradientInfo = null)
        {
            TargetDistance = targetDistance;
            TargetSpeed = targetSpeed;
            Deceleration = deceleration;
            IdleTime = idleTime;
            GradientInfo = gradientInfo ?? new List<List<double>>();
            GenerateSpeedProfile();
            SortedKeys = SpeedProfile.Keys.ToList();
            SortedKeys.Sort();
        }

        private void GenerateSpeedProfile()
        {
            double currentDistance = TargetDistance;
            while (currentDistance >= 0 && TargetDistance - currentDistance <= 3000)
            {
                double interval = GetInterval(TargetDistance - currentDistance);
                double speed = CalculateSpeedAtDistance(currentDistance);
                double shiftedDistance = currentDistance - (speed * IdleTime / 3.6);  // 空走時間を考慮した位置シフト計算
                if (shiftedDistance >= 0)  // シフトした位置が負の値にならないようにする
                {
                    SpeedProfile[shiftedDistance] = speed;
                }
                currentDistance -= interval;
            }
        }


        private double GetInterval(double remainingDistance)
        {
            if (remainingDistance < 50) return 0.05;
            else if (remainingDistance < 100) return 0.1;
            else if (remainingDistance < 200) return 0.5;
            else if (remainingDistance < 300) return 1;
            else if (remainingDistance < 500) return 2;
            else if (remainingDistance < 1000) return 5;
            else if (remainingDistance < 2000) return 10;
            else return 20;
        }

        private double CalculateSpeedAtDistance(double distance)
        {
            var firstGradientInfo = GradientInfo.Where(g => distance >= g[0]).OrderByDescending(g => g[0]).FirstOrDefault();
            double gradient = firstGradientInfo != null ? firstGradientInfo[1] : 0;
            return CalculateSpeedWithGradient(TargetSpeed, TargetDistance, Deceleration, distance, gradient);
        }

        public double PatternSpeed(double currentDistance)
        {
            if (SortedKeys.Count == 0)
            {
                return TargetSpeed;  // リストが空の場合はデフォルト速度を返す
            }

            int index = SortedKeys.BinarySearch(currentDistance);
            if (index >= 0)
            {
                return SpeedProfile[SortedKeys[index]];  // 正確に一致するキーが見つかった場合
            }

            int insertionIndex = ~index;  // 挿入位置（現在の距離の直後のキーの位置）
            if (insertionIndex == 0)
            {
                return SpeedProfile[SortedKeys[0]];  // 最初のキー未満の場合、最初のキーの速度を返す
            }
            else if (insertionIndex >= SortedKeys.Count)
            {
                return SpeedProfile[SortedKeys[SortedKeys.Count - 1]];  // 最後のキーを超える場合、最後のキーの速度を返す
            }

            // 線形補間を行う
            double keyBefore = SortedKeys[insertionIndex - 1];
            double keyAfter = SortedKeys[insertionIndex];
            double speedBefore = SpeedProfile[keyBefore];
            double speedAfter = SpeedProfile[keyAfter];

            // 距離の差による補間係数を計算
            double fraction = (currentDistance - keyBefore) / (keyAfter - keyBefore);
            return speedBefore + fraction * (speedAfter - speedBefore);
        }



        private double CalculateSpeedWithGradient(double targetSpeed, double toDistance, double deceleration, double forDistance, double gradient)
        {
            double X = 31;
            double S = toDistance - forDistance;
            double beta = deceleration;
            double G = gradient;
            double V2 = targetSpeed;
            double Y = beta + (G / X);
            double speedSquared = 7.2 * S * Y + Math.Pow(V2, 2);

            return Double.IsNaN(speedSquared) || speedSquared < 0 ? -30 : Math.Sqrt(speedSquared);
        }

        public override string ToString() => $"(目標位置:{TargetDistance},目標速度:{TargetSpeed})";
    }
}
