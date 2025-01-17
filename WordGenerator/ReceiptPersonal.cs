﻿using AppCache;
using BE.PersData;
using DB.DataBase;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordGenerator.Extentions;
using WordGenerator.interfaces;
using ZXing;

namespace WordGenerator
{
    //название документа ReceiptPersonal
    public class ReceiptPersonalLk : IPdfGenerate
    {
        public PersDataDocumentLoad Generate(string LIC, DateTime date)
        {
            ICacheApp cacheApp = new CacheApp();
            if (cacheApp.GetValueProgress(LIC) != null)
                cacheApp.Delete(LIC);
            cacheApp.AddProgress(LIC, $"Получаю данные из БД за {date}");
            using (var db = new DbLIC())
            {
                var SubLic = LIC.Substring(3, 6);
                if (SubLic.StartsWith("0"))
                {
                    SubLic = SubLic.Substring(1, 5);
                    if (SubLic.StartsWith("0"))
                    {
                        SubLic = SubLic.Substring(1, 4);
                        if (SubLic.StartsWith("0"))
                        {
                            SubLic = SubLic.Substring(1, 3);
                        }
                    }
                }
                try
                {
                    var Lic = db.KVIT.FirstOrDefault(x => x.lic == SubLic && x.period.Value.Year == date.Year && x.period.Value.Month == date.Month);
                    if (Lic == null)
                    {
                        cacheApp.AddProgress(LIC, "Ничего не найдено за выбранный период");
                        throw new Exception("Ничего не найдено за выбранный период");
                    }
                    if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + $@"\Template\KvitLk\{date:MMMM-yyyy}"))
                        Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + $@"\Template\KvitLk\{date:MMMM-yyyy}");
                    string path = AppDomain.CurrentDomain.BaseDirectory + $@"Template\KvitLk\{date:MMMM-yyyy}\";
                    if (File.Exists(path + $@"Образец квитанции {LIC} {date.Month}.docx")) File.Delete(path + $@"Образец квитанции {LIC} {date.Month}.docx");

