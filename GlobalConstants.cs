using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSorter
{
    public static class GlobalConstants
    {
        public const int size_HashDownSample = 8;
        // 直方图参数
        public static ushort h_binSize = 256;
    }

    public enum E_SortType
    {
        /// <summary>
        /// 均值哈希
        /// </summary>
        AHash,
        /// <summary>
        /// 感知哈希
        /// </summary>
        PHash,
        /// <summary>
        /// 差值哈希
        /// </summary>
        DHash,
        /// <summary>
        /// 直方图
        /// </summary>
        HistogramCompare,
    }
}
