using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXY
{
    public class GPoint : NotificationObject
    {
        private string name;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        private double x;

        public double X
        {
            get => x;
            set
            {
                x = value;
                RaisePropertyChanged("X");
            }
        }

        private double y;

        public double Y
        {
            get => y;
            set
            {
                y = value;
                RaisePropertyChanged("Y");
            }
        }

        private double z;

        public double Z
        {
            get => z;
            set
            {
                z = value;
                RaisePropertyChanged("Z");
            }
        }

    }
}
