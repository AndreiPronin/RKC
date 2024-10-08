﻿using BE.Counter;
using DB.DataBase;
using DB.Model;
using DB.Model.Court.DictiomaryModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DIMENSION = DB.Model.DIMENSION;

namespace BL.Services
{
    public interface IDictionary
    {
        Task<List<IpuArchiveReason>> GetIpuArchiveReason();
        Task<List<IpuRecoverReason>> GetIpuRecoverReason();
        Task<List<DIMENSION>> GetDIMENSION();
        Task<List<Dictionary>> GetDictionary(int? Id,string Text, string Type, string TypePU);
        Task<List<string>> GetCourtNameDictionaries(string Text, int Id);
        Task<List<CourtNameDictionary>> GetAllCourtNameDictionaries();
        Task<List<CourtValueDictionary>> GetCourtValueDictionaryId(int Id);
        Task<List<CourtNameDictionary>> GetCourtDictionaries();
        List<FlatTypeDto> GetFlatType();
        List<Benefit> GetAllBenefit();
        Benefit GetBenefitById(int Id);
    }
    public class Dictionarys : IDictionary
    {
        /// <summary>
        /// Справочник едениц измерений ПУ
        /// </summary>
        /// <returns></returns>
        public async Task<List<DIMENSION>> GetDIMENSION()
        {
            using (var db = new DbTPlus())
                return await db.DIMENSIONs.ToListAsync();
        }
        public async Task<List<IpuArchiveReason>> GetIpuArchiveReason()
        {
            using (var db = new DbTPlus())
                return await db.IpuArchiveReasons
                    .Where(x=>x.IsActual == true)
                    .ToListAsync();
        }
        public async Task<List<IpuRecoverReason>> GetIpuRecoverReason()
        {
            using (var db = new DbTPlus())
                return await db.IpuRecoverReason
                    .Where(x => x.IsActual == true)
                    .ToListAsync();
        }
        public async Task<List<string>> GetCourtNameDictionaries(string Text, int Id)
        {
            using (var db = new ApplicationDbContext())
            {
                if (string.IsNullOrEmpty(Text))
                    return await db.CourtValueDictionary.Where(x => x.CourtNameDictionaryId == Id).Select(x => x.Name).ToListAsync();
                var Result = await db.CourtValueDictionary.Where(x => x.CourtNameDictionaryId == Id && x.Name.Contains(Text)).Select(x=>x.Name).ToListAsync();
                return Result;
            }
        }
        public async Task<List<CourtValueDictionary>> GetCourtValueDictionaryId(int Id)
        {
            using (var db = new ApplicationDbContext())
            {
                var Result = await db.CourtValueDictionary.Where(x => x.CourtNameDictionaryId == Id).Include(x=>x.CourtNameDictionary).ToListAsync();
                return Result;
            }
        }
        public async Task<List<CourtNameDictionary>> GetAllCourtNameDictionaries()
        {
            using (var db = new ApplicationDbContext())
            {
                var Result = await db.CourtNameDictionaries.ToListAsync();
                return Result;
            }
        }
        public async Task<List<CourtNameDictionary>> GetCourtDictionaries()
        {
            using (var db = new ApplicationDbContext())
            {
                var Result = await db.CourtNameDictionaries.Include(x=>x.CourtValueDictionaries).ToListAsync();
                return Result;
            }
        }
        public List<FlatTypeDto> GetFlatType()
        {
            using (var db = new DbLIC())
            {
                var Result = db.FlatTypes.ToList();
                return Result;
            }
        }
        public List<Benefit> GetAllBenefit()
        {
            using (var db = new ApplicationDbContext())
            {
                var Result = db.Benefit.ToList();
                return Result;
            }
        }
        public Benefit GetBenefitById(int Id)
        {
            using (var db = new ApplicationDbContext())
            {
                var Result = db.Benefit.FirstOrDefault(x=>x.Id == Id);
                return Result;
            }
        }

        public async Task<List<Dictionary>> GetDictionary(int? Id, string Text, string Type, string TypePU)
        {
            var dictionary = new List<Dictionary>();
            if (Type == "BRAND_PU")
            {
                using (var db = new DbTPlus())
                {
                    var dictionaryBrand = await db.BRAND
                        .Where(x => x.TYPE_PU == TypePU)
                        .ToListAsync();
                    foreach (var Item in dictionaryBrand)
                        dictionary.Add(new Dictionary { Id = Item.ID, Text = Item.BRAND_NAME, Type = Type });
                    return dictionary;
                }
            }
            if (Type == "MODEL_PU")
            { 
                using (var db = new DbTPlus())
                {
                    var dictionaryBrand = await db.BRAND.Include(x => x.MODEL)
                        .Where(x => x.BRAND_NAME == Text && x.TYPE_PU == TypePU)
                        .ToListAsync();
                    foreach (var Item in dictionaryBrand)
                        foreach (var Items in Item.MODEL)
                            dictionary.Add(new Dictionary { Id = Items.ID, Text = Items.MODEL_NAME, Type = Type });
                    return dictionary;
                }
            }
            return dictionary;
        }
    }
}
