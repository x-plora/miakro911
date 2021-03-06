﻿using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class Vaccine
    {
        public const int MAX_VACS_COUNT = 127;
        public const int V_ID_LUST = -1;

        public readonly int ID=0;
        public string Name;
        public int Duration = 0;
        public int Age = 0;
        public int After = 0;
        public bool Zoo = true;
        public int DoTimes = 0;

        public Vaccine(int id,string name,int duration,int age,int after,bool zoo,int times)
        {
            this.ID = id;
            this.Name = name;
            this.Duration = duration;
            this.Age = age;
            this.After = after;
            this.Zoo = zoo;
            this.DoTimes = times;
        }
    }
}
