﻿#define LOCALDEBUG
#define WriteEnable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Guardant;
using log4net;

namespace RabGRD
{

    public sealed partial class GRDVendorKey : GRD_Base, IDisposable
    {
        const ushort AlgoNumGSII64 = 0;
        const ushort AlgoNumHash64 = 1;

        private bool _isActive = false;

        //private byte _model;
        private ushort _keyType;
        private ushort _lanRes;
        //private uint _uamOffset;
        private uint _prog;

        //private byte[] _userBuf=new byte[4096];
        //private ushort _userBufSize = 0;
        private byte[] _keyTRU = new byte[]{0xC6, 0x3B, 0x04, 0xF0, 0xB0, 0x37, 0x56, 0x88, 
                                            0x76, 0x89, 0x8A, 0x2F, 0xAE, 0xD1, 0x8E, 0x07 };//этот ключ-обновления записан во все проданые ключи

        public GRDVendorKey()
        {
            log = LogManager.GetLogger(typeof(GRDVendorKey));
            _findPropDongleType = GrdDT.TRU;
            connect();
        }

        /*public void CleanUserBuf()
        {
            _userBuf = new byte[4096];
            _userBufSize = 0;
        }*/

        protected override GrdE setAccessCodes()
        {
            return GrdApi.GrdSetAccessCodes(_grdHandle,             // Handle to Guardant protected container
                                           PublicCode + CryptPu,    // Public code, should always be specified
                                           ReadCode + CryptRd,      // Private read code; you can omit this code and all following via using of overloaded function;
                                           WriteCode + CryptWr,
                                           MasterCode + CryptMs);
        }



        public GRDVendorKey(uint keyID, byte[] truKey)
        {
            uint len = 16;
            if (truKey.Length < 16)
            {
                len = (uint)truKey.Length;
            }
            Array.Copy(truKey, _keyTRU, len);
            _findPropDongleFlags = GrdFM.ID;
            _findPropDongleID = keyID;
            _findPropDongleType = GrdDT.TRU;
            connect();
        }

        /*public uint UAMOffset
        {
            get { return _uamOffset; }
        }*/

        /*public uint ProgID
        {
            get { return _prog; }
        }*/

        public uint ID { get { return _id; } }

        private GrdE connect()
        {
            GrdE retCode; // Error code for all Guardant API functions

            FindInfo findInfo; //= new FindInfo();   // structure used in GrdFind()
            uint dongleID;
            string logStr = "";

            prepareHandle();

            logStr = "Searching for all specified dongles and print info about it's : ";
            retCode = GrdApi.GrdFind(_grdHandle, GrdF.First, out dongleID, out findInfo);
            if (retCode == GrdE.OK) // Print table header if at least one dongle found
            {
                logStr += String.Format("; Found dongle with following ID : {0,8:X}", _findPropDongleID);
                _id = findInfo.dwID;
                _keyType = findInfo.wType;
                _model = (byte)findInfo.dwModel;
                _prog = findInfo.byNProg;
                _lanRes = findInfo.wRealNetRes;
            }
            else
            {
                ErrorHandling(_grdHandle, retCode);
                return retCode;
            }

            logStr = "Searching for the specified local or remote dongle and log in : ";
            retCode = GrdApi.GrdLogin(_grdHandle, GrdLM.PerStation);
            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            if (retCode != GrdE.OK)
            {
                return ErrorHandling(_grdHandle, retCode); ;
            }
            _isActive = true;
            return GrdE.OK;
        }

        ~GRDVendorKey()
        {
            _isActive = false;
            disconnect();
        }

        public void Dispose()
        {
            _isActive = false;
            disconnect();
        }

        public bool Active
        {
            get { return _isActive; }
        }

        public void WriteMask(string org, int farms, int flags, DateTime startDate, DateTime endDate)
        {
            if (!GrdApi.GrdIsValidHandle(_grdHandle))
            {
                return;
            }

            byte[] userBuff = makeUserBuff(org, farms, flags, startDate, endDate);
            GrdE retCode; // Error code for all Guardant API functions
                       
            uint protectLength;
            ushort wNumberOfItems;
            byte[] pbyWholeMask = createNewMask(userBuff, out protectLength, out wNumberOfItems);

            string logStr = "Setting TruKey ";
            retCode = GrdApi.GrdTRU_SetKey(_grdHandle, _keyTRU);
            logStr += retCode.ToString() + " " + GrdApi.PrintResult((byte)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);

            logStr = "Writing test user buffer : ";
            retCode = GrdApi.GrdWrite(_grdHandle,
                                      (uint)0,
                                      pbyWholeMask.Length,
                                      pbyWholeMask);

            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);

