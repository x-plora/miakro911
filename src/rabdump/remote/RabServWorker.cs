﻿#if DEBUG
//#define NOCATCH
#endif
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using log4net;
using SeasideResearch.LibCurlNet;
using System.Xml;
using System.Net;
using System.IO;
#if PROTECTED
using RabGRD;
using pEngine;
using System.Collections.Specialized;
#endif

namespace rabdump
{
    /// <summary>
    /// Класс отсылающий РК и Статистику на сервер
    /// </summary>
    static class RabServWorker
    {
        

        public enum State
        {
            /// <summary>
            /// Класс не выполняет никакой работы
            /// </summary>
            Free,
            Conection_Failed,

            UploadDump_Loading,
            /// <summary>
            /// Успешная загрузка на сервер
            /// </summary>
            UploadDump_Loaded,
            /// <summary>
            /// Ошибка при загрузке на сервер
            /// </summary>
            UploadDump_LoadErr,
            /// <summary>
            /// На сервере самая последняя РК
            /// </summary>
            UploadDump_Latest,
            /// <summary>
            /// На компьютере пользователя нет РК
            /// </summary>
            UploadDump_Dumpless,

            /// <summary>
            /// Ожидание списка Дампов от сервера
            /// </summary>
            DumpList_Wait,
            /// <summary>
            /// Список дампов получен. На сервере имеются дампы
            /// </summary>
            DumpList_NodesYes,
            /// <summary>
            /// Список дампов получен. На сервере дампов нет.
            /// </summary>
            DumpList_NodesNo,
            /// <summary>
            /// Процесс скачивания дампа
            /// </summary>
            DownloadDump_Loading,
            /// <summary>
            /// Дамп скачан
            /// </summary>
            DownloadDump_Loaded,
            /// <summary>
            /// Ошибка при скачке дампа
            /// </summary>
            DownloadDump_LoadErr,
            /// <summary>
            /// Получение даты последнего отчета
            /// </summary>
            WebRep_WaitLD,
            WebRep_WaitOK,
            WebRep_WaitErr,
            WebRep_Uploading,
            WebRep_Uploaded,
            WebRep_UploadErr,
        }

        struct ServData
        {
            internal string farmname;
            internal string dumpPath;
            internal FileStream uploadFS;
            internal System.Diagnostics.Stopwatch watcher;
            internal Double lastUploadedBytes;
            internal string webRepLD;
        }
        private static string _url = "http://192.168.0.95/rabServ/";
        //private static string uploadDumpURI { get { return _url + "/uploader.php"; } }
        //private static string downloadDumpURI { get { return  _url + "/getdump.php";} }
        private static ILog _logger = LogManager.GetLogger(typeof(RabServWorker));
        private static ServData _crossData;
        private static ArchiveJobThread _ajt;
        /// <summary>
        /// В каком состоянии находися Класс в данный момент
        /// </summary>
        private static State _state = State.Free;

        private static RequestSender _reqSend = null;
        internal static RequestSender ReqSender
        {
            get
            {
                if (_reqSend == null)
                {
                    _reqSend = new RequestSender();
                    _reqSend.UserID = GRD.Instance.GetClientID();
                    _reqSend.Key = GRD.Instance.GetKeyCode();
                    _reqSend.Url = _url;
                }
                return _reqSend;
            }
        }

        //public static State CurState { get { return _state; } }

        public static event MessageSenderCallbackDelegate OnMessage;

        /// <summary>
        /// Адрес удаленного сервера
        /// </summary>
        public static string Url
        {
            get { return _url; }
            set 
            {
                if (value == null || value == "")
                    return;
                _url = value; 
            }
        }

        /*public static void SetServerUrl(string url)
        {
            if (_state != State.Free || url == "") 
                return;
        }*/

        public static void MakeDump(ArchiveJob j)
        {
            //_crossData = new ServData();
            _ajt = new ArchiveJobThread(j);
            _ajt.Run();
            Thread t = new Thread(makeDump);
            t.Start();
        }

        /// <summary>
        /// Скачивает Дам с сервера
        /// </summary>
        /// <param name="filename">Название файла</param>
        /// <returns>Путь к скачанному файлу</returns>
        public static string DownloadDump(string filename)
        {
#if !NOCATCH
            try
            {
#endif
                callOnMessage("Скачиваем с сервера Резервную копию", "Начало загрузки", 1);
                long offset = unDownloaded(filename);
                downloadDump(filename, offset);

                callOnMessage(String.Format("Файл успешно скачан{0:s}Переходим к востановлению.", Environment.NewLine), "Успех", 1);
                return Path.GetTempPath() + filename;
#if !NOCATCH
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("DownloadDump", ex);
                return "";
            }
#endif
        }

