﻿using System;
using System.Collections.Generic;

namespace rabnet
{

    public static class TierFloor
    {
        public static int None = 0;
        public static int Upper = 1;
        public static int Lower = 2;
    }

    public enum BuildingType
    {
        None, Female, DualFemale, Jurta, Quarta, Vertep, Barin, Cabin, Complex
    }

    /// <summary>
    /// Класс олицетворяющий Адрес кролика (его местоположение)
    /// </summary>
    public class RabPlace
    {
        /// <summary>
        /// [0] Minifarm ID
        /// </summary>
        public readonly int Farm;
        /// <summary>
        /// [1] Тут записан этаж, но по стечению обстоятельств в базе он имеет имя tier_id
        /// </summary>
        public readonly int Floor;        
        /// <summary>
        /// [2]
        /// </summary>
        public readonly BuildingType Type;
        /// <summary>
        /// [3]
        /// </summary>
        public string Delims;
        /// <summary>
        /// [4]
        /// </summary>
        public string Heaters;
        /// <summary>
        /// [5]
        /// </summary>
        public int Section = -1;

        public RabPlace(int farm, int tier_id, int sec, BuildingType type, String delims, String heaters)
        {
            this.Farm = farm;
            this.Floor = tier_id;
            this.Type = type;
            this.Section = sec;
            this.Delims = delims;
            this.Heaters = heaters;
        }

        public bool CanHaveNest()
        {
            return Building.CanHaveNest(this.Type, this.Section);
        }

        public static RabPlace Parse(String rawAddress)
        {
            String[] dts = rawAddress.Split(',');
            // todo бывает так, что адрес не в правильной форме
            return new RabPlace(int.Parse(dts[0]), int.Parse(dts[1]), int.Parse(dts[2]), Building.ParseType(dts[3]), dts[4], dts[5]);
        }
        
    }

    /// <summary>
    /// Клас олицетворяющий МИНИферму
    /// </summary>
    public class Building : RabPlace, IData
    {
        #region consts
        public const string FEMALE_Eng = "female";
        public const string FEMALE_Rus = "Крольчихин";
        public const string FEMALE_Short = "крлч";

        public const string DOUBLE_Female_Eng = "dfemale";
        public const string DOUBLE_Female_Rus = "Двукрольчихин";
        public const string DOUBLE_Female_Short = "2крл";

        public const string JURTA_Eng = "jurta";
        public const string JURTA_Rus = "Юрта";
        public const string JURTA_Short = "юрта";

        public const string QUARTA_Eng = "quarta";
        public const string QUARTA_Rus = "Кварта";
        public const string QUARTA_Short = "кврт";

        public const string VERTEP_Eng = "vertep";
        public const string VERTEP_Rus = "Вертеп";
        public const string Vertep_Short = "вртп";

        public const string Barin_Eng = "barin";
        public const string Barin_Rus = "Барин";
        public const string Barin_Short = "барн";

        public const string Cabin_Eng = "cabin";
        public const string Cabin_Rus = "Хижина";
        public const string Cabin_Short = "хижн";

        public const string Complex_Eng = "complex";
        public const string Complex_Rus = "Комплексный";
        public const string Complex_Short = "кмпл";
        #endregion consts

        public readonly int ID;

        public string[] Notes;

        public bool Repair;

        public string Nests;

        public readonly int Sections;

        public readonly RabInBuild[] Busy;


        public Building(int id, int farm, int tier_id, BuildingType type, string delims, string notes, bool repair, string nests, string heaters)
            : base(farm, tier_id, -1, type, delims, heaters)
        {
            ID = id;
            
            //TypeName_Rus = typeLoc;
            Delims = delims;

            Repair = repair;
            Nests = nests;
            Heaters = heaters;

            Sections = GetRSecCount(Type);
            Busy = new RabInBuild[Sections];
            //for (int i = 0; i < Sections; i++)
            //{
            //    FullName[i] = Building.FullRName(Farm, TierID, i, TypeName, Delims, false, true, true);
            //    SmallName[i] = Building.FullRName(Farm, TierID, i, TypeName, Delims, true, false, false);
            //    MedName[i] = Building.FullRName(Farm, TierID, i, TypeName, Delims, false, true, false);
            //}
            Notes = new string[Sections];
            string[] tmpNotes = notes.Split(new char[] { '|' });
            for (int i = 0; i < Sections; i++) {
                Notes[i] = i < tmpNotes.Length ? tmpNotes[i] : "";
            }

        }

