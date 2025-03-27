using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSorter
{
    public class DistanceComparer : IComparer<PictureBox_Advance>
    {
        int IComparer<PictureBox_Advance>.Compare(PictureBox_Advance? x, PictureBox_Advance? y)
        {
            return  x.img_data.distance.CompareTo(y.img_data.distance);
        }
    }
}
