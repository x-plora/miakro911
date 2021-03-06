using System;
using System.Collections.Generic;
using System.Xml;
using System.Drawing;

namespace rabnet
{
    /// <summary>
    /// ��������� ������� ������������ ����� ���� ������ IDataGetter
    /// </summary>
    public interface IData { }

    /// <summary>
    /// ��������� ������� ������������ ����� ���������(MySqlDataReader) ��� ���������� ����������� ���� �� �������.
    /// </summary>
    public interface IDataGetter
    {
        int getCount();
        int getCount2();
        int getCount3(); //+gambit
        float getCount4(); //
        /// <summary>
        /// ��������� DataReader
        /// </summary>
        void Close();
        /// <summary>
        /// �������� ��������� �������
        /// </summary>
        IData GetNextItem();
    }

    public interface IRabNetDataLayer
    {
        void Init(String connectionString);
        void Close();
        //ENVIRONMENT
        List<sUser> GetUsers();
        sUser getUser(int uid);
        int checkUser(String name, String password);
        String getOption(String name, String subname, uint uid);
        void setOption(String name, String subname, uint uid, String value);
        DateTime now();
        String[] getFilterNames(String type);
        Filters getFilter(String type, String name);
        void setFilter(String type, String name, Filters filter);
        
        /// <summary>
        /// �������� ����� ��������� ��������
        /// </summary>
        /// <returns></returns>
        int getMFCount();
        string GetRabGenoms(int rId);
        RabTreeData rabbitGenTree(int rabbit);
        BldTreeData buildingsTree();       
        YoungRabbitList GetYoungers(int momId);
        OneRabbit[] GetNeighbors(int rabId);
        int[] getTiers(int farm);
        Building getBuilding(int tier);       
        
        /// <summary>
        /// ������ ���������
        /// </summary>
        IDataGetter GetYoungers(Filters filters);
        IDataGetter getRabbits(Filters filters);
        IDataGetter getBuildingsRows(Filters filters);
        IDataGetter getNames(Filters filters);
        IDataGetter zooTeh(Filters f);        
        IDataGetter getDead(Filters filters);        
        List<String> getFuckMonths();
        void changeDeadReason(int rid, int reason);
        List<String> getDeadsMonths();
        OneRabbit GetRabbit(int rid);
        OneRabbit GetRabbit(int rid,RabAliveState state);
        void SetRabbit(OneRabbit r);
        ICatalogs catalogs();
        void RabNetLog(int type, int user, int r1, int r2, string a1, string a2, String text);
        Fucks GetFucks(Filters f);
        void CancelFuckEnd(int fuckID);
        //Fucks GetAllFuckers(int female,bool geterosis,bool inbreeding,int malewait);
        FuckPartner[] GetAllFuckers(Filters f);
        void setBon(int rabbit,String bon);

        /// <summary>
        /// ������� ���������
        /// </summary>
        /// <param name="femaleId">ID ���������</param>
        /// <param name="maleId">ID �����</param>
        /// <param name="daysPast">���� ������</param>
        /// <param name="worker">ID ������������ (���������)</param>
        /// <param name="syntetic);">������������� ����������</param>
        /// <param name="syntetic">������������ ����������</param>
        void MakeFuck(int femaleId, int maleId, int daysPast, int worker, bool syntetic);
        void makeProholost(int female, int daysPast);
        int makeOkrol(int female, int daysPast, int children, int dead);
        String makeName(int nm, int sur, int sec, int grp, Rabbit.SexType sex);
        bool unblockName(int id);

        BuildingList getBuildings(Filters f);
        void replaceRabbit(int rid, int farm, int floor, int sec);
        //void replaceYounger(int yid, int farm, int tier_id, int sec);
        int NewRabbit(OneRabbit r,int mom);
                
        void updateBuilding(Building b);
        int AddName(Rabbit.SexType sex, string name, string surname);
        void changeName(string orgName, string name, string surname);
        RabNamesList GetNames();

        /// <summary>
        /// �������� �������
        /// </summary>
        /// <param name="id">ID �������</param>
        /// <param name="daysPast">���� ������</param>
        /// <param name="reason">������� ��������</param>
        /// <param name="notes">�������</param>
        void KillRabbit(int id, int daysPast,int reason,string notes);