        public static void SendWebReport()
        {           
            Thread t = new Thread(sendWebReport);
            t.Start();
        }

        /// <summary>
        /// Отправляет Веб-отчет
        /// </summary>
        private static void sendWebReport()
        {          
            const int MAX_DAYS = 200;
            //XmlDocument doc = newWebRepDoc();
            //DataSources могут изменить
            RabnetConfig.rabDataSource[] dataSources = new RabnetConfig.rabDataSource[RabnetConfig.DataSources.Count];
            RabnetConfig.DataSources.CopyTo(dataSources);
            sWebRepOneDay[] reps = null;
            foreach (RabnetConfig.rabDataSource rds in dataSources)
            {
                if (!rds.WebReport) continue;
                string webRepLD = "";
                //getLatestWebReport(_crossData.farmname, rds.Params.DataBase);

                string lastDate = RabServWorker.ReqSender.ExecuteMethod(MethodName.WebRep_GetLastDate,
                    MPN.farm, _crossData.farmname,
                    MPN.db, rds.Params.DataBase).Value as string;
                /*if (_state == State.Conection_Failed)
                {
                    callOnMessage("Не удалось подключиться к серверу", "", 2);
                    return;
                }*/
                rabnet.Engine.get().initEngine(rds.Params.ToString());

                //Если Получена дата последнего Отчета
                //if (_state == State.WebRep_WaitOK)
                //{
                DateTime stDate = DateTime.MaxValue;
                if (lastDate == "")
                {
                    stDate = rabnet.Engine.db().GetFarmStartTime();
                    if (stDate == DateTime.MaxValue) continue;
                    if (DateTime.Now.Date.Subtract(stDate.Date).Days > MAX_DAYS)
                        stDate = DateTime.Now.AddDays(-MAX_DAYS);
                }
                else
                {
                    DateTime.TryParse(webRepLD, out stDate);
                    if (stDate == DateTime.MaxValue)
                    {
                        _logger.ErrorFormat("LAST DATE ERROR: {0:s}", webRepLD);
                        continue;
                    }
                }
                if (stDate.Date == DateTime.Now.AddDays(-1))
                {
                    _logger.Info("На сервере самый свежий отчет");
                    continue;
                }
                ///далее Прибавляем все нужные отчеты
                reps = appendWR_Global(rds.Params.DataBase, DateTime.Now.AddDays(-1), stDate);
                //}
            }
            //if (exec)
            //{
                /*if (_state == State.WebRep_WaitErr)
                {
                    callOnMessage("Ошибка при отправке отчета", "", 2);
                    _state = State.Free;
                    return;
                }*/
                //TODO если не надо ни одного отчета отправить ???
                if (reps!=null && reps.Length>0)
                {//todo request sender

                    RabServWorker.ReqSender.ExecuteMethod(MethodName.WebRep_SendGlobal, MPN.value, reps);
                    //_crossData.webrepXML = doc.InnerXml;
                    //sendWRonServ();
                    callOnMessage("Статистика отослана", " ", 1);
                }
                else callOnMessage("На сервере самая свежая информация", "Отправка статистики отменена", 1);//TODO исправить условие
            //}
        }

        /// <summary>
        /// Составляет отчет за каждый день
        /// </summary>
        private static sWebRepOneDay[] appendWR_Global(string DBname,DateTime reportDate,DateTime startDate)
        {
            if (reportDate.Date == startDate.Date) return null;
            List<sWebRepOneDay> reps = new List<sWebRepOneDay>();
            while (startDate.Date < reportDate.Date)
            {
                reps.Add(new sWebRepOneDay(rabnet.Engine.db().WebReportGlobal(reportDate)));
                reportDate = reportDate.AddDays(-1);
            }
            return reps.ToArray();
        }

