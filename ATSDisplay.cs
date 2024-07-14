using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace TatehamaATS
{
    internal class ATSDisplay
    {
        /// <summary>
        /// 表示器上段：種別
        /// </summary>
        private string L1;
        /// <summary>
        /// 表示器中段：速度
        /// </summary>
        private string L2;
        /// <summary>
        /// 表示器下段：動作状態
        /// </summary>
        private string[] L3;

        public ATSDisplay(string L1, string L2, string[] L3)
        {
            this.L1 = L1;
            this.L2 = L2;
            this.L3 = L3;
        }

        /// <summary>
        /// 状態から1要素増やす
        /// </summary>
        public void AddState(string addL3)
        {
            L3.Append(addL3);
        }

        /// <summary>
        /// 状態から1要素除く
        /// </summary>
        /// <param name="removeL3"></param>
        public void RemoveState(string removeL3)
        {
            var list = new List<string>();
            list.AddRange(L3);
            list.Remove(removeL3);
            L3 = list.ToArray();
        }
    }
}