        /// <summary>
        /// ������� ���������
        /// </summary>
        /// <param name="rid">ID ���������</param>
        /// <param name="dead">���������� ��������</param>
        /// <param name="killed">���������� ����������</param>
        /// <param name="added">�����������</param>
        /// <param name="yid">� ����� ������ ���������</param>
        void CountKids(int rid, int dead, int killed, int added,int yid);       
        void setRabbitSex(int rid,Rabbit.SexType sex);
        int CloneRabbit(int rid, int count, Rabbit.SexType sex, int mom);
        string userGroup(int uid);
        void deleteUser(int uid);
        void changeUser(int uid, string name, int group, string password, bool chpass);
        bool hasUser(string name);
        int addUser(string name, int group, string password);
        
        void ResurrectRabbit(int rid);       
        void placeSucker(int sucker, int mother);
        void combineGroups(int rabfrom, int rabto);

        AdultRabbit[] getMothers(int age,int agediff);                 
        void setBuildingName(int bid,String name);
        void addBuilding(int parent, String name);
        void replaceBuilding(int bid, int toBuilding);
        void deleteBuilding(int bid);
        int addFarm(int parent, BuildingType uppertype, BuildingType lowertype, String name, int id);
        bool FarmExists(int id);
        void ChangeFarm(int fid, BuildingType uppertype, BuildingType lowertype);
        void deleteFarm(int fid);             
        String[] getWeights(int rabbit);

        int GetAliveChildrenCount(int rid, Rabbit.SexType parentSex);
        Dictionary<string, int> GetDeadChildrenCount(int rid, Rabbit.SexType parentSex);        

        // vaccines
        RabVac[] GetRabVac(int rabId);
        List<Vaccine> GetVaccines(bool withSpec);
        Vaccine GetVaccine(int vid);
        void EditVaccine(int id, string name, int duration, int age, int after, bool zoo,int times);

        int AddVaccine(string name, int duration, int age, int after, bool zoo,int times);


        void addWeight(int rabbit, int weight, DateTime date);
        void deleteWeight(int rabbit, DateTime date);
        OneRabbit[] getParents(int rabbit,int age);
        OneRabbit getLiveDeadRabbit(int rabbit);
        double[] getMaleChildrenProd(int male);
        void changeFucker(int fid, int newFucker, DateTime newFuckDate);
        void changeWorker(int fid, int worker);
		RabbitGen getRabbitGen(int rid);
		Dictionary<int, Color> getBreedColors();       

        //zooTech
        ZootehJob[] GetZooTechJobs(Filters f, JobType type);

        //catalogs
        ICatalog getDeadReasons();      
        ICatalog getBreeds();
        ICatalog getZones();
        ICatalog getProductTypes();
        //ICatalog getVaccines();

        void SetRabbitVaccine(int rid, int vid, DateTime date);
        void SetRabbitVaccine(int rid, int vid);
        void RabVacUnable(int rid, int vid, bool unable);

        LogList getLogs(Filters f);

        Rabbit[] GetDescendants(int ascendantId);

        
        /// <summary>
        /// ��������� �� ������ �� AI � ������� Rabbits
        /// </summary>
        void RabbitsTableAiCheck();
  
        void ArchLogs();
        String[] logNames();        

        IDataGetter getButcherDates(Filters f);
        List<String> getButcherMonths();
        List<sMeat> getMeats(DateTime date);
        DeadRabbit[] GetVictims(DateTime dt);

        XmlDocument makeReport(myReportType type, Filters f);
        XmlDocument makeReport(string query);

        //for buther
        List<sMeal> getMealPeriods();
        void addMealIn(DateTime start, int amount);
        void addMealOut(DateTime start, int amount);
        void deleteMeal(int id);

        //for scale
        //List<ScalePLUSummary> getPluSummarys(DateTime date);
        //void addPLUSummary(int prodid, string prodname, int tsell, int tsumm, int tweight, DateTime cleared);
        //void deletePLUsummary(int sid,DateTime lastClear);

        //for webreports
        string WebReportGlobal(DateTime dt);
        string[] WebReportsGlobal(DateTime dt,int days);
        DateTime GetFarmStartTime();      

        void ImportRabbit(int rId, int count);
        void ImportRabbit(int rId, int count, int clientId, int oldRID, string fileGuid);
        void ImportAscendant(OneRabbit r);
        List<OneImport> ImportSearch(Filters f);
        bool ImportAscendantExists(int rabId, int clientId);        

        int NewRabbit(OneRabbit r, int mom, int clientId, string fileGuid);

        ClientsList GetClients();
        void AddClient(int id, string name, string address);

        /// <summary>
        /// ����� ������ � ������ ��� �������������� ����������
        /// </summary>
        /// <param name="rId"></param>
        void SpermTake(int rId);

        IRabNetDataLayer Clone();        
        
        BreedsList GetBreeds();
        int AddBreed(string name,string shrt,string color);

        
    }

}