        private static void makeDump()
        {
            try
            {
                if (!System.IO.File.Exists(Options.Get().Path7Z))
                    throw new Exception("7z not specified");
                while (_ajt.JobIsBusy)
                    Thread.Sleep(2000);
                _logger.Debug("making Server Dump");
                callOnMessage("Начало отсылки" + Environment.NewLine + _ajt.JobName, "Отправка", 1, false);
                //RequestSender reqSend = MainForm.newReqSender();
                //sDump[] dumpList = reqSend.ExecuteMethod(MethodName.GetDumpList).Value as sDump[];
                //if (dumpList.Length > 0) //todo если что можно сделать diff
                //{
                    /*_logger.Debug("we have a DumpList"); 
                    bool coincidence = false;
                    foreach (sDump nd in dumpList)
                    {
                        if (_ajt.FileExists(nd.FileName, nd.MD5))
                        {
                            string srcFile = nd.FileName;
                            string trgFile = _ajt.GetLatestDump();
                            if (srcFile != Path.GetFileName(trgFile))
                            {
                                srcFile = Path.GetDirectoryName(trgFile) + "\\" + srcFile;
                                srcFile = ArchiveJobThread.ExtractDump(srcFile);
                                trgFile = ArchiveJobThread.ExtractDump(trgFile);
                                string diffPath = makeDiff(srcFile, trgFile);
                                diffPath = ArchiveJobThread.ZipFile(diffPath, true);
                                File.Delete(srcFile);
                                File.Delete(trgFile);
                                uploadDump(diffPath);
                                File.Delete(diffPath);
                                break;
                            }
                            else
                                callOnMessage("На сервере сама последняя версия БД", "загрузка отменена", 0);
                        }
                    }*/
                //}
                //else
                //{
                    string dumpPath = _ajt.GetLatestDump();
                    if (dumpPath != "")
                    {
                        string dump = ArchiveJobThread.ExtractDump(dumpPath);
                        string md5Dump = Helper.GetMD5FromFile(dump);
                        File.Delete(dump);
                        uploadDump(dumpPath, md5Dump);
                        callOnMessage("Файл отправлен успешно", "Успех", 1);
                    }
                //}               
            }
            catch (Exception)
            {
                callOnMessage("Ошибка при отправлении файла", "Внимание", 2);
            }

        }

        /// <summary>
        /// Файл должен делать патч
        /// </summary>
        /// <param name="srcFile">Старый файл</param>
        /// <param name="trgFile">Новый Файл</param>
        /// <returns>Путь к файлу с Дифом</returns>
        private static string makeDiff(string srcFile, string trgFile)
        {
            //TODO реализовать diff
            /*FileStream srcFS = new FileStream(srcFile, FileMode.Open, FileAccess.Read);
            StreamReader srcSR = new StreamReader(srcFile);
            FileStream trgFS = new FileStream(trgFile, FileMode.Open, FileAccess.Read);
            StreamReader trgSR = new StreamReader(trgFile);
            srcSR.Close(); srcFS.Close();
            trgSR.Close(); trgFS.Close();*/
            return trgFile;
        }

#region curl_usage

        /*private static void downloadDump(string farmname,string dumpFile, string offset)
        {
            _logger.Info("start Downloading Dump");
            _state = State.DownloadDump_Loading;
            _crossData.farmname = farmname;
            _crossData.dumpPath = dumpFile;
            _crossData.dumpOffset = offset;

            try
            {
                Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL);

                MultiPartForm mf = new MultiPartForm();
                mf.AddSection(CURLformoption.CURLFORM_COPYNAME, "farm",
                    CURLformoption.CURLFORM_COPYCONTENTS, defendString(farmname),
                    CURLformoption.CURLFORM_END);
                mf.AddSection(CURLformoption.CURLFORM_COPYNAME, "file",
                    CURLformoption.CURLFORM_COPYCONTENTS, dumpFile,
                    CURLformoption.CURLFORM_END);
                if (offset != "-1")               
                    mf.AddSection(CURLformoption.CURLFORM_COPYNAME, "offset",
                        CURLformoption.CURLFORM_COPYCONTENTS, offset.ToString(),
                        CURLformoption.CURLFORM_END);
                
                Easy easy = new Easy();

                easy.SetOpt(CURLoption.CURLOPT_LOW_SPEED_LIMIT, 10);
                easy.SetOpt(CURLoption.CURLOPT_LOW_SPEED_TIME, 30);

                easy.SetOpt(CURLoption.CURLOPT_HTTPPOST, mf);
                easy.SetOpt(CURLoption.CURLOPT_VERBOSE, true);
                easy.SetOpt(CURLoption.CURLOPT_URL, downloadDumpURI);
                Easy.WriteFunction wf = new Easy.WriteFunction(onWriteData);
                easy.SetOpt(CURLoption.CURLOPT_WRITEFUNCTION, wf);
                Easy.DebugFunction df = new Easy.DebugFunction(onDebug);
                easy.SetOpt(CURLoption.CURLOPT_DEBUGFUNCTION, df);             
                Easy.ProgressFunction pf = new Easy.ProgressFunction(onProgress);
                easy.SetOpt(CURLoption.CURLOPT_PROGRESSFUNCTION, pf);            
               
                easy.Perform();
                easy.Cleanup();
                mf.Free();

                Curl.GlobalCleanup();

                if (_state != State.DownloadDump_Loaded)
                {
                    _state = State.DownloadDump_LoadErr;
                    if (_crossData.uploadBW != null)
                    {
                        _crossData.uploadBW.Close();
                        _crossData.uploadBW = null;
                    }
                    if (_crossData.uploadFS != null)
                    {
                        _crossData.uploadFS.Close();
                        _crossData.uploadFS = null;
                    }
                }
                if (_crossData.watcher != null)
                {
                    _crossData.watcher.Stop();
                    _crossData.watcher = null;
                }
            }
            catch (NullReferenceException ex)
            {
                callOnMessage("Возможно отсутствует сетевое подключение", "Ошибка при отправке", 3);
                _logger.Error("sendfile", ex);
                if (_crossData.watcher != null)
                {
                    _crossData.watcher.Stop();
                    _crossData.watcher = null;
                }
                _state = State.Free;
            }
#if !NOCATCH          
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                _state = State.Free;
            }
#endif
        }
        private static void downloadDump()
        {
            downloadDump(_crossData.farmname,_crossData.dumpPath,_crossData.dumpOffset);
        }*/