            logStr = "Protecting : ";
            retCode = GrdApi.GrdProtect(_grdHandle,
                                        protectLength,
                                        protectLength,
                                        wNumberOfItems,
                                        0);
            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);

            log.Debug(" ====> " + (protectLength).ToString());
        }       

        public GrdE GetTRUAnswer(out string base64_answer, string base64_question, string org, int farms, int flags, DateTime startDate, DateTime endDate)
        {            
            byte[] buf = Convert.FromBase64String(base64_question);

            TRUQuestionStruct qq = (TRUQuestionStruct)GRDUtils.RawDeserialize(buf, typeof(TRUQuestionStruct));

            base64_answer = "";
            GrdE retCode; // Error code for all Guardant API functions
            string logStr = "Decrypt and validate question: ";

            retCode = GrdApi.GrdTRU_DecryptQuestion(_grdHandle,     // handle to Guardant protected container of dongle that contains 
                                                                    // GSII64 algorithm with the same key as in remote dongle 
                                                    AlgoNumGSII64,  // dongle GSII64 algorithm number with same key as in remote dongle 
                                                    AlgoNumHash64,  // dongle HASH64 algorithm number with same key as in remote dongle 
                                                    qq.question,    // pointer to Question					8 bytes (64 bit) 
                                                    qq.id,          // ID									4 bytes 
                                                    qq.pubKey,      // Public Code							4 bytes 
                                                    qq.hash);       // pointer to Hash of previous 16 bytes	8 bytes 

            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK)
                return retCode;

            uint protectLength;
            ushort wNumberOfItems;
            byte[] userBuff = makeUserBuff(org, farms, flags, startDate, endDate);
            byte[] pbyWholeMask = createNewMask(userBuff, out protectLength, out wNumberOfItems);

            logStr = "Set Init & Protect parameters for Trusted Remote Update: ";
            retCode = GrdApi.GrdTRU_SetAnswerProperties(_grdHandle,                                         // handle to Guardant protected container 
                                                        GrdTRU.Flags_Init | GrdTRU.Flags_Protect,           // use Init & Protect 
                                                        protectLength,                                      // SAM address of the first byte available for writing in bytes 
                                                        protectLength,                                      // SAM address of the first byte available for reading in bytes 
                                                        wNumberOfItems,                                     // number of hardware-implemented algorithms in the dongle including all protected items and LMS table of Net III 
                                                        0,                                                  // LMS Item number 
                                                        0);                                                 // Global Flags 


            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK)
                return retCode;

            int ansSize;
            byte[] answer = new byte[pbyWholeMask.Length * 3 + 128];

            logStr = "Encrypt answer for Trusted Remote Update: ";
            retCode = GrdApi.GrdTRU_EncryptAnswer(_grdHandle,                   // handle to Guardant protected container 
                // GSII64 algorithm with the same key as in remote dongle 
                // and pre-stored GrdTRU_SetAnswerProperties data if needed 
                                                  GRDConst.GrdSAMToUAM,         // starting address for writing in dongle 
                                                  (int)pbyWholeMask.Length,     // size of data to be written 
                                                  pbyWholeMask,                 // buffer for data to be written 
                                                  qq.question,                  // pointer to decrypted Question 
                                                  AlgoNumGSII64,                // dongle GSII64 algorithm number with the same key as in remote dongle 
                                                  AlgoNumHash64,                // dongle HASH64 algorithm number with the same key as in remote dongle 
                                                  out answer,                   // pointer to the buffer for Answer data 
                                                  out ansSize);                 // IN: Maximum buffer size for Answer data, OUT: Size of pAnswer buffer 
            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK)
                return retCode;

            //string base64data = 
            base64_answer = Convert.ToBase64String(answer, 0, ansSize, Base64FormattingOptions.InsertLineBreaks);
            return GrdE.OK;
        }

        private byte[] makeUserBuff(string org, int farms, int flags, DateTime startDate, DateTime endDate)
        {
            byte[] userBuff = new byte[USER_DATA_LENGTH]; //TODO говнокод!
            byte[] tmp = Encoding.GetEncoding(1251).GetBytes(DEV_MARKER);
            Array.Copy(tmp, userBuff, tmp.Length);

            tmp = Encoding.GetEncoding(1251).GetBytes(org);
            Array.Copy(tmp, 0, userBuff, ORGANIZATION_NAME_OFFSET, tmp.Length);

            tmp = BitConverter.GetBytes(farms);
            Array.Copy(tmp, 0, userBuff, MAX_BUILDINGS_COUNT_OFFSET, tmp.Length);

            tmp = BitConverter.GetBytes(flags);
            Array.Copy(tmp, 0, userBuff, FLAGS_MASK_OFFSET, tmp.Length);

            tmp = Encoding.GetEncoding(1251).GetBytes(startDate.ToString("yyyy-MM-dd"));
            Array.Copy(tmp, 0, userBuff, FARM_START_DATE_OFFSET, tmp.Length);

            tmp = Encoding.GetEncoding(1251).GetBytes(endDate.ToString("yyyy-MM-dd"));
            Array.Copy(tmp, 0, userBuff, FARM_STOP_DATE_OFFSET, tmp.Length);

            tmp = BitConverter.GetBytes(15);
            Array.Copy(tmp, 0, userBuff, TEMP_FLAGS_MASK_OFFSET, tmp.Length);

            tmp = Encoding.GetEncoding(1251).GetBytes(endDate.AddMonths(1).ToString("yyyy-MM-dd"));
            Array.Copy(tmp, 0, userBuff, TEMP_FLAGS_END_OFFSET, tmp.Length);

            return userBuff;
        }

        private byte[] createNewMask(byte[] userBuff, out uint protectLength, out ushort wNumberOfItems)
        {
            const byte nsafl_ST_III = 8;
            const byte nsafl_ActivationSrv = 16;
            const byte nsafl_DeactivationSrv = 32;
            const byte nsafl_UpdateSrv = 64;
            const ushort nsafh_ReadSrv = 128;
            const ushort nsafh_ReadPwd = 2;
            const byte rs_algo_GSII64 = 5;
            const byte GrdAdsGSII64Demo = 16;
            const byte GrdArsGSII64Demo = 8;
            const UInt32 GrdApGSII64DemoActivation = 0xAAAAAAAA;
            const UInt32 GrdApGSII64DemoDeactivation = 0xDDDDDDDD;
            const UInt32 GrdApGSII64DemoRead = 0xBBBBBBBB;
            const UInt32 GrdApGSII64DemoUpdate = 0xCCCCCCCC;

            const byte RsAlgoHash64 = 6;
            const byte GrdAdsHash64Demo = 16;
            const byte GrdArsHash64Demo = 8;
            const UInt32 GrdApHash64DemoActivation = 0xAAAAAAAA;
            const UInt32 GrdApHash64DemoDeactivation = 0xDDDDDDDD;
            const UInt32 GrdApHash64DemoRead = 0xBBBBBBBB;
            const UInt32 GrdApHash64DemoUpdate = 0xCCCCCCCC;

            DongleHeaderStruct dongleHeader;
            dongleHeader.ProgID = 55;
            dongleHeader.Version = 10;
            dongleHeader.SerialNumber = 123;
            dongleHeader.Mask = 12121;

            GrdE retCode;
            string logStr = "CreatingNewMask : ";

            byte[] abyDongleHeader = GRDUtils.RawSerialize(dongleHeader, 14);
            byte[] abyMask = new byte[4096];
            byte[] abyMaskHeader = new byte[4096];

            ushort wMaskSize = 0;
            ushort wASTSize = 0;
            wNumberOfItems = 0;

            WriteMaskInit(_keyType, _model);

            AddAlgorithm(abyMask,
                         abyMaskHeader,
                         AlgoNumGSII64,
                         (byte)(nsafl_ST_III + nsafl_ActivationSrv + nsafl_DeactivationSrv + nsafl_UpdateSrv),
                         (ushort)(nsafh_ReadSrv + nsafh_ReadPwd),
                         rs_algo_GSII64,
                         GrdAdsGSII64Demo,
                         GrdArsGSII64Demo,
                         GrdApGSII64DemoActivation,
                         GrdApGSII64DemoDeactivation,
                         GrdApGSII64DemoRead,
                         GrdApGSII64DemoUpdate,
                         null,
                         null,
                         null,
                         null,
                         0xFFFF,
                         0xFFFF,
                         _keyTRU,
                         ref wMaskSize,
                         ref wASTSize,
                         ref wNumberOfItems);

            AddAlgorithm(abyMask,
                         abyMaskHeader,
                         AlgoNumHash64,
                         (byte)(nsafl_ST_III + nsafl_ActivationSrv + nsafl_DeactivationSrv + nsafl_UpdateSrv),
                         (ushort)(nsafh_ReadSrv + nsafh_ReadPwd),
                         RsAlgoHash64,
                         GrdAdsHash64Demo,
                         GrdArsHash64Demo,
                         GrdApHash64DemoActivation,
                         GrdApHash64DemoDeactivation,
                         GrdApHash64DemoRead,
                         GrdApHash64DemoUpdate,
                         null,
                         null,
                         null,
                         null,
                         0xFFFF,
                         0xFFFF,
                         _keyTRU,
                         ref wMaskSize,
                         ref wASTSize,
                         ref wNumberOfItems);            

            byte[] pbyWholeMask = new byte[WHOLE_MASK_LENGTH];

            Array.Copy(abyDongleHeader,
                       0,
                       pbyWholeMask,
                       0,
                       GRDConst.GrdWmUAMOffset);
            Array.Copy(abyMaskHeader,
                       0,
                       pbyWholeMask,
                       GRDConst.GrdWmUAMOffset,
                       wASTSize);
            Array.Copy(abyMask,
                       0,
                       pbyWholeMask,
                       GRDConst.GrdWmUAMOffset + wASTSize,
                       wMaskSize);
            Array.Copy(userBuff,
                       0,
                       pbyWholeMask,
                       USER_DATA_BEGINING,
                       userBuff.Length);

            protectLength = (uint)(GRDConst.GrdWmSAMOffset + wASTSize + wMaskSize);
            return pbyWholeMask;
        }

        #region writemask.dll

        [DllImport("WriteMask.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "WriteMaskInit")]
        private static extern void WriteMaskInit(ushort wType,
                                                 byte byDongleModel);

        [DllImport("WriteMask.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LMS_DET_Prepare")]
        private static extern void LMS_DET_Prepare([MarshalAs(UnmanagedType.LPArray)] byte[] pMem,
                                                   ushort wFlags,
                                                   byte byLMSSize,
                                                   ushort wLANResource,
                                                   [MarshalAs(UnmanagedType.LPArray)] ushort[] wLMSModules,
                                                   ref ushort wLMSDetSize);

        [DllImport("WriteMask.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "CODE_DET_Prepare")]
        private static extern void CODE_DET_Prepare(Handle hGrd,
                                                           [MarshalAs(UnmanagedType.LPArray)] byte[] pMem,
                                                           byte byNumberOfCode,
                                                           [MarshalAs(UnmanagedType.LPArray)] byte[] szCodeMaskImage,
                                                           uint dwRAMStart,
                                                           uint dwRAMSize,
                                                           [MarshalAs(UnmanagedType.LPArray)] byte[] pPubECCKey4Sign,
                                                           [MarshalAs(UnmanagedType.LPArray)] byte[] pPrivECCKey4Key,
                                                           ref ushort pwCodeDetSize);

        [DllImport("WriteMask.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "AddAlgorithm")]
        private static extern void AddAlgorithm([MarshalAs(UnmanagedType.LPArray)] byte[] pbyAlgos,
                                                [MarshalAs(UnmanagedType.LPArray)] byte[] pbyAST,
                                                ushort wNumericName,
                                                byte byLoFlags,
                                                ushort wHiFlags,
                                                byte byAlgorithmCode,
                                                ushort wKeyLength,
                                                ushort wBlockLength,
                                                uint dwActivatePwd,
                                                uint dwDeactivatePwd,
                                                uint dwReadPwd,
                                                uint dwUpdatePwd,
            /*GrdTime*/ [MarshalAs(UnmanagedType.LPArray)] byte[] pBirthTime,
            /*GrdTime*/ [MarshalAs(UnmanagedType.LPArray)] byte[] pDeadTime,
            /*GrdLifeTime*/ [MarshalAs(UnmanagedType.LPArray)] byte[] pLifeTime,
            /*GrdLifeTime*/ [MarshalAs(UnmanagedType.LPArray)] byte[] pFlipTime,
                            ushort wGpCounter,
                            ushort wErrorCounter,
                            [MarshalAs(UnmanagedType.LPArray)] byte[] pbyDet,
                            ref ushort pwMaskSize,
                            ref ushort pwNewASTSize,
                            ref ushort pwNumberOfItems);



        [StructLayout(LayoutKind.Explicit)]
        struct DongleHeaderStruct
        {
            [FieldOffset(0)]
            public byte ProgID;
            [FieldOffset(1)]
            public byte Version;
            [FieldOffset(2)]
            public UInt16 SerialNumber;
            [FieldOffset(4)]
            public UInt16 Mask;
        }

        #endregion writemask.dll
    }
}