        public string Areas(int sec)
        {
            if (sec > Sections) {
                return "";
            }
            return (Floor == 0 ? "" : (Floor == 1 ? "^" : "-")) + Building.GetSecRus(Type, sec, Delims);
        }

        public string Descr(int sec, bool shr)
        {
            if (sec > Sections) {
                return "";
            }
            return Building.GetDescrRus(Type, shr, sec, Delims);
        }

        public int NestHeaterCount
        {
            get { return Building.GetRNHCount(Type); }
        }

        public string FullName(int sec)
        {
            if (sec > Sections) {
                return "";
            }
            return Building.FullNameRus(Farm, Floor, sec, Type, Delims, false, true, true);
        }

        public string MedName(int sec)
        {
            if (sec > Sections) {
                return "";
            }
            return Building.FullNameRus(Farm, Floor, sec, Type, Delims, false, true, false);
        }

        public string SmallName(int sec)
        {
            if (sec > Sections) {
                return "";
            }
            return Building.FullNameRus(Farm, Floor, sec, Type, Delims, true, false, false);
        }

        /// <summary>
        /// Объединена ли с другой клеткой путем удаления перегородки.
        /// </summary>
        /// <param name="sec">Номер секции. Начиная от 0</param>
        /// <returns></returns>
        public bool IsAbsorbed(int sec)
        {
            if (this.Type == BuildingType.Quarta) {
                return sec != 0 && this.Delims[sec - 1] == '0';
            }
            return false;
        }

        /// <summary>
        /// Можно ли установить гнездовье в данную секцию
        /// </summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        public bool CanHaveNest(int sec)
        {
            return Building.CanHaveNest(this.Type, sec);
        }

        /// <summary>
        /// Установлено ли гнездовье в секции
        /// </summary>
        /// <param name="sec">Индекс секции (с Нуля)</param>
        /// <returns></returns>
        public bool HasNest(int sec)
        {
            return Building.HasNest(this.Type, sec, this.Nests);
        }

        /**
         * Гамбит не знал как лучше сделать строку, которую можно установить в начале программы,
         * ее мог использовать класс Building и BuildingPanel    
         */
        #region format
        private static int _smbls = 6;
        private static char _dsym = ' ';

        /// <summary>
        /// Установить формат адреса МИНИфермы (По просьбе Татищево)
        /// </summary>
        /// <param name="symbols">Сколько цифр в строке адреса</param>
        /// <param name="defchar">Символ заполнитель</param>
        public static void SetDefFmt(int symbols, char defchar)
        {
            if (symbols < 4) {
                _smbls = 4;
            } else if (symbols > 10) {
                _smbls = 10;
            } else {
                _smbls = symbols;
            }
            if (defchar != '/' && defchar != '\\') {
                _dsym = defchar;
            }
        }
        public static void SetDefFmt(char defchar)
        {
            SetDefFmt(_smbls, defchar);
        }
        public static void SetDefFmt(int symbols)
        {
            SetDefFmt(symbols, _dsym);
        }

        /// <summary>
        /// Правило форматирования Номера клетки
        /// </summary>
        public static string Format(int farmN, int symbols, char defchar)
        {
            if (symbols < 4) {
                symbols = 4;
            }
            if (symbols > 10) {
                symbols = 10;
            }
            if (defchar == '/' || defchar == '\\') {
                defchar = _dsym;
            }

            string res = farmN.ToString();
            if (res.Length < symbols) {
                while (symbols != res.Length) {
                    res = defchar + res;
                }
            }
            return res;
        }
        /// <summary>
        /// Приводит номер фермы к нужному виду 
        /// (начало заполнено нулями или пробелами)
        /// </summary>
        /// <param name="farmN">Номер фермы</param>
        /// <returns></returns>
        public static string Format(int farmN)
        {
            return Format(farmN, _smbls, _dsym);
        }
        public static string Format(string farmN)
        {
            int fn = 0;
            int.TryParse(farmN, out fn);
            return fn == 0 ? farmN : Format(fn, _smbls, _dsym);
        }

        #endregion

        #region static

        public static bool CanHaveNest(BuildingType type, int sec)
        {
            int c = Building.GetRNHCount(type);
            return c > sec;
        }

        public static bool HasNest(BuildingType type, int sec, String nests)
        {
            if (!Building.CanHaveNest(type, sec)) {
                return false;
            }
            if (type == BuildingType.DualFemale) {
                return nests[sec] == '1';
            }
            return nests[0] == '1';
        }