        private static void downloadDump(string dumpFile, long offset)
        {
            string address = Path.Combine(ReqSender.Url, "uploader.php");

            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                NameValueCollection values = new NameValueCollection();
                values.Add("clientId", GRD.Instance.GetClientID().ToString());
                values.Add("fileName", dumpFile);
                if(offset!=-1)
                    values.Add("offset", offset.ToString());
                wc.QueryString = values;
                wc.DownloadFile(address, dumpFile);                
            }
        }

        private static void uploadDump(string dumpPath, string md5dump)
        {
            string address = Path.Combine(ReqSender.Url, "uploader.php");
            
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                NameValueCollection values = new NameValueCollection();
                values.Add("clientId", GRD.Instance.GetClientID().ToString());
                values.Add("md5dump", md5dump); 
                wc.QueryString = values;
                byte[] buff = wc.UploadFile(address, dumpPath);
                address = Encoding.UTF8.GetString(buff);
            }
        }

        

        /*private static MultiPartForm addVarsMPP(Dictionary<String,String> vars)
        {
            MultiPartForm mpp = new MultiPartForm();
            foreach (KeyValuePair<string, string> pair in vars)
            {
                // <input name="pair.Key">
                mpp.AddSection(CURLformoption.CURLFORM_COPYNAME, pair.Key,
                    CURLformoption.CURLFORM_COPYCONTENTS, defendString(pair.Value),
                    CURLformoption.CURLFORM_END);
            }
            return mpp;
        }*/

        /// <summary>
        /// При получении данных от сервера
        /// </summary>
        /// <returns>size * nmemb</returns>
        /*private static Int32 onWriteData(Byte[] buf, Int32 size, Int32 nmemb, Object extraData)
        {
            string msg = ""; 
            if (_state != State.DownloadDump_Loading)
            {
                msg = System.Text.Encoding.UTF8.GetString(buf);
                _logger.DebugFormat("Curl WriteData: {0:s}", msg);
            }
            if (_state == State.DumpList_Wait)
            {
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.LoadXml(msg);
                    XmlNode dumplist = doc.SelectSingleNode(dumpListNodeName);
                    if (dumplist != null)
                        if (dumplist.ChildNodes.Count > 0)
                        {
                            //_crossData.dumpList = dumpListDateDSort(doc);
                            _state = State.DumpList_NodesYes;                       
                        }
                        else _state = State.DumpList_NodesNo;
                }
                catch (XmlException) { }
            }
            else if (_state == State.DownloadDump_Loading)
            {
                if (_crossData.uploadFS == null)
                {
                    _crossData.uploadFS = new FileStream(Path.GetTempPath() + Path.GetFileName(_crossData.dumpPath) + ".tmp", FileMode.OpenOrCreate, FileAccess.Write);
                    _crossData.uploadBW = new BinaryWriter(_crossData.uploadFS);
                    _crossData.uploadBW.Seek(0, SeekOrigin.End);
                }
                _crossData.uploadBW.Write(buf);
            }
            if (_state == State.WebRep_WaitLD)
            {
                _crossData.webRepLD = msg;
                _state = State.WebRep_WaitOK;
            }
            return size * nmemb;
        }*/

        /// <summary>
        /// Возникает если есть прогресс передачи данных.
        /// </summary>
        /*private static Int32 onProgress(Object extraData, Double dlTotal, Double dlNow, Double ulTotal, Double ulNow)
        {
            _logger.DebugFormat("Curl Progress: {0} {1} {2} {3}", dlTotal, dlNow, ulTotal, ulNow);
            if (_state == State.DownloadDump_Loading && dlNow != 0 && dlTotal == dlNow)
            {
                _state = State.DownloadDump_Loaded;
                _crossData.uploadBW.Close(); 
                _crossData.uploadFS.Close();
                File.Move(_crossData.uploadFS.Name, _crossData.uploadFS.Name.Replace(".tmp",""));
                _crossData.dumpPath = _crossData.uploadFS.Name;
                _crossData.uploadFS = null;            
            }
            else if (_state == State.UploadDump_Loading || _state == State.DownloadDump_Loading)
            {
                Double Total = 0, Now = 0;
                if (_state == State.UploadDump_Loading) { Total = ulTotal; Now = ulNow; }
                if (_state == State.DownloadDump_Loading)
                {
                    if (dlTotal == 0) return 0;//Пока идет отправка запроса, и не получен размер файла
                    Total = dlTotal;
                    Now = dlNow;
                }
                if (Total == Now)
                {
                    if (_state == State.UploadDump_Loading) _state = State.UploadDump_Loaded;
                    if (_state == State.DownloadDump_Loading) _state = State.DownloadDump_Loaded;
                    _crossData.watcher.Stop();
                    _crossData.watcher = null;
                }
                else
                {
                    if (_crossData.watcher == null)
                    {
                        _crossData.watcher = new System.Diagnostics.Stopwatch();
                        _crossData.lastUploadedBytes = Now;
                        _crossData.watcher.Start();
                    }
                    else
                    {
                        if (_crossData.watcher.Elapsed.Seconds > 10)
                        {
                            _crossData.watcher.Stop();
                            double speed = (Now - _crossData.lastUploadedBytes) / _crossData.watcher.Elapsed.Seconds;

                            _crossData.watcher.Reset();

                            if (_state == State.UploadDump_Loading)
                                callOnMessage(String.Format("Отправленно: {0:s}{1:s}Скорость: {2:s}/сек", getSize(Now), Environment.NewLine, getSize(speed)), "отправка", 1);
                            if (_state == State.DownloadDump_Loading)
                                callOnMessage(String.Format("Скачано: {0:s}{1:s}Скорость: {2:s}/сек", getSize(_crossData.uploadFS.Length), Environment.NewLine, getSize(speed)), "Загрузка", 1);

                            _crossData.lastUploadedBytes = Now;
                            _crossData.watcher.Start();
                        }
                    }
                }
            }
            return 0;
        }*/

        /*private static void onDebug(CURLINFOTYPE infoType, String msg, Object extraData)
        {
            if((infoType ==CURLINFOTYPE.CURLINFO_DATA_IN && msg.Length<1000) || infoType == CURLINFOTYPE.CURLINFO_TEXT
#if DEBUG
                || infoType == CURLINFOTYPE.CURLINFO_HEADER_OUT || infoType == CURLINFOTYPE.CURLINFO_HEADER_IN
#endif
                )
            {
                _logger.DebugFormat("Curl Debug: [{1:s}] {0}", msg,infoType.ToString());
                servMsg(msg);
            }
        }*/

        /*private static string defendString(string str)
        {
            return str.Replace("\"", "").Replace("\\", "").Replace("/", "");
        }*/

        /// <summary>
        /// Вылавливание информации, которую нужно показать в ToolTip
        /// </summary>
        /// <param name="msg"></param>
        /*private static void servMsg(string msg)
        {
            if(msg.Contains("couldn't connect to host"))
            {
                callOnMessage("Не удалось подключиться к серверу", "Ошибка", 3);
            }
            else if(msg.Contains("[File Upload Successful]"))
            {
                _state = State.UploadDump_Loaded;
                //callOnMessage("Файл отправлен успешно", "Успех", 1);
            }
            else if(msg.Contains("[File Upload Error]"))
            {
                _state = State.UploadDump_LoadErr;
                //callOnMessage("Ошибка при отправлении файла", "Внимание", 2);
            }
            else if (msg.StartsWith("transfer closed with") && msg.EndsWith("bytes remaining to read\n"))
            {
                _state = State.DownloadDump_LoadErr;
            }
            else if (msg == "connect() timed out!\n")          
                _state = State.Conection_Failed;
            
        }*/
