using ImageSorter.Data;

namespace ImageSorter.Logic
{
    public class DistanceComparer : IComparer<PictureBox_Advance>
    {
        int IComparer<PictureBox_Advance>.Compare(PictureBox_Advance? x, PictureBox_Advance? y)
        {
            return x.img_data.distance.CompareTo(y.img_data.distance);
        }
    }
}
