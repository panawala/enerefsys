using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using EnerefsysDAL.Model;

namespace DataContext
{
    public class EnerefsysInitializer : DropCreateDatabaseIfModelChanges<EnerefsysContext>
    {
        protected override void Seed(EnerefsysContext context)
        {
            var dayLoads = new List<DayLoad>
            {
                new DayLoad{BeginTime="2001-02-22",EndTime="2002-03-23",Load=3223,Ratio=0.8,BuildingType="写字楼"},
                new DayLoad{BeginTime="2003-02-22",EndTime="2003-03-23",Load=4223,Ratio=0.8,BuildingType="写字楼"}
            };
            dayLoads.ForEach(s => context.DayLoads.Add(s));
            context.SaveChanges();

            var electronics = new List<Electronic>
            {
                new Electronic{StartTime="2003-3-22",EndTime="2004-03-23",ElectronicPrice=0.6,DefaultValue=0.6},
                new Electronic{StartTime="2004-3-22",EndTime="2005-03-23",ElectronicPrice=0.6,DefaultValue=0.6}
            };
            electronics.ForEach(s=>context.Electronics.Add(s));
            context.SaveChanges();

            var engineFitResults = new List<EngineFitResult> 
            {
                new EngineFitResult{B1=1.0f,B2=2.0f,B3=3.9f,B4=3.4f,B5=2.9f,B6=53.2f,Temperature=23,Type="CSD"},
                new EngineFitResult{B1=1.0f,B2=2.0f,B3=3.9f,B4=3.4f,B5=2.9f,B6=53.2f,Temperature=23,Type="VSD"}
            };
            engineFitResults.ForEach(s => context.EngineFitResults.Add(s));
            context.SaveChanges();

            var fitResults = new List<FitResult>
            {
                new FitResult{B1=2.0f,B2=3.2f,B3=4.2f,B4=5.2f,B5=43.2f,B6=2.2f,Temperature=23},
                new FitResult{B1=2.0f,B2=3.2f,B3=4.2f,B4=5.2f,B5=43.2f,B6=2.2f,Temperature=24}
            };
            fitResults.ForEach(s => context.FitResults.Add(s));
            context.SaveChanges();

            var optimizationResults=new List<OptimizationResult>
            {
                new OptimizationResult{Day="2003-3-2",Time="22:00-23:00",Temperature=23,Load=2323,
                EngineType="VSD",EngineValue=2323,EngineLoadRatio=0.23,EnginePower=232,Flow=0.23,
                SystemMinPower=323,FreezePumpPower=233,CoolingPumpPower=232,CoolingPower=23
                },
                new OptimizationResult{Day="2003-3-2",Time="22:00-23:00",Temperature=23,Load=2323,
                EngineType="VSD",EngineValue=2323,EngineLoadRatio=0.23,EnginePower=232,Flow=0.32,
                SystemMinPower=323,FreezePumpPower=233,CoolingPumpPower=232,CoolingPower=23
                }
            };
            optimizationResults.ForEach(s=>context.OptimizationResults.Add(s));
            context.SaveChanges();

            var pumpFitResults=new List<PumpFitResult>
            {
                new PumpFitResult{B1=23,B2=3,B3=4,Type="1"},
                new PumpFitResult{B1=23,B2=3,B3=4,Type="2"}
            };
            pumpFitResults.ForEach(s=>context.PumpFitResults.Add(s));
            context.SaveChanges();


            var pumpInfos=new List<PumpInfo>
            {
                new PumpInfo{PumpFlow=23,PumpCount=3,PumpType="2",PumpDesignFlow=323},
                new PumpInfo{PumpFlow=23,PumpCount=3,PumpType="1",PumpDesignFlow=323}
            };
            pumpInfos.ForEach(s=>context.PumpInfoes.Add(s));
            context.SaveChanges();

        }
    }
}
