namespace ZXY
{
    public class Point
    {
        private string name;
        public string Name
        {
            get => name;
            set => name = value;
        }

        private string code;
        public string Code
        {
            get => code;
            set => code = value;
        }

        private double x;
        public double X
        {
            get => x;
            set => x = value;
        }

        private double y;
        public double Y
        {
            get => y;
            set => y = value;
        }

        private double z;
        public double Z
        {
            get => z;
            set => z = value;
        }


        public override string ToString()
        {
            return $"{Name}, {Code}, {X}, {Y}, {Z}";
        }

    }
}
