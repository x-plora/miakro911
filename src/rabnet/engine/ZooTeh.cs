﻿using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public enum JobType {NONE, OKROL, VUDVOR, COUNT_KIDS };
    
    public class ZootehJob:IData
    {
        public JobType type = JobType.OKROL;
//        public String options = "";
        public int days=0;
        public string job="";
        public string address="";
        public string name="";
        public int age = 0;
        public string comment = "";
        public string names="";
        public string addresses="";
        public int id=0;
        public int id2;
        public ZootehJob()
        {
        }
        public ZootehJob(JobType type)
        {
            this.type = type;
        }
        public ZootehJob Okrol(int id,String nm,String ad,int stat,int age,int srok)
        {
            type=JobType.OKROL; job="Принять окрол";
            days = srok; name = nm; address = ad;
            this.age = age; this.id = id;
            comment = "окрол " + stat.ToString();
            return this;
        }
        public ZootehJob Vudvor(int id,String nm,String ad,int stat,int age,int srok,int id2)
        {
            type = JobType.VUDVOR; job = "Выдворение";
            days = srok; name = nm; address = ad;
            this.age = age; this.id = id;
            this.id2=id2;
            return this;
        }
        public ZootehJob Counts(int id, String nm, String ad, int age)
        {
            type = JobType.COUNT_KIDS; job = "Подсчет гнездовых";
            days = 0; name = nm; address = ad;
            this.age = age; this.id = id;
            comment = "возраст " + age.ToString();
            return this;
        }
    }

    public class JobHolder:List<ZootehJob>{}
    
    public class RabEngZooTeh
    {
        private RabNetEngine eng;
        public RabEngZooTeh(RabNetEngine eng)
        {
            this.eng = eng;
        }

        public ZootehJob[] makeZooTehPlan(Filters f)
        {
            JobHolder zjobs = new JobHolder();
            getOkrols(zjobs);
            getVudvors(zjobs);
            getCounts(zjobs);
            return zjobs.ToArray();
        }

        public void getOkrols(JobHolder jh)
        {
            int days=eng.options().getIntOption(Options.OPT_ID.OKROL);
            ZooJobItem[] jobs=eng.db().getOkrols(days);
            foreach(ZooJobItem z in jobs)
                jh.Add(new ZootehJob().Okrol(z.id,z.name,z.place,z.status+1,z.age,z.i[0]-days));
        }
        public void getVudvors(JobHolder jh)
        {
            int days = eng.options().getIntOption(Options.OPT_ID.VUDVOR);
            ZooJobItem[] jobs = eng.db().getVudvors(days);
            foreach (ZooJobItem z in jobs)
                jh.Add(new ZootehJob().Vudvor(z.id, z.name, z.place, z.status + 1, z.age, z.i[0] - days,z.i[1]));
        }
        public void getCounts(JobHolder jh)
        {
            for (int i = 0; i < 3; i++)
            {
                Options.OPT_ID cnt = Options.OPT_ID.COUNT1;
                if (i == 1) cnt = Options.OPT_ID.COUNT2;
                if (i == 2) cnt = Options.OPT_ID.COUNT3;
                int days = eng.options().getIntOption(cnt);
                ZooJobItem[] jobs = eng.db().getCounts(days);
                foreach (ZooJobItem z in jobs)
                    jh.Add(new ZootehJob().Counts(z.id, z.name, z.place, z.age));
            }
        }
    }
}
