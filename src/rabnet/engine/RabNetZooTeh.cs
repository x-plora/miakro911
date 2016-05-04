﻿using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{

    /// <summary>
    /// Является List из ZootehJob
    /// </summary>
    public class JobHolder : List<ZootehJob> { }

    public class RabEngZooTeh
    {
        private RabNetEngine eng;

        private Filters f = null;

        public RabEngZooTeh(RabNetEngine eng)
        {
            this.eng = eng;
        }

        public ZootehJob[] makeZooTehPlan(Filters f, int type)
        {
            JobHolder zjobs = new JobHolder();
            this.f = f;

            switch (type) {
                case 0:
                    if (f.safeValue("act", "O").Contains("O")) {
                        zjobs.AddRange(eng.db2().GetZooTechJobs(f, JobType.OKROL));
                    }
                    break;
                case 1:
                    if (f.safeValue("act", "V").Contains("V")) {
                        zjobs.AddRange(eng.db2().GetZooTechJobs(f, JobType.NEST_OUT));
                    }
                    break;
                case 2:
                    if (f.safeValue("act", "C").Contains("C")) {
                        this.getCounts(zjobs);
                    }
                    break;
                case 3:
                    if (f.safeValue("act", "P").Contains("P")) {
                        zjobs.AddRange(eng.db2().GetZooTechJobs(f, JobType.PRE_OKROL));
                    }
                    break;
                case 4:
                    if (f.safeValue("act", "R").Contains("R")) {
                        getBoysGirlsOut(zjobs);
                    }
                    break;
                case 5:
                    if (f.safeValue("act", "F").Contains("F")) {
                        getFucks(zjobs, 0);
                    }
                    break;
                case 6:
                    if (f.safeValue("act", "f").Contains("f")) {
                        getFucks(zjobs, 1);
                    }
                    break;
                case 7:
                    if (f.safeValue("act", "v").Contains("v")) {
                        zjobs.AddRange(eng.db2().GetZooTechJobs(f, JobType.VACC));
                    }
                    break;
                case 8:
                    if (f.safeValue("act", "N").Contains("N")) {
                        zjobs.AddRange(eng.db2().GetZooTechJobs(f, JobType.SET_NEST));
                    }
                    break;
                case 9:
                    if (f.safeValue("act", "B").Contains("B")) {
                        zjobs.AddRange(eng.db2().GetZooTechJobs(f, JobType.BOYS_BY_ONE));
                    }
                    break;
                case 10:
                    if (f.safeValue("act", "S").Contains("S")) {
                        zjobs.AddRange(eng.db2().GetZooTechJobs(f, JobType.SPERM_TAKE));
                    }
                    break;
            }

            return zjobs.ToArray();
        }

        private void getCounts(JobHolder jh)
        {
            for (int i = 1; i < 4; i++) {
                f["days"] = f.safeInt("count" + i.ToString()).ToString();
                f["next"] = i == 3 ? "-1" : f.safeInt("count" + (i + 1).ToString()).ToString();
                jh.AddRange(eng.db2().GetZooTechJobs(f, JobType.COUNT_KIDS));
            }
        }

        private void getBoysGirlsOut(JobHolder jh)
        {
            jh.AddRange(eng.db2().GetZooTechJobs(f, JobType.BOYS_OUT));
            jh.AddRange(eng.db2().GetZooTechJobs(f, JobType.GIRLS_OUT));
        }

        /// <summary>
        /// Добавляет к работам Случки или Вязки
        /// </summary>
        /// <param name="jh">Список работ</param>
        /// <param name="type">0- Случка, 1-Вязка</param>
        private void getFucks(JobHolder jh, int type)
        {
            f[Filters.MAKE_BRIDE] = eng.brideAge().ToString();
            f[Filters.TYPE] = type.ToString();
            jh.AddRange(eng.db2().GetZooTechJobs(f, JobType.FUCK));//ztGetZooFuck(f));
        }
    }
}