        /// <summary>
        /// Описание клетки
        /// </summary>
        /// <param name="type"></param>
        /// <param name="shr"></param>
        /// <param name="sec"></param>
        /// <param name="delims"></param>
        /// <returns></returns>
        public static String GetDescrRus(BuildingType type, bool shr, int sec, String delims)
        {
            String res = "";
            switch (type) {
                case BuildingType.Female:
                case BuildingType.DualFemale: res = shr ? "гн+выг" : "гнездовое+выгул"; break;
                case BuildingType.Complex:
                    if (sec == 0) {
                        res = shr ? "гн+выг" : "гнездовое+выгул";
                    } else {
                        res = shr ? "отк" : "откормочное";
                    }
                    break;
                case BuildingType.Jurta:
                    if (sec == 0) {
                        if (delims[0] == '0') {
                            res = (shr ? "гн" : "гнездовое") + "+";
                        }
                        res += shr ? "мвг" : "м.выгул";
                    } else {
                        if (delims[0] == '1') {
                            res = (shr ? "гн" : "гнездовое") + "+";
                        }
                        res += shr ? "бвг" : "б.выгул";
                    }
                    break;
                case BuildingType.Cabin:
                case BuildingType.Quarta: res = shr ? "отк" : "откормочное"; break;
                case BuildingType.Vertep:
                case BuildingType.Barin: res = shr ? "врт" : "Вертеп"; break;
            }
            return res;
        }

        /// <summary>
        /// Секции на русском
        /// </summary>
        /// <param name="type">Тип яруса</param>
        /// <param name="sec">Количество секций</param>
        /// <param name="delims">Перегородки</param>
        /// <returns></returns>
        public static String GetSecRus(BuildingType type, int sec, String delims)
        {
            if (type == BuildingType.Female || type == BuildingType.Cabin) {
                return "";
            }

            const String SECNAMES = "абвг";
            String res = "" + SECNAMES[sec];
            if (type == BuildingType.Quarta && delims != "111") {
                for (int i = sec - 1; i >= 0; i--) {
                    if (delims[i] == '0') {
                        res = SECNAMES[i] + res;
                    } else {
                        break;
                    }
                }

                for (int i = sec; i < 3; i++) {
                    ///todo тройку в константу
                    if (delims[i] == '0') {
                        res = res + SECNAMES[i + 1];
                    } else {
                        break;
                    }
                }

            } else if (type == BuildingType.Barin && delims[0] == '0') {
                res = "аб";
            }
            return res;
        }

        public static String GetSecRus(string type, int sec, String delims)
        {
            return GetSecRus(ParseType(type), sec, delims);
        }

        /// <summary>
        /// Возвращает количество секций у данного типа МИНИфермы
        /// </summary>
        public static int GetRSecCount(BuildingType type)
        {
            int res = 2;
            switch (type) {
                case BuildingType.Cabin:
                case BuildingType.Female: res = 1; break;
                case BuildingType.Complex: res = 3; break;
                case BuildingType.Quarta: res = 4; break;
            }
            return res;
        }

        public static int GetRSecCount(string type)
        {
            return GetRSecCount(ParseType(type));
        }

        /// <summary>
        /// Rab Nest Heaater
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetRNHCount(BuildingType type)
        {
            int res = 1;
            switch (type) {
                case BuildingType.DualFemale: res = 2; break;
                case BuildingType.Quarta:
                case BuildingType.Vertep:
                case BuildingType.Barin: res = 0; break;
            }
            return res;
        }

        public static String FullNameRus(int farm, int tireFloor, int sec, BuildingType type, String delims, bool shrt, bool showTier, bool ShowDescr)
        {
            String res = Building.Format(farm);
            if (tireFloor == TierFloor.Upper) {
                res += "^";
            }
            if (tireFloor == TierFloor.Lower) {
                res += "-";
            }

            res += GetSecRus(type, sec, delims);
            if (showTier) {
                res += " [" + GetNameRus(type, shrt) + "]";
            }
            if (ShowDescr) {
                res += " (" + GetDescrRus(type, shrt, sec, delims) + ")";
            }
            return res;
        }

        public static String FullPlaceName(String rawAddres)
        {
            return FullPlaceName(rawAddres, false, false, false);
        }

        public static String FullPlaceName(String rawAddress, bool shrt, bool showTier, bool showDescr)
        {
            if (rawAddress == "") {
                return Rabbit.NULL_ADDRESS;
            }

            RabPlace rabPlace = RabPlace.Parse(rawAddress);
            return FullNameRus(rabPlace.Farm, rabPlace.Floor, rabPlace.Section, rabPlace.Type, rabPlace.Delims, shrt, showTier, showDescr);
        }       