                    File.Copy(AppDomain.CurrentDomain.BaseDirectory + $@"Template\Образец квитанции.docx",
                        AppDomain.CurrentDomain.BaseDirectory + $@"Template\KvitLk\{date:MMMM-yyyy}\Образец квитанции {LIC} {date.Month}.docx");
                    cacheApp.Update(LIC, $"Начинаю формировать квитанцию за {date.ToString("dd-MMMM-yyyy")}");
                    Application app = new Application();
                    var TempFile = $@"Образец квитанции {LIC} {date.Month}.docx";
                    _Document doc = app.Documents.Open(path + TempFile);
                    try
                    {
                        doc.Content.Find.Execute("{address}", false, true, false, false, false, true, 1, false, $@"{Lic.ul.Trim()},дом {Lic.dom.Trim()},кв. {Lic.kw.Trim()}", 2,
    false, false, false, false);
                        doc.Content.Find.Execute("{lic}", false, true, false, false, false, true, 1, false, $"{LIC}", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{month}", false, true, false, false, false, true, 1, false, Lic.period.Value.ToString("MMMM,yyyy").Replace(",", " ").ToUpper(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{fio}", false, true, false, false, false, true, 1, false, Lic.fio.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{sobs}", false, true, false, false, false, true, 1, false, Lic.sobs?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{kl}", false, true, false, false, false, true, 1, false, Lic.kl?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{els}", false, true, false, false, false, true, 1, false, Lic.els?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{igku}", false, true, false, false, false, true, 1, false, Lic.igku?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{month2}", false, true, false, false, false, true, 1, false, Lic.period.Value.AddMonths(1).ToString("MMMM,yyyy").Replace(",", " ").ToUpper(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{sted2}", false, true, false, false, false, true, 1, false, Lic.sted2?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{sted5}", false, true, false, false, false, true, 1, false, Lic.sted5?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{sted3}", false, true, false, false, false, true, 1, false, Lic.sted3?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{koled2}", false, true, false, false, false, true, 1, false, Lic.koled2?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{koled5}", false, true, false, false, false, true, 1, false, Lic.koled5?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{koled3}", false, true, false, false, false, true, 1, false, Lic.koled3?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{koled6}", false, true, false, false, false, true, 1, false, Lic.koled6?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{koled4}", false, true, false, false, false, true, 1, false, Lic.koled4?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{sn2}", false, true, false, false, false, true, 1, false, Lic.sn2?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{sn5}", false, true, false, false, false, true, 1, false, Lic.sn5?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{sn3}", false, true, false, false, false, true, 1, false, Lic.sn3?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{sn6}", false, true, false, false, false, true, 1, false, Lic.sn6?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{sn4}", false, true, false, false, false, true, 1, false, Lic.sn4?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{sr2}", false, true, false, false, false, true, 1, false, Lic.sr2?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{sr5}", false, true, false, false, false, true, 1, false, Lic.sr5?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{sr3}", false, true, false, false, false, true, 1, false, Lic.sr3?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{sr15}", false, true, false, false, false, true, 1, false, Lic.sr15?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{sr6}", false, true, false, false, false, true, 1, false, Lic.sr6?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{sr4}", false, true, false, false, false, true, 1, false, Lic.sr4?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{it2}", false, true, false, false, false, true, 1, false, Lic.it2?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{it5}", false, true, false, false, false, true, 1, false, Lic.it5?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{it3}", false, true, false, false, false, true, 1, false, Lic.it3?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{it15}", false, true, false, false, false, true, 1, false, Lic.it15?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{it6}", false, true, false, false, false, true, 1, false, Lic.it6?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{it4}", false, true, false, false, false, true, 1, false, Lic.it4?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{u1oplzku}", false, true, false, false, false, true, 1, false, Lic.u1oplzku?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{u1oplpeny}", false, true, false, false, false, true, 1, false, Lic.u1oplpeny?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{u1nachzku}", false, true, false, false, false, true, 1, false, Lic.u1nachzku?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{u1nachpeny}", false, true, false, false, false, true, 1, false, Lic.u1nachpeny?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{kopl}", false, true, false, false, false, true, 1, false, Lic.kopl?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{ipuxv1_1}", false, true, false, false, false, true, 1, false, Lic.ipuxv1_1?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{ipuxv2_1}", false, true, false, false, false, true, 1, false, Lic.ipuxv2_1?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{ipuxv3_1}", false, true, false, false, false, true, 1, false, Lic.ipuxv3_1?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{ipuxv4_1}", false, true, false, false, false, true, 1, false, Lic.ipuxv4_1?.Trim(), 2,
        false, false, false, false);
                        var Comment = Substring(Lic.komment?.Trim());
                        doc.Content.Find.Execute("{komment1}", false, true, false, false, false, true, 1, false, Comment[0], 2,
      false, false, false, false);
                        doc.Content.Find.Execute("{komment2}", false, true, false, false, false, true, 1, false, Comment[1], 2,
     false, false, false, false);
                        doc.Content.Find.Execute("{komment3}", false, true, false, false, false, true, 1, false, Comment[2], 2,
     false, false, false, false);
                        doc.Content.Find.Execute("{komment4}", false, true, false, false, false, true, 1, false, Comment[3], 2,
    false, false, false, false);
                        doc.Content.Find.Execute("{ipuxv1_2}", false, true, false, false, false, true, 1, false, Lic.ipuxv1_2?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{ipuxv2_2}", false, true, false, false, false, true, 1, false, Lic.ipuxv2_2?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{ipuxv3_2}", false, true, false, false, false, true, 1, false, Lic.ipuxv3_2?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{ipuxv4_2}", false, true, false, false, false, true, 1, false, Lic.ipuxv4_2?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{ipuot1_1}", false, true, false, false, false, true, 1, false, Lic.ipuot1_1?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{ipuot2_1}", false, true, false, false, false, true, 1, false, Lic.ipuot2_1?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{ipuot3_1}", false, true, false, false, false, true, 1, false, Lic.ipuot3_1?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{ipuot1_2}", false, true, false, false, false, true, 1, false, Lic.ipuot1_2?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{ipuot2_2}", false, true, false, false, false, true, 1, false, Lic.ipuot2_2?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{ipuot3_2}", false, true, false, false, false, true, 1, false, Lic.ipuot3_2?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{dpuotrasx}", false, true, false, false, false, true, 1, false, Lic.dpuotrasx?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{dpuXVrasx}", false, true, false, false, false, true, 1, false, Lic.dpuxvrasx?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{dpuOTsum}", false, true, false, false, false, true, 1, false, Lic.dpuotsum?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{dpugvsum}", false, true, false, false, false, true, 1, false, Lic.dpugvsum?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{dpuXVsum}", false, true, false, false, false, true, 1, false, Lic.dpuxvsum?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{dpuOTnach}", false, true, false, false, false, true, 1, false, Lic.dpuotnach?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{dpugvnach}", false, true, false, false, false, true, 1, false, Lic.dpugvnach?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{dpuXVnach}", false, true, false, false, false, true, 1, false, Lic.dpuxvnach?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{dpuOTnez}", false, true, false, false, false, true, 1, false, Lic.dpuotnez?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{dpugvnez}", false, true, false, false, false, true, 1, false, Lic.dpugvnez?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{dpuXVnez}", false, true, false, false, false, true, 1, false, Lic.dpuxvnez?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{dpuELRASX}", false, true, false, false, false, true, 1, false, Lic.dpuelrasx?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{dpuELNEZ}", false, true, false, false, false, true, 1, false, Lic.dpuelnez?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula1}", false, true, false, false, false, true, 1, false,
                            !string.IsNullOrEmpty(Lic.sr15?.Replace(" ", "")) ? "Перерасчет сальдо" : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula2}", false, true, false, false, false, true, 1, false,
                           !string.IsNullOrEmpty(Lic.sr6?.Replace(" ", "")) || !string.IsNullOrEmpty(Lic.sn6?.Replace(" ", "")) ? "ОДН компонент ХВ" : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula3}", false, true, false, false, false, true, 1, false,
                          !string.IsNullOrEmpty(Lic.sr4?.Replace(" ", "")) || !string.IsNullOrEmpty(Lic.sn4?.Replace(" ", "")) ? "ОДН компонент ТЭ" : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula4}", false, true, false, false, false, true, 1, false,
                           !string.IsNullOrEmpty(Lic.sr6?.Replace(" ", "")) || !string.IsNullOrEmpty(Lic.sn6?.Replace(" ", "")) ? "Куб.м." : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula5}", false, true, false, false, false, true, 1, false,
                          !string.IsNullOrEmpty(Lic.sr4?.Replace(" ", "")) || !string.IsNullOrEmpty(Lic.sn4?.Replace(" ", "")) ? "Гкал" : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula6}", false, true, false, false, false, true, 1, false,
                          !string.IsNullOrEmpty(Lic.sr6?.Replace(" ", "")) || !string.IsNullOrEmpty(Lic.sn6?.Replace(" ", "")) ? Lic.sted5.Trim() : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula7}", false, true, false, false, false, true, 1, false,
                          !string.IsNullOrEmpty(Lic.sr4?.Replace(" ", "")) || !string.IsNullOrEmpty(Lic.sn4?.Replace(" ", "")) ? Lic.sted3.Trim() : "", 2,
        false, false, false, false);
                        var formula = Math.Round(Convert.ToDouble(Lic.u1dolgzku?.Trim().Replace(".", ",")) + Convert.ToDouble(Lic.u1oplzku?.Trim().Replace(".", ",")), 2);
                        doc.Content.Find.Execute("{formula8}", false, true, false, false, false, true, 1, false,
                        formula.ToString() != "" ? formula.ToString() : "", 2,
        false, false, false, false);
                        formula = Math.Round(Convert.ToDouble(Lic.u1dolgpeny?.Trim().Replace(".", ",")) + Convert.ToDouble(Lic.u1oplpeny?.Trim().Replace(".", ",")), 2);
                        doc.Content.Find.Execute("{formula9}", false, true, false, false, false, true, 1, false,
                        formula.ToString() != "" ? formula.ToString() : "", 2,
        false, false, false, false);
                        formula = Math.Round(Convert.ToDouble(Lic.u1dolgzku?.Trim().Replace(".", ",")) + Convert.ToDouble(Lic.u1dolgpeny?.Trim().Replace(".", ",")) + Convert.ToDouble(Lic.u1nachzku?.Trim().Replace(".", ",")) + Convert.ToDouble(Lic.u1nachpeny?.Trim().Replace(".", ",")), 2);
                        doc.Content.Find.Execute("{formula10}", false, true, false, false, false, true, 1, false,
                       formula.ToString() != "" ? formula.ToString() : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula11}", false, true, false, false, false, true, 1, false,
                        !string.IsNullOrEmpty(Lic.ipuxv1_1?.Replace(" ", "")) ? "ГВС1" : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula12}", false, true, false, false, false, true, 1, false,
                         !string.IsNullOrEmpty(Lic.ipuxv2_1?.Replace(" ", "")) ? "ГВС2" : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula13}", false, true, false, false, false, true, 1, false,
                         !string.IsNullOrEmpty(Lic.ipuxv3_1?.Replace(" ", "")) ? "ГВС3" : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula14}", false, true, false, false, false, true, 1, false,
                         !string.IsNullOrEmpty(Lic.ipuxv4_1?.Replace(" ", "")) ? "ГВС4" : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula15}", false, true, false, false, false, true, 1, false,
                         !string.IsNullOrEmpty(Lic.ipuot1_1?.Replace(" ", "")) ? "Отопление1" : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula16}", false, true, false, false, false, true, 1, false,
                         !string.IsNullOrEmpty(Lic.ipuot2_1?.Replace(" ", "")) ? "Отопление2" : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula17}", false, true, false, false, false, true, 1, false,
                         !string.IsNullOrEmpty(Lic.ipuot3_1?.Replace(" ", "")) ? "Отопление3" : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula18}", false, true, false, false, false, true, 1, false,
                        !string.IsNullOrEmpty(Lic.ipuxv1_1?.Replace(" ", "")) ? Lic.ngvs1?.Trim().Replace(" ", "") : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula19}", false, true, false, false, false, true, 1, false,
                         !string.IsNullOrEmpty(Lic.ipuxv2_1?.Replace(" ", "")) ? Lic.ngvs2?.Trim().Replace(" ", "") : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula20}", false, true, false, false, false, true, 1, false,
                         !string.IsNullOrEmpty(Lic.ipuxv3_1?.Replace(" ", "")) ? Lic.ngvs3?.Trim().Replace(" ", "") : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula21}", false, true, false, false, false, true, 1, false,
                         !string.IsNullOrEmpty(Lic.ipuxv4_1?.Replace(" ", "")) ? Lic.ngvs4?.Trim().Replace(" ", "") : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula22}", false, true, false, false, false, true, 1, false,
                        !string.IsNullOrEmpty(Lic.ipuot1_1?.Replace(" ", "")) ? Lic.notp1?.Trim().Replace(" ", "") : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula23}", false, true, false, false, false, true, 1, false,
                         !string.IsNullOrEmpty(Lic.ipuot2_1?.Replace(" ", "")) ? Lic.notp2?.Trim().Replace(" ", "") : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula24}", false, true, false, false, false, true, 1, false,
                        !string.IsNullOrEmpty(Lic.ipuot3_1?.Replace(" ", "")) ? Lic.notp3?.Trim().Replace(" ", "") : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula25}", false, true, false, false, false, true, 1, false,
                        !string.IsNullOrEmpty(Lic.ipuxv1_1?.Replace(" ", "")) ? Lic.dgvs1?.Trim() : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula26}", false, true, false, false, false, true, 1, false,
                        !string.IsNullOrEmpty(Lic.ipuxv2_1?.Replace(" ", "")) ? Lic.dgvs2?.Trim() : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula27}", false, true, false, false, false, true, 1, false,
                        !string.IsNullOrEmpty(Lic.ipuxv3_1?.Replace(" ", "")) ? Lic.dgvs3?.Trim() : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula28}", false, true, false, false, false, true, 1, false,
                        !string.IsNullOrEmpty(Lic.ipuxv4_1?.Replace(" ", "")) ? Lic.dgvs4?.Trim() : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula29}", false, true, false, false, false, true, 1, false,
                        !string.IsNullOrEmpty(Lic.ipuot1_1?.Replace(" ", "")) ? Lic.dotp1?.Trim() : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula30}", false, true, false, false, false, true, 1, false,
                        !string.IsNullOrEmpty(Lic.ipuot2_1?.Replace(" ", "")) ? Lic.dotp2?.Trim() : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{formula31}", false, true, false, false, false, true, 1, false,
                        !string.IsNullOrEmpty(Lic.ipuot3_1?.Replace(" ", "")) ? Lic.dotp3?.Trim() : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{s_gil}", false, true, false, false, false, true, 1, false, Lic.dpukasobs?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{s_nez}", false, true, false, false, false, true, 1, false, Lic.dpukanach?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{s_oi}", false, true, false, false, false, true, 1, false, Lic.s_oi?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{s_notp}", false, true, false, false, false, true, 1, false, Lic.s_notp?.Trim(), 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{pr_ras_gv1}", false, true, false, false, false, true, 1, false,
                        !string.IsNullOrEmpty(Lic.ipuxv1_1?.Replace(" ", "")) ? Lic.pr_ras_gv1?.Trim() : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{pr_ras_gv2}", false, true, false, false, false, true, 1, false,
                        !string.IsNullOrEmpty(Lic.ipuxv2_1?.Replace(" ", "")) ? Lic.pr_ras_gv2?.Trim() : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{pr_ras_gv3}", false, true, false, false, false, true, 1, false,
                        !string.IsNullOrEmpty(Lic.ipuxv3_1?.Replace(" ", "")) ? Lic.pr_ras_gv3?.Trim() : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{pr_ras_gv4}", false, true, false, false, false, true, 1, false,
                        !string.IsNullOrEmpty(Lic.ipuxv4_1?.Replace(" ", "")) ? Lic.pr_ras_gv4?.Trim() : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{pr_ras_ot1}", false, true, false, false, false, true, 1, false,
                        !string.IsNullOrEmpty(Lic.ipuot1_1?.Replace(" ", "")) ? Lic.pr_ras_ot1?.Trim() : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{pr_ras_ot2}", false, true, false, false, false, true, 1, false,
                        !string.IsNullOrEmpty(Lic.ipuot2_1?.Replace(" ", "")) ? Lic.pr_ras_ot2?.Trim() : "", 2,
        false, false, false, false);
                        doc.Content.Find.Execute("{pr_ras_ot3}", false, true, false, false, false, true, 1, false,
                        !string.IsNullOrEmpty(Lic.ipuot3_1?.Replace(" ", "")) ? Lic.pr_ras_ot3?.Trim() : "", 2,
        false, false, false, false);
                        cacheApp.Update(LIC, $"Сформировал квитацнию за {date}");
                        BarcodeWriter generator = new BarcodeWriter() { Format = BarcodeFormat.QR_CODE };
                        ZXing.QrCode.QrCodeEncodingOptions opt = new ZXing.QrCode.QrCodeEncodingOptions
                        {
                            CharacterSet = "windows-1251",
                            Width = 140,
                            Height = 140,
                            Margin = 2
                        };
                        generator.Options = opt;
                        var FIO = Lic.fio.Trim().Split(' ');
                        var sum = Convert.ToDouble(Lic.kopl.Trim().Replace(".", ",")) * 100;
                        var STR = $@"ST00011|Name=Пензенский филиал ПАО 'Т Плюс'|PersonalAcc=40702810748000001123|
