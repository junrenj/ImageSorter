namespace ImageSorter
{
    public static class GlobalConstants
    {
        public const int size_HashDownSample = 8;
        // 直方图参数
        public static ushort h_binSize = 32;
    }

    public enum E_ImgSize
    {
        S_512,
        S_1024,
        S_2048,
        Others,
        All
    }

    public enum E_SortType
    {
        /// <summary>
        /// 均值哈希
        /// </summary>
        AHash,
        /// <summary>
        /// 差值哈希
        /// </summary>
        DHash,
        /// <summary>
        /// 直方图
        /// </summary>
        HistogramCompare,
        /// <summary>
        /// 结构相似性指数
        /// </summary>
        SSIM,
        /// <summary>
        /// 直方图移动距离
        /// </summary>
        EMD,
    }

    public enum E_ExecuteType
    {
        /// <summary>
        /// 删除目前显示的所有图片
        /// </summary>
        DeleteAll,
        /// <summary>
        /// 删除现在所选择的图片
        /// </summary>
        DeleteSelect,
        /// <summary>
        /// 基于目标图片批量重命名所有显示中的图片
        /// </summary>
        RenameAllBaseOnTarget,
        /// <summary>
        /// 基于目标图片批量重命名所有选择的图片
        /// </summary>
        RenameSelectBaseOnTarget
    }

    public enum E_ExportType
    {
        /// <summary>
        /// 导出目前显示的所有图片
        /// </summary>
        ExportAll,
        /// <summary>
        /// 导出现在选择的图片
        /// </summary>
        ExportSelect,
    }
}