#endregion curl_usage
        /// <summary>
        /// Запускает событие
        /// </summary>
        /// <param name="msg">Сообщение</param>
        /// <param name="ttl">Заголовок</param>
        /// <param name="type">Тип(none,info,warning,error)</param>
        /// <param name="hide">Спрятать Значек в трее</param>
        private static void callOnMessage(string msg, string ttl, int type, bool hide)
        {
            if (OnMessage != null)
            {
                OnMessage(msg, ttl, type, hide);
            }
        }
        private static void callOnMessage(string msg, string ttl, int type)
        {
            callOnMessage(msg, ttl, type, false);
        }
        private static void callOnMessage(string msg, string ttl)
        {
            callOnMessage(msg, ttl, 0, false);
        }
        private static void callOnMessage(string msg)
        {
            callOnMessage(msg, "", 0, false);
        }


        

        /// <summary>
        /// Округляет число байтов до Кбайт,Мбайт,ГБайт
        /// </summary>
        /// <param name="bytes">Число байт</param>
        /// <returns>Строку типа "56,98 КБ"</returns>
        private static string getSize(Double bytes)
        {
            string units = "байт";
            double amount = bytes;
            if (amount / 1000 > 1)
            {
                units = "КБ";
                amount = bytes / 1000;
            }
            if (amount / 1000 > 1)
            {
                units = "МБ";
                amount = amount / 1000;
            }
            if (amount / 1000 > 1)
            {
                units = "ГБ";
                amount = amount / 1000;
            }
            return String.Format("{0:0.00} {1:s}",amount,units);
        }

        /// <summary>
        /// Возвращает размер (offset) недокачанного файла
        /// Если нет недокачанного, то возвращает  "-1"
        /// </summary>
        /// <param name="filename">Название файла</param>
        /// <returns></returns>
        private static long unDownloaded(string filename)
        {
            filename = Path.GetTempPath() + Path.GetFileName(filename) + ".tmp";
            if(File.Exists(filename))
            {
                FileInfo fi = new FileInfo(filename);
                return fi.Length;
            }
            else return -1;
        }

        #region to_delete
        /*private static XmlDocument newWebRepDoc()
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", "no"));
            doc.AppendChild(doc.CreateElement("webReports"));
            return doc;
        }*/

        /*private static void makeGlobalXml(XmlDocument doc, string db, string[] reps)
        {
            const string globRep = "global";

            XmlElement el = doc.CreateElement(globRep);
            el.Attributes.Append(doc.CreateAttribute("database"));
            el.Attributes["database"].Value = db;
            foreach (string oneday in reps)
            {
                string[] cols = oneday.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                XmlElement col = doc.CreateElement("oneglobalday");
                foreach(string s2 in cols)
                {
                    string[] dict = s2.Split(new char[] { '=' });
                    col.Attributes.Append(doc.CreateAttribute(dict[0]));
                    col.Attributes[dict[0]].Value = dict[1];
                }
                el.AppendChild(col);
            }
            doc.SelectSingleNode("webReports").AppendChild(el);
        }*/

        /*public static List<string> GetDumpList(string farmname,string dbName)
        {
            

            List<string> result = new List<string>();
            if (_crossData.dumpList!= null && _crossData.dumpList.FirstChild.ChildNodes.Count>0)
            {          
                foreach (XmlNode nd in _crossData.dumpList.FirstChild.ChildNodes)
                {
                    if (nd.Attributes["filename"].Value.Contains(dbName.Replace(' ','_')))
                    {
                        if(!result.Contains(nd.Attributes["filename"].Value))
                            result.Add( nd.Attributes["filename"].Value);                      
                    }
                }
            }
            _state = State.Free;
            return result;
        }*/

        /// <summary>
        /// Сортирует Ноды дампов по убыванию Дат.
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <returns>Отсортированная XML по датам</returns>
        /*private static XmlDocument dumpListDateDSort(XmlDocument srcDocument)
        {
            XmlDocument sortedDoc = new XmlDocument();
            XmlElement rootNode = sortedDoc.CreateElement(dumpListNodeName);
            sortedDoc.AppendChild(rootNode);
            foreach (XmlNode srcND in srcDocument.SelectSingleNode(dumpListNodeName))
            {
                if (srcND.Name != dumpListItemName) continue;
                if (rootNode.ChildNodes.Count == 0)
                    rootNode.AppendChild(sortedDoc.ImportNode(srcND,true));
                else
                {
                    DateTime srcDT = DateTime.Parse(srcND.Attributes["datetime"].Value);
                    foreach (XmlNode destND in rootNode.ChildNodes)
                    {                      
                        DateTime destDT = DateTime.Parse(destND.Attributes["datetime"].Value);
                        if (srcDT > destDT)
                        {
                            rootNode.InsertBefore(sortedDoc.ImportNode(srcND, true), destND);
                            break;
                        }
                        if (destND == rootNode.LastChild)
                        {
                            rootNode.AppendChild(sortedDoc.ImportNode(srcND, true));
                            break;
                        }
                    }
                }
            }
#if DEBUG
            sortedDoc.Save("sortedDoc.xml");
#endif
            return sortedDoc;
        }*/

        /*private static void configEasy(Easy easy,int timeout)
        {
            if (timeout != 0)
                easy.SetOpt(CURLoption.CURLOPT_CONNECTTIMEOUT, timeout);

            easy.SetOpt(CURLoption.CURLOPT_VERBOSE, true);
            easy.SetOpt(CURLoption.CURLOPT_URL, lastDumpURI);
            //easy.SetOpt(CURLoption.CURLOPT_HTTPPOST, mf);

            Easy.WriteFunction wf = new Easy.WriteFunction(onWriteData);
            easy.SetOpt(CURLoption.CURLOPT_WRITEFUNCTION, wf);

            Easy.DebugFunction df = new Easy.DebugFunction(onDebug);
            easy.SetOpt(CURLoption.CURLOPT_DEBUGFUNCTION, df);

            Easy.ProgressFunction pf = new Easy.ProgressFunction(onProgress);
            easy.SetOpt(CURLoption.CURLOPT_PROGRESSFUNCTION, pf);
        }
        private static void configEasy(Easy easy)
        {
            configEasy(easy, 0);
        }*/

        /*private static void getDumpList(string farmname)
        {
            try
            {
                _state = State.DumpList_Wait;
                _crossData.farmname = farmname;
                Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL);

                MultiPartForm mf = new MultiPartForm();
                mf.AddSection(CURLformoption.CURLFORM_COPYNAME, "farm",
                    CURLformoption.CURLFORM_COPYCONTENTS, defendString(farmname),
                    CURLformoption.CURLFORM_END);

                Easy easy = new Easy();

                easy.SetOpt(CURLoption.CURLOPT_CONNECTTIMEOUT, 10);
                easy.SetOpt(CURLoption.CURLOPT_VERBOSE, true);
                easy.SetOpt(CURLoption.CURLOPT_URL, lastDumpURI);
                easy.SetOpt(CURLoption.CURLOPT_HTTPPOST, mf);
                Easy.WriteFunction wf = new Easy.WriteFunction(onWriteData);
                easy.SetOpt(CURLoption.CURLOPT_WRITEFUNCTION, wf);
                Easy.DebugFunction df = new Easy.DebugFunction(onDebug);
                easy.SetOpt(CURLoption.CURLOPT_DEBUGFUNCTION, df);             
                Easy.ProgressFunction pf = new Easy.ProgressFunction(onProgress);
                easy.SetOpt(CURLoption.CURLOPT_PROGRESSFUNCTION, pf);


                easy.Perform();
                easy.Cleanup();
                mf.Free();

                Curl.GlobalCleanup();
                if (_state == State.DumpList_Wait)
                    _state = State.DumpList_NodesNo;
            }
            catch (NullReferenceException ex)
            {
                callOnMessage("Возможно отсутствует сетевое подключение", "Ошибка при отправке", 3);
                _logger.Error("sendfile", ex);
                _state = State.Free;
            }
#if !NOCATCH          
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                _state = State.Free;
            }
#endif
        }
        private static void getDumpList()
        {
            getDumpList(_crossData.farmname);
        }*/

        /*private static void uploadDump(string farmname,string dumpPath)
        {
            try
            {
                _state = State.UploadDump_Loading;
                _crossData.farmname = farmname;
                _crossData.dumpPath = dumpPath;

                _logger.Info("sending dump on server");
                Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL);

                MultiPartForm mform = new MultiPartForm();


                mform.AddSection(CURLformoption.CURLFORM_COPYNAME, "farm",
                    CURLformoption.CURLFORM_COPYCONTENTS, farmname,
                    CURLformoption.CURLFORM_END);

                mform.AddSection(CURLformoption.CURLFORM_COPYNAME, "type",
                    CURLformoption.CURLFORM_COPYCONTENTS, "dump",
                    CURLformoption.CURLFORM_END);
              
                mform.AddSection(CURLformoption.CURLFORM_COPYNAME, "uploadedfile",
                    CURLformoption.CURLFORM_FILE, dumpPath,
                    CURLformoption.CURLFORM_CONTENTTYPE, "application/binary",
                    CURLformoption.CURLFORM_END);

                Easy easy = new Easy();

                easy.SetOpt(CURLoption.CURLOPT_CONNECTTIMEOUT, 60);
                easy.SetOpt(CURLoption.CURLOPT_VERBOSE, true);
                easy.SetOpt(CURLoption.CURLOPT_URL, uploadDumpURI);
                easy.SetOpt(CURLoption.CURLOPT_HTTPPOST, mform);
                Easy.DebugFunction df = new Easy.DebugFunction(onDebug);
                easy.SetOpt(CURLoption.CURLOPT_DEBUGFUNCTION, df);            
                Easy.ProgressFunction pf = new Easy.ProgressFunction(onProgress);
                easy.SetOpt(CURLoption.CURLOPT_PROGRESSFUNCTION, pf);

                easy.Perform();
                easy.Cleanup();
                mform.Free();

                Curl.GlobalCleanup();
                if (_crossData.watcher != null)
                {
                    _crossData.watcher.Stop();
                    _crossData.watcher = null;
                } 
            }
            catch (NullReferenceException ex)
            {
                callOnMessage("Возможно отсутствует сетевое подключение", "Ошибка при отправке", 3);
                _logger.Error("sendfile", ex);
                _state = State.Free;
            }
#if !NOCATCH
            catch (ApplicationException ex)
            {
                callOnMessage(ex.Message, "Ошибка при отправке", 3);
                _logger.Error("sendfile", ex);
                _state = State.Free;
            }
            catch (Exception ex)
            {
                _logger.Fatal("sendfile", ex);
                _state = State.Free;
            }
#endif
        }
        private static void uploadDump()
        {
            uploadDump(_crossData.farmname, _crossData.dumpPath);
        }*/

        /*private static void getLatestWebReport(string farmname,string db)
        {
            try
            {
                _state = State.WebRep_WaitLD;
                _crossData.farmname = farmname;
                _crossData.DBname = db;
                Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL);

                MultiPartForm mf = new MultiPartForm();
                mf.AddSection(CURLformoption.CURLFORM_COPYNAME, "farm",
                    CURLformoption.CURLFORM_COPYCONTENTS, defendString(farmname),
                    CURLformoption.CURLFORM_END);
                mf.AddSection(CURLformoption.CURLFORM_COPYNAME, "db",
                    CURLformoption.CURLFORM_COPYCONTENTS, db,
                    CURLformoption.CURLFORM_END);

                Easy easy = new Easy();
                easy.SetOpt(CURLoption.CURLOPT_VERBOSE, true);
                easy.SetOpt(CURLoption.CURLOPT_URL, webrepGetLastDateURI);
                easy.SetOpt(CURLoption.CURLOPT_HTTPPOST, mf);
                easy.SetOpt(CURLoption.CURLOPT_CONNECTTIMEOUT, 10);

                Easy.WriteFunction wf = new Easy.WriteFunction(onWriteData);
                easy.SetOpt(CURLoption.CURLOPT_WRITEFUNCTION, wf);

                Easy.DebugFunction df = new Easy.DebugFunction(onDebug);
                easy.SetOpt(CURLoption.CURLOPT_DEBUGFUNCTION, df);

                Easy.ProgressFunction pf = new Easy.ProgressFunction(onProgress);
                easy.SetOpt(CURLoption.CURLOPT_PROGRESSFUNCTION, pf);


                easy.Perform();
                easy.Cleanup();
                mf.Free();

                Curl.GlobalCleanup();
                if (_state == State.WebRep_WaitLD)
                    _state = State.WebRep_WaitErr;
            }
            catch (NullReferenceException ex)
            {
                callOnMessage("Возможно отсутствует сетевое подключение", "Ошибка при отправке ВебОтчета", 3);
                _logger.Error("sendfile", ex);
                _state = State.Free;
            }
        }*/

        /*private static void sendWRonServ(string farmname, string xml)
        {
            try
            {
                _state = State.WebRep_Uploading;
                _crossData.farmname = farmname;
                _crossData.webrepXML = xml;
                Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL);

                MultiPartForm mf = new MultiPartForm();
                mf.AddSection(CURLformoption.CURLFORM_COPYNAME, "farm",
                    CURLformoption.CURLFORM_COPYCONTENTS, defendString(farmname),
                    CURLformoption.CURLFORM_END);
                mf.AddSection(CURLformoption.CURLFORM_COPYNAME, "report",
                    CURLformoption.CURLFORM_COPYCONTENTS, xml,
                    CURLformoption.CURLFORM_END);

                Easy easy = new Easy();

                easy.SetOpt(CURLoption.CURLOPT_CONNECTTIMEOUT, 10);
                easy.SetOpt(CURLoption.CURLOPT_VERBOSE, true);
                easy.SetOpt(CURLoption.CURLOPT_URL, webrepUploadURI);
                easy.SetOpt(CURLoption.CURLOPT_HTTPPOST, mf);
                Easy.WriteFunction wf = new Easy.WriteFunction(onWriteData);
                easy.SetOpt(CURLoption.CURLOPT_WRITEFUNCTION, wf);
                Easy.DebugFunction df = new Easy.DebugFunction(onDebug);
                easy.SetOpt(CURLoption.CURLOPT_DEBUGFUNCTION, df);
                Easy.ProgressFunction pf = new Easy.ProgressFunction(onProgress);
                easy.SetOpt(CURLoption.CURLOPT_PROGRESSFUNCTION, pf);


                easy.Perform();
                easy.Cleanup();
                mf.Free();

                Curl.GlobalCleanup();
                if (_state == State.WebRep_Uploading)
                    _state = State.WebRep_UploadErr;
            }
            catch (NullReferenceException ex)
            {
                callOnMessage("Возможно отсутствует сетевое подключение", "Ошибка при отправке Статистики", 3);
                _logger.Error("sendfile", ex);
                _state = State.Free;
            }
        }
        private static void sendWRonServ()
        {
            sendWRonServ(_crossData.farmname,_crossData.webrepXML);
        }*/
        #endregion to_delete
    }
}