BankName=Пензенское отделение № 8624 ПАО 'Сбербанк России' г. Пенза|BIC=045655635|CorrespAcc=30101810000000000635|PayeeINN=6315376946|
Category=7|PersAcc={LIC}|LastName={FIO.TryGetValue(0)}|FitstName={FIO.TryGetValue(1)}|MiddleName={FIO.TryGetValue(2)}
|PayerAddress={Lic.ul.Trim()}, дом {Lic.dom.Trim()}, кв. {Lic.kw.Trim()}|Sum={sum}";
                        generator.Write(UTF8ToWin1251(STR)).Save(path + $@"{LIC}_QR.png");
                        try
                        {
                            Microsoft.Office.Interop.Word.Range qr = doc.Range(0, 0);
                            qr.Find.Execute("{qr}");
                            doc.Content.Find.Execute("{qr}", false, true, false, false, false, true, 1, false, "", 2,
        false, false, false, false);
                            Range rangeImgQr = doc.Range(qr.Start, qr.End);
                            var ImgQr = doc.InlineShapes.AddPicture(path + $@"{LIC}_QR.png", false, true, rangeImgQr).ConvertToShape();
                            ImgQr.WrapFormat.Type = Microsoft.Office.Interop.Word.WdWrapType.wdWrapFront;

                        }
                        catch (Exception ex) { }
                    }
                    catch (Exception ex)
                    {
                        doc.Close(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges,
                           Microsoft.Office.Interop.Word.WdOriginalFormat.wdOriginalDocumentFormat,
                           false);
                        app.DisplayAlerts = Microsoft.Office.Interop.Word.WdAlertLevel.wdAlertsNone;
                        app.Quit();
                    }
                    try
                    {

                        //doc.Save();
                        doc.ExportAsFixedFormat(path + $@"Квитанция {LIC} {date.Month}.pdf", Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);
                        doc.Close(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges,
                           Microsoft.Office.Interop.Word.WdOriginalFormat.wdOriginalDocumentFormat,
                           false);
                        app.DisplayAlerts = Microsoft.Office.Interop.Word.WdAlertLevel.wdAlertsNone;
                        app.Quit();
                    }
                    catch (Exception ex)
                    {
                        cacheApp.Update(LIC, $"Ошибка {ex.Message}");
                        app.Quit();
                    }
                    cacheApp.Update(LIC, $"Очищаю временные файлы квитанции");
                    if (File.Exists(path + $@"Образец квитанции {LIC} {date.Month}.docx"))
                        File.Delete(path + $@"Образец квитанции {LIC} {date.Month}.docx");
                    if (File.Exists(path + $@"{LIC}.png")) File.Delete(path + $@"{LIC}.png");
                    if (File.Exists(path + $@"{LIC}_QR.png")) File.Delete(path + $@"{LIC}_QR.png");
                    cacheApp.Delete(LIC);

                    return new PersDataDocumentLoad()
                    {
                        FileBytes = File.ReadAllBytes(path + $@"Квитанция {LIC} {date.Month}.pdf"),
                        FileName = $@"Квитанция {LIC} {date.Month}.pdf"
                    };
                }
                catch (Exception ex)
                {
                    cacheApp.AddProgress(LIC, ex.Message);
                    return new PersDataDocumentLoad()
                    {
                        FileBytes = new byte[0],
                        FileName = $@"Квитанция {LIC} {date.Month}.pdf"
                    };
                }

            }
        }
        public string GenerateHtml(string LIC, DateTime date)
        {
           throw new NotImplementedException();
        }
        public string[] Substring(string T)
        {
            int z = 0;
            var arr = new string[4];
            if (T == null) return arr;
            for (int i = 0; i < T.Length - 1; i++)
            {
                if (i > 1000) break;
                if (i == 252 || i == 506 || i == 761)
                    z++;
                arr[z] += T[i];
            }
            return arr;
        }
        private static string UTF8ToWin1251(string sourceStr)
        {
            Encoding utf8 = Encoding.UTF8;
            Encoding win1251 = Encoding.GetEncoding(1251);
            byte[] utf8Bytes = utf8.GetBytes(sourceStr);
            byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);
            var result = win1251.GetString(win1251Bytes);
            return win1251.GetString(win1251Bytes);
        }
    }
    
}