        public static bool HasNest(String rawAddres)
        {
            if (rawAddres == "") {
                return false;
            }

            String[] dts = rawAddres.Split(',');
            return HasNest(ParseType(dts[3]), int.Parse(dts[2]), dts[5]);
        }

        /// <summary>
        /// Преобразует строковое название типа клетки в enum
        /// </summary>
        /// <param name="type">Название на русском или английском</param>
        /// <returns>Тип клетки. None если не смог распарсить</returns>
        public static BuildingType ParseType(string type)
        {
            switch (type) {
                case FEMALE_Eng:
                case FEMALE_Rus: return BuildingType.Female;
                case DOUBLE_Female_Eng:
                case DOUBLE_Female_Rus: return BuildingType.DualFemale;
                case JURTA_Eng:
                case JURTA_Rus: return BuildingType.Jurta;
                case QUARTA_Eng:
                case QUARTA_Rus: return BuildingType.Quarta;
                case VERTEP_Eng:
                case VERTEP_Rus: return BuildingType.Vertep;
                case Barin_Eng:
                case Barin_Rus: return BuildingType.Barin;
                case Cabin_Eng:
                case Cabin_Rus: return BuildingType.Cabin;
                case Complex_Eng:
                case Complex_Rus: return BuildingType.Complex;
                default: return BuildingType.None;
            }
        }

        public static string GetName(BuildingType type)
        {
            String res = "Нет";
            switch (type) {
                case BuildingType.Female: res = FEMALE_Eng; break;
                case BuildingType.DualFemale: res = DOUBLE_Female_Eng; break;
                case BuildingType.Jurta: res = JURTA_Eng; break;
                case BuildingType.Quarta: res = QUARTA_Eng; break;
                case BuildingType.Vertep: res = VERTEP_Eng; break;
                case BuildingType.Barin: res = Barin_Eng; break;
                case BuildingType.Cabin: res = Cabin_Eng; break;
                case BuildingType.Complex: res = Complex_Eng; break;
            }
            return res;
        }

        public static String GetNameRus(BuildingType type, bool shr)
        {
            String res = "Нет";
            switch (type) {
                case BuildingType.Female: res = shr ? FEMALE_Short : FEMALE_Rus; break;
                case BuildingType.DualFemale: res = shr ? DOUBLE_Female_Short : DOUBLE_Female_Rus; break;
                case BuildingType.Jurta: res = shr ? JURTA_Short : JURTA_Rus; break;
                case BuildingType.Quarta: res = shr ? QUARTA_Short : QUARTA_Rus; break;
                case BuildingType.Vertep: res = shr ? Vertep_Short : VERTEP_Rus; break;
                case BuildingType.Barin: res = shr ? Barin_Short : Barin_Rus; break;
                case BuildingType.Cabin: res = shr ? Cabin_Short : Cabin_Rus; break;
                case BuildingType.Complex: res = shr ? Complex_Short : Complex_Rus; break;
            }
            return res;
        }

        public static String GetNameRus(BuildingType type)
        {
            return GetNameRus(type, false);
        }

        #endregion static

    }

    public class RabInBuild
    {
        public int ID;
        public string Name;

        public RabInBuild(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
    }

    public class BuildingList : List<Building>
    {
        public Address SearchByMedName(string medAddress)
        {
            if (medAddress == Rabbit.NULL_ADDRESS) {
                return null;
            }

            for (int b = 0; b < this.Count; b++) {
                for (int sec = 0; sec < this[b].Sections; sec++) {
                    if (this[b].MedName(sec) == medAddress) {
                        return new Address(this[b].Farm, this[b].Floor, sec);
                    }
                }
            }
            return new Address(0, 0, 0);
        }
    }

    public class Address
    {
        /// <summary>
        /// МИНИферма
        /// </summary>
        public int Farm;
        /// <summary>
        /// Ярус (0,1)
        /// </summary>
        public int Floor;
        /// <summary>
        /// Клетка (1,2,3,4)
        /// </summary>
        public int Section;

        public Address(int farm, int floor, int sec)
        {
            this.Farm = farm;
            this.Floor = floor;
            this.Section = sec;
        }
    }

    public class AddressR : Address
    {
        /// <summary>
        /// ID яруса в базе
        /// </summary>
        public int TierId = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="farm">ID МИНИфермы</param>
        /// <param name="floor">Этаж (0,1)</param>
        /// <param name="sec">Секция</param>
        /// <param name="tier">ID яруса</param>
        public AddressR(int farm, int floor, int sec, int tier)
            : base(farm, floor, sec)
        {
            this.TierId = tier;
        }
    }
}
