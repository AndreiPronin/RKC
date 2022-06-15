﻿using BE.Counter;
using BE.PersData;
using DB.DataBase;
using DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BL.Helper
{
    public static class ConvertToModel
    {
        public static IPU_COUNTERS ModelAddpu_To_IPU_COUNTERS(ModelAddPU model)
        {
            return new IPU_COUNTERS()
            {
                BRAND_PU = model.BRAND_PU,
                CLOSE_ = model.CLOSE_,
                DATE_CHECK = model.DATE_CHECK,
                DATE_CHECK_NEXT = model.DATE_CHECK_NEXT,
                TYPE_PU = GetDescriptionEnum.GetDescription(model.TYPE_PU),
                INSTALLATIONDATE = model.INSTALLATIONDATE,
                FACTORY_NUMBER_PU = model.FACTORY_NUMBER_PU,
                DESCRIPTION = model.DESCRIPTION,
                TIMESTAMP = DateTime.Now,
                SEALNUMBER = model.SEALNUMBER,
                MODEL_PU = model.MODEL_PU,
                SEAL_NUMBER = model.SEALNUMBER,
                TYPEOFSEAL = model.TYPEOFSEAL,
                FULL_LIC = model.FULL_LIC
                
            };
        }
        public static IPU ModelAddpu_To_IPU(ModelAddPU model)
        {
            
            var Typepu = GetDescriptionEnum.GetDescription(model.TYPE_PU);
            var IPU = new IPU()
            {
                BRAND_PU = model.BRAND_PU,
                MODEL_PU = model.MODEL_PU,
                TYPE_PU = Typepu.Substring(0, Typepu.Length-1),
                GIS_ID_PU = null,
                FULL_LIC = model.FULL_LIC,
                N_LIC = null,
                LINK1 = Typepu,
                LINK2 = Typepu,
            };
            if (model.TYPE_PU == TypePU.GVS1) IPU.FKUBSXVS = 1;
            if (model.TYPE_PU == TypePU.GVS2) IPU.FKUBSXV_2 = 1;
            if (model.TYPE_PU == TypePU.GVS3) IPU.FKUBSXV_3 = 1;
            if (model.TYPE_PU == TypePU.GVS4) IPU.FKUBSXV_4 = 1;
            if (model.TYPE_PU == TypePU.ITP1) IPU.FKUBSOT_1 = 1;
            if (model.TYPE_PU == TypePU.ITP2) IPU.FKUBSOT_2 = 1;
            if (model.TYPE_PU == TypePU.ITP3) IPU.FKUBSOT_3 = 1;
            if (model.TYPE_PU == TypePU.ITP4) IPU.FKUBSOT_4 = 1;
            return IPU;
        }
        public static PersData PersDataModel_To_PersData(PersDataModel persDataModel)
        {
            return new PersData()
            {
                idPersData = persDataModel.idPersData,
                DateAdd = persDataModel.DateAdd,
                Comment = persDataModel.Comment,
                Comment1 = persDataModel.Comment1,
                Comment2 = persDataModel.Comment2,
                DateOfBirth = persDataModel.DateOfBirth,
                Email = persDataModel.Email,
                FirstName = persDataModel.FirstName,
                Inn = persDataModel.Inn,
                IsDelete = persDataModel.IsDelete,
                LastName = persDataModel.LastName,
                Lic = persDataModel.Lic,
                Main = persDataModel.Main == null ? false : persDataModel.Main,
                MiddleName = persDataModel.MiddleName,
                NumberOfPersons = persDataModel.NumberOfPersons,
                PassportDate = persDataModel.PassportDate,
                PassportIssued = persDataModel.PassportIssued,
                PassportNumber = persDataModel.PassportNumber,
                PassportSerial = persDataModel.PassportSerial,
                PlaceOfBirth = persDataModel.PlaceOfBirth,
                RoomType = persDataModel.RoomType,
                SnilsNumber = persDataModel.SnilsNumber,
                Square = persDataModel.Square,
                StateLic = persDataModel.StateLic,
                Tel1 = persDataModel.Tel1,
                Tel2 = persDataModel.Tel2,
                UserName = persDataModel.UserName
            };
        }
        
    } 
}